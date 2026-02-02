using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using KASHOP.PL.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KASHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService ProductService;
        private readonly IStringLocalizer<SharedResource> localizer;

        public ProductsController(IProductService ProductService, IStringLocalizer<SharedResource> localizer)
        {
            this.ProductService = ProductService;
            this.localizer = localizer;
        }
        [HttpGet("")]
        public IActionResult Index([FromQuery] string lang = "en" ,[FromQuery] int page =1 ,[FromQuery] int limit =3 ,
            [FromQuery] string? search = null)
        {
            var response = ProductService.GetAllProductsForUser(lang , page , limit , search);
            return Ok(new { message = "Success", response = response.Result });
        }

        [HttpGet("{id}")]
        public IActionResult Index([FromRoute] int id,[FromQuery] string lang = "en")
        {
            var response = ProductService.GetAllProductsDetailsForUser(id,lang);
            return Ok(new { message = "Success", response = response.Result });
        }
    }
}
