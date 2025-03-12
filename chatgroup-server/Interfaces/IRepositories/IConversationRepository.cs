using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IConversationRepository
    {
        Task<Conversation> AddAsync(Conversation conversation);
        Task<Conversation?> GetByIdAsync(int id);
        Task<IEnumerable<Conversation>> GetAllConversation(int userId);
        Task UpdateInForConversation(ConversationUpdateInfor conversation);    
        Task Update(Conversation conversation);
        void Delete(Conversation conversation);
    }
}
