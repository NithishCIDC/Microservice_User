using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.Application.Interface;
using User.Application.DTO;
using User.Domain.Modal;
using User.WebApi.Controllers;
using AutoMapper;
using System.Collections.Generic;

namespace User.UnitTests
{
    public class UserControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _controller = new UserController(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        private class ErrorMessage()
        {
            public string message { get; set; }
        }

        [Fact]
        public async Task AddCustomer_ReturnsOkResult_WithCustomerData()
        {
            // Arrange
            var userDto = new UserDTO { Username = "TestUser", Email = "test@example.com", Password = "Password123", Address = "TestAddress" };
            var userModel = new UserModal { UserId = 1, Username = "TestUser", Email = "test@example.com", Password = "Password123", Address = "TestAddress" };

            _mockMapper.Setup(m => m.Map<UserModal>(userDto)).Returns(userModel);
            _mockUnitOfWork.Setup(u => u.CutomerRepository.AddAsync(userModel)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CutomerRepository.SaveAsync()).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CutomerRepository.GetByIdAsync(1)).ReturnsAsync(userModel);

            // Act
            var result = await _controller.AddCustomer(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomer = Assert.IsType<UserModal>(okResult.Value);
            Assert.Equal(userModel.UserId, returnedCustomer.UserId);
        }

        [Fact]
        public async Task GetCustomer_ReturnsOkResult_WithCustomerList()
        {
            // Arrange
            var customers = new List<UserModal>
            {
                new UserModal { UserId = 1, Username = "User1", Email = "user1@example.com", Password = "Password123", Address = "Address1" },
                new UserModal { UserId = 2, Username = "User2", Email = "user2@example.com", Password = "Password123", Address = "Address2" }
            };

            _mockUnitOfWork.Setup(u => u.CutomerRepository.GetAllAsync()).ReturnsAsync(customers);

            // Act
            var result = await _controller.GetCustomer();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomers = Assert.IsType<List<UserModal>>(okResult.Value);
            Assert.Equal(2, returnedCustomers.Count);
        }

        [Fact]
        public async Task GetCustomerByID_ReturnsOkResult_WithCustomer()
        {
            // Arrange
            var customer = new UserModal { UserId = 1, Username = "User1", Email = "user1@example.com", Password = "Password123", Address = "Address1" };

            _mockUnitOfWork.Setup(u => u.CutomerRepository.GetByIdAsync(1)).ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomerByID(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomer = Assert.IsType<UserModal>(okResult.Value);
            Assert.Equal(customer.UserId, returnedCustomer.UserId);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var customer = new UserModal { UserId = 1, Username = "User1", Email = "user1@example.com", Password = "Password123", Address = "Address1" };

            _mockUnitOfWork.Setup(u => u.CutomerRepository.GetByIdAsync(1)).ReturnsAsync(customer);
            _mockUnitOfWork.Setup(u => u.CutomerRepository.Delete(customer));
            _mockUnitOfWork.Setup(u => u.ProductService.DeleteProduct(1));
            _mockUnitOfWork.Setup(u => u.CutomerRepository.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCustomer(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedValue = Assert.IsType<ErrorMessageDTO>(okResult.Value);
            Assert.Equal("User deleted successfully", returnedValue.Message);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.CutomerRepository.GetByIdAsync(1)).ReturnsAsync((UserModal)null);

            // Act
            var result = await _controller.DeleteCustomer(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnedValue = Assert.IsType <ErrorMessageDTO>( notFoundResult.Value); // Using dynamic
            Assert.Equal("User not found", returnedValue.Error);
        }
    }
}
