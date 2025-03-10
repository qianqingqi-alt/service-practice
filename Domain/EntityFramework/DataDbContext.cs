using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Domain.EntityFramework
{
    public partial class DataDbContext : BaseDbContext
    {
        private UserContext _userContext;

        public DataDbContext(DbContextOptions<DataDbContext> options, UserContext userContext) : base(options, userContext)
        {
            _userContext = userContext;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SystemModelCreating(modelBuilder);
            base.OnModelCreating(modelBuilder);

            //DateTime（或DateTime?）类型的属性，转换为UTC时间
            var dateTimeProperties = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));
            foreach (var property in dateTimeProperties)
            {
                property.SetValueConverter(new DateTimeWithTimeZoneConverter());
            }
        }
    }
}
