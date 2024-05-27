using Microsoft.EntityFrameworkCore;
using CloudDevelopmentPOE.Models;

namespace CloudDevelopmentPOE.Data
{
    public class KhumaloCraftEmporiumDBContext : DbContext
    {
        public KhumaloCraftEmporiumDBContext(DbContextOptions<KhumaloCraftEmporiumDBContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}
