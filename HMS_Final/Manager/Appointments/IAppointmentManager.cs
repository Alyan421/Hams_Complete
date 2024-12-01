using HMS_Final.Models;

namespace HMS_Final.Manager.Appointments
{
    public interface IAppointmentManager
    {
        Task<Appointment> UpdateAsync(Appointment entity);
        Task<Appointment> GetByIdAsync(int id);
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task DeleteAsync(int id);
    }
}
