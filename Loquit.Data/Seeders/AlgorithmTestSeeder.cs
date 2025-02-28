using Loquit.Data.Entities;
using Loquit.Data.Repositories;
using Loquit.Data.Repositories.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Loquit.Data.Seeders
{
    public static class AlgorithmTestSeeder
    {
        public static async Task Initialize(IServiceProvider serviceProvider, bool i)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var postRepository = serviceProvider.GetRequiredService<IPostRepository>();

            var algorhytmtestuser = new AppUser
            {
                UserName = "algo@algo.com",
                Email = "algo@algo.com",
                EmailConfirmed = true
            };

            string password = "algoPassword@12345";
            var user = await userManager.FindByEmailAsync(algorhytmtestuser.Email);
            if (user == null)
            {
                var created = await userManager
                    .CreateAsync(algorhytmtestuser, password);
                if (created.Succeeded)
                {
                    await userManager.AddToRoleAsync(algorhytmtestuser, "Admin");
                }
            }
            //
            int r = 60;
            string k = "";
            if (i)
            {
                for (int j = 1; j <= r; j++)
                {
                    bool d1 = false;
                    bool d2 = false;
                    if (j % 10 == 1)
                    {
                        d1 = true;
                    }
                    if (j % 12 == 1)
                    {
                        d2 = true;
                    }
                    k = j.ToString();
                    var testpost = new Post
                    {
                        Title = $"[$Test #{k}$] ",
                        BodyText = "booodyyyy",
                        CreatorId = userManager.FindByEmailAsync(algorhytmtestuser.Email).Id.ToString(),
                        Category = GetStringByNumber(j % 9),
                        IsNsfw = d1,
                        IsSpoiler = d2
                    };
                    if (await postRepository.GetByIdAsync(testpost.Id) == null)
                    {
                        await postRepository.AddAsync(testpost);
                    }
                }
            }
            //
            else
            {
                for (int j = 1; j <= r; j++)
                {
                    k = j.ToString();
                    var post = await postRepository.GetAsync(item => item.Title.Contains($"[$Test #{k}$]"));
                    if (post != null)
                    {
                        try
                        {
                            await postRepository.DeleteByIdAsync(post.First().Id);
                        }
                        catch (Exception)
                        {
                            
                        }
                    }
                }
            }
        }
        private static string GetStringByNumber(int input)
        {
            switch (input)
            {
                case 1:
                    return "Food";
                case 2:
                    return "Music/Art";
                case 3:
                    return "Nature";
                case 4:
                    return "Pets";
                case 5:
                    return "Videogames";
                case 6:
                    return "Culture";
                case 7:
                    return "Funny";
                case 8:
                    return "Factology";
                default:
                    return "None";
            }
        }
    }
}
