using AutoMapper;
using Customer.Application.DTO;
using Customer.Domain.Modal;

namespace Customer.Application.Mapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<CustomerDTO, CustomerModal>();
        }
    }
}
