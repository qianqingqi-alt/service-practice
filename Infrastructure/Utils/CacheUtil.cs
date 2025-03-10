using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace Infrastructure.Utils
{
    public static class CacheUtil
    {
        private static IDistributedCache _cache;
        static CacheUtil()
        {
            if (_cache == null)
            {
                _cache = DIServiceUtil.GetService<IDistributedCache>();
            }
        }
        public static T? GetCache<T>(IDistributedCache cache, string cacheKey, TimeSpan cacheExpiration, Func<T> getData)
        {
            var cacheValue = cache.GetString(cacheKey);
            if (cacheValue != null)
            {
                return JsonConvert.DeserializeObject<T>(cacheValue);
            }
            else
            {
                var datas = getData();
                if (datas != null)
                {
                    var cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(cacheExpiration);
                    var binaryData = JsonConvert.SerializeObject(datas);
                    _cache.SetString(cacheKey, binaryData, cacheEntryOptions);
                }

                return datas;
            }
        }

        /// <summary>
        /// 获取缓存的字符串(键)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string? GetString(string key)
        {
            return _cache.GetString(key);
        }
        /// <summary>
        /// 设置缓存的字符串(键,值,时间/秒)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expires">过期时间</param>
        /// <returns></returns>
        public static void SetString(string key, string value, double expires = 0, bool isSlidingExp = false)
        {
            if (expires > 0)
            {
                var options = new DistributedCacheEntryOptions();

                if (isSlidingExp)
                    options.SlidingExpiration = TimeSpan.FromSeconds(expires);
                else
                    options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expires); // 设置过期时间

                _cache.SetString(key, value, options);
            }
            else
            {
                _cache.SetString(key, value);
            }
        }
        /// <summary>
        /// 删除缓存的字符串(键)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
