using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Utils
{
    public class RoleSeedData : ISeedData
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleSeedData(RoleManager <IdentityRole> roleManager) 
        {
            this.roleManager = roleManager;
        }
        public async Task DataSeed()
        {
            string[] roles = ["SuperAdmin", "Admin", "User"];
            if (! await roleManager.Roles.AnyAsync())
            {
                foreach (var role in roles) {
                  await  roleManager.CreateAsync(new IdentityRole(role));
                }
            }
         }
    }
}
