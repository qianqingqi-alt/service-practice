using Domain.EntityFramework;
using Domain.System.Entities;
using Infrastructure;
using Infrastructure.Exception;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Domain.System.Services
{
    public class UserService
    {
        private readonly DataDbContext _dataDbContext;
        private readonly IDistributedCache _cache;
        private readonly UserContext _userContext;
        public UserService(UserContext userContext, DataDbContext dataDbContext, IDistributedCache cache)
        {
            _userContext = userContext;
            _dataDbContext = dataDbContext;
            _cache = cache;
        }

        /// <summary>
        /// 根据ID获取用户
        /// </summary>
        public User GetUser(Guid userId)
        {
            var user = _dataDbContext.User
            .AsNoTracking()
            .Where(e => e.UserId == userId)
            .FirstOrDefault() ?? throw new CustomException("");
            return user;
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        public Guid CreateUser(User user)
        {
            _dataDbContext.User.Add(user);
            _dataDbContext.SaveChanges();
            return user.UserId;
        }
    }
}
