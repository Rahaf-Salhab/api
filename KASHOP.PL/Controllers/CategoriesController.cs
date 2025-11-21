using Azure;
using KASHOP.DAL.Data;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using KASHOP.PL.Resources;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IStringLocalizer<SharedResource> localizer;

        public CategoriesController(ApplicationDbContext context, IStringLocalizer<SharedResource> localizer) 
        {
            this.context = context;
            this.localizer = localizer;
        }
        [HttpGet("")]

        public IActionResult index()
        {
            var categories = context.Categories.Include(c => c.Translations).ToList();
            var response = categories.Adapt<List<CategoryResponse>>();
            return Ok(new {message= localizer["Success"].Value , response });
        }
        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            context.Add(category);
            context.SaveChanges();
            return Ok(new { message = localizer["Success"].Value});
        }
        
    }
}
