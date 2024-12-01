using HMS_Final.Manager.Doctors;
using System.ComponentModel.DataAnnotations;

namespace HMS_Final.Controllers.Doctors.DTO
{
    public class UpdateDoctorDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string DoctorName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        // List of Hospital IDs for update
        public List<DoctorHospital1DTO> HospitalIds { get; set; }
    }
}
