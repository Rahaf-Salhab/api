using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KASHOP.PL.Areas.Identity
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Register(LoginRequest request)
        {
            var result = await authenticationService.LoginAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await authenticationService.RegisterAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("ConfirmEmail")]

        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            var result = await authenticationService.ConfirmEmailAsync(token, userId);

            return Ok(result);
        }
        [HttpPost("SendCode")]
        public async Task<IActionResult> RequestPasswordReset(ForgotPasswordRequest request)
        {
            var result = await authenticationService.RequestPasswordReset(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
             return Ok(result);

        }


        [HttpPatch("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var result = await authenticationService.ResetPassword(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }
    }
}
