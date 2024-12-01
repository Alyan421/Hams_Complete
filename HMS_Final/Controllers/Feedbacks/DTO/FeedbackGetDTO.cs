using HMS_Final.Models;

namespace HMS_Final.Controllers.Feedbacks.DTO
{
    public class FeedbackGetDTO
    {
        public int Id { get; set; }
        public string Comments { get; set; } // Feedback comments
        public int Rating { get; set; } // Rating out of 5 or 10
        public int AppointmentId { get; set; } // Foreign key to Appointment

    }
}
