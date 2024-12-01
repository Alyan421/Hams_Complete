using AutoMapper;
using HMS_Final.Controllers.Hospitals.DTO;
using HMS_Final.Manager.Departments.DTO_temp_;
using HMS_Final.Manager.Hospitals;
using HMS_Final.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS_Final.Controllers.Hospitals
{
    [ApiController]
    [Route("api/[controller]")]
    public class HospitalController : ControllerBase, IHospitalController
    {
        private readonly IHospitalManager _hospitalManager;
        private readonly IMapper _mapper;

        public HospitalController(IHospitalManager hospitalManager, IMapper mapper)
        {
            _hospitalManager = hospitalManager;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateHospital([FromBody] CreateHospitalDTO dto)
        {
            try
            {
                var hospital = _mapper.Map<Hospital>(dto);

                // Map department DTOs if provided
                var departmentDTOs = dto.Departments != null
                    ? _mapper.Map<List<HospitalDepartmentDTO1>>(dto.Departments)
                    : null;

                // Map doctor DTOs if provided
                var doctorDTOs = dto.Doctors != null
                    ? _mapper.Map<List<HospitalDoctorDTO2>>(dto.Doctors)
                    : null;

                var createdHospital = await _hospitalManager.CreateHospital(hospital, departmentDTOs, doctorDTOs);
                return Ok(new { Message = "Hospital created successfully.", HospitalId = createdHospital.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteHospital(int id)
        {
            try
            {
                var deleted = await _hospitalManager.DeleteHospital(id);

                if (!deleted) return NotFound(new { Message = "Hospital not found." });

                return Ok(new { Message = "Hospital deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the hospital.", Error = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllHospitals()
        {
            try
            {
                var hospitals = await _hospitalManager.GetAllHospitals();
                var result = _mapper.Map<IEnumerable<GetHospitalsDTO>>(hospitals);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving all hospitals.", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHospitalById(int id)
        {
            try
            {
                var hospital = await _hospitalManager.GetHospital(id);
                if (hospital == null) return NotFound(new { Message = "Hospital not found." });

                var result = _mapper.Map<GetHospitalByIdDTO>(hospital);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the hospital.", Error = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateHospital([FromBody] UpdateHospitalDTO dto)
        {
            try
            {
                var hospital = _mapper.Map<Hospital>(dto);

                // Map department DTOs if provided
                var departmentDTOs = dto.Departments != null
                    ? _mapper.Map<List<HospitalDepartmentDTO1>>(dto.Departments)
                    : null;

                // Map doctor DTOs if provided
                var doctorDTOs = dto.Doctors != null
                    ? _mapper.Map<List<HospitalDoctorDTO2>>(dto.Doctors)
                    : null;

                var isUpdated = await _hospitalManager.UpdateHospital(hospital, departmentDTOs, doctorDTOs);

                if (!isUpdated)
                {
                    return NotFound(new { Message = "Hospital not found." });
                }

                return Ok(new { Message = "Hospital updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while updating the hospital.",
                    Error = ex.Message
                });
            }
        }

        [HttpPost("{hospitalId}/users/{userId}")]
        public async Task<IActionResult> AddHospitalToUser(int hospitalId, int userId)
        {
            try
            {
                await _hospitalManager.AddHospitalToUserAsync(hospitalId, userId);
                return Ok(new { message = "Hospital added to user successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{hospitalId}/users/{userId}")]
        public async Task<IActionResult> RemoveHospitalFromUser(int hospitalId, int userId)
        {
            try
            {
                await _hospitalManager.RemoveHospitalFromUserAsync(hospitalId, userId);
                return Ok(new { message = "Hospital removed from user successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

