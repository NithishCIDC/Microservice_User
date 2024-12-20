using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using User.Domain.Modal;
using User.Application.Interface;
using User.Application.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using User.infrastructure.Repository;

namespace User.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUnitOfWork _unitOfWork, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddCustomer([FromBody] UserDTO entity)
        {
            if (await _unitOfWork.CutomerRepository.IsEmailRegistered(entity.Email))
            {
                var CustomerData = mapper.Map<UserModal>(entity);
                await _unitOfWork.CutomerRepository.AddAsync(CustomerData);
                await _unitOfWork.CutomerRepository.SaveAsync();
                return Ok(CustomerData);
            }
            return BadRequest(new ErrorMessageDTO { Error = "Email already registered" });
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
            if (customer == null)
            {
                return NotFound(new ErrorMessageDTO { Error = "User not found" });
            }
            return Ok(customer);
        }

        [HttpGet("WithProduct")]
        public async Task<IActionResult> GetWithProduct()
        {
            try
            {
                var productData = await _unitOfWork.ProductService.GetProduct();
                var customer = await _unitOfWork.CutomerRepository.GetAllAsync();

                var combinedData = customer.Select(customer => new
                {
                    customer.UserId,
                    customer.Username,
                    CustomerAddress = customer.Address,
                    Products = productData.Where(product => product.CustomerId == customer.UserId).Select(product => new
                    {
                        product.ProductId,
                        product.ProductName,
                    })
                });

                return Ok(combinedData);
            }
            catch (Exception) { return BadRequest(new ErrorMessageDTO { Error = "Product Database is not connected" }); }


        }

        [HttpGet("WithProduct/{id}")]
        public async Task<IActionResult> GetCustomerwithProduct(int id)
        {
            try
            {
                var productData = await _unitOfWork.ProductService.GetProductByCID(id);
                var customer = await _unitOfWork.CutomerRepository.GetByIdAsync(id);

                var combinedData = new
                {
                    customer.UserId,
                    customer.Username,
                    CustomerAddress = customer.Address,
                    product = productData.Select(product => new
                    {
                        product.ProductId,
                        product.ProductName,
                    })

                };
                return Ok(combinedData);
            }
            catch (Exception)
            {
                return BadRequest(new ErrorMessageDTO { Error = "Product Database is not connected" });
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditCustomer([FromBody] UserModal entity)
        {
            _unitOfWork.CutomerRepository.Update(entity);
            await _unitOfWork.CutomerRepository.SaveAsync();
            return Ok(await _unitOfWork.CutomerRepository.GetByIdAsync(entity.UserId));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var entity = await _unitOfWork.CutomerRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound(new ErrorMessageDTO { Error = "User not found" });
            }
            try
            {
                _unitOfWork.CutomerRepository.Delete(entity);
                _unitOfWork.ProductService.DeleteProduct(id);
                await _unitOfWork.CutomerRepository.SaveAsync();
            }
            catch (Exception)
            {
                return BadRequest(new ErrorMessageDTO { Error = "Something went wrong" });
            }


            return Ok("User deleted successfully");
        }

    }
}
