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
        public async Task<Product?> FindByIdAsync(int id)
        {
            return await context.Products.Include(c => c.Translations)
                .Include(c => c.SubImages)
                .Include(c => c.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        //put products in server Ram
        //AsQueryable() : store in server ram vs ToList() : store in user ram
        //we use this in pagination , filter ,...
        public IQueryable<Product> Query()
        {
            return context.Products.Include(p => p.Translations)
                .AsQueryable();
        }

        public async Task<bool> DecreaseQuantitiesAsync(List<(int productId, int quantity)> items)
        {
            var productIds = items.Select(p => p.productId).ToList();
            var products = await context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            foreach (var product in products)
            {
                var item = items.First(p => p.productId == product.Id);

                if (product.Quantity < item.quantity)
                {
                    return false;
                }

                product.Quantity -= item.quantity;
            }

            await context.SaveChangesAsync();
            return true;
        }

    }
}
