using HMS_Final.Models;
using System.ComponentModel.DataAnnotations;

namespace HMS_Final.Controllers.Feedbacks.DTO
{
    public class FeedbackUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int AppointmentId { get; set; } // Foreign key to Appointment
        [Required]
        public string Comments { get; set; } // Feedback comments
        [Required]
        public int Rating { get; set; } // Rating out of 5 or 10
    }
}
