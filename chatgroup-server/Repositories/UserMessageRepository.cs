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

        public async Task<IEnumerable<UserMessageResponseDto>> GetAllUserMessageByIdAsync(int senderId, int receiverId, DateTime? lastCreateAt = null, int pageSize = 10)
        {
            var query = _context.UserMessages.AsNoTracking().Where(x => (x.SenderId == senderId && x.ReceiverId == receiverId)
                 || (x.SenderId == receiverId && x.ReceiverId == senderId));
            if (lastCreateAt.HasValue)
            {
                query = query.Where(x => x.CreateAt < lastCreateAt);
            }
            var messages=await query.OrderByDescending(x => x.CreateAt)
            .Take(pageSize)
            .Select(x => new UserMessageResponseDto
            {
                UserMessageId = x.UserMessageId,
                SenderId = x.SenderId,
                SenderName = x.Sender.UserName,
                SenderAvatar = x.Sender.Avatar,
                ReceiverId = x.ReceiverId,
                ReceiverName = x.Receiver.UserName,
                ReceiverAvatar = x.Receiver.Avatar,
                MessageType = x.MessageType,
                Content = x.Content,
                CreateAt = x.CreateAt,
                Status = x.Status,
                Files = x.userMessageFiles.Select(f => new UserMessageFileDto
                {
                    FileId = f.File.MaFile,
                    FileName = f.File.TenFile,
                    TypeFile = f.File.LoaiFile,
                    FileUrl = f.File.DuongDan,
                    SizeFile = f.File.KichThuocFile
                }).ToList()
            })
            .ToListAsync();
            return messages.OrderBy(m => m.CreateAt);
            //return await _context.UserMessages.AsNoTracking().Where(x => (x.SenderId == senderId && x.ReceiverId == receiverId)
            //|| (x.SenderId == receiverId && x.ReceiverId == senderId)).Select(
            //    x => new UserMessageResponseDto()
            //    {
            //        UserMessageId=x.UserMessageId,
            //        SenderId=x.SenderId,
            //        SenderName=x.Sender.UserName,
            //        SenderAvatar=x.Sender.Avatar,
            //        ReceiverId=x.ReceiverId,
            //        ReceiverName=x.Receiver.UserName,
            //        ReceiverAvatar=x.Receiver.Avatar,
            //        MessageType=x.MessageType,
            //        Content=x.Content,
            //        CreateAt=x.CreateAt,
            //        Status=x.Status,
            //        Files=x.userMessageFiles.Select(f=>new UserMessageFileDto()
            //        {
            //            FileId=f.File.MaFile,
            //            FileName=f.File.TenFile,
            //            TypeFile=f.File.LoaiFile,
            //            FileUrl=f.File.DuongDan,
            //            SizeFile=f.File.KichThuocFile
            //        }).ToList()
            //    }).ToListAsync();
        }

        public async Task<UserMessageResponseDto> GetUserMessageById(int userMessageId)
        {
            var result = await _context.UserMessages.AsNoTracking().Where(x => x.UserMessageId == userMessageId).Select(
                x => new UserMessageResponseDto()
                {
                    UserMessageId = x.UserMessageId,
                    SenderId = x.SenderId,
                    SenderName = x.Sender.UserName,
                    SenderAvatar = x.Sender.Avatar,
                    ReceiverId = x.ReceiverId,
                    ReceiverName = x.Receiver.UserName,
                    ReceiverAvatar = x.Receiver.Avatar,
                    MessageType = x.MessageType,
                    Content = x.Content,
                    CreateAt = x.CreateAt,
                    Status = x.Status
                }).FirstOrDefaultAsync();
            return result;
        }
    }
}
