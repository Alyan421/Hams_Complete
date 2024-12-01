using AutoMapper;
using HMS_Final.Controllers.Departments.DTO;
using HMS_Final.Models;

namespace HMS_Final.Controllers.Departments
{
    public class DepartmentMappingProfile: Profile
    {
        public DepartmentMappingProfile()
        {
            // Map CreateDepartmentDTO to Department
            CreateMap<CreateDepartmentDTO, Department>();

            // Map UpdateDepartmentDTO to Department
            CreateMap<UpdateDepartmentDTO, Department>();

            // Map Department to GetDepartmentByIdDTO
            CreateMap<Department, GetDepartmentById>();
               
            // Map Department to GetAllDepartmentsDTO
            CreateMap<Department, GetAllDepartmentDTO>();

        }
    }
}
