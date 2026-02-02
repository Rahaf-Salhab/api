using KASHOP.DAL.Data;
using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext context;

        public OrderItemRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task CreateRangeAsync(List<OrderItem> orderItems)
        {
            await context.AddRangeAsync(orderItems);
            await context.SaveChangesAsync();
         }

       
    }
}
