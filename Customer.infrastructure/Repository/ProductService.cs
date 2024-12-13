using Customer.Application.DTO;
using Customer.Application.Interface;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.VisualBasic;

namespace Customer.infrastructure.Repository
{
    public class ProductService(IHttpClientFactory _httpClient) : IProductService
    {
        private readonly HttpClient client = _httpClient.CreateClient("Product");
        public async Task<List<ProductDTO>> GetProduct()
        {
            var productData = await client.GetFromJsonAsync<List<ProductDTO>>("Product");
            return productData;
        }
        public async Task<List<ProductDTO>> GetProductByCID(int id)
        {
            var productData = await client.GetFromJsonAsync<List<ProductDTO>>($"Product/WithCustomer/{id}");
            return productData;
        }

        public async void DeleteProduct(int id)
        {
            await client.DeleteAsync($"Product/{id}");
        }
    }
}
