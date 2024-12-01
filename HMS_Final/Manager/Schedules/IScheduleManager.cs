using HMS_Final.Models;

namespace HMS_Final.Manager.Schedules
{
    public interface IScheduleManager
    {
        Task<Schedule> CreateSchedule(Schedule schedule);
        Task<bool> UpdateSchedule(Schedule schedule);
        Task<bool> DeleteSchedule(int id);
        Task<IEnumerable<Schedule>> GetAllSchedules();
        Task<Schedule> GetScheduleById(int id);
        //Task<bool> BookSchedule(int userId, int scheduleId); // Patient-specific booking logic
        Task<bool> BookSchedule(int userId, int scheduleId, DateTime consultationDate);
    }
}
