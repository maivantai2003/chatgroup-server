using chatgroup_server.Data;
using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Repositories
{
    public class UserMessageRepository : IUserMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public UserMessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserMessages>> GetMessagesBySenderIdAsync(int senderId)
        {
            return await _context.UserMessages
                .Include(um => um.Receiver)
                .Where(um => um.SenderId == senderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserMessages>> GetMessagesByReceiverIdAsync(int receiverId)
        {
            return await _context.UserMessages
                .Include(um => um.Sender)
                .Where(um => um.ReceiverId == receiverId)
                .ToListAsync();
        }

        public async Task AddUserMessageAsync(UserMessages userMessage)
        {
            await _context.UserMessages.AddAsync(userMessage);
        }

        public void UpdateUserMessage(UserMessages userMessage)
        {
            _context.UserMessages.Update(userMessage);
        }

        public void DeleteUserMessage(UserMessages userMessage)
        {
            _context.UserMessages.Remove(userMessage);
        }

        public async Task<IEnumerable<UserMessageDto>> GetAllUserMessageByIdAsync(int senderId, int receiverId)
        {
            return await _context.UserMessages
         .Where(um =>
             (um.SenderId == senderId && um.ReceiverId == receiverId) ||
             (um.SenderId == receiverId && um.ReceiverId == senderId))
         .Include(um => um.Sender)
         .Include(um => um.Receiver)
         .Include(um => um.userMessageReactions)
             .ThenInclude(r => r.User)
         .Include(um => um.userMessageStatuses)
             .ThenInclude(ms => ms.Receiver)
         .Include(um => um.userMessageFiles)
             .ThenInclude(f => f.File)
         .Select(um => new UserMessageDto
         {
             UserMessageId = um.UserMessageId,
             SenderId = um.SenderId,
             SenderName = um.Sender.UserName,
             SenderAvatar = um.Sender.Avatar,
             ReceiverId = um.ReceiverId,
             ReceiverName = um.Receiver.UserName,
             ReceiverAvatar = um.Receiver.Avatar,
             MessageType = um.MessageType,
             Content = um.Content,
             CreateAt = um.CreateAt,
             Status = um.Status,
             Reactions = um.userMessageReactions.Select(r => new UserMessageReactionDto
             {
                 ReactionId = r.ReactionId,
                 UserId = r.UserId,
                 UserName = r.User.UserName,
                 UserAvatar = r.User.Avatar,
                 ReactionType = r.ReactionType,
                 ReactionDate = r.ReactionDate,
                 ReactionCount = r.ReactionCount
             }).ToList(),
             MessageStatuses = um.userMessageStatuses.Select(ms => new UserMessageStatusDto
             {
                 UserMessageStatusId = ms.UserMessageStatusId,
                 ReceiverId = ms.ReceiverId,
                 ReceiverName = ms.Receiver.UserName,
                 IsReceived = ms.IsReceived,
                 IsRead = ms.IsRead,
                 ReceivedAt = ms.ReceivedAt,
                 ReadAt = ms.ReadAt
             }).ToList(),
             Files = um.userMessageFiles.Select(f => new UserMessageFileDto
             {
                 FileId = f.FileId,
                 FileName = f.File.TenFile,
                 FileUrl = f.File.DuongDan,
                 TypeFile = f.File.LoaiFile,
                 SizeFile = f.File.KichThuocFile
             }).ToList()
         })
         .ToListAsync();
        }
    }
}
