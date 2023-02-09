using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop_Models;
using System.Reflection;


namespace Shop_DataAccess
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<TestModel> TestModel { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<AppUser> AppUser { get; set; }
    }
}
