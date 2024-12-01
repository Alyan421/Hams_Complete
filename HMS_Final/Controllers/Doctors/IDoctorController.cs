using HMS_Final.Controllers.Doctors.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HMS_Final.Controllers.Doctors
{
    public interface IDoctorController
    {
        Task<IActionResult> CreateDoctor(CreateDoctorDTO dto);
        Task<IActionResult> UpdateDoctor(UpdateDoctorDTO dto);
        Task<IActionResult> DeleteDoctor(int id);
        Task<IActionResult> GetDoctorById(int id);
        Task<IActionResult> GetAllDoctors();
    }
}
