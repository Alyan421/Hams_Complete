using HMS_Final.Models;
using HMS_Final.Repository;
using Microsoft.EntityFrameworkCore;

namespace HMS_Final.Manager.Schedules
{
    public class ScheduleManager : IScheduleManager
    {
        private readonly IGenericRepository<Schedule> _scheduleRepository;
        private readonly IGenericRepository<Appointment> _appointmentRepository;
        private readonly ILogger<ScheduleManager> _logger;

        public ScheduleManager(
            IGenericRepository<Schedule> scheduleRepository,
            IGenericRepository<Appointment> appointmentRepository,
            ILogger<ScheduleManager> logger)
        {
            _scheduleRepository = scheduleRepository;
            _appointmentRepository = appointmentRepository;
            _logger = logger;
        }

        // Admin: Create Schedule
        public async Task<Schedule> CreateSchedule(Schedule schedule)
        {
            try
            {
                await _scheduleRepository.AddAsync(schedule);
                await _scheduleRepository.SaveChangesAsync();
                return schedule;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating schedule.");
                throw new Exception("Failed to create schedule. Please try again.");
            }
        }

        // Admin: Update Schedule
        public async Task<bool> UpdateSchedule(Schedule schedule)
        {
            try
            {
                var existingSchedule = await _scheduleRepository.GetByIdAsync(schedule.Id);
                if (existingSchedule == null)
                    return false;

                existingSchedule.ConsultationDay = schedule.ConsultationDay;
                existingSchedule.ConsultationTime = schedule.ConsultationTime;
                existingSchedule.ConsultationDate = schedule.ConsultationDate;

                await _scheduleRepository.UpdateAsync(existingSchedule);
                await _scheduleRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating schedule with ID {schedule.Id}.");
                throw new Exception("Failed to update schedule. Please try again.");
            }
        }

        // Admin: Delete Schedule
        public async Task<bool> DeleteSchedule(int id)
        {
            try
            {
                var schedule = await _scheduleRepository.GetByIdAsync(id);
                if (schedule == null)
                    return false;

                await _scheduleRepository.DeleteAsync(schedule);
                await _scheduleRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting schedule with ID {id}.");
                throw new Exception("Failed to delete schedule. Please try again.");
            }
        }

        // Admin: Get All Schedules
        public async Task<IEnumerable<Schedule>> GetAllSchedules()
        {
            try
            {
                return await _scheduleRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving schedules.");
                throw new Exception("Failed to fetch schedules. Please try again.");
            }
        }

        // Admin: Get Schedule By ID
        public async Task<Schedule> GetScheduleById(int id)
        {
            try
            {
                return await _scheduleRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving schedule with ID {id}.");
                throw new Exception("Failed to fetch schedule. Please try again.");
            }
        }

        // Patient: Book Schedule
        public async Task<bool> BookSchedule(int userId, int scheduleId, DateTime consultationDate)
        {
            try
            {
                var schedule = await _scheduleRepository.GetByIdAsync(scheduleId);
                if (schedule == null)
                {
                    throw new Exception("Schedule not found.");
                }

                // Check if the slot for the specified date and time is already booked
                var existingAppointment = await _appointmentRepository.GetDbSet()
                    .FirstOrDefaultAsync(a => a.ScheduleId == scheduleId && a.AppointmentDateTime.Date == consultationDate.Date);

                if (existingAppointment != null)
                {
                    // If slot is booked, move to the next available week's same day
                    consultationDate = consultationDate.AddDays(7);

                    // Check again for the new date
                    existingAppointment = await _appointmentRepository.GetDbSet()
                        .FirstOrDefaultAsync(a => a.ScheduleId == scheduleId && a.AppointmentDateTime.Date == consultationDate.Date);

                    if (existingAppointment != null)
                    {
                        throw new Exception("The next week's slot for the same day is also booked. Please try another day or time.");
                    }
                }

                // Create the appointment
                var appointment = new Appointment
                {
                    UserId = userId,
                    ScheduleId = scheduleId,
                    AppointmentDateTime = consultationDate
                };
                await _appointmentRepository.AddAsync(appointment);
                await _appointmentRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error booking schedule with ID {scheduleId}.");
                throw new Exception("Failed to book schedule. Please try again.");
            }
        }


        // Helper method to calculate the next week's date for the same day
        private DateTime CalculateNextWeekDate(string consultationDay, DateTime currentDate)
        {
            DayOfWeek dayOfWeek = Enum.Parse<DayOfWeek>(consultationDay, true);
            int daysToAdd = ((int)dayOfWeek - (int)currentDate.DayOfWeek + 7) % 7;
            daysToAdd = daysToAdd == 0 ? 7 : daysToAdd; // Always go to the next week for the same day
            return currentDate.AddDays(daysToAdd);
        }

    }
}
