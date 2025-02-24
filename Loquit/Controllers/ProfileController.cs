using Loquit.Data.Entities;
using Loquit.Services.DTOs;
using Loquit.Services.Services.Abstractions;
using Loquit.Utils;
using Loquit.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ProfileController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IWebHostEnvironment _environment;
    private readonly IUserService _userService;

    public ProfileController(UserManager<AppUser> userManager, IWebHostEnvironment environment, IUserService userService)
    {
        _userManager = userManager;
        _environment = environment;
        _userService = userService;
    }

    [Route("Profile/{username}")]
    public async Task<IActionResult> Index(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return NotFound();
        }

        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [Route("/Profile/Friends")]
    public async Task<IActionResult> Friends()
    {

        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            return NotFound();
        }

        return View(currentUser);
    }

    [Route("Profile/Edit/{username}")]
    public async Task<IActionResult> Edit(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return NotFound();
        }

        var user = await _userManager.FindByNameAsync(username);

        var model = new UserEditViewModel()
        {
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            ColorThemeId = user.ColorThemeId,
            Description = user.Description,
            DateOfBirth = user.DateOfBirth,
            ProfilePictureUrl = user.ProfilePictureUrl,
            AllowNsfw = user.AllowNsfw
    };

        return View(model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Profile/EditUser/{username}")]
    public async Task<IActionResult> EditUser(string username, UserEditViewModel user)
    {
        if (username != user.UserName)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var userToUpdate = await _userManager.FindByNameAsync(username);

            if (userToUpdate == null)
            {
                return NotFound();
            }
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Email = user.Email;
            userToUpdate.ColorThemeId = user.ColorThemeId;
            userToUpdate.Description = user.Description;
            userToUpdate.DateOfBirth = user.DateOfBirth;
            userToUpdate.AllowNsfw = user.AllowNsfw;

            if (user.Picture != null && user.Picture.Length > 0)
            {
                var newFileName = await FileUpload.UploadAsync(user.Picture, _environment.WebRootPath);
                userToUpdate.ProfilePictureUrl = newFileName;
            }

            try
            {
                await _userManager.UpdateAsync(userToUpdate);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(username))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            try
            {
                await _userManager.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(username))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index", "Profile", new { username = username });
        }
        return View(user);
    }

    [HttpGet("/Profile/SendFriendRequest/{id}")]
    public async Task<IActionResult> SendFriendRequest(string id)
    {
        var userId = (await _userManager.GetUserAsync(User)).Id;

        try
        {
            string result = await _userService.SendFriendRequest(userId, id);
            return Json(new { success = true, result = result });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }

    [HttpGet("/Profile/DeclineFriendRequest/{id}")]
    public async Task<IActionResult> DeclineFriendRequest(string id)
    {
        var userId = (await _userManager.GetUserAsync(User)).Id;

        try
        {
            string result = await _userService.DeclineFriendRequest(id, userId);
            return Json(new { success = true, result = result });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }

    [HttpGet("/Profile/AcceptFriendRequest/{id}")]
    public async Task<IActionResult> AcceptFriendRequest(string id)
    {
        var userId = (await _userManager.GetUserAsync(User)).Id;

        try
        {
            string result = await _userService.AcceptFriendRequest(id, userId);
            return Json(new { success = true, result = result });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }

    [HttpGet("/Profile/CancelFriendRequest/{id}")]
    public async Task<IActionResult> CancelFriendRequest(string id)
    {
        var userId = (await _userManager.GetUserAsync(User)).Id;

        try
        {
            string result = await _userService.CancelFriendRequest(userId, id);
            return Json(new { success = true, result = result });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }

    [HttpGet("/Profile/RemoveFriend/{id}")]
    public async Task<IActionResult> RemoveFriend(string id)
    {
        var userId = (await _userManager.GetUserAsync(User)).Id;

        try
        {
            string result = await _userService.RemoveFriend(userId, id);
            return Json(new { success = true, result = result });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }

    private async Task<bool> UserExists(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        return user != null;
    }


    [HttpGet("/Profile/ReloadPartialProfile/{id}")]
    public async Task<IActionResult> ReloadPartialProfile(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        return PartialView("PartialViewProfile", user);
    }
    [HttpGet("/Profile/ReloadPartialFriends")]
    [HttpGet("/Profile/ReloadPartialFriends/{username}")]
    public async Task<IActionResult> ReloadPartialFriends(string username = "")
    {
        return PartialView("PartialViewFriends", username);
    }
    [HttpGet("/Profile/ReloadPartialFriendRequests")]
    [HttpGet("/Profile/ReloadPartialFriendRequests/{username}")]
    public async Task<IActionResult> ReloadPartialFriendRequests(string username = "")
    {
        return PartialView("PartialViewFriendRequests", username);
    }
}