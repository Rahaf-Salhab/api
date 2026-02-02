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
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;

        public  OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Order> CreateAsync(Order Request)
        {
            await context.AddAsync(Request);
            await context.SaveChangesAsync();
            return Request;
        }

        public async Task<Order> GetBySessionIdAsync(string sessionId)
        {
            return await context.Orders.FirstOrDefaultAsync(o => o.SessionId == sessionId);
         }
        public async Task<Order> UpdateAsync(Order order)
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
            return order;
        }
    }
}
