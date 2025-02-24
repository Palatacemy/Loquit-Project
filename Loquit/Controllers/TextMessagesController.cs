using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Loquit.Data;
using Loquit.Data.Entities.MessageTypes;
using Loquit.Data.Entities.Abstractions;
using Loquit.Data.Entities;
using Loquit.Services.DTOs;
using Microsoft.AspNetCore.Identity;
using Loquit.Services.DTOs.MessageTypesDTOs;
using Loquit.Services.Services.Abstractions.ChatTypesAbstractions;
using Loquit.Services.Services.Abstractions.MessageTypesAbstractions;

namespace Loquit.Web.Controllers
{
    public class TextMessagesController : Controller
    {
        private readonly ITextMessageService _textMessageService;
        private readonly IDirectChatService _directChatService;
        private readonly UserManager<AppUser> _userManager;

        public TextMessagesController(ITextMessageService textMessageService, IDirectChatService directChatService, UserManager<AppUser> userManager)
        {
            _textMessageService = textMessageService;
            _directChatService = directChatService;
            _userManager = userManager;
        }

        /* [HttpPost]
         [ValidateAntiForgeryToken]*/
        /*public async Task<IActionResult> Create(TextMessageDTO textMessage)
        {
            if (ModelState.IsValid)
            {
                textMessage.TimeOfSending  = DateTime.Now;
                textMessage.SenderUser = await _userManager.GetUserAsync(User);
                await _textMessageService.AddTextMessageAsync(textMessage);
                textMessage.SenderUser.MessagesWritten++;
                var directChat = await _directChatService.GetDirectChatByIdAsync(textMessage.)
                return PartialView("PartialViewDirectChat", );
            }
            return NotFound();
        }*/

        /*// GET: TextMessages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TextMessages.Include(t => t.SenderUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TextMessages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var textMessage = await _context.TextMessages
                .Include(t => t.SenderUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (textMessage == null)
            {
                return NotFound();
            }

            return View(textMessage);
        }

        // GET: TextMessages/Create
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_context.Set<BaseMessage>(), "Id", "Discriminator");
            ViewData["SenderUserId"] = new SelectList(_context.Set<AppUser>(), "Id", "Id");
            return View();
        }

        // POST: TextMessages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Text,IsEdited,TimeOfSending,SenderUserId,ParentId,Id")] TextMessage textMessage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(textMessage);
                textMessage.SenderUser.MessagesWritten++;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SenderUserId"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", textMessage.SenderUserId);
            return View(textMessage);
        }

        // GET: TextMessages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var textMessage = await _context.TextMessages.FindAsync(id);
            if (textMessage == null)
            {
                return NotFound();
            }
            ViewData["SenderUserId"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", textMessage.SenderUserId);
            return View(textMessage);
        }

        // POST: TextMessages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Text,IsEdited,TimeOfSending,SenderUserId,ParentId,Id")] TextMessage textMessage)
        {
            if (id != textMessage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(textMessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TextMessageExists(textMessage.Id))
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
            ViewData["SenderUserId"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", textMessage.SenderUserId);
            return View(textMessage);
        }

        // GET: TextMessages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var textMessage = await _context.TextMessages
                .Include(t => t.SenderUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (textMessage == null)
            {
                return NotFound();
            }

            return View(textMessage);
        }

        // POST: TextMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var textMessage = await _context.TextMessages.FindAsync(id);
            if (textMessage != null)
            {
                _context.TextMessages.Remove(textMessage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TextMessageExists(int id)
        {
            return _context.TextMessages.Any(e => e.Id == id);
        }*/
    }
}
