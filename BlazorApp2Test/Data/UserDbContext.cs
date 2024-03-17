using BlazorApp2Test.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2Test.Data
{
    public class UserDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
    }
}