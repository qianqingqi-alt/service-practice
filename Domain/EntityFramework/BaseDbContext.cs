using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Infrastructure;

namespace Domain.EntityFramework
{
    public class BaseDbContext : DbContext
    {
        private UserContext _userContext;

        public BaseDbContext(UserContext userContext) : base()
        {
            _userContext = userContext;
        }

        public BaseDbContext(DbContextOptions options, UserContext userContext) : base(options)
        {
            _userContext = userContext;
        }

        /// <summary>
        /// 重写SaveChanges方法，添加自动处理处理创建人/时间，更新人/时间
        /// 调用不带参数的SaveChanges()，也会使用此方法
        /// </summary>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBefereSaveChanges();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// 重写SaveChangesAsync方法，添加自动处理处理创建人/时间，更新人/时间
        /// 调用不带参数的SaveChangesAsync()，也会使用此方法
        /// </summary>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBefereSaveChanges();
            return (await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken));
        }

        /// <summary>
        /// 继承BaseEntity的实体，自动处理创建人/时间，更新人/时间
        /// </summary>
        private void OnBefereSaveChanges()
        {
            var changeEntries = ChangeTracker.Entries();
            Guid currentUserId = _userContext.UserId;
            DateTime currentTime = DateTime.UtcNow;

            // 属性为 null 不更新
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
            {
                var properties = entry.Properties.Where(p => p.IsModified).ToList();

                foreach (var prop in properties)
                {
                    if (prop.CurrentValue == null)
                    {
                        prop.IsModified = false;
                    }
                }
            }

            foreach (var changeEntity in changeEntries)
            {
                if (changeEntity.Entity is BaseEntityWithCreateUpdate baseEntityWithCreateUpdate)
                {
                    switch (changeEntity.State)
                    {
                        case EntityState.Added:
                            //创建人和创建时间
                            baseEntityWithCreateUpdate.CreateBy = currentUserId;
                            baseEntityWithCreateUpdate.CreateTime = currentTime;
                            //改更新人和更新时间
                            baseEntityWithCreateUpdate.UpdateBy = currentUserId;
                            baseEntityWithCreateUpdate.UpdateTime = currentTime;
                            //changeEntity.Property("UpdateBy").IsModified = false;
                            //changeEntity.Property("UpdateTime").IsModified = false;
                            break;
                        case EntityState.Modified:
                            //创建人和创建时间（不改）
                            changeEntity.Property("CreateBy").IsModified = false;
                            changeEntity.Property("CreateTime").IsModified = false;
                            //更新人和更新时间
                            baseEntityWithCreateUpdate.UpdateBy = currentUserId;
                            baseEntityWithCreateUpdate.UpdateTime = currentTime;
                            break;
                    }
                }
            }
        }
    }
}
