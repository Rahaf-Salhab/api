using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles =("Admin"))]
    public class ManagesController : ControllerBase
    {
        private readonly IManageUserService manageUser;

        public ManagesController(IManageUserService manageUser)
        {
            this.manageUser = manageUser;
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await manageUser.GetUsersAsync();
            return Ok(result);
        }
        [HttpPatch("block/{id}")]
        public async Task<IActionResult> BlockUser([FromRoute] string id)
        {
            var result = await manageUser.BlockedUserAsync(id);
            return Ok(result);
        }

        [HttpPatch("unblock/{id}")]
        public async Task<IActionResult> UnBlockUser([FromRoute] string id)
        {
            var result = await manageUser.UnBlockedUserAsync(id);
            return Ok(result);
        }

        [HttpPatch("change-role")]
        [Authorize(Roles = ("superAdmin"))]

        public async Task<IActionResult> ChangeRole(ChangeUserRoleRequest request)
        {
            var result = await manageUser.ChangeUserRoleAsync(request);
            return Ok(result);
        }

    }
}
