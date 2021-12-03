using Mars.Rover.Domain.Aggregate.Navigate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mars.Rover.Domain.Context.Context
{
    public class MarsRoverContext : IdentityDbContext<IdentityUser>
    {
        public MarsRoverContext()
        {
        }

        public MarsRoverContext(DbContextOptions<MarsRoverContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Plataeu> Plataeus { get; set; }
        public virtual DbSet<Surface> Surfaces { get; set; }
        public virtual DbSet<MarsRover> MarsRovers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(
                    "Data Source=DESKTOP-9OSHF57;Database=Mars.Rover;User Id=*****;Password=*****;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}