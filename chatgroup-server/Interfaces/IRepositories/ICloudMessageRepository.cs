using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface ICloudMessageRepository
    {
        Task<IEnumerable<CloudMessageResponseDto>> GetMessagesByUserIdAsync(int userId);
        Task<CloudMessage?> GetMessageByIdAsync(int id);
        Task<CloudMessageResponseDto?> GetCloudMessageByIdAsync(int id);
        Task AddMessageAsync(CloudMessage message);
        void UpdateMessage(CloudMessage message);
        void DeleteMessage(CloudMessage message);
    }
}
