using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Models
{
    [PrimaryKey(nameof(ProductId),nameof(userId))]
    public class Cart
    {

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string userId { get; set; }
        public ApplicationUser User { get; set; }
        public int Count { get; set; }
    }
}
