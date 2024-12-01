﻿using HMS_Final.Models;
using HMS_Final.Repository;
using HMS_Final.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HMS_Final.Manager.Users
{
    public class UserManager : IUserManager
    {
        private readonly IEmailService _emailService;
        private readonly IGenericRepository<User> _repository;
        private readonly IGenericRepository<UserHospital> _userHospitalRepository;

        public UserManager(IGenericRepository<User> repository, IEmailService emailService, IGenericRepository<UserHospital> userHospitalRepository)
        {
            _repository = repository;
            _emailService = emailService;
            _userHospitalRepository = userHospitalRepository;
        }

        public async Task<User> CreateAsync(User entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity), "User entity cannot be null.");
                }

                // Check for duplicate username
                var existingUser = await _repository.GetAllAsync();
                if (existingUser.Any(u => u.UserName == entity.UserName))
                {
                    throw new InvalidOperationException("Username already exists.");
                }

                entity.GenerateOTP();
                await _repository.AddAsync(entity);
                await _repository.SaveChangesAsync();

                // Send OTP email
                string subject = "HAMS OTP Code";
                string body = $"Your OTP is {entity.OTP}. It will expire in 5 minutes. You can safely ignore the mail if this is not you trying to login.";
                await _emailService.SendEmailAsync(entity.Email, subject, body);

                return entity;
            }
            catch (Exception ex)
            {
                // Log exception if logging is enabled
                throw new Exception("Error occurred while creating a User.", ex);
            }
        }

        public async Task<User> UpdateAsync(User entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity), "User entity cannot be null.");
                }

                var existingUser = await _repository.GetByIdAsync(entity.Id);
                if (existingUser == null)
                {
                    throw new KeyNotFoundException("User not found.");
                }

                existingUser.UserName = entity.UserName;
                await _repository.UpdateAsync(existingUser);
                await _repository.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating the User.", ex);
            }
        }

        public async Task<User> GetByIdAsync(int id)
        {
            try
            {
                var user = await _repository.GetByIdAsync(id);
                if (user == null)
                {
                    throw new KeyNotFoundException("User not found.");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving the user by ID.", ex);
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving all users.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var user = await _repository.GetByIdAsync(id);
                if (user == null)
                {
                    throw new KeyNotFoundException("User not found.");
                }

                await _repository.DeleteAsync(user);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting the user.", ex);
            }
        }

        public async Task<bool> VerifyOTPAsync(int userId, long otp)
        {
            try
            {
                var user = await _repository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new KeyNotFoundException("User not found.");
                }

                bool isVerified = user.VerifyOTP(otp);
                if (isVerified)
                {
                    await _repository.UpdateAsync(user);
                    await _repository.SaveChangesAsync();
                }

                return isVerified;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while verifying the OTP.", ex);
            }
        }

        public async Task GenerateOTPAsync(int userId)
        {
            try
            {
                var user = await _repository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new KeyNotFoundException("User not found.");
                }
                if (user.ResendCount >= 3)
                {
                    await _repository.DeleteAsync(user);
                    await _repository.SaveChangesAsync();
                    throw new InvalidOperationException("You have reached the maximum number of OTP resends.");
                }
                user.ResendCount++;
                user.GenerateOTP();
                await _repository.UpdateAsync(user);
                await _repository.SaveChangesAsync();

                string subject = "HAMS OTP Code";
                string body = $"Your OTP is {user.OTP}. It will expire in 5 minutes. You can safely ignore the mail if this is not you trying to login.";
                await _emailService.SendEmailAsync(user.Email, subject, body);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while generating OTP.", ex);
            }
        }

        public async Task AddUserToHospitalAsync(int userId, int hospitalId)
        {
            var userHospital = new UserHospital { UserId = userId, HospitalId = hospitalId };
            await _userHospitalRepository.AddAsync(userHospital);
            await _userHospitalRepository.SaveChangesAsync();
        }

        public async Task RemoveUserFromHospitalAsync(int userId, int hospitalId)
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
