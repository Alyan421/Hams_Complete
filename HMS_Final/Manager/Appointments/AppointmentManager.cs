using HMS_Final.Models;
using HMS_Final.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS_Final.Manager.Appointments
{
    public class AppointmentManager : IAppointmentManager
    {
        private readonly IGenericRepository<Appointment> _repository;

        public AppointmentManager(IGenericRepository<Appointment> repository)
        {
            _repository = repository;
        }

        public async Task<Appointment> UpdateAsync(Appointment entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Appointment entity cannot be null.");
            }

            try
            {
                var existingAppointment = await _repository.GetByIdAsync(entity.Id);
                if (existingAppointment == null)
                {
                    throw new KeyNotFoundException("Appointment not found.");
                }

                existingAppointment.AppointmentDateTime = entity.AppointmentDateTime;
                existingAppointment.ScheduleId = entity.ScheduleId;
                existingAppointment.UserId = entity.UserId;

                await _repository.UpdateAsync(existingAppointment);
                await _repository.SaveChangesAsync();
                return existingAppointment;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating the Appointment.", ex);
            }
        }

        public async Task<Appointment> GetByIdAsync(int id)
        {
            try
            {
                var appointment = await _repository.GetByIdAsync(id);
                if (appointment == null)
                {
                    throw new KeyNotFoundException("Appointment not found.");
                }

                return appointment;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving the appointment by ID.", ex);
            }
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving all appointments.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var appointment = await _repository.GetByIdAsync(id);
                if (appointment == null)
                {
                    throw new KeyNotFoundException("Appointment not found.");
                }

                await _repository.DeleteAsync(appointment);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting the appointment.", ex);
            }
        }
    }
}
