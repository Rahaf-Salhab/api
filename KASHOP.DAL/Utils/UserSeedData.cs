using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Utils
{
    public class UserSeedData : ISeedData
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserSeedData(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task DataSeed()
         {
            if (! await userManager.Users.AnyAsync())
            {
                var user1 = new ApplicationUser
                {
                    UserName = "rsalhab",
                    Email = "r@gmail.com",
                    FullName = "Rahaf Salhab",
                    EmailConfirmed = true,

                };
                var user2 = new ApplicationUser
                {
                    UserName = "asalhab",
                    Email = "a@gmail.com",
                    FullName = "Amal Salhab",
                    EmailConfirmed = true,

                };
                var user3 = new ApplicationUser
                {
                    UserName = "bsalhab",
                    Email = "b@gmail.com",
                    FullName = "Bahaa Salhab",
                    EmailConfirmed = true,

                };

                await userManager.CreateAsync(user1 , "Pass@1199");
                await userManager.CreateAsync(user2, "Pass@1199");
                await userManager.CreateAsync(user3, "Pass@1199");

                await userManager.AddToRoleAsync(user1 , "SuperAdmin");
                await userManager.AddToRoleAsync(user2, "Admin");
                await userManager.AddToRoleAsync(user3, "User");


            }
        }
    }
}
