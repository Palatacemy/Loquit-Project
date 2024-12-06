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
    public class DirectChatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DirectChatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DirectChats
        public async Task<IActionResult> Index()
        {
            return View(await _context.DirectChats.ToListAsync());
        }

        // GET: DirectChats/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: DirectChats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DirectChats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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
        }

        private bool DirectChatExists(int id)
        {
            return _context.DirectChats.Any(e => e.Id == id);
        }
    }
}
