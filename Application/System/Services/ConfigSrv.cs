using Application.System.Dtos;
using AutoMapper;
using Domain.System.Entities;
using Domain.System.Services;
using Infrastructure.Exception;
namespace Application.System.Services
{
    public class ConfigSrv
    {
        private readonly IMapper _mapper;
        private readonly ConfigService _configService;

        public ConfigSrv(IMapper mapper, ConfigService configService)
        {
            _mapper = mapper;
            _configService = configService;
        }

        public IEnumerable<ConfigDto> GetPublicConfigs()
        {
            return _mapper.Map<IEnumerable<ConfigDto>>(_configService.GetPublicConfigs());
        }

        /// <summary>
        /// 通过Key获取配置
        /// </summary>
        public ConfigDto GetConfig(string key)
        {
            ConfigDto config = _mapper.Map<ConfigDto>(_configService.GetConfig(key));
            if (key == "System.FileStorageId")
            {
                try
                {
                    Guid.Parse(config.ConfigValue);
                }
                catch
                {
                    throw new CustomException($"不存在 key 值 {key}");
                }
            }
            return config;
        }

        public void SetConfigs(IEnumerable<ConfigDto> configs)
        {
            _configService.SetConfigs(_mapper.Map<IEnumerable<Config>>(configs));
        }
    }
}
