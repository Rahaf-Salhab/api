using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public interface IproductRepository
    {
        Task<Product> AddAsync(Product request);
        Task<List<Product>> GetAllAsync();

    }
}
