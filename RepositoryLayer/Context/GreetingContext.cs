using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using ModelLayer.Models;

namespace RepositoryLayer.Context
{
    public class GreetingContext : DbContext
    {
        public GreetingContext(DbContextOptions<GreetingContext> options) : base(options) { }

        public DbSet<GreetingModel> Greetings { get; set; }
        public DbSet<User> Users { get; set; }
    }
}