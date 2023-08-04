using Microsoft.EntityFrameworkCore;
using SharedShoppingListApi.Models;
using SharedShoppingListApi.Models.Submodels;

namespace SharedShoppingListApi.Data
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>()
                .HasKey(ug => new { ug.UserId, ug.GroupId });

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.Groups)
                .HasForeignKey(ug => ug.UserId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany(g => g.Members)
                .HasForeignKey(ug => ug.GroupId);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}
