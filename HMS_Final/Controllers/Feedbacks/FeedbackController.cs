using AutoMapper;
using HMS_Final.Controllers.Feedbacks.DTO;
using HMS_Final.Manager.Feedbacks;
using Microsoft.AspNetCore.Mvc;
using HMS_Final.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS_Final.Controllers.Feedbacks
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase, IFeedbackController
    {
        private readonly IFeedbackManager _feedbackManager;
        private readonly IMapper _mapper;

        public FeedbackController(IFeedbackManager feedbackManager, IMapper mapper)
        {
            _feedbackManager = feedbackManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackCreateDTO dto)
        {
            try
            {
                var feedback = _mapper.Map<Feedback>(dto);
                var createdFeedback = await _feedbackManager.CreateAsync(feedback);
                return Ok(new { message = "Feedback created successfully.", Feedback = createdFeedback });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFeedback([FromBody] FeedbackUpdateDTO dto)
        {
            try
            {
                if (dto == null || dto.Id <= 0)
                {
                    return BadRequest("Valid feedback data is required.");
                }

                var feedback = _mapper.Map<Feedback>(dto);
                var updatedFeedback = await _feedbackManager.UpdateAsync(feedback);

                return Ok(new { message = "Feedback updated successfully."});
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            try
            {
                await _feedbackManager.DeleteAsync(id);
                return Ok(new { message = "Feedback deleted successfully." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Feedback not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeedbackById(int id)
        {
            try
            {
                var feedback = await _feedbackManager.GetByIdAsync(id);
                if (feedback == null)
                {
                    return NotFound("Feedback not found.");
                }

                var feedbackDTO = _mapper.Map<FeedbackGetByIdDTO>(feedback);
                return Ok(feedbackDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            try
            {
                var feedbacks = await _feedbackManager.GetAllAsync();
                var feedbackDTOs = _mapper.Map<IEnumerable<FeedbackGetDTO>>(feedbacks);
                return Ok(feedbackDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
