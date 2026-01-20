using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Identity;
 
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
            await CreateUserIfNotExists(
                "rsalhab",
                "r@gmail.com",
                "Rahaf Salhab",
                "SuperAdmin");

            await CreateUserIfNotExists(
                "asalhab",
                "a@gmail.com",
                "Amal Salhab",
                "Admin");

            await CreateUserIfNotExists(
                "bsalhab",
                "b@gmail.com",
                "Bahaa Salhab",
                "User");
        }

        private async Task CreateUserIfNotExists(
            string userName,
            string email,
            string fullName,
            string role)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = userName,
                    Email = email,
                    FullName = fullName,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, "Pass@1199");
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}

