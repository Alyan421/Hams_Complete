using AutoMapper;
using HMS_Final.Controllers.Insurances.DTO;
using HMS_Final.Manager.Insurances;
using Microsoft.AspNetCore.Mvc;
using HMS_Final.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS_Final.Controllers.Insurances
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsuranceController : ControllerBase, IInsuranceController
    {
        private readonly IInsuranceManager _insuranceManager;
        private readonly IMapper _mapper;

        public InsuranceController(IInsuranceManager insuranceManager, IMapper mapper)
        {
            _insuranceManager = insuranceManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInsurance([FromBody] InsuranceCreateDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Insurance data is required.");
            }

            try
            {
                var insurance = _mapper.Map<Insurance>(dto);
                var createdInsurance = await _insuranceManager.CreateAsync(insurance);
                return Ok(new { Message = "Insurance created successfully.", Insurance = createdInsurance });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInsurance([FromBody] InsuranceUpdateDTO dto)
        {
            if (dto == null || dto.Id <= 0)
            {
                return BadRequest("Valid insurance data is required.");
            }

            try
            {
                var insurance = _mapper.Map<Insurance>(dto);
                var updatedInsurance = await _insuranceManager.UpdateAsync(insurance);
                return Ok(new { Message = "Insurance updated successfully." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Insurance with ID {dto.Id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInsurance(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid insurance ID.");
            }

            try
            {
                await _insuranceManager.DeleteAsync(id);
                return Ok(new { Message = "Insurance deleted successfully." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Insurance not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInsuranceById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid insurance ID.");
            }

            try
            {
                var insurance = await _insuranceManager.GetByIdAsync(id);
                if (insurance == null)
                {
                    return NotFound("Insurance not found.");
                }

                var insuranceDTO = _mapper.Map<InsuranceGetByIdDTO>(insurance);
                return Ok(insuranceDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInsurances()
        {
            try
            {
                var insurances = await _insuranceManager.GetAllAsync();
                var insuranceDTOs = _mapper.Map<IEnumerable<InsuranceGetDTO>>(insurances);
                return Ok(insuranceDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
