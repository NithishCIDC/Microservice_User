using AutoMapper;
using Customer.Application.DTO;
using Customer.Domain.Modal;
using Microsoft.AspNetCore.Mvc;
using Customer.Application.Interface;
using System.Text.Json;

namespace Customer.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(IUnitOfWork _unitOfWork, IMapper mapper,IHttpClientFactory _httpClient) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerDTO entity)
        {
            var CustomerData = mapper.Map<CustomerModal>(entity);
            await _unitOfWork.CutomerRepository.AddAsync(CustomerData);
            await _unitOfWork.CutomerRepository.SaveAsync();
            return Ok(await _unitOfWork.CutomerRepository.GetByIdAsync(CustomerData.CustomerId));
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomer()
        {
            var customer = await _unitOfWork.CutomerRepository.GetAllAsync();
            return Ok(customer);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCustomerByID(int id)
        {
            var customer = await _unitOfWork.CutomerRepository.GetByIdAsync(id);
            return Ok(customer);
        }

        //[HttpGet("with-orders")]
        //public IActionResult GetCustomerByOrder() {
        //    return Ok(_unitOfWork.CutomerRepository.GetCustomersWithOrders());
        //}

        [HttpGet("WithProduct")]
        public async Task<IActionResult> GetWithProduct() {
            var client = _httpClient.CreateClient("Product");
            var productData = await client.GetFromJsonAsync<List<ProductDTO>>("Product");

            var customer = await _unitOfWork.CutomerRepository.GetAllAsync();

            var combinedData = customer.Select(customer => new {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                CustomerAddress = customer.Address,
                Products = productData.Where(product => (int)product.CustomerId == customer.CustomerId).Select(product => new {
                    ProductId = (int)product.ProductId,
                    ProductName = (string)product.ProductName,
                })
            });

            return Ok(combinedData);
        }

        [HttpPut]
        public async Task<IActionResult> EditCustomer([FromBody] CustomerModal entity)
        {
            _unitOfWork.CutomerRepository.Update(entity);
            await _unitOfWork.CutomerRepository.SaveAsync();
            return Ok(await _unitOfWork.CutomerRepository.GetByIdAsync(entity.CustomerId));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var entity = await _unitOfWork.CutomerRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound(new { message = "Customer not found" });
            }

            _unitOfWork.CutomerRepository.Delete(entity);
            await _unitOfWork.CutomerRepository.SaveAsync();

            return Ok(new { message = "Customer deleted successfully" });
        }

    }
}
