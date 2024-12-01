using System.ComponentModel.DataAnnotations;

namespace HMS_Final.Controllers.Countries.DTO
{
    public class CreateCountryDTO
    {
        [Required]
        public string CountryName { get; set; }
    }
}
