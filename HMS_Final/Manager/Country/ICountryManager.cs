using HMS_Final.Models;

namespace HMS_Final.Manager.Countries
{
    public interface ICountryManager
    {
        Task<Country> CreateCountry(Country country);
        Task<Country> GetCountryById(int id);
        Task<IEnumerable<Country>> GetAllCountries();
        Task<bool> UpdateCountry(Country country);
        Task<bool> DeleteCountry(int id);
    }
}
