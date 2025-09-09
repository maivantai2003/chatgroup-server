using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IServices;
using Microsoft.AspNetCore.DataProtection;

namespace chatgroup_server.Services
{
    public class RecaptchaService : IRecaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public RecaptchaService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<bool> Verify(string token)
        {
            var secretKey = _configuration["ReCaptchaSettings:SecretKey"];
            var response = await _httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={token}",
            null);
            var json = await response.Content.ReadAsStringAsync();
            var result = System.Text.Json.JsonSerializer.Deserialize<ReCaptchaResponse>(json);
            return result?.success ?? false;
        }
    }
}
