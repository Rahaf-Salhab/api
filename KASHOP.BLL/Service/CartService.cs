using KASHOP.DAL.DTO.Request;
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
    public class CartService : ICartService
    {
        private readonly IproductRepository productRepository;
        private readonly ICartRepository cartRepository;

        public CartService(IproductRepository productRepository , ICartRepository cartRepository) 
        {
            this.productRepository = productRepository;
            this.cartRepository = cartRepository;
        }
        public async Task<BaseResponse> AddToCartAsync(string userId, AddToCartRequest request)
        {
            var product = await productRepository.FindByIdAsync(request.ProductId);
            if (product is null) 
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "product not found"
                };
            }
            if (product.Quantity < request.Count) 
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "not enough stock"
                };
            }
            var cartItem = await cartRepository.GetCartItemAsync(userId , request.ProductId);
            if (cartItem is not null) 
            {
                cartItem.Count += request.Count;
                await cartRepository.UpdateAsync(cartItem);
            }
            else
            {
                var cart = request.Adapt<Cart>();
                cart.userId = userId;

                await cartRepository.CreateAsync(cart);
            }
            return new BaseResponse
            {
                Success = true,
                Message = "product added successfully"
            };

         }

        public async Task<CartSummaryResponse> GetUserCartAsync(string userId, string lang = "en")
        {
            var cartItems = await cartRepository.GetUserCartAsync(userId);
             var items = cartItems.Select(c => new CartResponse
            {
                ProductId = c.ProductId,
                ProductName = c.Product.Translations.FirstOrDefault(t => t.lang == lang).Name,
                Count = c.Count,
                Price = c.Product.Price
            }).ToList();
            return new CartSummaryResponse
            {
                Items = items
            };
          }
        public async Task<BaseResponse> ClearCartAsync(string userId)
        {
            await cartRepository.ClearCartAsync(userId);
            return new BaseResponse
            {
                Success = true,
                Message = "cart cleared successfully"
            };
        }
    }
}
