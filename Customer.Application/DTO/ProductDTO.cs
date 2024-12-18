using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? ProductCompany { get; set; }

        public decimal? ProductPrice { get; set; }

        public decimal? ProductQuantity { get; set; }

        public int ProductDiscount { get; set; }

        public int CustomerId { get; set; }
    }
}
