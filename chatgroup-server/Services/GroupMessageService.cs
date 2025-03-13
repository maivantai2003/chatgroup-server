using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;

namespace chatgroup_server.Services
{
    public class GroupMessageService:IGroupMessageService
    {
        private readonly IGroupMessageRepository _groupMessageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GroupMessageService(IGroupMessageRepository groupMessageRepository, IUnitOfWork unitOfWork)
        {
            _groupMessageRepository = groupMessageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GroupMessages>> GetAllMessagesAsync()
        {
            return await _groupMessageRepository.GetAllMessagesAsync();
        }

        public async Task<GroupMessages?> GetMessageByIdAsync(int id)
        {
            //return await _groupMessageRepository.GetGroupMessageByIdAsync(id);
            throw new NotImplementedException();
        }

        public async Task<bool> SendMessageAsync(GroupMessages message)
        {
            //await _groupMessageRepository.AddMessageAsync(message);
            //return await _unitOfWork.SaveChangesAsync() > 0;
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateMessageAsync(GroupMessages message)
        {
            _groupMessageRepository.UpdateMessage(message);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteMessageAsync(int id)
        {
            //var message = await _groupMessageRepository.GetMessageByIdAsync(id);
            //if (message == null) return false;

            //_groupMessageRepository.DeleteMessage(message);
            //return await _unitOfWork.SaveChangesAsync() > 0;
            throw new NotImplementedException();
        }
        public async Task<ApiResponse<GroupMessageResponseDto>> AddGroupMessage(GroupMessages groupMessage)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _groupMessageRepository.AddGroupMessageAsync(groupMessage);
                await _unitOfWork.CommitAsync();
                var result = await _groupMessageRepository.GetGroupMessageByIdAsync(groupMessage.GroupedMessageId);
                return ApiResponse<GroupMessageResponseDto>.SuccessResponse("Nhắn tin thành công", result);
            }
            catch (Exception ex) {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<GroupMessageResponseDto>.ErrorResponse("Nhắn tin thành công", new List<string>()
                {
                    ex.Message
                });
            }
        }

        public async Task<ApiResponse<IEnumerable<GroupMessageResponseDto>>> GetAllGroupMessageById(int groupId)
        {
            try
            {
                var result=await _groupMessageRepository.GetAllGroupMessageById(groupId);
                return ApiResponse<IEnumerable<GroupMessageResponseDto>>.SuccessResponse("Lấy danh sách tin nhắn nhóm thành công", result);
            }
            catch (Exception ex) {
                return ApiResponse<IEnumerable<GroupMessageResponseDto>>.ErrorResponse("Lấy danh sách tin nhắn nhóm thành công", new List<string>()
                {
                    ex.Message
                });
            }
        }
    }
}
