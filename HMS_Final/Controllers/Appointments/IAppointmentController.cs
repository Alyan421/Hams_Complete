using HMS_Final.Controllers.Appointments.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS_Final.Controllers.Appointments
{
    public interface IAppointmentController
    {
        Task<IActionResult> GetByIdAsync(int id);
        Task<IActionResult> UpdateAsync(AppointmentUpdateDTO updateAppointmentDTO);
        Task<IActionResult> DeleteAppointmentAsync(int id);
        Task<IActionResult> GetAllAsync();
    }
}
