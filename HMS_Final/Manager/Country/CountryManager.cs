using HMS_Final.Manager.Countries;
using HMS_Final.Models;
using HMS_Final.Repository;
using Microsoft.EntityFrameworkCore;

namespace HMS_Final.Manager.Countries
{
    public class CountryManager : ICountryManager
    {
        private readonly IGenericRepository<Country> _countryRepository;
        private readonly ILogger<CountryManager> _logger;

        public CountryManager(IGenericRepository<Country> countryRepository, ILogger<CountryManager> logger)
        {
            _countryRepository = countryRepository;
            _logger = logger;
        }

        public async Task<Country> CreateCountry(Country country)
        {
            try
            {
                //Check if a country with the same name already exists
               var existingCountry = await _countryRepository.GetDbSet()
                   .FirstOrDefaultAsync(c => c.CountryName.ToLower() == country.CountryName.ToLower());

                if (existingCountry != null)
                {
                    throw new Exception($"A country with the name '{country.CountryName}' already exists.");
                }

                // Add the new country
                await _countryRepository.AddAsync(country);
                await _countryRepository.SaveChangesAsync();

                return country; // Return the newly created country
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating country.");
                throw new Exception("An error occurred while creating the country. Please try again.");
            }
        }





        public async Task<Country> GetCountryById(int id)
        {
            try
            {
                var country = await _countryRepository.GetByIdAsync(id);
                if (country == null)
                {
                    throw new Exception($"Country with Id {id} not found.");
                }

                // Debugging log
                _logger.LogInformation($"Country: {country.CountryName}");

                return country;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving country with Id {id}.");
                throw new Exception($"An error occurred while fetching the country: {ex.Message}");
            }
        }



        public async Task<IEnumerable<Country>> GetAllCountries()
        {
            try
            {
                var countries = await _countryRepository.GetAllAsync();
                if (countries == null || !countries.Any())
                {
                    throw new Exception("No countries found.");
                }

                return countries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all countries.");
                throw new Exception($"An error occurred while fetching countries: {ex.Message}");
            }
        }


        public async Task<bool> UpdateCountry(Country country)
        {
            try
            {
                // Ensure the country exists
                var existingCountry = await _countryRepository.GetByIdAsync(country.Id);
                if (existingCountry == null)
                {
                    throw new Exception($"Country with Id {country.Id} not found.");
                }

                // Check if the name already exists in another country
                var duplicateCountry = await _countryRepository.GetDbSet()
              .FirstOrDefaultAsync(c => c.CountryName.ToLower() == country.CountryName.ToLower());

                if (duplicateCountry != null)
                {
                    throw new Exception($"A country with the name '{country.CountryName}' already exists.");
                }

                // Update country details
                existingCountry.CountryName = country.CountryName;

                await _countryRepository.UpdateAsync(existingCountry);
                await _countryRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating country with Id {country.Id}.");
                throw new Exception($"An error occurred while updating the country: {ex.Message}");
            }
        }


        public async Task<bool> DeleteCountry(int id)
        {
            try
            {
                var country = await _countryRepository.GetByIdAsync(id);
                if (country == null) return false;

                await _countryRepository.DeleteAsync(country);
                await _countryRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting country.");
                throw;
            }
        }
    }
}
 