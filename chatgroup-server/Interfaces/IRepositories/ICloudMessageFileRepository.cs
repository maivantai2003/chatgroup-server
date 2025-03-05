using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface ICloudMessageFileRepository
    {
        Task<IEnumerable<CloudMessageFile>> GetAllAsync();
        Task<CloudMessageFile?> GetByIdAsync(int id);
        Task AddAsync(CloudMessageFile cloudMessageFile);
        void Update(CloudMessageFile cloudMessageFile);
        void Delete(CloudMessageFile cloudMessageFile);
    }
}
