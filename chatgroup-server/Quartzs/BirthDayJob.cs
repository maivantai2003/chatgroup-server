using chatgroup_server.Data;
using chatgroup_server.RabbitMQ.Models;
using chatgroup_server.RabbitMQ.Producer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using System.Text;
using System.Threading.Channels;

namespace chatgroup_server.Quartzs
{
    public class BirthDayJob : IJob
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly BirthDayProducer _producer;
        public BirthDayJob(IServiceScopeFactory scopeFactory,BirthDayProducer producer)
        {
            _scopeFactory = scopeFactory;
            _producer = producer;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var todayVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
            Console.WriteLine("Time 1: "+DateTime.Now);
            Console.WriteLine("Time 2: "+DateTime.UtcNow);
            Console.WriteLine("Time 3: " + todayVn);
            using var scope=_scopeFactory.CreateScope();
            var dbContext=scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var usersBirthday = await dbContext.Users.AsNoTracking().Where(u => u.Birthday.Month == todayVn.Month && u.Birthday.Day == todayVn.Day && u.Status==1)
                .Select(x=>new
                {
                    x.UserId,
                    x.UserName
                }).ToListAsync();
            var userIds=usersBirthday.Select(x=>x.UserId).ToList();
            var friends = await dbContext.Friends.AsNoTracking()
                .Where(f => (userIds.Contains(f.UserId) || userIds.Contains(f.FriendId)) && f.Status==1)
                .Select(f => new
                {
                    SenderId = userIds.Contains(f.UserId) ? f.UserId : f.FriendId,
                    FriendId = userIds.Contains(f.UserId) ? f.FriendId : f.UserId
                })
                .ToListAsync();
            foreach (var user in usersBirthday)
            {
                var userFriends = friends.Where(f => f.SenderId == user.UserId).Select(f => f.FriendId);
                Console.WriteLine($"{user.UserId}-friend: "+userFriends);
                foreach (var friendId in userFriends)
                {
                    var model = new BirthdayModel()
                    {
                        SenderId = user.UserId,
                        FriendId = friendId,
                        UserName = user.UserName
                    };

                    //var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload));
                    await _producer.PublishBirthdayAsync(model); 
                    //_producer.Publish("birthday.exchange", "", body);
                }
            }
        }
    }
}
