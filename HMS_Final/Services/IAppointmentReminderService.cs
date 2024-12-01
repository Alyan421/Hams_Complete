using System.Threading.Tasks;

namespace HMS_Final.Services
{
    public interface IAppointmentReminderService
    {
        Task SendRemindersAsync();
    }
} 