using AutoMapper;
using HMS_Final.Controllers.Schedules.DTO;
using HMS_Final.Manager.Schedules;
using HMS_Final.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS_Final.Controllers.Schedules
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase, IScheduleController
    {
        private readonly IScheduleManager _scheduleManager;
        private readonly IMapper _mapper;

        public ScheduleController(IScheduleManager scheduleManager, IMapper mapper)
        {
            _scheduleManager = scheduleManager;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleDTO dto)
        {
            try
            {
                var schedule = _mapper.Map<Schedule>(dto);
                var createdSchedule = await _scheduleManager.CreateSchedule(schedule);
                return Ok(new { Message = "Schedule created successfully.", Schedule = createdSchedule });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateSchedule([FromBody] UpdateScheduleDTO dto)
        {
            try
            {
                var schedule = _mapper.Map<Schedule>(dto);
                var isUpdated = await _scheduleManager.UpdateSchedule(schedule);
                if (!isUpdated) return NotFound(new { Message = "Schedule not found." });

                return Ok(new { Message = "Schedule updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            try
            {
                var isDeleted = await _scheduleManager.DeleteSchedule(id);
                if (!isDeleted) return NotFound(new { Message = "Schedule not found." });

                return Ok(new { Message = "Schedule deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllSchedules()
        {
            var schedules = await _scheduleManager.GetAllSchedules();
            var result = _mapper.Map<IEnumerable<GetAllSchedule>>(schedules);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduleById(int id)
        {
            var schedule = await _scheduleManager.GetScheduleById(id);
            if (schedule == null) return NotFound(new { Message = "Schedule not found." });

            var result = _mapper.Map<GetAllSchedule>(schedule);
            return Ok(result);
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookSchedule([FromBody] BookScheduleDTO dto)
        {
            try
            {
                if (!dto.ConsultationDate.HasValue)
                {
                    return BadRequest(new { Message = "Consultation date is required." });
                }

                bool isBooked = await _scheduleManager.BookSchedule(
                    dto.UserId,
                    dto.ScheduleId,
                    dto.ConsultationDate.Value // Pass the non-nullable value
                );

                if (!isBooked)
                {
                    return BadRequest(new { Message = "Slot already booked. Please try another slot or select the next available week." });
                }

                return Ok(new { Message = "Schedule booked successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }


    }
}
