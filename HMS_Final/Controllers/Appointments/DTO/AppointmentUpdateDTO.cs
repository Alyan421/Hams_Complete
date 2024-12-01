using HMS_Final.Models;
using System.ComponentModel.DataAnnotations;

namespace HMS_Final.Controllers.Appointments.DTO
{
    public class AppointmentUpdateDTO
    {
        [Required]
        public int Id;
        [Required]
        public DateTime AppointmentDateTime { get; set; } // Captures when the appointment was booked
        // Foreign Key
        [Required]
        public int UserId { get; set; } // The ID of the patient (user)
        [Required]
        public int ScheduleId { get; set; }
    }
}
