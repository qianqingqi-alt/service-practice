using Application.System.Dtos;
using AutoMapper;
using Domain.System.Entities;
using Domain.System.Services;
using Infrastructure.Exception;

namespace Application.System.Services
{
    public class FileStorageSrv
    {
        private readonly IMapper _mapper;
        private readonly FileStorageService _fileStorageService;
        private readonly ConfigSrv _configSrv;

        public FileStorageSrv(IMapper mapper, FileStorageService fileStorageService, ConfigSrv configSrv)
        {
            _mapper = mapper;
            _fileStorageService = fileStorageService;
            _configSrv = configSrv;
        }

        public Guid CreateFileStorage(FileStorageDto fileStorage)
        {
            //if (!TestConnect(fileStorage.FileStorageType, fileStorage.FileSystemBasePath, fileStorage.AzureStorageAccountConnectionString, fileStorage.AzureStorageAccountContainerName))
            //{
            //    throw new CustomException(_localizer["Sys.Connect.Failure"]);
            //}
            return _fileStorageService.CreateFileStorage(_mapper.Map<FileStorage>(fileStorage));
        }

        public FileStorageDto GetFileStorage(Guid fileStorageId)
        {
            return _mapper.Map<FileStorageDto>(_fileStorageService.GetFileStorage(fileStorageId));
        }

        public IEnumerable<FileStorageNameDto> GetFileStorages()
        {
            return _mapper.Map<IEnumerable<FileStorageNameDto>>(_fileStorageService.GetFileStorages());
        }

        public void setDefaultFileStorage(Guid fileStorageId)
        {
            List<ConfigDto> configs = new List<ConfigDto>
                {
                     new ConfigDto{ConfigKey="System.FileStorageId",ConfigValue=fileStorageId.ToString()}
                };
            _configSrv.SetConfigs(configs);
        }

        public void UpdateFileStorage(FileStorageDto fileStorage)
        {
            _fileStorageService.UpdateFileStorage(_mapper.Map<FileStorage>(fileStorage));
        }

        public void DeleteFileStorage(Guid fileStorageId)
        {
            _fileStorageService.DeleteFileStorage(fileStorageId);
        }
    }
}
