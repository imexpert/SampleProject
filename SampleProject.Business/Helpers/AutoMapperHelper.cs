using AutoMapper;
using SampleProject.Entities.Concrete;
using SampleProject.Entities.Dtos;

namespace SampleProject.Business.Helpers
{
    public class AutoMapperHelper : Profile
    {
        public AutoMapperHelper()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, LoginUserDto>().ReverseMap();
        }
    }
}
