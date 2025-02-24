using Loquit.Data.Entities;
using Loquit.Models;
using Microsoft.AspNetCore.Identity;
using Loquit.Services.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Loquit.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(IPostService postService, ILogger<HomeController> logger, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _postService = postService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string searchQuery)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (!string.IsNullOrEmpty(searchQuery))
            {
                return View(await _postService.GetPostsWithTitleAsync(searchQuery));
            }
            else
            {
                if (currentUser != null)
                {
                    return View(await _postService.GetPostsByAlgorithmAsync(currentUser.AllowNsfw, currentUser.CategoryPreferences, currentUser.EvaluationPreferences, currentUser.RecentlyOpenedPostsIds));
                }
                else
                {
                    return View(await _postService.GetPostsAsync());
                }
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public void Like(int id)
        {

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
