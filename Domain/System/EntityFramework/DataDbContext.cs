using Domain.EntityFramework;
using Domain.System.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.EntityFramework
{
    public partial class DataDbContext : BaseDbContext
    {
        public DbSet<Config> Config { get; set; }
        public DbSet<FileStorage> FileStorage { get; set; }

        private void SystemModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Config>(b => { });
            modelBuilder.Entity<FileStorage>(b => { });
        }
    }
}
