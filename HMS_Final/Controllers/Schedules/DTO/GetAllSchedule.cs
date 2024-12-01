namespace HMS_Final.Controllers.Schedules.DTO
{
    public class GetAllSchedule
    {
        public int Id { get; set; }
        public string ConsultationDay { get; set; }
        public string ConsultationTime { get; set; }
        //public DateTime? ConsultationDate { get; set; }
        public int DoctorId { get; set; }
    }
}
