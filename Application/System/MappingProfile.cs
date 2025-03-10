using Application.System.Dtos;
using AutoMapper;
using Domain.System.Entities;

namespace Application.System
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Config
            CreateMap<Config, ConfigDto>();
            CreateMap<ConfigDto, Config>();

            // User
            CreateMap<User, UserBase>();

            // FileStorage
            CreateMap<FileStorage, FileStorageDto>();
            CreateMap<FileStorage, FileStorageNameDto>();
            CreateMap<FileStorageDto, FileStorage>();
        }
    }
}
