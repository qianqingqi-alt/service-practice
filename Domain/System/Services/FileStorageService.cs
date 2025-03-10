using Domain.EntityFramework;
using Domain.System.Entities;
using Infrastructure.Exception;
using Microsoft.EntityFrameworkCore;

namespace Domain.System.Services
{
    public class FileStorageService
    {
        private DataDbContext _dbContext;

        public FileStorageService(DataDbContext dataDbContext)
        {
            _dbContext = dataDbContext;
        }

        /// <summary>
        /// 创建文件储存
        /// </summary>
        public Guid CreateFileStorage(FileStorage fileStorage)
        {
            if (_dbContext.FileStorage.AsNoTracking().Any(e => e.Name == fileStorage.Name))
            {
                throw new CustomException($"{fileStorage.Name} 名称已存在");
            }
            _dbContext.FileStorage.AddAsync(fileStorage);
            _dbContext.SaveChanges();
            return fileStorage.FileStorageId;
        }

        /// <summary>
        /// 根据Id获取文件储存
        /// </summary>
        public FileStorage GetFileStorage(Guid fileStorageId)
        {
            return _dbContext.FileStorage
            .AsNoTracking()
            .Include(e => e.CreateByUser)
            .Include(e => e.UpdateByUser)
            .Where(e => e.FileStorageId == fileStorageId)
            .FirstOrDefault() ?? throw new CustomException($"没有找到当前Id{fileStorageId}");
        }

        /// <summary>
        /// 获取所有文件储存
        /// </summary>
        public IQueryable<FileStorage> GetFileStorages()
        {
            return _dbContext.FileStorage.AsNoTracking();
        }

        /// <summary>
        /// 修改文件储存
        /// </summary>
        public void UpdateFileStorage(FileStorage fileStorage)
        {
            if (_dbContext.FileStorage.AsNoTracking().Any(e => e.FileStorageId != fileStorage.FileStorageId && e.Name == fileStorage.Name))
            {
                throw new CustomException("名称已存在");
            }
            _dbContext.FileStorage.Update(fileStorage);
            _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 删除文件储存
        /// </summary>
        public void DeleteFileStorage(Guid fileStorageId)
        {
            FileStorage fileStorage = GetFileStorage(fileStorageId);
            _dbContext.Remove(fileStorage);
            _dbContext.SaveChangesAsync();
        }
    }
}
