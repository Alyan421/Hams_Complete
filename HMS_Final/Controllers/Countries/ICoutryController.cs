using HMS_Final.Controllers.Countries.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HMS_Final.Controllers.Countries
{
    public interface ICountryController
    {
        Task<IActionResult> CreateCountry(CreateCountryDTO dto);
        Task<IActionResult> GetCountryById(int id);
        Task<IActionResult> GetAllCountries();
        Task<IActionResult> UpdateCountry(UpdateCountryDTO dto);
        Task<IActionResult> DeleteCountry(int id);
    }
}
