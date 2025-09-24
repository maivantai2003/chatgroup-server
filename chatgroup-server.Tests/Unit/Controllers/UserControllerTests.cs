using Bogus;
using chatgroup_server.Common;
using chatgroup_server.Controllers;
using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatgroup_server.Tests.Unit.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _userController;
        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _userController = new UserController(_mockUserService.Object);
        }
        [Fact]
        public async Task CheckPhoneNumber_ShouldReturnOkResult_WhenServiceReturnsSuccess()
        {
            //Arrange
            string phoneNumber = "0123456789";
            _mockUserService.Setup(s => s.CheckPhoneNumber(phoneNumber))
                .ReturnsAsync(ApiResponse<bool>.SuccessResponse("Ok",true));
            //Act 
            var result = await _userController.CheckPhoneNumber(phoneNumber) as ObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.True(((bool)result.Value));
        }
        [Fact]
        public async Task CheckPhoneNumber_ShouldReturnOkResultWithErrors_WhenServiceReturnsError()
        {
            //Arrange
            string phoneNumber = "0123456789";
            var errors = new List<string> { "Failed"};
            _mockUserService.Setup(s => s.CheckPhoneNumber(phoneNumber))
                .ReturnsAsync(ApiResponse<bool>.ErrorResponse("Failed", errors));
            //Act 
            var result = await _userController.CheckPhoneNumber(phoneNumber) as ObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var value = Assert.IsType<List<string>>(result.Value);
            Assert.Equal(errors, value);
        }
        [Fact]
        public async Task GetAllUser_ShouldReturnOkResult_WhenServiceReturnsSuccess()
        {
            //Arrange
            var fakeUsers=new Faker<UserDto>().RuleFor(u => u.UserId, f => f.IndexFaker + 1)
                .RuleFor(u => u.UserName, f => f.Name.FullName())
                .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
                .RuleFor(u => u.Status, f => f.Random.Int(0, 1))
                .Generate(10).ToList();
            _mockUserService.Setup(s => s.GetAllUsersAsync(1))
                .ReturnsAsync(ApiResponse<IEnumerable<UserDto>>.SuccessResponse("Ok", fakeUsers));
            //Act
            var result = await _userController.GetAllUser(1) as ObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var users=Assert.IsAssignableFrom<IEnumerable<UserDto>>(result.Value);
            Assert.Equal(10, ((List<UserDto>)users).Count);
        }
        [Fact]
        public async Task UpdateUser_ShouldReturnOk_WithData_WhenSuccess()
        {
            //Arrange
            var fakeUser=new Faker<UserUpdateDto>()
                .RuleFor(u => u.UserName, f => f.Name.FullName())
                .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
                .RuleFor(u => u.Status, f => f.Random.Int(0, 1))
                .RuleFor(u => u.Bio, f => f.Lorem.Sentence())
                .RuleFor(u => u.CoverPhoto, f => f.Internet.Avatar()).
                RuleFor(u => u.UserId, f => f.IndexFaker + 1)
                .Generate();
            _mockUserService.Setup(s => s.UpdateUserAsync(It.IsAny<User>()))
                .ReturnsAsync(ApiResponse<User>.SuccessResponse("Ok",new User()
                {
                    UserId = fakeUser.UserId,
                    UserName = fakeUser.UserName,
                    Avatar = fakeUser.Avatar,
                    Status = fakeUser.Status,
                    Bio = fakeUser.Bio,
                    CoverPhoto = fakeUser.CoverPhoto,
                }));
            //Act
            var result = await _userController.UpdateUser(fakeUser.UserId, fakeUser) as ObjectResult;
            //Assert
            Assert.NotNull(result);
            var response=result.Value as User;
            Assert.Equal(fakeUser.UserName, response.UserName);
        }
    }
}
