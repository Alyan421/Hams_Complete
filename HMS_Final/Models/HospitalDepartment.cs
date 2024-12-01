namespace HMS_Final.Models
{
    public class HospitalDepartment
    {
        public int HospitalId { get; set; }
        public int DepartmentId { get; set; }

        //foreign key

        public Hospital Hospital { get; set; }

        public Department Department { get; set; }
    }
}
