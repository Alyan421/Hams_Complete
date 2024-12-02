using AutoMapper;
using HMS_Final.Models;
using HMS_Final.Controllers.Users.DTO;

namespace HMS_Final.Mapping
{
    public class UserAutoMapping : Profile
    {
        public UserAutoMapping()
        {
            CreateMap<UserDTO, User>();
            CreateMap<CreateUserDTO, User>();
            CreateMap<UpdateUserDTO, User>();
            CreateMap<LoginUserDTO, User>().ReverseMap();
            CreateMap<UserGetDTO, User>().ReverseMap();
            CreateMap<User, UserDTO>();
            CreateMap<User,CreateUserDTO>();
            CreateMap<User,UpdateUserDTO>();
        }
    }
} 