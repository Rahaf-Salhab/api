using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService ProductService;
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly IReviewService reviewService;

        public ProductsController(IProductService ProductService,
            IStringLocalizer<SharedResource> localizer ,
            IReviewService reviewService)
        {
            this.ProductService = ProductService;
            this.localizer = localizer;
            this.reviewService = reviewService;
        }
        [HttpGet("")]
        public IActionResult Index([FromQuery] string lang = "en" ,[FromQuery] int page =1 ,[FromQuery] int limit =3 ,
            [FromQuery] string? search = null , [FromQuery] int? categoryId = null ,
            [FromQuery] decimal? minPrice = null , [FromQuery] decimal? maxPrice = null ,
             string? sortBy = null, bool asc = true)
        {
            var response = ProductService.GetAllProductsForUser(lang , page , limit , search ,
                categoryId , minPrice , maxPrice , sortBy , asc);
            return Ok(new { message = "Success", response = response.Result });
        }

        [HttpGet("{id}")]
        public IActionResult Index([FromRoute] int id,[FromQuery] string lang = "en")
        {
            var response = ProductService.GetAllProductsDetailsForUser(id,lang);
            return Ok(new { message = "Success", response = response.Result });
        }
        [Authorize]
        [HttpPost("{productId}/reviews")]
        public async Task<IActionResult> AddReview([FromRoute] int productId , [FromBody] CreateReviewRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await reviewService.AddReviewAsync(userId,productId ,request);

            if (!response.Success) return BadRequest(new { message = response.Message });
            return Ok(new { message = response.Message });

        }
    }
}
