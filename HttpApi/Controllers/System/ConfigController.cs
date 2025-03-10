using Application.System.Services;
using Application.System.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HttpApi.Controllers.System
{
    [Route("api/System/[controller]/[action]")]
    [ApiController]
    public class ConfigController : Controller
    {
        private readonly ConfigSrv _configSrv;
        public ConfigController(ConfigSrv configSrv)
        {
            _configSrv = configSrv;
        }

        /// <summary>
        /// 获取公开的配置
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ConfigDto>> GetPublicConfigs()
        {
            IEnumerable<ConfigDto> configValue = _configSrv.GetPublicConfigs();
            return Ok(configValue);
        }
    }
}
