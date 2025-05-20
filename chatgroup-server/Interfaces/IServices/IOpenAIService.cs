using chatgroup_server.Common;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IOpenAIService
    {
        Task TrainAsync(string openKey);
        Task<List<string>> SuggestAsync(string input);
        Task<ApiResponse<string>> QuestionChat(string question);
    }
}
