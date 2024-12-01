using HMS_Final.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS_Final.Manager.Doctors
{
    public interface IDoctorManager
    {
        Task<Doctor> CreateDoctor(Doctor doctor, List<DoctorHospital1DTO> hospitalIds);
        Task<bool> UpdateDoctor(Doctor doctor, List<DoctorHospital1DTO> hospitalIds);
        Task<bool> DeleteDoctor(int id);
        Task<IEnumerable<Doctor>> GetAllDoctors();
        Task<Doctor> GetDoctorById(int id);
    }
}
