using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Loquit.Data;
using Loquit.Data.Entities;
using Loquit.Utils;
using Microsoft.Extensions.Hosting;
using Loquit.Services.Abstractions;
using Loquit.Services.DTOs;
using Microsoft.AspNetCore.Identity;
using Loquit.Services.Services;

namespace Loquit.Web.Controllers
{

    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<AppUser> _userManager;

        public CommentsController(ICommentService commentService, UserManager<AppUser> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        // GET: Comments
        /*public async Task<IActionResult> Index()
        {
            return View(await _context.Comments.ToListAsync());
        }*/

        // GET: Comments/Details/5
        /*public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }*/

        // GET: Comments/Create

        [HttpGet("/Comments/Like/{id}")]
        public async Task<IActionResult> Like(int id)
        {
            var userId = (await _userManager.GetUserAsync(User)).Id;

            try
            {
                string result = await _commentService.LikeComment(id, userId);
                return Json(new { success = true, result = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("/Comments/Dislike/{id}")]
        public async Task<IActionResult> Dislike(int id)
        {
            var userId = (await _userManager.GetUserAsync(User)).Id;

            try
            {
                string result = await _commentService.DislikeComment(id, userId);
                return Json(new { success = true, result = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentDTO comment)
        {
            if (ModelState.IsValid)
            {
                comment.TimeOfCommenting = DateTime.Now;
                comment.Commenter = await _userManager.GetUserAsync(User);
                await _commentService.AddCommentAsync(comment);
                return RedirectToAction("Details", "Posts", new { id = comment.PostId });
            }
            return View(comment);
        }

        // GET: Comments/Edit/5
        /*public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }*/

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Text,TimeOfCommenting,IsEdited,Id")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
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
            return View(comment);
        }
        */
        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _commentService.GetCommentByIdAsync(id.Value);
            if (post == null)
            {
                return NotFound();
            }
            await _commentService.DeleteCommentByIdAsync(post.Id);
            return RedirectToAction("Details", "Posts", new { id = post.PostId});
        }
        /*
        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }*/
    }
}
