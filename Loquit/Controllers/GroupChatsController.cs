using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Loquit.Data;
using Loquit.Data.Entities.ChatTypes;

namespace Loquit.Web.Controllers
{
    public class GroupChatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupChatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GroupChats
        public async Task<IActionResult> Index()
        {
            return View(await _context.GroupChats.ToListAsync());
        }

        // GET: GroupChats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupChat = await _context.GroupChats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groupChat == null)
            {
                return NotFound();
            }

            return View(groupChat);
        }

        // GET: GroupChats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GroupChats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DateOfCreation,PictureUrl,Id")] GroupChat groupChat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupChat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(groupChat);
        }

        // GET: GroupChats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupChat = await _context.GroupChats.FindAsync(id);
            if (groupChat == null)
            {
                return NotFound();
            }
            return View(groupChat);
        }

        // POST: GroupChats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DateOfCreation,PictureUrl,Id")] GroupChat groupChat)
        {
            if (id != groupChat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupChat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupChatExists(groupChat.Id))
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
            return View(groupChat);
        }

        // GET: GroupChats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupChat = await _context.GroupChats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groupChat == null)
            {
                return NotFound();
            }

            return View(groupChat);
        }

        // POST: GroupChats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupChat = await _context.GroupChats.FindAsync(id);
            if (groupChat != null)
            {
                _context.GroupChats.Remove(groupChat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupChatExists(int id)
        {
            return _context.GroupChats.Any(e => e.Id == id);
        }
    }
}
