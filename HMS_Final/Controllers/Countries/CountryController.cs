using AutoMapper;
using HMS_Final.Controllers.Countries;
using HMS_Final.Controllers.Countries.DTO;
using HMS_Final.Manager.Countries;
using HMS_Final.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS_Final.Controllers.Countries
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase, ICountryController
    {
        private readonly ICountryManager _countryManager;
        private readonly IMapper _mapper;

        public CountryController(ICountryManager countryManager, IMapper mapper)
        {
            _countryManager = countryManager;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCountry(CreateCountryDTO dto)
        {
            try
            {
                // Map DTO to the Country entity
                var country = _mapper.Map<Country>(dto);

                // Create the country using the manager
                var createdCountry = await _countryManager.CreateCountry(country);

                // Check if the country creation failed
                if (createdCountry == null)
                {
                    return StatusCode(500, new { Message = "Failed to create country. Please try again." });
                }

                // Return a success message with only the created country's ID
                return Ok(new
                {
                    Message = "Country created successfully.",
                    CountryId = createdCountry.Id
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions and return error response
                return StatusCode(500, new
                {
                    Message = "An error occurred while creating the country.",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCountryById(int id)
        {
            var country = await _countryManager.GetCountryById(id);
            if (country == null)
                return NotFound(new { Message = "Country not found." });

            var result = _mapper.Map<GetCountryByIdDTO>(country);
            return Ok(result);
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = await _countryManager.GetAllCountries();
            return Ok(_mapper.Map<IEnumerable<GetAllCountries>>(countries));
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCountry(UpdateCountryDTO dto)
        {
            var country = _mapper.Map<Country>(dto);
            var updated = await _countryManager.UpdateCountry(country);
            if (!updated) return NotFound(new { Message = "Country not found." });

            return Ok(new { Message = "Country updated successfully." });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var deleted = await _countryManager.DeleteCountry(id);
            if (!deleted) return NotFound(new { Message = "Country not found." });

            return Ok(new { Message = "Country deleted successfully." });
        }
    }
}
