using HMS_Final.Models;
using HMS_Final.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS_Final.Manager.Feedbacks
{
    public class FeedbackManager : IFeedbackManager
    {
        private readonly IGenericRepository<Feedback> _repository;

        public FeedbackManager(IGenericRepository<Feedback> repository)
        {
            _repository = repository;
        }

        public async Task<Feedback> CreateAsync(Feedback feedback)
        {
            try
            {
                await _repository.AddAsync(feedback);
                await _repository.SaveChangesAsync();
                return feedback;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("An error occurred while creating the feedback.", ex);
            }
        }

        public async Task<Feedback> UpdateAsync(Feedback feedback)
        {
            try
            {
                var existingFeedback = await _repository.GetByIdAsync(feedback.Id);
                if (existingFeedback == null)
                {
                    throw new KeyNotFoundException("Feedback not found.");
                }
                existingFeedback.AppointmentId = feedback.AppointmentId;
                existingFeedback.Rating = feedback.Rating;
                existingFeedback.Comments = feedback.Comments;
                await _repository.UpdateAsync(existingFeedback);
                await _repository.SaveChangesAsync();
                return existingFeedback;
            }
            catch (KeyNotFoundException)
            {
                throw; // Re-throw specific exceptions if needed
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("An error occurred while updating the feedback.", ex);
            }
        }

        public async Task<Feedback> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("An error occurred while retrieving the feedback by ID.", ex);
            }
        }

        public async Task<IEnumerable<Feedback>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("An error occurred while retrieving all feedbacks.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var feedback = await _repository.GetByIdAsync(id);
                if (feedback == null)
                {
                    throw new KeyNotFoundException("Feedback not found.");
                }

                await _repository.DeleteAsync(feedback);
                await _repository.SaveChangesAsync();
            }
            catch (KeyNotFoundException)
            {
                throw; // Re-throw specific exceptions if needed
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("An error occurred while deleting the feedback.", ex);
            }
        }
    }
}
