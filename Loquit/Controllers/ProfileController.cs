using Loquit.Data.Entities;
using Loquit.Services.DTOs;
using Loquit.Utils;
using Loquit.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ProfileController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IWebHostEnvironment _environment;

    public ProfileController(UserManager<AppUser> userManager, IWebHostEnvironment environment)
    {
        _userManager = userManager;
        _environment = environment;
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
            ProfilePictureUrl = user.ProfilePictureUrl
            
        };

        return View(model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Profile/EditUser/{username}")]
    public async Task<IActionResult> EditUser(string username, UserEditViewModel user)
    {
        if(username != user.UserName)
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

    private async Task<bool> UserExists(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        return user != null;
    }
}