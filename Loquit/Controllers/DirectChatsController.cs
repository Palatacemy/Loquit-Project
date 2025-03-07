using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Loquit.Data;
using Loquit.Data.Entities.ChatTypes;
using Loquit.Data.Entities;
using Loquit.Services.DTOs.ChatTypesDTOs;
using Microsoft.AspNetCore.Identity;
using Loquit.Services.DTOs.AbstracionsDTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Loquit.Services.DTOs.MessageTypesDTOs;
using Loquit.Utils;
using Microsoft.AspNetCore.Hosting;
using Loquit.Services.DTOs;
using Loquit.Web.Models;
using Loquit.Services.Services.Abstractions;
using Loquit.Services.Services.Abstractions.ChatTypesAbstractions;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Loquit.Web.Controllers
{
    public class DirectChatsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IDirectChatService _directChatService;
        private readonly IUserService _userService;

        public DirectChatsController(UserManager<AppUser> userManager, IWebHostEnvironment environment, IDirectChatService directChatService, IUserService userService)
        {
            _userManager = userManager;
            _environment = environment;
            _directChatService = directChatService;
            _userService = userService;
        }

        public async Task<IActionResult> Index(int? activeChatId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return NotFound("Current user not found.");
            }

            var directChatDTOs = await _directChatService.GetChatsForUserAsync(currentUser.Id);

            var currentUserDTO = await _userService.GetChatParticipantUserDTOByIdAsync(currentUser.Id);

            if (!directChatDTOs.Any())
            {
                var noChatsModel = new DirectChatViewModel
                {
                    ChatsList = new ChatsListViewModel 
                    { 
                        DirectChats = directChatDTOs 
                    },
                    CurrentChat = null
                };
                return View(noChatsModel);
            }

            var selectedChat = activeChatId.HasValue
                ? directChatDTOs.FirstOrDefault(c => c.Id == activeChatId.Value)
                : directChatDTOs.First();

            var model = new DirectChatViewModel
            {
                ChatsList = new ChatsListViewModel()
                {
                    DirectChats = directChatDTOs
                },
                CurrentChat = new CurrentChatViewModel()
                {
                    CurrentChat = selectedChat,
                    CurrentUser = currentUserDTO
                }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat(string id)
        {
            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId == null)
            {
                return NotFound("Current user not found.");
            }

            var currentUserDTO = await _userService.GetChatParticipantUserDTOByIdAsync(currentUserId);
            if (currentUserDTO == null)
            {
                return NotFound("Current user DTO not found.");
            }

            var otherUserDTO = await _userService.GetChatParticipantUserDTOByIdAsync(id);
            if (otherUserDTO == null)
            {
                return NotFound("Other user DTO not found.");
            }

            /*if (!currentUserDTO.Friends.Contains(otherUserDTO))
            {
                return NotFound();
            }*/

            if (currentUserDTO.Chats.Any(chat => chat.Chat.Members.Any(member => member.UserId == otherUserDTO.Id)))
            {
                return RedirectToAction("Index", "DirectChats");
            }

            var directChatDTO = new DirectChatDTO()
            {
                Members = new List<ChatUserDTO>
                {
                    new ChatUserDTO { UserId = currentUserDTO.Id, TimeOfJoining = DateTime.Now },
                    new ChatUserDTO { UserId = otherUserDTO.Id, TimeOfJoining = DateTime.Now }
                }, 
                Messages = new List<BaseMessageDTO>()
            };
            await _directChatService.AddDirectChatAsync(directChatDTO);

            var currentChatDTO = (await _directChatService.GetDirectChatsAsync()).Last();

            return RedirectToAction("Index", "DirectChats", new { activeChatId = currentChatDTO.Id });
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromForm] SendMessageViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) 
            {
                return NotFound("Current user not found.");
            }

            var currentUserDTO = await _userService.GetChatParticipantUserDTOByIdAsync(currentUser.Id);
            if (currentUserDTO == null)
            {
                return NotFound("Current user DTO not found.");
            }

            var directChatDTO = await _directChatService.GetDirectChatWithMessagesAsync(model.ChatId);
            if (directChatDTO == null)
            {
                return NotFound("Chat not found.");
            }

            var previousMessage = directChatDTO.Messages.Any()
                ? directChatDTO.Messages.OrderByDescending(m => m.TimeOfSending).FirstOrDefault()
                : null;

            BaseMessageDTO messageDTO;

            try
            {
                switch (model.MessageType)
                {
                    case "text":
                        messageDTO = new TextMessageDTO
                        {
                            SenderUserId = currentUserDTO.Id,
                            Text = model.Text,
                            ChatId = model.ChatId,
                            TimeOfSending = DateTime.Now
                        };
                        break;
                    case "image" when model.ImageFile != null:
                        var fileName = await FileUpload.UploadAsync(model.ImageFile, _environment.WebRootPath);
                        messageDTO = new ImageMessageDTO
                        {
                            SenderUserId = currentUserDTO.Id,
                            PictureUrl = fileName,
                            ChatId = model.ChatId,
                            TimeOfSending = DateTime.Now
                        };
                        break;
                    default:
                        return BadRequest("Invalid message type or missing file.");
                }

                var updatedChatDTO = await _directChatService.AddMessageToChatAsync(directChatDTO.Id, messageDTO);

                currentUserDTO.SentMessages.Add(messageDTO);
                currentUserDTO.MessagesWritten++;
                var updatedCurrentUserDTO = await _userService.UpdateChatParticipantUserAsync(currentUserDTO);

                var messageViewModel = new MessageViewModel
                {
                    MessageId = messageDTO.Id,
                    Message = messageDTO,
                    ChatId = updatedChatDTO.Id,
                    Chat = updatedChatDTO,
                    SenderUser = updatedCurrentUserDTO,
                    CurrentUser = updatedCurrentUserDTO,
                    PreviousMessage = previousMessage
                };

                return PartialView("_MessagePartial", messageViewModel);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> OpenChat(int? activeChatId)
        {
            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var currentUserDTO = await _userService.GetChatParticipantUserDTOByIdAsync(currentUserId);

            var chatDTO = await _directChatService.GetDirectChatByIdAsync(activeChatId.Value, currentUserId);
            if (chatDTO == null)
            {
                return NotFound("Chat not found");
            }

            var currentChatViewModel = new CurrentChatViewModel
            {
                CurrentChat = chatDTO,
                CurrentUser = currentUserDTO
            };
            return PartialView("PartialViewDirectChat", currentChatViewModel);
        }

        /*[HttpPost]
        public async Task<IActionResult> DeleteMessage(int chatId, int messageId)
        {
            var currentUserId = _userManager.GetUserId(User);
            var messageDTO = await _directChatService.GetMessageByIdAsync(messageId);

            if (messageDTO == null)
            {
                return Json(new { success = false, message = "Message not found" });
            }

            bool isImage = messageDTO is ImageMessageDTO;

            var result = await _directChatService.DeleteMessageFromChatAsync(chatId, messageId, currentUserId, isImage);

            if (result)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Failed to delete message" });
        }

        public async Task<IActionResult> LoadMessages(int chatId, int lastMessageId)
        {
            var messages = await _directChatService.GetMessagesBeforeIdAsync(chatId, lastMessageId, 50);
            return PartialView("_DirectChatMessagesPartial", messages);
        }*/


        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] DirectChat directChat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(directChat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(directChat);
        }

        // GET: DirectChats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var directChat = await _context.DirectChats.FindAsync(id);
            if (directChat == null)
            {
                return NotFound();
            }
            return View(directChat);
        }

        // POST: DirectChats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] DirectChat directChat)
        {
            if (id != directChat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(directChat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DirectChatExists(directChat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(directChat);
        }

        // GET: DirectChats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var directChat = await _context.DirectChats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (directChat == null)
            {
                return NotFound();
            }

            return View(directChat);
        }

        // POST: DirectChats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var directChat = await _context.DirectChats.FindAsync(id);
            if (directChat != null)
            {
                _context.DirectChats.Remove(directChat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/
    }
}
