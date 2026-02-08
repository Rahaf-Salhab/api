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
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext context;

        public ReviewRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<bool> HasUserReviewProduct(string userId, int productId)
        {
            return await context.Reviews
                .AnyAsync(r => r.UserId == userId && r.ProductId == productId);
         }
        public async Task<Review> CreateAsync(Review Request)
        {
            await context.AddAsync(Request);
            await context.SaveChangesAsync();
            return Request;
        }
    }
}
