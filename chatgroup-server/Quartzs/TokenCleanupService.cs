using chatgroup_server.Data;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace chatgroup_server.Quartzs
{
    public class TokenCleanupService : IJob
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public TokenCleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            using var scope= _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var now= DateTime.UtcNow;
            var expiredTokens = await dbContext.UserRefreshTokens.Where(x=>x.ExpirationDate<=now || x.IsInvalidades).ToListAsync();
            if (expiredTokens.Any()) {
                dbContext.UserRefreshTokens.RemoveRange(expiredTokens);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
