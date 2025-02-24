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
    public static class AdminSeeder
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            var adminuser = new AppUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                EmailConfirmed = true
            };
            string password = "Admin#123";
            var user = await userManager.FindByEmailAsync(adminuser.Email);
            if (user == null)
            {
                var created = await userManager
                    .CreateAsync(adminuser, password);
                if (created.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminuser, "Admin");
                }
            }
        }
    }
}
