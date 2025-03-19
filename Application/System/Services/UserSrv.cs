using Application.System.Dtos;
using AutoMapper;
using Domain.System.Entities;
using Domain.System.Services;

namespace Application.System.Services
{
    public class UserSrv
    {
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public UserSrv(IMapper mapper, UserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public UserDto GetUser(Guid userId)
        {
            return _mapper.Map<UserDto>(_userService.GetUser(userId));
        }

        public Guid CreateUser(UserDto user)
        {
            return _userService.CreateUser(_mapper.Map<User>(user));
        }
    }
}
