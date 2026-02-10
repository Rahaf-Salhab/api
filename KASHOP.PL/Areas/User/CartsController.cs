using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.Repository;
using KASHOP.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace KASHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartService cartService;
        private readonly IStringLocalizer<SharedResource> localizer;

        public CartsController(ICartService cartService, IStringLocalizer<SharedResource> localizer)
        {
            this.cartService = cartService;
            this.localizer = localizer;
        }
        [Authorize]

        [HttpPost("")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await cartService.AddToCartAsync(userId, request);
            return Ok(result);
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await cartService.GetUserCartAsync(userId);
            return Ok(result);

        }

        [HttpPatch("{productId}")]
        public async Task<IActionResult> UpdateQuantity([FromRoute] int productId ,[FromBody] UpdateQuantityRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await cartService.UpdateQuantityAsync(userId , productId , request.Count);
            if (! result.Success) return BadRequest(result);
            return Ok(result);

        }


        [HttpDelete("")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await cartService.ClearCartAsync(userId);
            return Ok(result);

        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteItemFromCart([FromRoute] int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await cartService.RemoveFromCartAsync(userId , productId);
            return Ok(result);

        }
    }
}
