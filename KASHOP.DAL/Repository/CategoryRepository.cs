using KASHOP.DAL.Data;
using KASHOP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext context;

        public CategoryRepository(ApplicationDbContext context)
           {
            this.context = context;
        }

        public Category Create(Category Request)
        {
            context.Add(Request);
            context.SaveChanges();
            return Request;
         }

        public List<Category> GetAll()
        {
            return context.Categories.Include(c => c.Translations).ToList();

        }
    }
}
