using chatgroup_server.Common;
using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;

namespace chatgroup_server.Services
{
    public class UserMessageReactionService:IUserMessageReactionService
    {
        private readonly IUserMessageReactionRepository _reactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UserMessageReactionService(IUserMessageReactionRepository reactionRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _reactionRepository = reactionRepository;   
        }
        public async Task<ApiResponse<UserMessageReaction>> AddReactionAsync(UserMessageReaction reaction)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _reactionRepository.AddReactionAsync(reaction);
                await _unitOfWork.CommitAsync();
                return ApiResponse<UserMessageReaction>.SuccessResponse("Reaction added successfully",reaction);
            }
            catch (Exception ex) { 
                await _unitOfWork.RollbackAsync();
                return ApiResponse<UserMessageReaction>.ErrorResponse("Reaction added successfully", new List<string>()
                {
                    ex.Message  
                });
            }
        }

        public async Task<ApiResponse<IEnumerable<UserMessageReaction>>> GetReactionsByMessageIdAsync(int messageId)
        {
            try
            {
                var reactions = await _reactionRepository.GetReactionsByMessageIdAsync(messageId);
                return ApiResponse<IEnumerable<UserMessageReaction>>.SuccessResponse("Reactions retrieved", reactions);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<UserMessageReaction>>.ErrorResponse("Reaction added successfully", new List<string>()
                {
                    ex.Message
                });
            }
        }
    }
}
