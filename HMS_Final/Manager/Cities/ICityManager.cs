using HMS_Final.Models;

namespace HMS_Final.Manager.Cities
{
    public interface ICityManager
    {
        Task<City> CreateCity(City city);
        Task<City> GetCityById(int id);
        Task<IEnumerable<City>> GetAllCities();
        Task<bool> UpdateCity(City city);
        Task<bool> DeleteCity(int id);
    }
}
