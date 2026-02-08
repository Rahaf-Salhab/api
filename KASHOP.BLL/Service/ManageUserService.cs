using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class ManageUserService : IManageUserService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ManageUserService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<List<UserListResponse>> GetUsersAsync()
        {
            var users = await userManager.Users.ToListAsync();
            var result = users.Adapt<List<UserListResponse>>();
            for (int i =0; i < users.Count; i++)
            {
                var roles = await userManager.GetRolesAsync(users[i]);
                result[i].Roles = roles.ToList();
            }
             return result;
        }
        public Task<UserDetailsResponse> GetUserDetailsAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<BaseResponse> BlockedUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            await userManager.SetLockoutEnabledAsync(user , true);
            await userManager.SetLockoutEndDateAsync(user , DateTimeOffset.MaxValue);

            await userManager.UpdateAsync(user);
            return new BaseResponse
            {
                Success = true,
                Message = "user blocked"
            };
         }
        public async Task<BaseResponse> UnBlockedUserAsync(string userId)
        {
            var user =  await userManager.FindByIdAsync(userId);
            await userManager.SetLockoutEnabledAsync(user, false);
            await userManager.SetLockoutEndDateAsync(user, null);

            await userManager.UpdateAsync(user);
            return new BaseResponse
            {
                Success = true,
                Message = "user unblocked"
            };

        }

        public async Task<BaseResponse> ChangeUserRoleAsync(ChangeUserRoleRequest request)
        {
            var user = await userManager.FindByIdAsync(request.UserId);   
            var currentRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, currentRoles);
            await userManager.AddToRoleAsync(user , request.Role);

            return new BaseResponse
            {
                Success = true,
                Message = "role updated"
            };
        }
    }
}
