using Bogus;
using chatgroup_server.Data;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;
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
        protected UserDevice GenerateFakeUserDevice(int userId)
        {
            var faker = new Faker<UserDevice>()
                .RuleFor(ud => ud.UserDeviceId, f => f.IndexFaker + 1)
                .RuleFor(ud => ud.UserId, f => userId)
                .RuleFor(ud => ud.DeviceId, f => f.Random.Guid().ToString())
                .RuleFor(ud => ud.DeviceType, f => f.PickRandom(new[] { "iOS", "Android", "Web", "Desktop" }))
                .RuleFor(ud => ud.DeviceToken, f => f.Random.AlphaNumeric(20))
                .RuleFor(ud => ud.IsOnline, f => f.Random.Bool())
                .RuleFor(ud => ud.LastActiveAt, f => f.Date.Recent());
            return faker.Generate();
        }
    }
}
