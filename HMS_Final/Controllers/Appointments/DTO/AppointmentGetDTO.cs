using HMS_Final.Models;

namespace HMS_Final.Controllers.Appointments.DTO
{
    public class AppointmentGetDTO
    {
        public int Id { get; set; }
        public DateTime AppointmentDateTime { get; set; } // Captures when the appointment was booked

        // Foreign Key
        public int UserId { get; set; } // The ID of the patient (user)
        public int ScheduleId { get; set; }
    }
}
