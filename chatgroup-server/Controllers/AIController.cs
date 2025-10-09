using chatgroup_server.Interfaces.IServices;
using chatgroup_server.RabbitMQ.Models;
using chatgroup_server.RabbitMQ.Producer;
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
        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail(EmailMessageModel emailRequest)
        {
            try
            {
                var product = new EmailProducer();
                await product.SendEmailAsync(emailRequest);
                return Ok(new { message = "Email enqueued successfully!" });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
