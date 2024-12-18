using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.DTO;

namespace User.Application.Interface
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetProduct();
        Task<List<ProductDTO>> GetProductByCID(int id);
        void DeleteProduct(int id);
    }
}
