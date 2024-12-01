using HMS_Final.Models;
using HMS_Final.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS_Final.Manager.Insurances
{
    public class InsuranceManager : IInsuranceManager
    {
        private readonly IGenericRepository<Insurance> _repository;

        public InsuranceManager(IGenericRepository<Insurance> repository)
        {
            _repository = repository;
        }

        public async Task<Insurance> CreateAsync(Insurance insurance)
        {
            if (insurance == null)
            {
                throw new ArgumentNullException(nameof(insurance), "Insurance entity cannot be null.");
            }

            try
            {
                await _repository.AddAsync(insurance);
                await _repository.SaveChangesAsync();
                return insurance;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the insurance.", ex);
            }
        }

        public async Task<Insurance> UpdateAsync(Insurance insurance)
        {
            if (insurance == null)
            {
                throw new ArgumentNullException(nameof(insurance), "Insurance entity cannot be null.");
            }

            try
            {
                var existingInsurance = await _repository.GetByIdAsync(insurance.Id);
                if (existingInsurance == null)
                {
                    throw new KeyNotFoundException("Insurance not found.");
                }

                existingInsurance.InsuranceType = insurance.InsuranceType ?? existingInsurance.InsuranceType;
                existingInsurance.CompanyName = insurance.CompanyName ?? existingInsurance.CompanyName;
                existingInsurance.HospitalId = insurance.HospitalId != 0 ? insurance.HospitalId : existingInsurance.HospitalId;

                await _repository.UpdateAsync(existingInsurance);
                await _repository.SaveChangesAsync();
                return existingInsurance;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the insurance.", ex);
            }
        }

        public async Task<Insurance> GetByIdAsync(int id)
        {
            try
            {
                var insurance = await _repository.GetByIdAsync(id);
                if (insurance == null)
                {
                    throw new KeyNotFoundException("Insurance not found.");
                }

                return insurance;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the insurance by ID.", ex);
            }
        }

        public async Task<IEnumerable<Insurance>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all insurances.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var insurance = await _repository.GetByIdAsync(id);
                if (insurance == null)
                {
                    throw new KeyNotFoundException("Insurance not found.");
                }

                await _repository.DeleteAsync(insurance);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the insurance.", ex);
            }
        }
    }
}
