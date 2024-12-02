using AutoMapper;
using HMS_Final.Models;
using Microsoft.AspNetCore.Mvc;
using HMS_Final.Manager.Appointments;
using System.Collections.Generic;
using System.Threading.Tasks;
using HMS_Final.Controllers.Appointments.DTO;
using System;
using System.Linq;

namespace HMS_Final.Controllers.Appointments
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase, IAppointmentController
    {
        private readonly IAppointmentManager _appointmentManager;
        private readonly IMapper _mapper;

        public AppointmentController(IAppointmentManager appointmentManager, IMapper mapper)
        {
            _appointmentManager = appointmentManager;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid appointment ID.");
            }

            try
            {
                var appointment = await _appointmentManager.GetByIdAsync(id);
                var appointmentDTO = _mapper.Map<AppointmentGetByIdDTO>(appointment);
                return Ok(appointmentDTO);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Appointment with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(AppointmentUpdateDTO updateAppointmentDTO)
        {
            if (updateAppointmentDTO == null || updateAppointmentDTO.Id <= 0)
            {
                return BadRequest("Valid appointment data is required.");
            }

            try
            {
                var appointment = _mapper.Map<Appointment>(updateAppointmentDTO);
                var updatedAppointment = await _appointmentManager.UpdateAsync(appointment);
                return Ok(new { Message = "Appointment updated successfully." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Appointment with ID {updateAppointmentDTO.Id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointmentAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid appointment ID.");
            }

            try
            {
                await _appointmentManager.DeleteAsync(id);
                return Ok(new { Message = "Appointment deleted successfully." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Appointment with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var appointments = await _appointmentManager.GetAllAsync();
                var appointmentDTOs = _mapper.Map<IEnumerable<AppointmentGetDTO>>(appointments);
                return Ok(appointmentDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}