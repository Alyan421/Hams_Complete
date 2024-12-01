using AutoMapper;
using HMS_Final.Controllers.Doctors.DTO;
using HMS_Final.Models;

namespace HMS_Final.Controllers.Doctors
{
    public class DoctorMappingProfile : Profile
    {
        public DoctorMappingProfile() 
        {
            CreateMap<CreateDoctorDTO, Doctor>();
            CreateMap<UpdateDoctorDTO, Doctor>();

            CreateMap<Doctor,GetDoctorByIdDTO>();
            CreateMap<Doctor,GetDoctorDTO>();
        }
    }
}
