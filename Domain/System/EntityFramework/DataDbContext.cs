using Domain.EntityFramework;
using Domain.System.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.EntityFramework
{
    public partial class DataDbContext : BaseDbContext
    {
        public DbSet<Config> Config { get; set; }
        public DbSet<FileStorage> FileStorage { get; set; }
        public DbSet<User> User { get; set; }

        private void SystemModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Config>(b => {
                b.Property(u => u.CreateBy).HasColumnType("BINARY(16)");
                b.Property(u => u.UpdateBy).HasColumnType("BINARY(16)");
            });
            modelBuilder.Entity<FileStorage>(b => { });
            modelBuilder.Entity<User>(b =>
            {
                b.Property(u => u.UserId).HasColumnType("BINARY(16)");
                b.Property(u=> u.CreateBy).HasColumnType("BINARY(16)");
                b.Property(u=> u.UpdateBy).HasColumnType("BINARY(16)");
            });
        }
    }
}
