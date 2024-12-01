using HMS_Final.Controllers.Feedbacks.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HMS_Final.Controllers.Feedbacks
{
    public interface IFeedbackController
    {
        Task<IActionResult> CreateFeedback(FeedbackCreateDTO dto);
        Task<IActionResult> UpdateFeedback(FeedbackUpdateDTO dto);
        Task<IActionResult> DeleteFeedback(int id);
        Task<IActionResult> GetFeedbackById(int id);
        Task<IActionResult> GetAllFeedbacks();
    }
}
