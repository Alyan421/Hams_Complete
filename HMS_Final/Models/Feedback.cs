namespace HMS_Final.Models
{
    public class Feedback : BaseEntity<int>
    {
        public int AppointmentId { get; set; } // Foreign key to Appointment

        public string Comments { get; set; } // Feedback comments

        public int Rating { get; set; } // Rating out of 5 or 10

        // Navigation property
        public Appointment Appointment { get; set; }
    }
} 