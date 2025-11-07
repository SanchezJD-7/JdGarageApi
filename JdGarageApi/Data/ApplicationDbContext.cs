using JdGarageApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JdGarageApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<BikeCategory> BikeCategory { get; set; }
        public DbSet<Bike> Bike { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Brands> Brands { get; set; }

    }
}
