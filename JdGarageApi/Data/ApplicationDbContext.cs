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

            builder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("Vehicles");
                entity.HasDiscriminator<string>("VehicleType")
                    .HasValue<Bike>("Bike")
                    .HasValue<Car>("Car");
            });
        }

        public DbSet<BikeCategory> BikeCategory { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Brands> Brands { get; set; }
    }
}
