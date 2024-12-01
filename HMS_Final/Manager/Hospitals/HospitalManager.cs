using HMS_Final.Manager.Departments.DTO_temp_;
using HMS_Final.Models;
using HMS_Final.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS_Final.Manager.Hospitals
{
    public class HospitalManager : IHospitalManager
    {
        private readonly IGenericRepository<Hospital> _hospitalRepository;
        private readonly ILogger<HospitalManager> _logger;
        private readonly IGenericRepository<HospitalDepartment> _hospitalDepartmentRepository;
        private readonly IGenericRepository<DoctorHospital> _doctorHospitalRepository;
        private readonly IGenericRepository<Insurance> _insuranceRepository;
        private readonly IGenericRepository<UserHospital> _userHospitalRepository;

        public HospitalManager(
            IGenericRepository<Hospital> hospitalRepository,
            ILogger<HospitalManager> logger,
            IGenericRepository<HospitalDepartment> hospitalDepartmentRepository,
            IGenericRepository<DoctorHospital> doctorHospitalRepository,
            IGenericRepository<Insurance> insuranceRepository,
            IGenericRepository<UserHospital> userHospitalRepository)
        {
            _hospitalRepository = hospitalRepository;
            _logger = logger;
            _hospitalDepartmentRepository = hospitalDepartmentRepository;
            _doctorHospitalRepository = doctorHospitalRepository;
            _insuranceRepository = insuranceRepository;
            _userHospitalRepository = userHospitalRepository;
        }

        public async Task<Hospital> CreateHospital(
    Hospital hospital,
    List<HospitalDepartmentDTO1>? departmentDTOs = null,
    List<HospitalDoctorDTO2>? doctorDTOs = null)
        {
            try
            {
                // Check if a hospital with the same name exists in the given city
                var existingHospital = await _hospitalRepository.GetDbSet()
                    .FirstOrDefaultAsync(h => h.Name.ToLower() == hospital.Name.ToLower() && h.CityId == hospital.CityId);

                if (existingHospital != null)
                {
                    throw new Exception($"A hospital with the name '{hospital.Name}' already exists in the specified city.");
                }

                // Add the hospital
                await _hospitalRepository.AddAsync(hospital);
                await _hospitalRepository.SaveChangesAsync();

                // Add relationships in HospitalDepartment if departmentDTOs is not null or empty
                if (departmentDTOs != null && departmentDTOs.Any())
                {
                    foreach (var departmentDTO in departmentDTOs)
                    {
                        var hospitalDepartment = new HospitalDepartment
                        {
                            HospitalId = hospital.Id,
                            DepartmentId = departmentDTO.DepartmentId
                        };
                        await _hospitalDepartmentRepository.AddAsync(hospitalDepartment);
                    }

                    await _hospitalDepartmentRepository.SaveChangesAsync();
                }

                // Add relationships in DoctorHospital if doctorDTOs is not null or empty
                if (doctorDTOs != null && doctorDTOs.Any())
                {
                    foreach (var doctorDTO in doctorDTOs)
                    {
                        var doctorHospital = new DoctorHospital
                        {
                            HospitalId = hospital.Id,
                            DoctorId = doctorDTO.DoctorId
                        };
                        await _doctorHospitalRepository.AddAsync(doctorHospital);
                    }

                    await _doctorHospitalRepository.SaveChangesAsync();
                }

                return hospital;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update failed while creating a hospital.");
                throw new Exception("An error occurred while saving the hospital to the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating a hospital.");
                throw;
            }
        }

        public async Task<bool> DeleteHospital(int id)
        {
            try
            {
                // Retrieve the hospital
                var hospital = await _hospitalRepository.GetByIdAsync(id);
                if (hospital == null)
                {
                    _logger.LogWarning($"Hospital with Id {id} not found.");
                    return false;
                }

                // Check for related departments
                var departmentCount = await _hospitalDepartmentRepository.GetDbSet()
                    .CountAsync(hd => hd.HospitalId == id);
                if (departmentCount > 0)
                {
                    throw new Exception($"Cannot delete hospital with Id {id} because related departments exist. Please delete associated departments first.");
                }

                // Check for related doctors
                var doctorCount = await _doctorHospitalRepository.GetDbSet()
                    .CountAsync(dh => dh.HospitalId == id);
                if (doctorCount > 0)
                {
                    throw new Exception($"Cannot delete hospital with Id {id} because related doctors exist. Please delete associated doctors first.");
                }

                // Delete related insurances
                var insurances = await _insuranceRepository.GetDbSet()
                    .Where(i => i.HospitalId == id)
                    .ToListAsync();
                foreach (var insurance in insurances)
                {
                    await _insuranceRepository.DeleteAsync(insurance);
                }
                await _insuranceRepository.SaveChangesAsync();

                // If no related dependencies, delete the hospital
                await _hospitalRepository.DeleteAsync(hospital);
                await _hospitalRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting hospital with Id {id}.");
                throw new Exception($"An error occurred while deleting the hospital: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Hospital>> GetAllHospitals()
        {
            try
            {
                return await _hospitalRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all hospitals.");
                throw new Exception("An error occurred while fetching hospitals. Please try again.");
            }
        }

        public async Task<Hospital> GetHospital(int id)
        {
            try
            {
                var hospital = await _hospitalRepository.GetByIdAsync(id);
                if (hospital == null)
                {
                    _logger.LogWarning($"Hospital with Id {id} not found.");
                    return null;
                }

                return hospital;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving hospital with Id {id}.");
                throw new Exception("An error occurred while fetching the hospital. Please try again.");
            }
        }


        //public async Task<bool> UpdateHospital(Hospital hospital, List<HospitalDepartmentDTO1> departmentDTOs)
        //{
        //    try
        //    {
        //        // Check if the hospital exists
        //        var existingHospital = await _hospitalRepository.GetByIdAsync(hospital.Id);
        //        if (existingHospital == null)
        //        {
        //            _logger.LogWarning($"Hospital with Id {hospital.Id} not found.");
        //            return false;
        //        }

        //        // Check for duplicate hospital in the same city
        //        var duplicateHospital = await _hospitalRepository.GetDbSet()
        //            .FirstOrDefaultAsync(h => h.Name.ToLower() == hospital.Name.ToLower()
        //                                      && h.CityId == hospital.CityId
        //                                      && h.Id != hospital.Id);
        //        if (duplicateHospital != null)
        //        {
        //            throw new Exception($"Another hospital with the name '{hospital.Name}' already exists in the specified city.");
        //        }

        //        // Update hospital details
        //        existingHospital.Name = hospital.Name;
        //        existingHospital.ContactInfo = hospital.ContactInfo;
        //        existingHospital.CityId = hospital.CityId;

        //        await _hospitalRepository.UpdateAsync(existingHospital);
        //        await _hospitalRepository.SaveChangesAsync();

        //        // Remove old relationships in HospitalDepartment
        //        var existingAssociations = await _hospitalDepartmentRepository.GetDbSet()
        //            .Where(hd => hd.HospitalId == hospital.Id)
        //            .ToListAsync();

        //        foreach (var association in existingAssociations)
        //        {
        //            await _hospitalDepartmentRepository.DeleteAsync(association);
        //        }

        //        // Add new relationships in HospitalDepartment
        //        foreach (var departmentDTO in departmentDTOs)
        //        {
        //            var hospitalDepartment = new HospitalDepartment
        //            {
        //                HospitalId = hospital.Id,
        //                DepartmentId = departmentDTO.DepartmentId
        //            };
        //            await _hospitalDepartmentRepository.AddAsync(hospitalDepartment);
        //        }

        //        await _hospitalDepartmentRepository.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (DbUpdateException dbEx)
        //    {
        //        _logger.LogError(dbEx, $"Database update failed while updating hospital with Id {hospital.Id}.");
        //        throw new Exception("An error occurred while saving the updated hospital to the database.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Unexpected error occurred while updating hospital with Id {hospital.Id}.");
        //        throw;
        //    }
        //}

        public async Task<bool> UpdateHospital( Hospital hospital,List<HospitalDepartmentDTO1> departmentDTOs, List<HospitalDoctorDTO2>? doctorDTOs = null)
        {
            try
            {
                // Check if the hospital exists
                var existingHospital = await _hospitalRepository.GetByIdAsync(hospital.Id);
                if (existingHospital == null)
                {
                    _logger.LogWarning($"Hospital with Id {hospital.Id} not found.");
                    return false;
                }

                // Check for duplicate hospital in the same city
                var duplicateHospital = await _hospitalRepository.GetDbSet()
                    .FirstOrDefaultAsync(h => h.Name.ToLower() == hospital.Name.ToLower()
                                              && h.CityId == hospital.CityId
                                              && h.Id != hospital.Id);
                if (duplicateHospital != null)
                {
                    throw new Exception($"Another hospital with the name '{hospital.Name}' already exists in the specified city.");
                }

                // Update hospital details
                existingHospital.Name = hospital.Name;
                existingHospital.ContactInfo = hospital.ContactInfo;
                existingHospital.CityId = hospital.CityId;

                await _hospitalRepository.UpdateAsync(existingHospital);
                await _hospitalRepository.SaveChangesAsync();

                // Remove old HospitalDepartment relationships
                var existingHospitalDepartments = await _hospitalDepartmentRepository.GetDbSet()
                    .Where(hd => hd.HospitalId == hospital.Id)
                    .ToListAsync();
                foreach (var hospitalDepartment in existingHospitalDepartments)
                {
                    await _hospitalDepartmentRepository.DeleteAsync(hospitalDepartment);
                }

                // Add new HospitalDepartment relationships
                foreach (var departmentDTO in departmentDTOs)
                {
                    var hospitalDepartment = new HospitalDepartment
                    {
                        HospitalId = hospital.Id,
                        DepartmentId = departmentDTO.DepartmentId
                    };
                    await _hospitalDepartmentRepository.AddAsync(hospitalDepartment);
                }

                await _hospitalDepartmentRepository.SaveChangesAsync();

                // Remove old DoctorHospital relationships
                var existingDoctorHospitals = await _doctorHospitalRepository.GetDbSet()
                    .Where(dh => dh.HospitalId == hospital.Id)
                    .ToListAsync();
                foreach (var doctorHospital in existingDoctorHospitals)
                {
                    await _doctorHospitalRepository.DeleteAsync(doctorHospital);
                }

                // Add new DoctorHospital relationships
                if (doctorDTOs != null && doctorDTOs.Any())
                {
                    foreach (var doctorDTO in doctorDTOs)
                    {
                        var doctorHospital = new DoctorHospital
                        {
                            HospitalId = hospital.Id,
                            DoctorId = doctorDTO.DoctorId
                        };
                        await _doctorHospitalRepository.AddAsync(doctorHospital);
                    }

                    await _doctorHospitalRepository.SaveChangesAsync();
                }

                return true;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Database update failed while updating hospital with Id {hospital.Id}.");
                throw new Exception("An error occurred while saving the updated hospital to the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error occurred while updating hospital with Id {hospital.Id}.");
                throw;
            }
        }

        public async Task AddHospitalToUserAsync(int hospitalId, int userId)
        {
            var userHospital = new UserHospital { UserId = userId, HospitalId = hospitalId };
            await _userHospitalRepository.AddAsync(userHospital);
            await _userHospitalRepository.SaveChangesAsync();
        }

        public async Task RemoveHospitalFromUserAsync(int hospitalId, int userId)
        {
            var userHospital = await _userHospitalRepository.GetDbSet()
                .FirstOrDefaultAsync(uh => uh.UserId == userId && uh.HospitalId == hospitalId);
            if (userHospital != null)
            {
                await _userHospitalRepository.DeleteAsync(userHospital);
                await _userHospitalRepository.SaveChangesAsync();
            }
        }
    }
}
