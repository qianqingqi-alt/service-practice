using Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HttpApi.Controllers.System
{
    [Route("api/System/[controller]/[action]")]
    [ApiController]
    public class TestController : Controller
    {
        public TestController() { }

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<string> GetTest()
        {
            var testString = EncryptUtil.AESEncrypt("Data Source=10.0.0.1;Initial Catalog=Qiangqingqi;UID=sa;PWD=1qaz!QAZ; MultipleActiveResultSets=true;TrustServerCertificate=true", "QGDebXldIrMzO2IL7Ow9UVJ/gi6eFkQK");
            return Ok(testString);
        }
    }
}
