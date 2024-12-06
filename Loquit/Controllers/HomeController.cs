using Loquit.Models;
using Loquit.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Loquit.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;

        public HomeController(IPostService postService, ILogger<HomeController> logger)
        {
            _logger = logger;
            _postService = postService;
        }

        public async Task<IActionResult> Index(string searchQuery)
        {
            if (!string.IsNullOrEmpty(searchQuery))
            {
                return View(await _postService.GetPostsWithTitleAsync(searchQuery));
            }
            else
            {
                return View(await _postService.GetPostsAsync());
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
