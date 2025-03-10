using Domain.EntityFramework;
using Domain.System.Entities;
using Infrastructure.Exception;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Domain.System.Services
{
    public class ConfigService
    {
        private DataDbContext _dbContext;
        private readonly IDistributedCache _cache;
        public ConfigService(DataDbContext dataDbContext, IDistributedCache cache)
        {
            _dbContext = dataDbContext;
            _cache = cache;
        }

        public IQueryable<Config> GetPublicConfigs()
        {
            return _dbContext.Config.AsNoTracking().Where(e => e.IsPublic == true);
        }

        /// <summary>
        /// 通过Key获取配置
        /// </summary>
        public Config GetConfig(string key)
        {
            string cacheKey = "Cache_Config";
            var configs = CacheUtil.GetCache<IEnumerable<Config>>(_cache, cacheKey, TimeSpan.FromMinutes(60), () =>
            {
                return _dbContext.Config.AsNoTracking().ToList();
            });
            return configs.Where(e => e.ConfigKey == key).FirstOrDefault();
        }

        /// <summary>
        /// 设置配置
        /// </summary>
        public void SetConfigs(IEnumerable<Config> configs)
        {
            if (configs == null)
            {
                throw new CustomException("配置为空");
            }
            var configList = configs.ToList();
            if (!configList.Any())
            {
                return; // 如果传入空集合，直接返回
            }
            var configKeys = configList.Select(c => c.ConfigKey).ToHashSet();
            var existingConfigs = _dbContext.Config
                .Where(c => configKeys.Contains(c.ConfigKey))
                .ToDictionary(c => c.ConfigKey);
            foreach (var config in configList)
            {
                if (!existingConfigs.TryGetValue(config.ConfigKey, out var existingConfig))
                {
                    throw new CustomException($"不存在 key 值 {config.ConfigKey}");
                }
                existingConfig.UpdateConfigValue(config.ConfigValue);
                _dbContext.Config.Update(existingConfig);
            }

            CacheUtil.Remove("Cache_Config");
            _dbContext.SaveChanges();
        }
    }
}
