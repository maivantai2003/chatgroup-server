using chatgroup_server.Dtos;
using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using chatgroup_server.Services;
using FluentAssertions;
using k8s.Models;
using Moq;


namespace chatgroup_server.Tests.Unit.Services
{
    public class UserDeviceServiceTests
    {
        private readonly Mock<IUserDeviceRepository> _userDeviceRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly UserDeviceService _userDeviceService;
        public UserDeviceServiceTests()
        {
            _userDeviceRepositoryMock = new Mock<IUserDeviceRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userDeviceService = new UserDeviceService(_unitOfWorkMock.Object, _userDeviceRepositoryMock.Object);
        }
        [Fact]
        public async Task AddUserDevice_ValidInput_ShouldCommitAndReturnSuccess()
        {
            // Arrange
            var userDeviceAddDto = new UserDeviceAddDto
            {
                UserId = 1,
                DeviceId = "device-1",
                DeviceToken = "token",
                DeviceType = "iOS"
            };
            _userDeviceRepositoryMock.Setup(r => r.AddUserDevice(It.IsAny<UserDevice>())).Returns(Task.CompletedTask);
            // Act
            var result = await _userDeviceService.AddUserDevice(userDeviceAddDto, "127.0.0.1");
            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().BeTrue();
            _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Never);
        }
        [Fact]
        public async Task AddUserDevice_RepositoryThrowsException_ShouldRollbackAndReturnError()
        {
            // Arrange
            var userDeviceAddDto = new UserDeviceAddDto
            {
                UserId = 1,
                DeviceId = "device-1",
                DeviceToken = "token",
                DeviceType = "iOS"
            };
            _userDeviceRepositoryMock.Setup(r => r.AddUserDevice(It.IsAny<UserDevice>())).ThrowsAsync(new Exception("Database error"));
            //Act
            var result = await _userDeviceService.AddUserDevice(userDeviceAddDto,"127.0.0.1");
            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain("Database error");
            _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
        }
        [Fact]
        public async Task UpdateUserDeviceAsync_ValidDevice_ShouldCommitAndReturnSuccess()
        {
            // Arrange
            var dto = new UpdateUserDeviceDto
            {
                UserId = 1,
                DeviceId = "device-1"
            };

            _userDeviceRepositoryMock.Setup(r => r.UpdateUserDevice(dto, It.IsAny<string>())).ReturnsAsync(true);
            // Act
            var result = await _userDeviceService.UpdateUserDeviceAsync(dto, "127.0.0.1");
            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().BeTrue();
            _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Never);
        }
        [Fact]
        public async Task UpdateUserDeviceAsync_RepositoryThrowsException_ShouldRollbackAndReturnError()
        {
            // Arrange
            var dto = new UpdateUserDeviceDto
            {
                UserId = 1,
                DeviceId = "device-1"
            };

            _userDeviceRepositoryMock.Setup(r => r.UpdateUserDevice(dto, It.IsAny<string>())).ThrowsAsync(new Exception("Update failed"));
            // Act
            var result = await _userDeviceService.UpdateUserDeviceAsync(dto, "127.0.0.1");
            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain("Update failed");
            _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
        }
        [Fact]
        public async Task GetUserDevice_DeviceExists_ShouldReturnSuccessResponse()
        {
            // Arrange
            var device = new UserDevice
            {
                UserId = 1,
                DeviceId = "device-1"
            };

            _userDeviceRepositoryMock.Setup(r => r.GetUserDevice(1, "device-1")).ReturnsAsync(device);
            // Act
            var result = await _userDeviceService.GetUserDevice(1, "device-1");
            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }
        [Fact]
        public async Task GetUserDevice_RepositoryThrowsException_ShouldReturnErrorResponse()
        {
            // Arrange
            _userDeviceRepositoryMock.Setup(r => r.GetUserDevice(It.IsAny<int>(), It.IsAny<string>())).ThrowsAsync(new Exception("Query failed"));
            // Act
            var result = await _userDeviceService.GetUserDevice(1, "device-1");
            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain("Query failed");
        }
    }
}
