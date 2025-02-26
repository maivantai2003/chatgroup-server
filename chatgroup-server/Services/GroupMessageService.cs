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
            return await _groupMessageRepository.GetMessageByIdAsync(id);
        }

        public async Task<bool> SendMessageAsync(GroupMessages message)
        {
            await _groupMessageRepository.AddMessageAsync(message);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateMessageAsync(GroupMessages message)
        {
            _groupMessageRepository.UpdateMessage(message);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteMessageAsync(int id)
        {
            var message = await _groupMessageRepository.GetMessageByIdAsync(id);
            if (message == null) return false;

            _groupMessageRepository.DeleteMessage(message);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}
