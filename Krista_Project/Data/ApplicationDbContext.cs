using Krista_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Krista_Project.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
             : base(options)
        { 
        }

        public DbSet<Booking>? Bookings { get; set; }
    }
}
