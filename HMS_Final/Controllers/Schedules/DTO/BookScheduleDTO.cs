namespace HMS_Final.Controllers.Schedules.DTO
{
    public class BookScheduleDTO
    {
        public int UserId { get; set; }
        public int ScheduleId { get; set; }
        public DateTime? ConsultationDate { get; set; } // Optional field for specifying the next week's date
    }
}
