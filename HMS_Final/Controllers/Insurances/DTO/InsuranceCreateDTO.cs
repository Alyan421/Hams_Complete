using System.ComponentModel.DataAnnotations;
namespace HMS_Final.Controllers.Insurances.DTO
{
    public class InsuranceCreateDTO
    {
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string InsuranceType { get; set; }
        [Required]
        public int HospitalId { get; set; }
    }
} 