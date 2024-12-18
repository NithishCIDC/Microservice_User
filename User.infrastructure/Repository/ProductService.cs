using System.Net.Http.Json;
using User.Application.DTO;
using User.Application.Interface;

namespace User.infrastructure.Repository
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
