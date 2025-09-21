using chatgroup_server.Dtos;

namespace chatgroup_server.Interfaces.IServices
{
    public interface INewPaperService
    {
        Task<List<NewPaperDto>> GetNewPapersAsync();
    }
}
