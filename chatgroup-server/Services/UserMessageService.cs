using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;

namespace chatgroup_server.Services
{
    public class UserMessageService:IUserMessageService
    {
        private readonly IUserMessageRepository _userMessageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserMessageService(IUserMessageRepository userMessageRepository, IUnitOfWork unitOfWork)
        {
            _userMessageRepository = userMessageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserMessages?> GetUserMessageByIdAsync(int messageId)
        {
            return await _userMessageRepository.GetUserMessageByIdAsync(messageId);
        }

        public async Task<IEnumerable<UserMessages>> GetMessagesBySenderIdAsync(int senderId)
        {
            return await _userMessageRepository.GetMessagesBySenderIdAsync(senderId);
        }

        public async Task<IEnumerable<UserMessages>> GetMessagesByReceiverIdAsync(int receiverId)
        {
            return await _userMessageRepository.GetMessagesByReceiverIdAsync(receiverId);
        }

        public async Task<bool> AddUserMessageAsync(UserMessages userMessage)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _userMessageRepository.AddUserMessageAsync(userMessage);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> UpdateUserMessageAsync(UserMessages userMessage)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _userMessageRepository.UpdateUserMessage(userMessage);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> DeleteUserMessageAsync(int messageId)
        {
            var userMessage = await _userMessageRepository.GetUserMessageByIdAsync(messageId);
            if (userMessage == null) return false;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _userMessageRepository.DeleteUserMessage(userMessage);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }
    }
}
