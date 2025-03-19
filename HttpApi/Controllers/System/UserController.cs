using Application.System.Dtos;
using Application.System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HttpApi.Controllers.System
{
    [Route("api/System/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserSrv _userSrv;
        public UserController(UserSrv userSrv)
        {
            _userSrv = userSrv;
        }

        [HttpGet]
        public ActionResult<FileStorageDto> GetUser([FromQuery] Guid userId)
        {
            return Ok(_userSrv.GetUser(userId));
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<string> CreateUser([FromBody] UserDto user)
        {
            return Ok(_userSrv.CreateUser(user));
        }
    }
}
