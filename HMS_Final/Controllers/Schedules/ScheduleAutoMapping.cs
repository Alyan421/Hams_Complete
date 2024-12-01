using AutoMapper;
using HMS_Final.Controllers.Schedules.DTO;
using HMS_Final.Models;

namespace HMS_Final.Controllers.Schedules
{
    public class ScheduleAutoMapping : Profile
    {
        public ScheduleAutoMapping() 
        {
            CreateMap<Schedule, CreateScheduleDTO>().ReverseMap();
            CreateMap<Schedule, UpdateScheduleDTO>().ReverseMap();

            CreateMap<GetAllSchedule, Schedule>().ReverseMap();
            CreateMap<GetSchdeuleById, Schedule>().ReverseMap();

            //Book Appointment DTO
            CreateMap<Appointment, BookScheduleDTO>()
      .ForMember(dest => dest.ConsultationDate, opt => opt.MapFrom(src => src.AppointmentDateTime)) // Map DesiredDate to AppointmentDateTime
      .ReverseMap()
      .ForMember(dest => dest.AppointmentDateTime, opt => opt.Ignore()); // Ignore this as it's handled manually


        }
    }
}




