using chatgroup_server.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupDetailController : ControllerBase
    {
        private readonly IGroupDetailService _groupDetailService;
        public GroupDetailController(IGroupDetailService groupDetailService) { 
            _groupDetailService = groupDetailService;   
        }
    }
}
