using KASHOP.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public interface IproductRepository
    {
        Task<Product> AddAsync(Product request);

        Task<List<Product>> GetAllAsync();

        Task<Product?> FindByIdAsync(int id);

        Task<bool> DecreaseQuantitiesAsync(
            List<(int productId, int quantity)> items);
        IQueryable<Product> Query();
    }
}
