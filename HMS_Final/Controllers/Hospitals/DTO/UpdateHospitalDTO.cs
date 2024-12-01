using HMS_Final.Manager.Hospitals;
using System.ComponentModel.DataAnnotations;

namespace HMS_Final.Controllers.Hospitals.DTO
{
    public class UpdateHospitalDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Contact Info must be a valid number.")]
        public int ContactInfo { get; set; }

        [Required]
        public int CityId { get; set; }

        // Include department relationships
        public List<HospitalDepartmentDTO1>? Departments { get; set; } // Optional list of departments

        // Include doctor relationships
        public List<HospitalDoctorDTO2>? Doctors { get; set; } // Optional list of doctors
    }
}
