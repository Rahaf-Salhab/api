using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using KASHOP.PL.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace KASHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService category;
        private readonly IStringLocalizer<SharedResource> localizer;

        public CategoriesController(ICategoryService category , IStringLocalizer<SharedResource>localizer )
        {
            this.category = category;
            this.localizer = localizer;
        }
        [HttpGet("")]
        public IActionResult Index([FromQuery] string lang = "en")
        {
            var response = category.GetAllCategoriesForUser(lang);
            return Ok(new {message= localizer["Success"].Value, response});
        }
        [HttpPost("")]
        public async Task<IActionResult> Create(CategoryRequest request)
        {
            var response = await category.GetAllCategoriesForUser();
            return Ok(new { message = localizer["Success"].Value , response});
        }
    }
}

