using HMS_Final.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS_Final.Manager.Feedbacks
{
    public interface IFeedbackManager
    {
        Task<Feedback> CreateAsync(Feedback feedback);
        Task<Feedback> UpdateAsync(Feedback feedback);
        Task<Feedback> GetByIdAsync(int id);
        Task<IEnumerable<Feedback>> GetAllAsync();
        Task DeleteAsync(int id);
    }
}
