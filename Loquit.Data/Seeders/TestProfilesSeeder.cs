using Loquit.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Seeders
{
    public static class TestProfilesSeeder
    {
        public static async Task Initialize(IServiceProvider serviceProvider, int i)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            string num = "";
            string password = "";
            for (int j = 1; j <= i; j++)
            {
                num = j.ToString();
                var testuser = new AppUser
                {
                    UserName = $"TestUser{num}",
                    Email = $"Test{num}@test.com",
                    EmailConfirmed = true
                };
                password = $"TestUser#{num}";
                var user = await userManager.FindByEmailAsync(testuser.Email);
                if (user == null)
                {
                    var created = await userManager
                        .CreateAsync(testuser, password);
                    if (created.Succeeded)
                    {
                        await userManager.AddToRoleAsync(testuser, "Admin");
                    }
                }
            }
        }
    }
}
