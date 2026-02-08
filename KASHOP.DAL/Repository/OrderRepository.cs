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
        public async Task<List<Order>> GetOrderByStatusAsync(OrderStatusEnum status)
        {
            return await context.Orders
                .Where(o => o.OrderStatus == status)
                .Include(o => o.User)
                 .ToListAsync();
         }
        public async Task<bool> HasUserDeliveredOrderForProduct(string userId, int productId)
        {
            return await context.Orders
                .Where(o => o.UserId == userId && o.OrderStatus == OrderStatusEnum.Delivered)
                .SelectMany(o => o.OrderItems)
                .AnyAsync(oi => oi.ProductId  == productId);
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
 
        }

        
    }
}
