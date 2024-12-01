using System.ComponentModel.DataAnnotations;

namespace HMS_Final.Controllers.Schedules.DTO
{
    public class CreateScheduleDTO
    {
        [Required]
        [RegularExpression("^(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)$", ErrorMessage = "Consultation day must be a valid day of the week (e.g., Monday).")]
        public string ConsultationDay { get; set; }

        [Required]
        [RegularExpression("^(0[1-9]|1[0-2]):[0-5][0-9] (AM|PM)$", ErrorMessage = "Consultation time must follow the format HH:MM AM/PM (e.g., 3:30 PM).")]
        public string ConsultationTime { get; set; }

        [Required]
        public int DoctorId { get; set; }
    }
}
