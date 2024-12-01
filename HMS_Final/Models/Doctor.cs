namespace HMS_Final.Models
{
    public class Doctor : BaseEntity<int>
    {
        public string DoctorName {  get; set; }
        public DateTime DateOfBirth { get; set; }

        //many-many
        public ICollection<DoctorHospital> DoctorHospitals { get; set; } // Many-to-Many with Hospital
        public ICollection<Schedule> Schedules { get; set; } // One-to-Many with Schedule
    }
}
