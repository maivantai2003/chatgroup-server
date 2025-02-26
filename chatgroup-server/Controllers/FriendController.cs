using chatgroup_server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly FriendService _friendService;
        public FriendController(FriendService friendService)
        {
            _friendService = friendService;
        }   
    }
}
