using HMS_Final.Manager.Doctors;
using HMS_Final.Models;
using HMS_Final.Repository;
using Microsoft.EntityFrameworkCore;

namespace HMS_Final.Manager.Doctors
{
    public class DoctorManager : IDoctorManager
    {
        private readonly IGenericRepository<Doctor> _doctorRepository;
        private readonly IGenericRepository<DoctorHospital> _doctorHospitalRepository;
        private readonly IGenericRepository<Appointment> _appointmentRepository;
        private readonly IGenericRepository<Schedule> _scheduleRepository;
        private readonly ILogger<DoctorManager> _logger;

        public DoctorManager(
            IGenericRepository<Doctor> doctorRepository,
            IGenericRepository<DoctorHospital> doctorHospitalRepository,
            ILogger<DoctorManager> logger, IGenericRepository<Appointment> appointmentRepository,
             IGenericRepository<Schedule> scheduleRepository)
        {
            _doctorRepository = doctorRepository;
            _doctorHospitalRepository = doctorHospitalRepository;
            _logger = logger;
            _appointmentRepository = appointmentRepository;
            _scheduleRepository = scheduleRepository;
        }

        public async Task<Doctor> CreateDoctor(Doctor doctor, List<DoctorHospital1DTO> hospitalDTOs)
        {
            try
            {
                // Check if a doctor with the same name and DOB already exists
                var existingDoctor = await _doctorRepository.GetDbSet()
                    .FirstOrDefaultAsync(d => d.DoctorName.ToLower() == doctor.DoctorName.ToLower() && d.DateOfBirth == doctor.DateOfBirth);

                if (existingDoctor != null)
                {
                    throw new Exception($"A doctor with the name '{doctor.DoctorName}' already exists.");
                }

                // Add the doctor
                await _doctorRepository.AddAsync(doctor);
                await _doctorRepository.SaveChangesAsync();

                // Add relationships in DoctorHospital
                foreach (var hospitalDTO in hospitalDTOs)
                {
                    var existingRelationship = await _doctorHospitalRepository.GetDbSet()
                        .FirstOrDefaultAsync(dh =>
                            dh.HospitalId == hospitalDTO.HospitalId &&
                            dh.DoctorId == doctor.Id);

                    if (existingRelationship == null)
                    {
                        var doctorHospital = new DoctorHospital
                        {
                            HospitalId = hospitalDTO.HospitalId,
                            DoctorId = doctor.Id
                        };
                        await _doctorHospitalRepository.AddAsync(doctorHospital);
                    }
                }

                await _doctorHospitalRepository.SaveChangesAsync();
                return doctor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating doctor.");
                throw new Exception("An error occurred while creating the doctor. Please try again.");
            }
        }

        public async Task<bool> UpdateDoctor(Doctor doctor, List<DoctorHospital1DTO> hospitalDTOs)
        {
            try
            {
                // Check if the doctor exists
                var existingDoctor = await _doctorRepository.GetByIdAsync(doctor.Id);
                if (existingDoctor == null)
                {
                    _logger.LogWarning($"Doctor with Id {doctor.Id} not found.");
                    return false;
                }

                // Check if another doctor with the same name and DOB exists
                var duplicateDoctor = await _doctorRepository.GetDbSet()
                    .FirstOrDefaultAsync(d => d.DoctorName.ToLower() == doctor.DoctorName.ToLower()
                        && d.DateOfBirth == doctor.DateOfBirth
                        && d.Id != doctor.Id);
                if (duplicateDoctor != null)
                {
                    throw new Exception($"Another doctor with the name '{doctor.DoctorName}' already exists.");
                }

                // Update doctor details
                existingDoctor.DoctorName = doctor.DoctorName;
                existingDoctor.DateOfBirth = doctor.DateOfBirth;
                await _doctorRepository.UpdateAsync(existingDoctor);

                // Remove old relationships
                var existingAssociations = await _doctorHospitalRepository.GetDbSet()
                    .Where(dh => dh.DoctorId == doctor.Id)
                    .ToListAsync();
                foreach (var association in existingAssociations)
                {
                    await _doctorHospitalRepository.DeleteAsync(association);
                }

                // Add new relationships using HospitalDoctor1DTO
                foreach (var hospitalDTO in hospitalDTOs)
                {
                    var doctorHospital = new DoctorHospital
                    {
                        HospitalId = hospitalDTO.HospitalId,
                        DoctorId = doctor.Id
                    };
                    await _doctorHospitalRepository.AddAsync(doctorHospital);
                }

                await _doctorHospitalRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating doctor with Id {doctor.Id}.");
                throw new Exception("An error occurred while updating the doctor. Please try again.");
            }
        }

        public async Task<bool> DeleteDoctor(int id)
        {
            try
            {
                // Retrieve the doctor
                var doctor = await _doctorRepository.GetByIdAsync(id);
                if (doctor == null) return false;

                // Remove related DoctorHospital records
                var associatedDoctorHospitalRecords = await _doctorHospitalRepository.GetDbSet()
                    .Where(dh => dh.DoctorId == id)
                    .ToListAsync();

                foreach (var record in associatedDoctorHospitalRecords)
                {
                    await _doctorHospitalRepository.DeleteAsync(record);
                }

                // Remove related Schedules
                var associatedSchedules = await _scheduleRepository.GetDbSet()
                    .Where(s => s.DoctorId == id)
                    .ToListAsync();

                foreach (var schedule in associatedSchedules)
                {
                    // Remove related Appointments for the schedule
                    var associatedAppointments = await _appointmentRepository.GetDbSet()
                        .Where(a => a.ScheduleId == schedule.Id)
                        .ToListAsync();

                    foreach (var appointment in associatedAppointments)
                    {
                        await _appointmentRepository.DeleteAsync(appointment);
                    }

                    // Remove the schedule
                    await _scheduleRepository.DeleteAsync(schedule);
                }

                // Save changes for Appointments and Schedules
                await _scheduleRepository.SaveChangesAsync();
                await _appointmentRepository.SaveChangesAsync();

                // Delete the doctor
                await _doctorRepository.DeleteAsync(doctor);
                await _doctorRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting doctor.");
                throw new Exception("An error occurred while deleting the doctor. Please try again.");
            }
        }


        public async Task<IEnumerable<Doctor>> GetAllDoctors()
        {
            try
            {
                return await _doctorRepository.GetDbSet()
                    .Include(d => d.DoctorHospitals)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all doctors.");
                throw new Exception("An error occurred while fetching doctors. Please try again.");
            }
        }

        public async Task<Doctor> GetDoctorById(int id)
        {
            try
            {
                var doctor = await _doctorRepository.GetDbSet()
                    .Include(d => d.DoctorHospitals)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (doctor == null)
                {
                    throw new Exception($"Doctor with Id {id} not found.");
                }
                return doctor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving doctor with Id {id}.");
                throw new Exception($"An error occurred while fetching the doctor: {ex.Message}");
            }
        }
    }
}
