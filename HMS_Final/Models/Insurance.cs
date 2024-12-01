namespace HMS_Final.Models
{
    public class Insurance : BaseEntity<int>
    {
        public string? CompanyName { get; set; }
        public string? InsuranceType { get; set; }
        public int? HospitalId { get; set; }

        // Foreign key for Hospital
        public Hospital Hospital { get; set; } // Navigation property
    }
}