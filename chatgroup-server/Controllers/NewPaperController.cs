using chatgroup_server.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewPaperController : ControllerBase
    {
        private readonly INewPaperService _newPaperService;
        public NewPaperController(INewPaperService newPaperService)
        {
            _newPaperService = newPaperService;
        }
        [HttpGet]   
        
        public async Task<IActionResult> GetNewPapers()
        {
            var response = await _newPaperService.GetNewPapersAsync();
            return Ok(response);
        }
    }
}
