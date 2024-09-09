using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebAgroApp.Models;

namespace WebAgroApp.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>().HasKey(t => t.Id);
        }
    }
}