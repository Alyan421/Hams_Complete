using HMS_Final.Manager.Departments.DTO_temp_;
using HMS_Final.Manager.Doctors;

namespace HMS_Final.Controllers.Doctors.DTO
{
    public class CreateDoctorDTO
    {
        public string DoctorName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<DoctorHospital1DTO> HospitalDTOs { get; set; }
    }
}
