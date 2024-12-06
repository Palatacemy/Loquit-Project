using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Loquit.Data;
using Loquit.Data.Entities;
using Loquit.Web.Models;
using Loquit.Utils;
using Microsoft.Extensions.Hosting;
using Loquit.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Loquit.Services.DTOs;

namespace Loquit.Web.Controllers
{

    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public PostsController(IPostService postService, IWebHostEnvironment environment, ICommentService commentService, UserManager<AppUser> userManager)
        {
            _postService = postService;
            _commentService = commentService;
            _environment = environment;
            _userManager = userManager;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await _postService.GetPostsAsync());
        }

        [HttpGet("/Posts/Like/{id}")]
        public async Task<IActionResult> Like(int id)
        {
            var userId = (await _userManager.GetUserAsync(User)).Id;

            try
            {
                string result = await _postService.LikePost(id, userId);
                return Json(new { success = true , result = result});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("/Posts/Dislike/{id}")]
        public async Task<IActionResult> Dislike(int id)
        {
            var userId = (await _userManager.GetUserAsync(User)).Id;

            try
            {
                string result = await _postService.DislikePost(id, userId);
                return Json(new { success = true, result = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }


        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postService.GetPostByIdAsync(id.Value);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public async Task<IActionResult> Create()
        {
            var userId = (await _userManager.GetUserAsync(User)).Id;

            var model = new PostCreateViewModel()
            {
                CreatorId = userId
            };
            return View(model);
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostCreateViewModel post)
        {
            if (ModelState.IsValid)
            {
                if (post.Picture != null && post.Picture.Length > 0)
                {
                    var newFileName = await FileUpload.UploadAsync(post.Picture, _environment.WebRootPath);
                    post.PictureUrl = newFileName;
                }
                post.TimeOfPosting = DateTime.Now;
                await _postService.AddPostAsync(post);
                return RedirectToAction("Index", "Home");
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PostDTO post = await _postService.GetPostByIdAsync(id.Value);

            var model = new PostCreateViewModel()
            {
                Title = post.Title,
                BodyText = post.BodyText,
                PictureUrl = post.PictureUrl,
                IsSpoiler = post.IsSpoiler,
                IsNsfw = post.IsNsfw,
                CreatorId = post.CreatorId,
                TimeOfPosting = post.TimeOfPosting,
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostCreateViewModel post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (post.Picture != null && post.Picture.Length > 0)
                {
                    var newFileName = await FileUpload.UploadAsync(post.Picture, _environment.WebRootPath);
                    post.PictureUrl = newFileName;
                }
                post.IsEdited = true;
                try
                {
                    await _postService.UpdatePostAsync(post);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Posts", new { id = id });
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postService.GetPostByIdAsync(id.Value);
            if (post == null)
            {
                return NotFound();
            }
            await _postService.DeletePostByIdAsync(post.Id);
            return RedirectToAction("Index", "Home");
        }

        // POST: Posts/Delete/5
        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post != null)
            {
                await _postService.DeletePostByIdAsync(id);
            }
     
            return RedirectToAction(nameof(Index));
        }*/

        private async Task<bool> PostExists(int id)
        {
                var post = await _postService.GetPostByIdAsync(id);
                return post != null;
        }
    }
}
