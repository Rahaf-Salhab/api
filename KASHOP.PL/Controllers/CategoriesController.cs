using Azure;
using KASHOP.BLL.Service;
using KASHOP.DAL.Data;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
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
        private readonly ICategoryService categoryService;

        public CategoriesController(IStringLocalizer<SharedResource> localizer , ICategoryService categoryService) 
        {
            this.context = context;
            this.localizer = localizer;
            this.categoryService = categoryService;
        }
        [HttpGet("")]

        public IActionResult index()
        {
            var response = categoryService.GetAllCategories();
             return Ok(new {message= localizer["Success"].Value , response });
        }
        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
       
            var response = categoryService.CreateCategory(request);
            return Ok(new { message = localizer["Success"].Value});
        }
        
    }
}
