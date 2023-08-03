using Microsoft.EntityFrameworkCore;
using SharedShoppingListApi.Models;

namespace SharedShoppingListApi.Data
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}
