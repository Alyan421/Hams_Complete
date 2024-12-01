using AutoMapper;
using HMS_Final.Controllers.Appointments.DTO;
using HMS_Final.Models;

namespace HMS_Final.Controllers.Appointments
{
    public class AppointmentMappingProfile : Profile
    {
        public AppointmentMappingProfile()
        {
            // Map from DTO to Model
            CreateMap<AppointmentDeleteDTO, Appointment>().ReverseMap();
            CreateMap<AppointmentUpdateDTO, Appointment>().ReverseMap();
            CreateMap<AppointmentGetDTO, Appointment>().ReverseMap();
            CreateMap<AppointmentGetByIdDTO, Appointment>().ReverseMap();
        }
    }
}