using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Repositories;
using chatgroup_server.Services;

namespace chatgroup_server.Extensions
{
    public static class ServiceExtention
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFriendRepository, FriendRepository>();
            services.AddScoped<IFriendService, FriendService>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IGroupDetailRepository, GroupDetailRepository>();
            services.AddScoped<IGroupDetailService, GroupDetailService>();
            services.AddScoped<IUserMessageRepository, UserMessageRepository>();
            services.AddScoped<IUserMessageService, UserMessageService>();
            services.AddScoped<IGroupMessageRepository, GroupMessageRepository>();
            services.AddScoped<IGroupMessageService, GroupMessageService>();
            return services;
        }
    }
}
