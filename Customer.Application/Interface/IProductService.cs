using Customer.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Application.Interface
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetProduct();
        Task<List<ProductDTO>> GetProductByCID(int id);
        void DeleteProduct(int id);
    }
}
