using AutoMapper;
using HMS_Final.Controllers.Insurances.DTO;
using HMS_Final.Models;

namespace HMS_Final.MappingProfiles
{
    public class InsuranceAutoMapper : Profile
    {
        public InsuranceAutoMapper()
        {
            CreateMap<InsuranceGetDTO, Insurance>().ReverseMap();
            CreateMap<InsuranceCreateDTO, Insurance>().ReverseMap();
            CreateMap<InsuranceUpdateDTO, Insurance>().ReverseMap();
            CreateMap<InsuranceDeleteDTO, Insurance>().ReverseMap();
            CreateMap<InsuranceGetByIdDTO,Insurance>().ReverseMap();
          }
    }
}
