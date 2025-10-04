using chatgroup_server.Data;
using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Repositories
{
    public class GroupMessageRepository : IGroupMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupMessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GroupMessages>> GetAllMessagesAsync()
        {
            return await _context.GroupMessages
                                 .Include(gm => gm.Sender)
                                 .Include(gm => gm.Group)
                                 .ToListAsync();
        }

        public async Task<GroupMessageResponseDto?> GetGroupMessageByIdAsync(int id)
        {
            return await _context.GroupMessages.AsNoTracking().Where(x => x.GroupedMessageId == id).Select(
                x => new GroupMessageResponseDto()
                {
                    GroupedMessageId = x.GroupedMessageId,
                    SenderId = x.SenderId,
                    SenderName = x.Sender.UserName,
                    SenderAvatar = x.Sender.Avatar,
                    GroupId = x.GroupId,
                    ReplyToMessageId = x.ReplyToMessageId,
                    Content = x.Content,
                    MessageType = x.MessageType,
                    CreateAt = x.CreateAt,
                    Status = x.Status,
                    Files = x.groupMessageFiles.Select(f => new GroupMessageFileDto()
                    {
                        FileId = f.FileId,
                        FileName = f.File.TenFile,
                        FileUrl = f.File.DuongDan,
                        TypeFile = f.File.LoaiFile,
                        SizeFile = f.File.KichThuocFile
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task AddGroupMessageAsync(GroupMessages message)
        {
            await _context.GroupMessages.AddAsync(message);
        }

        public void UpdateMessage(GroupMessages message)
        {
            _context.GroupMessages.Update(message);
        }

        public void DeleteMessage(GroupMessages message)
        {
            _context.GroupMessages.Remove(message);
        }

        public async Task<IEnumerable<GroupMessageResponseDto?>> GetAllGroupMessageById(int groupId, DateTime? lastCreateAt = null, int pageSize = 10)
        {
            var query = _context.GroupMessages.AsNoTracking().Where(x=>x.GroupId==groupId);
            if (lastCreateAt.HasValue)
            {
                query = query.Where(x=>x.CreateAt<lastCreateAt);
            }
            var messages=await query.OrderByDescending(x=>x.CreateAt).Take(pageSize)
                .Select(x => new GroupMessageResponseDto()
                {
                   GroupedMessageId = x.GroupedMessageId,
                   SenderId = x.SenderId,
                   SenderName = x.Sender.UserName,
                   SenderAvatar = x.Sender.Avatar,
                   GroupId = x.GroupId,
                   ReplyToMessageId = x.ReplyToMessageId,
                   Content = x.Content,
                   MessageType = x.MessageType,
                   CreateAt = x.CreateAt,
                   Status = x.Status,
                   Files = x.groupMessageFiles.Select(f => new GroupMessageFileDto()
                      {
                        FileId = f.FileId,
                        FileName = f.File.TenFile,
                        FileUrl = f.File.DuongDan,
                        TypeFile = f.File.LoaiFile,
                        SizeFile = f.File.KichThuocFile
                        }).ToList()
                 }).ToListAsync();
            return messages.OrderBy(m => m.CreateAt);
            //return await _context.GroupMessages.AsNoTracking().Where(x => x.GroupId == groupId)
            //     .Select(x => new GroupMessageResponseDto()
            //     {
            //         GroupedMessageId = x.GroupedMessageId,
            //         SenderId = x.SenderId,
            //         SenderName = x.Sender.UserName,
            //         SenderAvatar = x.Sender.Avatar,
            //         GroupId = x.GroupId,
            //         ReplyToMessageId = x.ReplyToMessageId,
            //         Content = x.Content,
            //         MessageType = x.MessageType,
            //         CreateAt = x.CreateAt,
            //         Status = x.Status,
            //         Files = x.groupMessageFiles.Select(f => new GroupMessageFileDto()
            //         {
            //             FileId = f.FileId,
            //             FileName = f.File.TenFile,
            //             FileUrl = f.File.DuongDan,
            //             TypeFile = f.File.LoaiFile,
            //             SizeFile = f.File.KichThuocFile
            //         }).ToList()
            //     }).ToListAsync();
        }
    }
}
