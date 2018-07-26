using Microsoft.EntityFrameworkCore;

namespace NanoFabric.Docimax.Identity.Models
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<LoginUser> LoginUsers { get; set; }
    }
}
