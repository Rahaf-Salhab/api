using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await orderRepository.GetOrderByIdAsync(orderId);
         }

        public async Task<List<OrderResponse>> GetOrdersAsync(OrderStatusEnum status)
        {
            var orders = await orderRepository.GetOrderByStatusAsync(status);
            return orders.Adapt<List<OrderResponse>>();
               
         }

        public async Task<BaseResponse> UpdateOrderStatusAsync(int orderId, OrderStatusEnum newStatus)
        {
            var order = await orderRepository.GetOrderByIdAsync(orderId);
            order.OrderStatus = newStatus;
            if(newStatus == OrderStatusEnum.Delivered)
            {
                order.PaymentStatus = PaymentStatusEnum.Paid;
            } else if (newStatus == OrderStatusEnum.Cancelled)
            {
                if (order.OrderStatus == OrderStatusEnum.Shipped) 
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Can't cancelled this order"
                    };
                }
            }
            await orderRepository.UpdateAsync(order);
            return new BaseResponse
            {
                Success = true,
                Message = "order status updated"
            };
         }
    }
}
