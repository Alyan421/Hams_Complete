namespace HMS_Final.Models
{
    public class Appointment : BaseEntity<int>
    {
        public DateTime AppointmentDateTime { get; set; } // Captures when the appointment was booked

        // Foreign Key
        public int UserId { get; set; } // The ID of the patient (user)
        public int ScheduleId { get; set; }

        // Navigation Property
        public Schedule Schedule { get; set; }
        public User User { get; set; }

        // One-to-One relationship with Feedback
        public Feedback Feedback { get; set; } // Single Feedback for each Appointment
    }
}
