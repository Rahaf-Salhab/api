using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.Models;
using KASHOP.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KASHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly IStringLocalizer<SharedResource> localizer;

        public CategoriesController(ICategoryService categoryService, IStringLocalizer<SharedResource> localizer)
        {
            this.categoryService = categoryService;
            this.localizer = localizer;
        }
        [HttpGet("")]
        public async Task< IActionResult> Index()
        {
            var response = await categoryService.GetAllCategoriesForAdmin();
            return Ok(new { message = localizer["Success"].Value, response });
        }
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] CategoryRequest request)
        {
   
            var response = await categoryService.CreateCategory(request);
            return Ok(new { message = localizer["Success"].Value  });
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id ,[FromBody] CategoryRequest request) 
        { 
               var result = await categoryService.UpdateCategoryAsync(id, request);
            if (!result.Success)
            {
                if (result.Message.Contains("Not Found"))
                {
                    return NotFound(result);
                }
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPatch("toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus (int id)
        {
            var result = await categoryService.ToggleStatus(id);
            if (!result.Success)
            {
                if (result.Message.Contains("Not Found"))
                {
                    return NotFound(result);
                }
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id) 
        {
              var result = await categoryService.DeleteCategoryAsync(id);
            if (!result.Success) 
            {
              if(result.Message.Contains("Not Found"))
                {
                    return NotFound(result);
                }
               return BadRequest();
            }
            return Ok(result);
        }

    }
}
