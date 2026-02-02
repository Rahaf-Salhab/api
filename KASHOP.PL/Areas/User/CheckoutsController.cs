using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System.Security.Claims;

namespace KASHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService checkoutService;

        public CheckoutsController(ICheckoutService checkoutService) 
        {
            this.checkoutService = checkoutService;
        }
        [HttpPost("")]
        public async Task<IActionResult> Payment([FromBody] CheckoutRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await checkoutService.ProcessPaymentAsync(request , userId);
            if (! response.Success)
            {
                return BadRequest(response);    
            }
            return Ok(response);
        }

        [HttpGet("success")]
        [AllowAnonymous]
        public async Task<IActionResult> Success([FromQuery] string session_id)
        {
            var response = await checkoutService.HandleSuccessAsync(session_id);
          

            if (! response.Success) return BadRequest(response);
            
            return Ok(response);
          

            
        }
    }
}
