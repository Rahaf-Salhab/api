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
    public class ProductRepository : IproductRepository
    {
        private readonly ApplicationDbContext context;

        public ProductRepository(ApplicationDbContext context) 
        {
            this.context = context;
        }
        public async Task<List<Product>> GetAllAsync()
        {
            return await context.Products.Include(c => c.Translations).Include(c => c.User)
                .ToListAsync();

        }
        public async Task<Product> AddAsync(Product request)
        {
           await context.AddAsync(request);
            await context.SaveChangesAsync();
            return request;
        
        }
    }
}
