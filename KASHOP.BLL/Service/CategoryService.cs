using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using KASHOP.DAL.DTO.Request;

namespace KASHOP.BLL.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public CategoryResponse CreateCategory(CategoryRequest Request)
        {
            var category = Request.Adapt<Category>();
            categoryRepository.Create(category);
            return category.Adapt<CategoryResponse>();
         }

        public List<CategoryResponse> GetAllCategories()
        {

            var categories = categoryRepository.GetAll();
            var response = categories.Adapt<List<CategoryResponse>>();
            return response;

        }
    }
}
