
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface ISendGmailService
    {
        public Task SendGmailAsync(Gmail gmail);
    }
}
