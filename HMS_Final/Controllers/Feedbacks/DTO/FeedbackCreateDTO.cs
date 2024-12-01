using HMS_Final.Models;
using System.ComponentModel.DataAnnotations;

namespace HMS_Final.Controllers.Feedbacks.DTO
{
    public class FeedbackCreateDTO
    {
        [Required]
        public string Comments { get; set; } // Feedback comments
        
        [Required]
        [Range(0, 5, ErrorMessage = "Contact Info must be a valid number.")]
        public int Rating { get; set; } // Rating out of 5
        [Required]
        public int AppointmentId { get; set; } // Foreign key to Appointment
    }
}
