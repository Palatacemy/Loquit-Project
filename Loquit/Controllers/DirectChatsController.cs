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

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return NotFound("Current user not found.");
            }

            var directChatDTOs = await _directChatService.GetChatsForUserAsync(currentUser.Id);

            var currentUserDTO = await _userService.GetChatParticipantUserDTOByIdAsync(currentUser.Id);

            var firstChatDTO = directChatDTOs.First();

            var messageDTOs = directChatDTOs.Any()
                ? await _directChatService.GetMessagesForChatAsync(firstChatDTO.Id)
                : new List<BaseMessageDTO>();

            var chatListModel = new ChatsListViewModel()
            {
                DirectChats = directChatDTOs
            };

            var currentChatModel = new CurrentChatViewModel()
            {
                CurrentChat = firstChatDTO,
                Messages = messageDTOs,
                CurrentUser = currentUserDTO
            };

            var model = new DirectChatViewModel
            {
                ChatsList = chatListModel,
                CurrentChat = currentChatModel
            };

            return View("Index", model);
        }

        [Route("/DirectChats")]
        public async Task<IActionResult> CreateChat(string id)
        {
            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId == null)
            {
                return NotFound("Current user Id not found.");
            }

            var currentUserDTO = await _userService.GetChatParticipantUserDTOByIdAsync(currentUserId);
            if (currentUserDTO == null)
            {
                return NotFound("Current user not found.");
            }

            var otherUserDTO = await _userService.GetChatParticipantUserDTOByIdAsync(id);
            if (otherUserDTO == null)
            {
                return NotFound("Other user not found.");
            }

            /*if (!currentUserDTO.Friends.Contains(otherUserDTO))
            {
                return NotFound();
            }*/

            if (currentUserDTO.Chats.Any(chat => chat.UserId == otherUserDTO.Id))
            {
                return RedirectToAction("Index");
            }
            var directChatDTO = new DirectChatDTO()
            {
                Members = new List<ChatUserDTO>
                {
                    new ChatUserDTO { UserId = currentUserDTO.Id},
                    new ChatUserDTO { UserId = otherUserDTO.Id}
                }, 
                Messages = new List<BaseMessageDTO>()
            };
            await _directChatService.AddDirectChatAsync(directChatDTO);

            var directChatDTOs = await _directChatService.GetChatsForUserAsync(currentUserDTO.Id);
            var messageDTOs = directChatDTOs.Any()
                ? await _directChatService.GetMessagesForChatAsync(directChatDTOs.First().Id)
                : new List<BaseMessageDTO>();

            var chatListModel = new ChatsListViewModel()
            {
                DirectChats = directChatDTOs
            };

            var currentChatModel = new CurrentChatViewModel()
            {
                CurrentChat = directChatDTO,
                Messages = messageDTOs,
                CurrentUser = currentUserDTO
            };

            var model = new DirectChatViewModel
            {
                ChatsList = chatListModel,
                CurrentChat = currentChatModel
            };

            return View("Index", model);
        }

        [HttpPost]
        [Route("/DirectChats")]
        public async Task<IActionResult> SendMessage([FromForm] SendMessageViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) 
            {
                return NotFound("Current user not found.");
            }

            var directChatDTO = await _directChatService.GetDirectChatByIdAsync(model.ChatId);
            if (directChatDTO == null)
            {
                return NotFound("Chat not found.");
            }

            BaseMessageDTO messageDTO;

            switch (model.MessageType)
            {
                case "text":
                    messageDTO = new TextMessageDTO
                    {
                        SenderUserId = currentUser.Id,
                        Text = model.Text,
                        ChatId = model.ChatId
                    };
                    break;
                case "image" when model.ImageFile != null:
                    var fileName = await FileUpload.UploadAsync(model.ImageFile, _environment.WebRootPath);
                    messageDTO = new ImageMessageDTO
                    {
                        SenderUserId = currentUser.Id,
                        PictureUrl = fileName,
                        ChatId = model.ChatId
                    };
                    break;
                default:
                    return BadRequest("Invalid message type or missing file.");
            }

            try
            {
                await _directChatService.AddMessageToChatAsync(directChatDTO, messageDTO);

                currentUser.MessagesWritten++;
                await _userManager.UpdateAsync(currentUser);

                var currentChatModel = new CurrentChatViewModel
                {
                    CurrentChat = directChatDTO,
                    Messages = directChatDTO.Messages,
                    CurrentUser = await _userService.GetChatParticipantUserDTOByIdAsync(currentUser.Id)
                };

                return PartialView("_DirectChatMessagesPartial", currentChatModel);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        public async Task<IActionResult> LoadMessages(int chatId, int lastMessageId)
        {
            var messages = await _directChatService.GetMessagesBeforeIdAsync(chatId, lastMessageId, 50);
            return PartialView("_DirectChatMessagesPartial", messages);
        }


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
