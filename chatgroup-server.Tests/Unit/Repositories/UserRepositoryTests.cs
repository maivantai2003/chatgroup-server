using Bogus;
using chatgroup_server.Data;
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
    public class UserRepositoryTests:TestBase
    {
        [Fact]
        public async Task AddUserAsync_ShouldAddUser()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            var user = new User { UserId = 1, UserName = "TestUser", PhoneNumber = "0123456789" };
            await context.SaveChangesAsync();
            var repo = new UserRepository(context);
            //Act
            await repo.AddUserAsync(user);
            //Assert
            var addedUser = await context.Users.FindAsync(1);
            Assert.NotNull(addedUser);
            Assert.Equal("TestUser", addedUser.UserName);
        }
        [Fact]
        public async Task CheckPhoneNumber_ShouldReturnTrue_WhenExists()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            context.Users.Add(new User()
            {
                UserId = 1, 
                Avatar = "avatar.png",
                PhoneNumber = "0123456789", 
            });
            await context.SaveChangesAsync();
            var repo = new UserRepository(context);
            var numberPhone = "0123456789";
            //Act
            var respone=await repo.CheckPhoneNumber(numberPhone);
            //Assert
            //Assert.True(respone);
            respone.Should().BeTrue();
        }
        [Fact]
        public async Task CheckPhoneNumber_ShouldReturnFalse_WhenBlock()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            var fakeUser=GenerateFakeUser();
            fakeUser.Status = 0;
            context.Users.Add(fakeUser);
            context.SaveChanges();
            var repo = new UserRepository(context);
            //Act
            var respone = await repo.CheckPhoneNumber(fakeUser.PhoneNumber);
            //Assert
            respone.Should().BeFalse();
        }
        [Fact]
        public async Task GetUserById_ShouldReturnUserInfor_WhenExists()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            var fakeUser = GenerateFakeUser();
            context.Users.Add(fakeUser);
            context.SaveChanges();
            var repo = new UserRepository(context);
            //Act
            var response=await repo.GetUserById(fakeUser.UserId);  
            //Assert 
            response.Should().NotBeNull();
            response!.UserName.Should().Be(fakeUser.UserName);
        }
        [Fact]
        public async Task UpdateUserByEmail_ShouldUpdatePassword()
        {
            //Arrange
            var context =GetInMemoryDbContext();
            var fakeUser = GenerateFakeUser();
            context.Users.Add(fakeUser);
            context.SaveChanges();
            var repo = new UserRepository(context);
            //Act
            await repo.UpdateUserByEmail("newpassword",fakeUser.Gmail!);
            await context.SaveChangesAsync();
            //Assert
            var updatedUser=await context.Users.FindAsync(fakeUser.UserId);
            updatedUser!.Password.Should().Be("newpassword");
        }
    }
}
