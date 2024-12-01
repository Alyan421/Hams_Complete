namespace HMS_Final.Models
{
    public class Schedule : BaseEntity<int>
    {
        public string ConsultationDay { get; set; }
        public string ConsultationTime { get; set; }
        public DateTime ConsultationDate { get; set; }
        public int DoctorId { get; set; }

        // Navigation Properties
        public Doctor Doctor { get; set; }

        //many-many
        public ICollection<Appointment> Appointments { get; set; } // One-to-Many with Appointment

    }
}
