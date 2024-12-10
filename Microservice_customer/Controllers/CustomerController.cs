using AutoMapper;
using Customer.Application.DTO;
using Customer.infrastructure.Data;
using Customer.Domain.Modal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Customer.Application.Interface;

namespace Customer.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(IUnitOfWork _unitOfWork, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerDTO entity)
        {
            var CustomerData = mapper.Map<CustomerModal>(entity);
            await _unitOfWork.CutomerRepository.AddAsync(CustomerData);
            await _unitOfWork.CutomerRepository.SaveAsync();
            return Ok(await _unitOfWork.CutomerRepository.GetByIdAsync(CustomerData.Id));
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

        [HttpPut]
        public async Task<IActionResult> EditCustomer([FromBody] CustomerModal entity)
        {
            _unitOfWork.CutomerRepository.Update(entity);
            await _unitOfWork.CutomerRepository.SaveAsync();
            return Ok(await _unitOfWork.CutomerRepository.GetByIdAsync(entity.Id));
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
