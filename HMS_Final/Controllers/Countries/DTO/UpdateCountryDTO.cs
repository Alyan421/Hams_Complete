using System.ComponentModel.DataAnnotations;

namespace HMS_Final.Controllers.Countries.DTO
{
    public class UpdateCountryDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string CountryName { get; set; }

    }
}
