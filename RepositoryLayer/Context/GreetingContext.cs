using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using ModelLayer.Models;
using RepositoryLayer.Service;

namespace RepositoryLayer.Context
{
    public class GreetingContext : DbContext
    {
        public GreetingContext(DbContextOptions<GreetingContext> options) : base(options) { }

        public DbSet<GreetingModel> Greetings { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ Define One-to-Many Relationship
            modelBuilder.Entity<GreetingModel>()
                .HasOne(g => g.User)
                .WithMany(u => u.Greetings)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}