namespace HMS_Final.Models
{
    public class UserHospital : BaseEntity<int>
    {
        public int UserId { get; set; }
        public int HospitalId { get; set; }

        //Navigation Property
        public User User { get; set; }
        public Hospital Hospital { get; set; }
    }
}
