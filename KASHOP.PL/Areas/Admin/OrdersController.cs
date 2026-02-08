using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.Models;
using KASHOP.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KASHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IStringLocalizer<SharedResource> localizer;

        public OrdersController(IOrderService orderService, IStringLocalizer<SharedResource> localizer)
        {
            this.orderService = orderService;
            this.localizer = localizer;

        }
        [HttpGet("")]
        public async Task<IActionResult> GetOrders([FromQuery] OrderStatusEnum status = OrderStatusEnum.Pending)
        {
            var orders = await orderService.GetOrdersAsync(status);
            return Ok(orders);

        }
        [HttpPatch("{orderId}")]
        public async Task<IActionResult> UpdateStatus([FromRoute]int orderId , [FromBody] UpdateOrderStatusRequest request)
        {
            var result = await orderService.UpdateOrderStatusAsync(orderId, request.Status);
            if (! result.Success) 
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
