using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.MapsterConfigurations
{
    public static class MapsterConfig
    {
        public static void MapsterConfRegister()
        {
            //  TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
            // .Map(dest => dest.CategoryId, source => source.Id).TwoWays();
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.CreatedBy, source => source.User.UserName);

            TypeAdapterConfig<Category, CategoryUserResponse>.NewConfig()
                .Map(dest => dest.Name , source => source.Translations
                .Where(t => t.lang == MapContext.Current.Parameters["lang"].ToString())
                .Select(t => t.Name).FirstOrDefault());
             
            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
                .Map(dest => dest.MainImage, source => $"https://localhost:7077/images/{source.MainImage}");
        }
    }
}
