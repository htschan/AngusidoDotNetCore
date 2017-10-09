using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Domain.UserConfiguration;

namespace Server.Data
{
    public class TpContext : IdentityDbContext<AppUser>
    {
        public TpContext() { }

        public TpContext(DbContextOptions options) : base(options) { }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=tp.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}