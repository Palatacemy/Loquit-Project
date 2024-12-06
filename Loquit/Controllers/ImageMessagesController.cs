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

namespace Loquit.Web.Controllers
{
    public class ImageMessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImageMessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ImageMessages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ImageMessages.Include(i => i.SenderUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ImageMessages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageMessage = await _context.ImageMessages
                .Include(i => i.SenderUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imageMessage == null)
            {
                return NotFound();
            }

            return View(imageMessage);
        }

        // GET: ImageMessages/Create
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_context.Set<BaseMessage>(), "Id", "Discriminator");
            ViewData["SenderUserId"] = new SelectList(_context.Set<AppUser>(), "Id", "Id");
            return View();
        }

        // POST: ImageMessages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PictureUrl,TimeOfSending,SenderUserId,ParentId,Id")] ImageMessage imageMessage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(imageMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SenderUserId"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", imageMessage.SenderUserId);
            return View(imageMessage);
        }

        // GET: ImageMessages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageMessage = await _context.ImageMessages.FindAsync(id);
            if (imageMessage == null)
            {
                return NotFound();
            }
            ViewData["SenderUserId"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", imageMessage.SenderUserId);
            return View(imageMessage);
        }

        // POST: ImageMessages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PictureUrl,TimeOfSending,SenderUserId,ParentId,Id")] ImageMessage imageMessage)
        {
            if (id != imageMessage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imageMessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageMessageExists(imageMessage.Id))
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
            ViewData["SenderUserId"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", imageMessage.SenderUserId);
            return View(imageMessage);
        }

        // GET: ImageMessages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageMessage = await _context.ImageMessages
                .Include(i => i.SenderUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imageMessage == null)
            {
                return NotFound();
            }

            return View(imageMessage);
        }

        // POST: ImageMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imageMessage = await _context.ImageMessages.FindAsync(id);
            if (imageMessage != null)
            {
                _context.ImageMessages.Remove(imageMessage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageMessageExists(int id)
        {
            return _context.ImageMessages.Any(e => e.Id == id);
        }
    }
}
