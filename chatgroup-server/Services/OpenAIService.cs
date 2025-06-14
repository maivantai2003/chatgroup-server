using chatgroup_server.Common;
using chatgroup_server.Interfaces.IServices;
using LangChain.Chains;
using LangChain.Providers.OpenAI;
//using LangChain.VectorStores.InMemory;
using LangChain.Schema;
using LangChain.Memory;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Microsoft.Identity.Client;
namespace chatgroup_server.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly IConfiguration _configuration;
        //private readonly InMemo _vectorStore;
        public OpenAIService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<ApiResponse<string>> QuestionChat(string question)
        {
            try
            {
                var apiKey = _configuration["OpenAI:ApiKey"];
                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    return ApiResponse<string>.ErrorResponse("Missing OpenAI API key.");
                }

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                if (question.Contains("create image", StringComparison.OrdinalIgnoreCase) ||
                    question.Contains("generate image", StringComparison.OrdinalIgnoreCase) ||
                    question.Contains("draw", StringComparison.OrdinalIgnoreCase) || question.Contains("hình ảnh", StringComparison.OrdinalIgnoreCase))
                {
                    var requestBody = new
                    {
                        prompt = question,
                        n = 1,
                        size = "1024x1024"
                    };

                    var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("https://api.openai.com/v1/images/generations", jsonContent);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        return ApiResponse<string>.ErrorResponse($"OpenAI error: {response.StatusCode} - {responseString}", new List<string> { responseString });
                    }

                    var json = JsonDocument.Parse(responseString);
                    var imageUrl = json.RootElement
                        .GetProperty("data")[0]
                        .GetProperty("url")
                        .GetString();

                    return ApiResponse<string>.SuccessResponse("Successfully generated image.", imageUrl ?? "No image generated.");
                }
                else
                {
                    var requestBody = new
                    {
                        model = "gpt-4o-mini",
                        store = true,
                        messages = new[] { new { role = "user", content = question } }
                    };

                    var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", jsonContent);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        return ApiResponse<string>.ErrorResponse($"OpenAI error: {response.StatusCode} - {responseString}", new List<string> { responseString });
                    }

                    var json = JsonDocument.Parse(responseString);
                    var message = json.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString();

                    return ApiResponse<string>.SuccessResponse("Successfully retrieved answer from OpenAI.", message ?? "No response from model.");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.ErrorResponse($"Error occurred: {ex.Message}", new List<string> { ex.Message });
            }
        }


        public Task<List<string>> SuggestAsync(string input)
        {
            throw new NotImplementedException();
        }

        public Task TrainAsync(string openKey)
        {
            throw new NotImplementedException();
        }
    }
}
