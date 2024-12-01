using HMS_Final.Manager.Doctors;

namespace HMS_Final.Controllers.Doctors.DTO
{
    public class GetDoctorDTO
    {
        public int Id { get; set; }
        public string DoctorName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<int> HospitalDTOs { get; set; }

    }
}
