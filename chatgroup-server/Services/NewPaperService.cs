using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IServices;
using CodeHollow.FeedReader;
using System.Text.RegularExpressions;

namespace chatgroup_server.Services
{
    public class NewPaperService : INewPaperService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly RedisService _redisService;
        private readonly IUserContextService _userContextService;
        public NewPaperService(IHttpClientFactory httpClientFactory, IConfiguration configuration,RedisService redisService,IUserContextService userContextService)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _redisService = redisService;
            _userContextService = userContextService;
        }
        public async Task<List<NewPaperDto>> GetNewPapersAsync()
        {
            var Key=$"NewPapersCache-{_userContextService.GetCurrentUserId()}";
            var listFeed=await _redisService.GetCacheAsync<List<NewPaperDto>>(Key);
            if (listFeed != null && listFeed.Any()) { 
                return listFeed;
            }
            string rssUrl=_configuration.GetValue<string>("RssFeedUrl") ?? "https://vnexpress.net/rss/tin-moi-nhat.rss";
            var feed=await FeedReader.ReadAsync(rssUrl);
            var result= feed.Items.Select(item => new NewPaperDto
            {
                Title = item.Title,
                Link = item.Link,
                Published = item.PublishingDate ?? DateTime.MinValue,
                Description = CleanDescription(item.Description),
                Image = ExtractImageUrl(item.Description) ?? ""

            }).ToList();
            await _redisService.SetCacheAsync<List<NewPaperDto>>(Key,result,TimeSpan.FromMinutes(10));
            return result;
        }
        private string? ExtractImageUrl(string? description)
        {
            if (string.IsNullOrEmpty(description)) return null;

            var match = Regex.Match(description, "<img.*?src=[\"'](.+?)[\"'].*?>");
            return match.Success ? match.Groups[1].Value : null;
        }
        private string CleanDescription(string? description)
        {
            if (string.IsNullOrEmpty(description)) return "";
            // Bỏ thẻ <img> và <a> để chỉ giữ text
            return Regex.Replace(description, "<.*?>", "").Trim();
        }
    }
}
