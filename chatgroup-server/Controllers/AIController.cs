using chatgroup_server.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly IOpenAIService _openAIService;
        public AIController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }
        [HttpPost]
        public async Task<IActionResult> QuestionChat(string question)
        {
            var response = await _openAIService.QuestionChat(question);
            if (!response.Success) {
                return Ok(response.Errors);
            }
            return Ok(response.Data);   
        }
    }
}
