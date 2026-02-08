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

            var cartItem = await cartRepository.GetCartItemAsync(userId, request.ProductId);
            var existingCount = cartItem?.Count ?? 0;

            if (product.Quantity < (existingCount + request.Count))
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "not enough stock"
                };
            }
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
        public async Task<BaseResponse> UpdateQuantityAsync(string userId , int productId , int count)
        {
            var cartItem = await cartRepository.GetCartItemAsync(userId ,productId);
            var product = await productRepository.FindByIdAsync(productId);
             if (count < 0) 
             {
                return new BaseResponse
                {
                  Success = false,
                  Message = "invalid count"
                };
             }
            if(count == 0)
            {
                await cartRepository.DeleteAsync(cartItem);
                return new BaseResponse
                {
                    Success = false,
                    Message = "item removed from cart"
                };
            }
           
            if (product.Quantity < count)
            {
                return new BaseResponse
                {
                   Success = false,
                   Message = "not enough stock"
                };
            }
            cartItem.Count = count;
            await cartRepository.UpdateAsync(cartItem);
            return new BaseResponse
            {
                Success = true,
                Message = "Quantity Updated Successfully"
            };


        }
        public async Task<BaseResponse> RemoveFromCartAsync(string userId , int productId)
        {
            var cartItem = await cartRepository.GetCartItemAsync(userId , productId);
              if(cartItem is null)
              {
                 return new BaseResponse
                 {
                    Success = false,
                    Message = "cart item not found"
                 };
              }
            await cartRepository.DeleteAsync(cartItem);
            return new BaseResponse
            {
                Success = true,
                Message = "item removed from cart"
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
