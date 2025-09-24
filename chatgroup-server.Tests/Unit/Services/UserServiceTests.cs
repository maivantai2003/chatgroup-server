using chatgroup_server.Helpers;
using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using chatgroup_server.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatgroup_server.Tests.Unit.Services
{
    public  class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ISendGmailService> _mockSendGmailService;
        private readonly Mock<IJwtService> _mockJwtService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IRedisService> _mockRedisService;
        private readonly UserService _userService;
        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mockSendGmailService = new Mock<ISendGmailService>();
            _mockJwtService = new Mock<IJwtService>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockRedisService = new Mock<IRedisService>();
            _userService = new UserService(_userRepositoryMock.Object, _unitOfWorkMock.Object, _mockSendGmailService.Object, _mockJwtService.Object, _mockConfiguration.Object, _mockRedisService.Object);
        }
        [Fact]
        public async Task AddUserAsync_ShouldReturnSuccess_WhenUserIsAdded()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                UserName = "Văn Tài"
            };
            _userRepositoryMock.Setup(r => r.AddUserAsync(user)).Returns(Task.CompletedTask);
            _mockRedisService.Setup(r => r.RemoveCacheAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            //Act
            var result=await _userService.AddUserAsync(user);   
            // Assert
            Assert.True(result.Success);
            Assert.Equal("Đăng Ký Thành Công", result.Message);
            _unitOfWorkMock.Verify(u=>u.BeginTransactionAsync(),Times.Once);
            _unitOfWorkMock.Verify(u=>u.CommitAsync(),Times.Once); 
            _mockRedisService.Verify(r=>r.RemoveCacheAsync(CacheKeys.Users(user.UserId)),Times.Once);
        }
        [Fact]
        public async Task CheckPhoneNumber_ShouldReturnTrue_WhenExists()
        {
            //Arrange
            string phoneNumber = "0123456789";
            _userRepositoryMock.Setup(r => r.CheckPhoneNumber(phoneNumber)).ReturnsAsync(true);
            //Act
            var result=await _userService.CheckPhoneNumber(phoneNumber);
            //Assert
            Assert.True(result.Success);
        }
        [Fact]
        public async Task CheckPhoneNumber_ShouldReturnFalse_WhenNotExists()
        {
            //Arrange
            string phoneNumber = "0123456789";
            _userRepositoryMock.Setup(r => r.CheckPhoneNumber(phoneNumber)).ReturnsAsync(false);
            //Act
            var result = await _userService.CheckPhoneNumber(phoneNumber);
            //Assert
            Assert.True(result.Success);
        }
        [Fact]
        public async Task GetUserById_ShouldReturnUserInfor_WhenExists()
        {
            //Arrange
            var userId = 1;
            _userRepositoryMock.Setup(r=>r.GetUserById(userId)).ReturnsAsync(new Dtos.UserInfor
            {
                UserId = userId,
                UserName = "Văn Tài"
            });
            //Act
            var result = await _userService.GetUserById(userId);
            //Assert
            Assert.True(result.Success);
        }
    }
}
