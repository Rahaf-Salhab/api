using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KASHOP.DAL.Validation;

namespace KASHOP.DAL.DTO.Request
{
    public class ProductRequest
    {
        public List<ProductTranslationRequest> Translations { get; set; }
        public decimal Price { get; set; }
        [MinValue(3)]
         public decimal DisCount { get; set; }
        public int Quantity { get; set; }
        public IFormFile MainImage { get; set; }
        public List<IFormFile> SubImages { get; set; }
        public int CategoryId { get; set; }

    }
}
