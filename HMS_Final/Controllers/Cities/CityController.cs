using AutoMapper;
using HMS_Final.Controllers.Cities.DTO;
using HMS_Final.Manager.Cities;
using HMS_Final.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS_Final.Controllers.Cities
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase, ICityController
    {
        private readonly ICityManager _cityManager;
        private readonly IMapper _mapper;

        public CityController(ICityManager cityManager, IMapper mapper)
        {
            _cityManager = cityManager;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCity(CreateCityDTO dto)
        {
            try
            {
                var city = _mapper.Map<City>(dto);
                var createdCity = await _cityManager.CreateCity(city);

                if (createdCity == null)
                {
                    return StatusCode(500, new { Message = "Failed to create city. Please try again." });
                }

                return Ok(new { Message = "City created successfully.", CityId = createdCity.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the city.", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            try
            {
                var city = await _cityManager.GetCityById(id);
                if (city == null) return NotFound(new { Message = "City not found." });

                var result = _mapper.Map<GetCityByIdDTO>(city);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the city.", Error = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCities()
        {
            try
            {
                var cities = await _cityManager.GetAllCities();
                var result = _mapper.Map<IEnumerable<GetAllCities>>(cities);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching cities.", Error = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCity(UpdateCityDTO dto)
        {
            try
            {
                var city = _mapper.Map<City>(dto);
                var updated = await _cityManager.UpdateCity(city);

                if (!updated) return NotFound(new { Message = "City not found." });

                return Ok(new { Message = "City updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the city.", Error = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            try
            {
                var deleted = await _cityManager.DeleteCity(id);

                if (!deleted) return NotFound(new { Message = "City not found." });

                return Ok(new { Message = "City deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the city.", Error = ex.Message });
            }
        }
    }
}
