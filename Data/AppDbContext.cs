using AllulExpressClientApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AllulExpressClientApi.Data
{
    public class AppDbContext : DbContext
    {


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        // Represent the "Employees" table
        public DbSet<Drivers> Drivers { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Cities> Cities { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<ValidTokenClients> ValidTokenClients { get; set; }
        // public DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}
