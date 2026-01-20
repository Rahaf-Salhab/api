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

        public async Task<Category> CreateAsync(Category Request)
        {
           await context.AddAsync(Request);
           await  context.SaveChangesAsync();
            return Request;
         }

        public async Task<List<Category>> GetAllAsync()
        {
            return await context.Categories.Include(c => c.Translations).Include(c=>c.User)
                .ToListAsync();

        }
      public async Task<Category?> FindByIdAsync(int id)
        {
            return await context.Categories.Include(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task DeleteAsync(Category category)
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
        }
        public async Task<Category?> UpdateAsync(Category category)
        {
            context.Categories.Update(category);
            await context.SaveChangesAsync();
            return category;
        }

    }
}
