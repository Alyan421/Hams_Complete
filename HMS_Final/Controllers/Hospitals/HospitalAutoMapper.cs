using AutoMapper;
using HMS_Final.Controllers.Hospitals.DTO;
using HMS_Final.Manager.Departments.DTO_temp_;
using HMS_Final.Manager.Hospitals;
using HMS_Final.Models;

namespace HMS_Final.Controllers.Hospitals
{
    public class HospitalAutoMapper : Profile
    {
        public HospitalAutoMapper()
        {
            // Map from DTO to Model
            CreateMap<CreateHospitalDTO, Hospital>().ReverseMap();
            CreateMap<UpdateHospitalDTO, Hospital>().ReverseMap();
            CreateMap<HospitalDepartmentDTO, HospitalDepartmentDTO1>().ReverseMap(); // Map for Department DTOs
            CreateMap<HospitalDoctorDTO2, DoctorHospital>().ReverseMap(); // Map for Doctor DTOs

            // Map from Model to DTO
            CreateMap<Hospital, GetHospitalByIdDTO>().ReverseMap();
            CreateMap<Hospital, GetHospitalsDTO>().ReverseMap();
            CreateMap<HospitalDepartmentDTO1, HospitalDepartment>().ReverseMap();
        }
    }
}
