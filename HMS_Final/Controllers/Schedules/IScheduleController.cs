using HMS_Final.Controllers.Schedules.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HMS_Final.Controllers.Schedules
{
    public interface IScheduleController
    {
        Task<IActionResult> CreateSchedule(CreateScheduleDTO dto);
        Task<IActionResult> UpdateSchedule(UpdateScheduleDTO dto);
        Task<IActionResult> DeleteSchedule(int id);
        Task<IActionResult> GetAllSchedules();
        Task<IActionResult> GetScheduleById(int id);
        Task<IActionResult> BookSchedule(BookScheduleDTO dto); // Patient-specific booking
    }
}
