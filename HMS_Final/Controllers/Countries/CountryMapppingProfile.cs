using AutoMapper;
using HMS_Final.Controllers.Countries.DTO;
using HMS_Final.Models;
using System.Runtime;

namespace HMS_Final.Controllers.Countries
{
    public class CountryMappingProfile : Profile
    {
        public CountryMappingProfile()
        {
            // Map from DTO to Model
            CreateMap<CreateCountryDTO, Country>();
            CreateMap<UpdateCountryDTO, Country>();


            CreateMap<Country, GetAllCountries>();
            CreateMap<Country, GetCountryByIdDTO>();

        }
    }

}
