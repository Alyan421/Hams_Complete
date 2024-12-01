namespace HMS_Final.Models
{
    public class DoctorHospital : BaseEntity<int>
    {
        public int DoctorId { get; set; }
        public int HospitalId { get; set; }
        
        //Navigation Property
        public Doctor Doctor { get; set; }
        public Hospital Hospital { get; set; }
    }
}
