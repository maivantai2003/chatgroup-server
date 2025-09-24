using Bogus;
using chatgroup_server.Data;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatgroup_server.Tests.Helpers
{
    public class TestBase
    {
        public ApplicationDbContext GetInMemoryDbContext()
        {
            var context = new DbContextOptionsBuilder<ApplicationDbContext>().
                UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).
                Options;
            return new ApplicationDbContext(context);
        }
        protected User GenerateFakeUser(int status = 1)
        {
            var faker = new Faker<User>()
                .RuleFor(u => u.UserId, f => f.IndexFaker + 1)
                .RuleFor(u => u.UserName, f => f.Name.FullName())
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber("0#########"))
                .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
                .RuleFor(u => u.Gmail, f => f.Internet.Email())
                .RuleFor(u => u.Status, f => status);

            return faker.Generate();
        }
    }
}
