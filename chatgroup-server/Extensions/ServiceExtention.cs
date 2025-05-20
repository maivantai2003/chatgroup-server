using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using chatgroup_server.Repositories;
using chatgroup_server.Services;

namespace chatgroup_server.Extensions
{
    public static class ServiceExtention
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) {
            services.AddSingleton<IManagerConection,ManagerConection>();
            services.AddSingleton<RedisService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            //Friend
            services.AddScoped<IFriendRepository, FriendRepository>();
            services.AddScoped<IFriendService, FriendService>();
            //Group
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IGroupService, GroupService>();
            //GroupDetail
            services.AddScoped<IGroupDetailRepository, GroupDetailRepository>();
            services.AddScoped<IGroupDetailService, GroupDetailService>();
            //UserMessage
            services.AddScoped<IUserMessageRepository, UserMessageRepository>();
            services.AddScoped<IUserMessageService, UserMessageService>();
            //GroupMessage
            services.AddScoped<IGroupMessageRepository, GroupMessageRepository>();
            services.AddScoped<IGroupMessageService, GroupMessageService>();
            //File
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IFileService, FileService>();
            //Conversation
            services.AddScoped<IConversationRepository, ConversationRepository>();
            services.AddScoped<IConversationService, ConversationService>();
            //UserMessageFile
            services.AddScoped<IUserMessageFileRepository, UserMessageFileRepository>();
            services.AddScoped<IUserMessageFileService, UserMessageFileService>();
            //UserMessageReaction
            services.AddScoped<IUserMessageReactionRepository, UserMessageReactionRepository>();
            services.AddScoped<IUserMessageReactionService, UserMessageReactionService>();
            //UserMessageStatus
            services.AddScoped<IUserMessageStatusRepository, UserMessageStatusRepository>();
            services.AddScoped<IUserMessageStatusService, UserMessageStatusService>();
            //CloudMessage
            services.AddScoped<ICloudMessageRepository, CloudMessageRepository>();
            services.AddScoped<ICloudMessageService, CloudMessageService>();
            //CloudMessageFile
            services.AddScoped<ICloudMessageFileRepository, CloudMessageFileRepository>();
            services.AddScoped<ICloudMessageFileService, CloudMessageFileService>();
            //GroupMessageFile
            services.AddScoped<IGroupMessageFileService, GroupMessageFileService>();
            services.AddScoped<IGroupMessageFileRepository, GroupMessageFileRepository>();
            //OpenAI
            services.AddScoped<IOpenAIService, OpenAIService>();
            return services;
        }
    }
}
