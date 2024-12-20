using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using User.Domain.Modal;
using User.Application.Interface;
using User.Application.DTO;
using Mapster;

namespace User.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUnitOfWork _unitOfWork) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddUser([FromBody] UserDTO entity)
        {
            if (await _unitOfWork.CutomerRepository.IsEmailRegistered(entity.Email))
            {
                var UserData = entity.Adapt<UserModal>();
                await _unitOfWork.CutomerRepository.AddAsync(UserData);
                await _unitOfWork.CutomerRepository.SaveAsync();
                return Ok(UserData);
            }
            return BadRequest(new ErrorMessageDTO { Error = "Email already registered" });
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var customer = await _unitOfWork.CutomerRepository.GetAllAsync();
            return Ok(customer);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserByID(int id)
        {
            var User = await _unitOfWork.CutomerRepository.GetByIdAsync(id);
            if (User == null)
            {
                return NotFound(new ErrorMessageDTO { Error = "User not found" });
            }
            return Ok(User);
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
        public async Task<IActionResult> GetUserwithProduct(int id)
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
        public async Task<IActionResult> EditUser([FromBody] UserModal entity)
        {
            try
            {
                _unitOfWork.CutomerRepository.Update(entity);
                await _unitOfWork.CutomerRepository.SaveAsync();
                return Ok(entity);
            }
            catch (Exception)
            {
                return BadRequest(new ErrorMessageDTO { Error = "Something went wrong, while updating User" });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
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
