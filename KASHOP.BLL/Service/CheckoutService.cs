using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository cartRepository;
        private readonly IOrderRepository orderRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender emailSender;
        private readonly IOrderItemRepository orderItemRepository;
        private readonly IproductRepository productRepository;

        public CheckoutService(ICartRepository cartRepository , IOrderRepository orderRepository
            , UserManager<ApplicationUser> userManager, IEmailSender emailSender ,
            IOrderItemRepository orderItemRepository ,
            IproductRepository productRepository
            )
        {
            this.cartRepository = cartRepository;
            this.orderRepository = orderRepository;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.orderItemRepository = orderItemRepository;
            this.productRepository = productRepository;
        }
        
        public async Task<CheckoutResponse> ProcessPaymentAsync(CheckoutRequest request, string userId)
        {
            var cartItems = await cartRepository.GetUserCartAsync(userId);
            if (!cartItems.Any())
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Message = "cart is empty"
                };
            }
            decimal TotalAmount = 0;

            

            foreach (var cart in cartItems)

            {
                if (cart.Product.Quantity < cart.Count)
                {
                    return new CheckoutResponse
                    {
                        Success = false,
                        Message = "not enough stock"
                    };
                }
                TotalAmount += cart.Product.Price * cart.Count;
            }
            Order order = new Order
            {
                UserId = userId,
                PaymentMethod = request.PaymentMethod,
                AmountPaid = TotalAmount,
                PaymentStatus = PaymentStatusEnum.UnPaid
            };

            if (request.PaymentMethod == PaymentMethodEnum.cash)
            {
                return new CheckoutResponse
                {
                    Success = true,
                    Message = "cash"
                };
            }
            else if (request.PaymentMethod == PaymentMethodEnum.visa)
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),


                    Mode = "payment",
                    SuccessUrl = $"https://localhost:7077/api/checkouts/success?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"https://localhost:7077/checkout/cancel",
                    Metadata = new Dictionary<string, string>
                    {
                        {"UserId" , userId}
                    }
                };
                foreach (var item in cartItems)
                {
                    options.LineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Translations.FirstOrDefault(t => t.lang == "en").Name,
                            },
                            UnitAmount = (long)item.Product.Price * 100,
                        },
                        Quantity = item.Count,
                    });
                }

                    var service = new SessionService();
                    var session = service.Create(options);
                    order.SessionId = session.Id;
                    order.PaymentStatus = PaymentStatusEnum.Paid; 

                await orderRepository.CreateAsync(order);
                    return new CheckoutResponse
                    {
                        Success = true,
                        Message = "payment session created",
                        Url = session.Url

                    };
                }
            else
                {
                    return new CheckoutResponse
                    {
                        Success = false,
                        Message = "invalid payment method"
                    };
                }
            }
        
         public async Task<CheckoutResponse> HandleSuccessAsync(string sessionId)
         {
             var service = new SessionService();
             var session = service.Get(sessionId);
              var userId = session.Metadata["UserId"];

            var order = await orderRepository.GetBySessionIdAsync(sessionId);
            order.PaymentId = session.PaymentIntentId;

            order.OrderStatus = OrderStatusEnum.Approved;

            await orderRepository.UpdateAsync(order);

            var user = await userManager.FindByIdAsync(userId);

            var cartItems = await cartRepository.GetUserCartAsync(userId);
            var orderItems = new List<OrderItem>();
            var productUpdated = new List<(int productId, int quantity)>();
            foreach (var cartItem in cartItems) 
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    UnitPrice = cartItem.Product.Price,
                    Quantity = cartItem.Count,
                    TotalPrice = cartItem.Product.Price  * cartItem.Count,
                };
                orderItems.Add(orderItem);
                productUpdated.Add((cartItem.ProductId, cartItem.Count));
             }
            //n+1 problem : we send list of request when we add more than product to cart
            //by using addRange , insted of send request for each product ex:if we have 
            //100 product we send just one request
             await orderItemRepository.CreateRangeAsync(orderItems);
             await cartRepository.ClearCartAsync(userId);
            await productRepository.DecreaseQuantitiesAsync(productUpdated);
            await emailSender.SendEmailAsync(user.Email, "Payment Successful","<h3>Thank you ..</h3>");

            return new CheckoutResponse
            {
                Success = true,
                Message = "Payment Completed Successfully"
            };

        }
    }

}
