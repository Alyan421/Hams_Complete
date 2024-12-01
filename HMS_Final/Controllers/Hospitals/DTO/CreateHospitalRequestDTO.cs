using HMS_Final.Manager.Hospitals;
using System.ComponentModel.DataAnnotations;

namespace HMS_Final.Controllers.Hospitals.DTO
{
    public class CreateHospitalRequestDTO
    {
        [Required]
        public CreateHospitalDTO Hospital { get; set; } // For Hospital details

        [Required]
        public List<HospitalDepartmentDTO1> DepartmentDTOs { get; set; } // For related departments
    }

}
