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
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext context;

        public CartRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Cart> CreateAsync(Cart Request)
        {
            await context.AddAsync(Request);
            await context.SaveChangesAsync();
            return Request;
        }

        public async Task<List<Cart>> GetUserCartAsync(string userId)
        {
            return await context.Carts
                .Where(c => c.userId == userId)
                .Include(c => c.Product)
                .ThenInclude(c => c.Translations)
                .ToListAsync();
         }
        public async Task<Cart?> GetCartItemAsync(string userId , int productId)
        {
            return await context.Carts.Include(c => c.Product)
             .FirstOrDefaultAsync(c => c.userId == userId && c.ProductId == productId);
        }
        public async Task<Cart> UpdateAsync(Cart cart)
        {
            context.Carts.Update(cart);
            await context.SaveChangesAsync();
            return cart;
        }

        public async Task ClearCartAsync(string userId)
        {
            var items = await context.Carts.Where(c => c.userId == userId).ToListAsync();
            context.Carts.RemoveRange(items);
            await context.SaveChangesAsync();
        }
        
    }
}
