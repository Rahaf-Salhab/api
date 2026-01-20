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
    public class ProductService : IProductService
    {
        private readonly IproductRepository productRepository;
        private readonly IFileService fileService;

        public ProductService(IproductRepository productRepository , IFileService fileService) 
        {
            this.productRepository = productRepository;
            this.fileService = fileService;
        }
        public async Task<ProductResponse> CreateProduct(ProductRequest request)
        {
            var product = request.Adapt<Product>();
            if (request.MainImage != null)
            {
                 var imagePath = await fileService.UploadAsync(request.MainImage);
                product.MainImage = imagePath;
            }
            if (request.SubImages != null) 
            {
                product.SubImages = new List<ProductImage>();
                foreach (var file in request.SubImages)
                { 
                    var imagePath = await fileService.UploadAsync(file);
                    product.SubImages.Add(new ProductImage
                    {
                        ImageName = imagePath
                    });
                }
            }
            await productRepository.AddAsync(product);

            return product.Adapt<ProductResponse>();    
          }
        public async Task<List<ProductResponse>> GetAllProductsForAdmin()
        {
            var products = await productRepository.GetAllAsync();
            var response = products.Adapt<List<ProductResponse>>();
            return response;

        }
    }
}
