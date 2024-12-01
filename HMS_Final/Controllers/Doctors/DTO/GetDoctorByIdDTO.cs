namespace HMS_Final.Controllers.Doctors.DTO
{
    public class GetDoctorByIdDTO
    {
        public string DoctorName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<int> HospitalIds { get; set; }
    }
}
