using chatgroup_server.Dtos;
using chatgroup_server.Models;
using chatgroup_server.Repositories;
using chatgroup_server.Tests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatgroup_server.Tests.Unit.Repositories
{
    public class UserDeviceRepositoryTests : TestBase
    {
        [Fact]
        public async Task AddUserDevice_ValidDevice_ShouldAddToDatabase()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            var repository = new UserDeviceRepository(context);
            var userDevice = GenerateFakeUserDevice(userId: 1);
            //Act
            await repository.AddUserDevice(userDevice);
            await context.SaveChangesAsync();
            //Assert
            var result = await context.UserDevices.FirstOrDefaultAsync(x=>x.DeviceId==userDevice.DeviceId && x.UserId==userDevice.UserId);
            result.Should().NotBeNull();
            result!.UserId.Should().Be(1);
        }
        [Fact]
        public async Task GetFcmTokensByUserIdAsync_UserHasValidTokens_ShouldReturnOnlyNonNullTokens()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            var repository = new UserDeviceRepository(context);
            context.UserDevices.AddRange(
                new UserDevice { UserId = 1, DeviceToken = "token-1" },
                new UserDevice { UserId = 1, DeviceToken = "token-2" },
                new UserDevice { UserId = 2, DeviceToken = "token-3" },
                new UserDevice { UserId = 1, DeviceToken = null }
            );
            await context.SaveChangesAsync();
            //Act
            var result = await repository.GetFcmTokensByUserIdAsync(1);
            //Assert
            result.Should().HaveCount(2);
            result.Should().Contain(new[] { "token-1", "token-2" });
        }
        [Fact]
        public async Task GetFcmTokensByUserIdsAsync_Should_Return_Tokens_For_Multiple_Users()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            var repository = new UserDeviceRepository(context);
            context.UserDevices.AddRange(
                new UserDevice { UserId = 1, DeviceToken = "token-1" },
                new UserDevice { UserId = 1, DeviceToken = "token-2" },
                new UserDevice { UserId = 2, DeviceToken = "token-3" },
                new UserDevice { UserId = 2, DeviceToken = "token-4" },
                new UserDevice { UserId = 3, DeviceToken = "token-5" },
                new UserDevice { UserId = 1, DeviceToken = null }
            );
            await context.SaveChangesAsync();
            //Act
            var result = await repository.GetFcmTokensByUserIdsAsync(new List<int> { 1, 2, 3 });
            //Assert
            result.Should().HaveCount(5);
            result.Should().Contain(new[] { "token-1", "token-2", "token-3", "token-4", "token-5" });
        }
        [Fact]
        public async Task GetUserDevice_Should_Return_Correct_Device()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            var repository = new UserDeviceRepository(context);
            var userDevice = new UserDevice
            {
                UserId = 1,
                DeviceId = "device-123",
                DeviceType = "iOS",
                DeviceToken = "token-123",
                Address = "123.456.789.000",
                IsOnline = true,
                LastActiveAt = DateTime.UtcNow,
                Browser = "Safari",
                OS = "iOS",
                DeviceName = "iPhone 12",
                IsVerified = true,
                VerifiedAt = DateTime.UtcNow,
                IpAddress = "123.456.789.000"
            };
            context.UserDevices.Add(userDevice);
            await context.SaveChangesAsync();
            //Act
            var result = await repository.GetUserDevice(1, "device-123");
            //Assert
            result.Should().NotBeNull();
            result.DeviceToken.Should().Be("token-123");
            result.OS.Should().Be("iOS");
        }
        [Fact]
        public async Task GetUserDevice_Should_Return_Null_If_Not_Found()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            var repository = new UserDeviceRepository(context);
            //Act
            var result = await repository.GetUserDevice(1, "not-exist-device");
            //Assert
            result.Should().BeNull();
        }
        [Fact]
        public async Task UpdateUserDevice_Should_Update_Device_And_Return_True()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            var repository = new UserDeviceRepository(context);
            var userDevice = new UserDevice
            {
                UserId = 1,
                DeviceId = "device-123",
                DeviceType = "iOS",
                DeviceToken = "token-123",
                IsOnline = false
            };
            context.UserDevices.Add(userDevice);
            await context.SaveChangesAsync();
            var updateDto = new chatgroup_server.Dtos.UpdateUserDeviceDto
            {
                UserId = 1,
                DeviceId = "device-123",
                DeviceToken = "new-token",
                Browser = "Chrome"
            };
            //Act
            var result = await repository.UpdateUserDevice(updateDto, "127.0.0.1");
            await context.SaveChangesAsync();
            //Assert
            result.Should().BeTrue();
            var updated = await context.UserDevices.FirstAsync();
            updated.DeviceToken.Should().Be("new-token");
            updated.Browser.Should().Be("Chrome");
            updated.IsOnline.Should().BeTrue();
            updated.IpAddress.Should().Be("127.0.0.1");
            updated.LastActiveAt.Should().NotBeNull();
        }
        [Fact]
        public async Task UpdateUserDevice_DeviceNotFound_ShouldReturnFalse()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new UserDeviceRepository(context);
            var dto = new UpdateUserDeviceDto
            {
                UserId = 99,
                DeviceId = "not-exist"
            };
            // Act
            var result = await repository.UpdateUserDevice(dto, "127.0.0.1");

            // Assert
            result.Should().BeFalse();
            context.UserDevices.Should().BeEmpty();
        }

    }
}
