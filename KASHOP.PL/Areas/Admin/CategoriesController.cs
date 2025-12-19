using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using KASHOP.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KASHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService category;
        private readonly IStringLocalizer<SharedResource> localizer;

        public CategoriesController(ICategoryService category, IStringLocalizer<SharedResource> localizer)
        {
            this.category = category;
            this.localizer = localizer;
        }
        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            var response = category.CreateCategory(request);
            return Ok(new { message = localizer["Success"].Value });
        }
    }
}
