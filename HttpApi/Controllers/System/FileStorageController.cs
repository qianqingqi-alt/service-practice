using Application.System.Dtos;
using Application.System.Services;
using Microsoft.AspNetCore.Mvc;

namespace HttpApi.Controllers.System
{
    [Route("api/System/[controller]/[action]")]
    [ApiController]
    public class FileStorageController : Controller
    {
        private readonly FileStorageSrv _fileStorageSrv;
        public FileStorageController(FileStorageSrv fileStorageSrv)
        {
            _fileStorageSrv = fileStorageSrv;
        }

        [HttpPost]
        public ActionResult<Guid> CreateFileStorage([FromBody] FileStorageDto fileStorageRequest)
        {
            var res = _fileStorageSrv.CreateFileStorage(fileStorageRequest);
            return Ok(res);
        }

        [HttpGet]
        public ActionResult<FileStorageDto> GetFileStorage([FromQuery] Guid fileStorageId)
        {
            var res = _fileStorageSrv.GetFileStorage(fileStorageId);
            return Ok(res);
        }

        [HttpGet]
        public ActionResult<bool> setDefaultFileStorage([FromQuery] Guid fileStorageId)
        {
            _fileStorageSrv.setDefaultFileStorage(fileStorageId);
            return Ok(true);
        }

        [HttpGet]
        public ActionResult<IEnumerable<FileStorageNameDto>> GetFileStorages()
        {
            return Ok(_fileStorageSrv.GetFileStorages());
        }

        /// <summary>
        /// 修改文件存储
        /// </summary>
        [HttpPost]
        public ActionResult<bool> UpdateFileStorage([FromBody] FileStorageDto fileStorageRequest)
        {
            _fileStorageSrv.UpdateFileStorage(fileStorageRequest);
            return Ok(true);
        }

        /// <summary>
        /// 删除文件存储
        /// </summary>
        [HttpPost]
        public ActionResult<bool> DeleteFileStorage(Guid fileStorageId)
        {
            _fileStorageSrv.DeleteFileStorage(fileStorageId);
            return Ok(true);
        }
    }
}
