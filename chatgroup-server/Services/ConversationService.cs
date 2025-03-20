using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using chatgroup_server.Common;
using chatgroup_server.Dtos;

namespace chatgroup_server.Services
{
    public class ConversationService:IConversationService
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RedisService _redisService;
        private const string ConversationCacheKey = "conversation_{0}";
        private const string AllConversationsCacheKey = "all_conversations_{0}";
        public ConversationService(IConversationRepository conversationRepository, IUnitOfWork unitOfWork, RedisService redisService)
        {
            _conversationRepository = conversationRepository;
            _unitOfWork = unitOfWork;
            _redisService = redisService;
        }

        public async Task<ApiResponse<Conversation>> AddConversationAsync(Conversation conversation)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _conversationRepository.AddAsync(conversation);
                await _unitOfWork.CommitAsync();
                return ApiResponse<Conversation>.SuccessResponse("Thêm thành công", conversation);
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<Conversation>.ErrorResponse("Thêm không thành công", new List<string>()
                {
                    ex.Message
                });
            }
        }

        public async Task<ApiResponse<Conversation?>> GetConversationByIdAsync(int id)
        {
            try
            {
                var result= await _conversationRepository.GetByIdAsync(id);
                return ApiResponse<Conversation?>.SuccessResponse("Tìm kiếm thành công", result);
            }
            catch (Exception ex) {
                return ApiResponse<Conversation?>.ErrorResponse("Tìm kiếm không thành công", new List<string>()
                {
                    ex.Message
                });
            }
        }

        public async Task<ApiResponse<Conversation>> UpdateConversationAsync(Conversation conversation)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _conversationRepository.Update(conversation);
                await _unitOfWork.CommitAsync();
                return ApiResponse<Conversation>.SuccessResponse("Cập nhật thành công", conversation);
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<Conversation>.ErrorResponse("Cập nhật không thành công", new List<string>()
                {
                    ex.Message
                });
            }
        }

        public async Task<bool> DeleteConversationAsync(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var conversation = await _conversationRepository.GetByIdAsync(id);
                if (conversation == null) return false;

                _conversationRepository.Delete(conversation);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<ApiResponse<IEnumerable<Conversation>>> GetAllConversation(int userId)
        {
            try
            {
                var result = await _conversationRepository.GetAllConversation(userId);
                return ApiResponse<IEnumerable<Conversation>>.SuccessResponse("Lấy danh sách thành công", result);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<Conversation>>.ErrorResponse("Lấy danh sách không thành công", new List<string>()
                {
                    ex.Message
                });
            }
        }

        public async Task<ApiResponse<ConversationUpdateInfor>> UpdateInForConversation(ConversationUpdateInfor conversation)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _conversationRepository.UpdateInForConversation(conversation);
                await _unitOfWork.CommitAsync();
                return ApiResponse<ConversationUpdateInfor>.SuccessResponse("Cập thông tin thành công", conversation);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<ConversationUpdateInfor>.ErrorResponse("Cập nhật thông tin không thành công", new List<string>
                {
                    ex.Message
                });
            }
        }

        public async Task<ApiResponse<ConversationUpdateGroupDto>> UpdateConversationGroup(ConversationUpdateGroupDto group)
        {
            await _unitOfWork.BeginTransactionAsync();  
            try
            {
                await _conversationRepository.UpdateConversationGroup(group);
                await _unitOfWork.CommitAsync();
                return ApiResponse<ConversationUpdateGroupDto>.SuccessResponse("Cập nhật thành công", group);
            }
            catch (Exception ex) { 
                await _unitOfWork.RollbackAsync();
                return ApiResponse<ConversationUpdateGroupDto>.ErrorResponse("Cập nhật thất bại", new List<string> {
                    ex.Message
                });
            }
        }
    }
}
