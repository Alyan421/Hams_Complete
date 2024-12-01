using HMS_Final.Manager.Departments.DTO_temp_;
using HMS_Final.Manager.Hospitals;
using System.ComponentModel.DataAnnotations;

namespace HMS_Final.Controllers.Hospitals.DTO
{
    public class CreateHospitalDTO
    {
        [Required]
        public int CityId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Contact Info must be a valid number.")]
        public int ContactInfo { get; set; }

        // Make departments optional
        public List<HospitalDepartmentDTO1>? Departments { get; set; } // Optional list of departments

        // Include optional doctor relationships
        public List<HospitalDoctorDTO2>? Doctors { get; set; } // Optional list of doctors
    }
}
