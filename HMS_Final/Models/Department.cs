namespace HMS_Final.Models
{
    public class Department : BaseEntity<int>
    {
        public string Name { get; set; }

        //many-many
        public ICollection<HospitalDepartment> HospitalDepartments { get; set; }

    }
}
