using HMS_Final.Controllers.Cities.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HMS_Final.Controllers.Cities
{
    public interface ICityController
    {
        Task<IActionResult> CreateCity(CreateCityDTO dto);
        Task<IActionResult> GetCityById(int id);
        Task<IActionResult> GetAllCities();
        Task<IActionResult> UpdateCity(UpdateCityDTO dto);
        Task<IActionResult> DeleteCity(int id);
    }
}
