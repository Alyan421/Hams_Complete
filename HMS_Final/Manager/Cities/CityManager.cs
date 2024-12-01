using HMS_Final.Models;
using HMS_Final.Repository;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace HMS_Final.Manager.Cities
{
    public class CityManager : ICityManager
    {
        private readonly IGenericRepository<City> _cityRepository;
        private readonly ILogger<CityManager> _logger;

        public CityManager(IGenericRepository<City> cityRepository, ILogger<CityManager> logger)
        {
            _cityRepository = cityRepository;
            _logger = logger;
        }

        public async Task<City> CreateCity(City city)
        {
            try
            {
                // Check if a city with the same name exists in the given country
                var existingCity = await _cityRepository.GetDbSet()
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == city.Name.ToLower() && c.CountryId == city.CountryId);

                if (existingCity != null)
                {
                    throw new Exception($"A city with the name '{city.Name}' already exists in the specified country.");
                }

                await _cityRepository.AddAsync(city);
                await _cityRepository.SaveChangesAsync();
                return city;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating city.");
                throw new Exception("An error occurred while creating the city. Please try again.");
            }
        }


        public async Task<City> GetCityById(int id)
        {
            try
            {
                var city = await _cityRepository.GetByIdAsync(id);
                if (city == null)
                {
                    throw new Exception($"City with Id {id} not found.");
                }
                return city;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving city with Id {id}.");
                throw new Exception($"An error occurred while fetching the city: {ex.Message}");
            }
        }

        public async Task<IEnumerable<City>> GetAllCities()
        {
            try
            {
                var cities = await _cityRepository.GetAllAsync();
                if (!cities.Any())
                {
                    throw new Exception("No cities found.");
                }
                return cities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all cities.");
                throw new Exception($"An error occurred while fetching cities: {ex.Message}");
            }
        }


        public async Task<bool> UpdateCity(City city)
        {
            try
            {
                var existingCity = await _cityRepository.GetByIdAsync(city.Id);
                if (existingCity == null)
                {
                    return false; // Return false if the city does not exist
                }

                // Check if another city with the same name exists in the same country
                var duplicateCity = await _cityRepository.GetDbSet()
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == city.Name.ToLower() && c.CountryId == city.CountryId && c.Id != city.Id);

                if (duplicateCity != null)
                {
                    throw new Exception($"Another city with the name '{city.Name}' already exists in the specified country.");
                }

                existingCity.Name = city.Name;
                existingCity.CountryId = city.CountryId;

                await _cityRepository.UpdateAsync(existingCity);
                await _cityRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating city.");
                throw new Exception("An error occurred while updating the city. Please try again.");
            }
        }



        public async Task<bool> DeleteCity(int id)
        {
            try
            {
                var city = await _cityRepository.GetByIdAsync(id);
                if (city == null) return false;

                await _cityRepository.DeleteAsync(city);
                await _cityRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting city.");
                throw;
            }
        }
    }
}
