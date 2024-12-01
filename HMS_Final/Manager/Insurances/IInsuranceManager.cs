using HMS_Final.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS_Final.Manager.Insurances
{
    public interface IInsuranceManager
    {
        Task<Insurance> CreateAsync(Insurance insurance);
        Task<Insurance> UpdateAsync(Insurance insurance);
        Task<Insurance> GetByIdAsync(int id);
        Task<IEnumerable<Insurance>> GetAllAsync();
        Task DeleteAsync(int id);
    }
}
