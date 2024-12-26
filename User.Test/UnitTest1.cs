using Moq;
using Microsoft.AspNetCore.Mvc;
using User.Application.Interface;
using User.Application.DTO;
using User.Domain.Modal;
using User.WebApi.Controllers;
using User.Services;

namespace User.UnitTests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserProcessor> _mockUserProcessor;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserProcessor = new Mock<IUserProcessor>();
            _controller = new UserController(_mockUserProcessor.Object);
        }
        [Fact]
        public async Task AddCustomer_ReturnsOk_WhenEmailNotRegistered()
        {
            // Arrange
            var userDto = new UserDTO { Username = "TestUser", Email = "test@gmail.com", Password = "Password123", Address = "TestAddress" };
            var userModal = new UserModal { UserId = 1, Username = "TestUser", Email = "test@example.com", Password = "Password123", Address = "TestAddress" };

            // Mock the repository methods for adding and saving the customer
            _mockUserProcessor.Setup(u => u.AddUser(It.IsAny<UserDTO>())).ReturnsAsync(userModal);

            // Act
            var result = await _controller.AddUser(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedValue = Assert.IsType<UserModal>(okResult.Value);
            Assert.Equal(userModal.Email, returnedValue.Email);
        }


        [Fact]
        public async Task AddCustomer_ReturnsBadRequest_WhenEmailAlreadyRegistered()
        {
            // Arrange
            var userDto = new UserDTO { Username = "TestUser", Email = "test@example.com", Password = "Password123", Address = "TestAddress" };

            _mockUserProcessor.Setup(u => u.AddUser(It.IsAny<UserDTO>())).ReturnsAsync((UserModal)null!);

            // Act
            var result = await _controller.AddUser(userDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedValue = Assert.IsType<ErrorMessageDTO>(badRequestResult.Value);
            Assert.Equal("Email already registered", returnedValue.Error);
        }

        [Fact]
        public async Task GetCustomer_ReturnsOkResult_WithCustomerList()
        {
            // Arrange
            var customers = new List<UserModal>
            {
                new() { UserId = 1, Username = "User1", Email = "user1@example.com", Password = "Password123", Address = "Address1" },
                new() { UserId = 2, Username = "User2", Email = "user2@example.com", Password = "Password123", Address = "Address2" }
            };

            _mockUserProcessor.Setup(u => u.GetAllUser()).ReturnsAsync(customers);

            // Act
            var result = await _controller.GetAllUser();

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

            _mockUserProcessor.Setup(u => u.GetUserbyID(1)).ReturnsAsync(customer);

            // Act
            var result = await _controller.GetUserByID(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomer = Assert.IsType<UserModal>(okResult.Value);
            Assert.Equal(customer.UserId, returnedCustomer.UserId);
        }

        [Fact]
        public async Task GetCustomerByID_Returns_NotFoundResult()
        {
            // Arrange
            _mockUserProcessor.Setup(u => u.GetUserbyID(2)).ReturnsAsync((UserModal)null!);

            // Act
            var result = await _controller.GetUserByID(1);

            // Assert
            var Result = Assert.IsType<NotFoundObjectResult>(result);
            var returnedCustomer = Assert.IsType<ErrorMessageDTO>(Result.Value);
            Assert.Equal("User not found", returnedCustomer.Error);
        }

        [Fact]
        public async Task Update_User_returnsOkObjectResult()
        {
            // Arrange
            var user = new UserModal { UserId = 1, Username = "User1", Email = "user1@example.com", Password = "Password123", Address = "Address1" };

            _mockUserProcessor.Setup(u => u.EditUser(user));

            var result = await _controller.EditUser(user);

            var Result = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(user, Result.Value);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var customer = new UserModal { UserId = 1, Username = "User1", Email = "user1@example.com", Password = "Password123", Address = "Address1" };

            _mockUserProcessor.Setup(u => u.DeleteUser(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedValue = Assert.IsType<string>(okResult.Value);
            Assert.Equal("User deleted successfully", returnedValue);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserProcessor.Setup(u => u.DeleteUser(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnedValue = Assert.IsType<ErrorMessageDTO>(notFoundResult.Value);
            Assert.Equal("User not found", returnedValue.Error);
        }
    }
}
