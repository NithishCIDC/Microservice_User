﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using User.Domain.Modal;
using User.Application.DTO;
using Serilog;
using User.Services;

namespace User.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserProcessor userProcessor;
        public UserController(IUserProcessor userProcessor)
        {
            this.userProcessor = userProcessor;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUser([FromBody] UserDTO entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Log.Information("Registering User");
                var User = await userProcessor.AddUser(entity);
                if (User is null)
                {
                    Log.Warning("Email is already registered");
                    return BadRequest(new ErrorMessageDTO { Error = "Email already registered" });
                }
                Log.Information("User Registered Successfully");
                return Ok(User);

            }
            catch (Exception)
            {
                Log.Error("Something went wrong, while registering User");
                return BadRequest(new ErrorMessageDTO { Error = "Something went wrong, while registering User" });

            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                Log.Information("Getting all User Data");
                var customer = await userProcessor.GetAllUser();
                Log.Information("Returning all User Data");
                return Ok(customer);
            }
            catch (Exception)
            {
                Log.Error("Something went wrong, while getting User");
                return BadRequest(new ErrorMessageDTO { Error = "Something went wrong, while getting User" });
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserByID(int id)
        {
            try
            {
                Log.Information("Getting User Data by ID");
                if (id <= 0)
                {
                    Log.Warning("Invalid ID");
                    return BadRequest(new ErrorMessageDTO { Error = "Invalid ID" });
                }
                var User = await userProcessor.GetUserbyID(id);
                if (User == null)
                {
                    Log.Warning("User not found");
                    return NotFound(new ErrorMessageDTO { Error = "User not found" });
                }
                Log.Information("Returning User Data");
                return Ok(User);
            }
            catch (Exception)
            {
                Log.Error("Something went wrong, while getting User by ID");
                return BadRequest(new ErrorMessageDTO { Error = "Something went wrong, while getting User by ID" });
            }
        }

        #region with Product
        //[HttpGet("WithProduct")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> GetWithProduct()
        //{
        //    try
        //    {
        //        Log.Information("Getting Product Data");
        //        var productData = await _unitOfWork.ProductService.GetProduct();
        //        Log.Information("Getting User Data");
        //        var customer = await _unitOfWork.UserRepository.GetAllAsync();

        //        var combinedData = customer.Select(customer => new
        //        {
        //            customer.UserId,
        //            customer.Username,
        //            CustomerAddress = customer.Address,
        //            Products = productData.Where(product => product.CustomerId == customer.UserId).Select(product => new
        //            {
        //                product.ProductId,
        //                product.ProductName,
        //            })
        //        });
        //        Log.Information("Returning User Data with Product");
        //        return Ok(combinedData);
        //    }
        //    catch (Exception)
        //    {
        //        Log.Error("Product Database is not connected");
        //        return BadRequest(new ErrorMessageDTO { Error = "Internal Server Error" });
        //    }
        //}
        #endregion

        #region With Product by id
        //[HttpGet("WithProduct/{id}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> GetUserwithProduct(int id)
        //{
        //    try
        //    {
        //        Log.Information("Getting Product Data by Customer ID");
        //        var productData = await _unitOfWork.ProductService.GetProductByCID(id);
        //        var customer = await _unitOfWork.UserRepository.GetByIdAsync(id);

        //        var combinedData = new
        //        {
        //            customer.UserId,
        //            customer.Username,
        //            CustomerAddress = customer.Address,
        //            product = productData.Select(product => new
        //            {
        //                product.ProductId,
        //                product.ProductName,
        //            })

        //        };
        //        Log.Information("Returning User Data with Product");
        //        return Ok(combinedData);
        //    }
        //    catch (Exception)
        //    {
        //        Log.Error("Product Database is not connected");
        //        return BadRequest(new ErrorMessageDTO { Error = "Product Database is not connected" });
        //    }
        //}
        #endregion

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditUser([FromBody] UserModal entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Log.Information("Updating User Data");
                await userProcessor.EditUser(entity);
                Log.Information("User Data Updated Successfully");
                return Ok(entity);
            }
            catch (Exception)
            {
                Log.Error("Something went wrong, while updating User");
                return BadRequest(new ErrorMessageDTO { Error = "Something went wrong, while updating User" });
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            Log.Information("Checking User in Database");
            try
            {
                Log.Information("Deleting User Data");
                var status = await userProcessor.DeleteUser(id);
                if (!status)
                {
                    Log.Warning("User not found");
                    return NotFound(new ErrorMessageDTO { Error = "User not found" });
                }
                Log.Information("User Data Deleted Successfully");
                return Ok("User deleted successfully");
            }
            catch (Exception)
            {
                Log.Error("Something went wrong");
                return BadRequest(new ErrorMessageDTO { Error = "Something went wrong" });
            }
        }
    }
}
