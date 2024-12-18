using AutoMapper;
using User.Application.DTO;
using User.Domain.Modal;

namespace User.Application.Mapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<UserDTO, UserModal>();
        }
    }
}
