using AutoMapper;
using HMS_Final.Controllers.Cities.DTO;
using HMS_Final.Models;

namespace HMS_Final.Controllers.Cities
{
    public class CityMappingProfile : Profile
    {
            public CityMappingProfile()
            {
                // Map from DTO to Model
                CreateMap<CreateCityDTO, City>();
                CreateMap<UpdateCityDTO, City>();
            
                CreateMap<City,GetAllCities>();
                CreateMap<City,GetCityByIdDTO>();
            }
        }
}
