using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options) { }
    }
}
