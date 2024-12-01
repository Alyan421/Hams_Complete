using AutoMapper;
using HMS_Final.Controllers.Doctors.DTO;
using HMS_Final.Manager.Doctors;
using HMS_Final.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace HMS_Final.Controllers.Doctors
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase, IDoctorController
    {
        private readonly IDoctorManager _doctorManager;
        private readonly IMapper _mapper;

        public DoctorController(IDoctorManager doctorManager, IMapper mapper)
        {
            _doctorManager = doctorManager;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorDTO dto)
        {
            try
            {
                var doctor = _mapper.Map<Doctor>(dto);
                var createdDoctor = await _doctorManager.CreateDoctor(doctor, dto.HospitalDTOs);
                return Ok(new { Message = "Doctor created successfully.", Doctor = createdDoctor });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateDoctor([FromBody] UpdateDoctorDTO dto)
        {
            try
            {
                var doctor = _mapper.Map<Doctor>(dto);
                var result = await _doctorManager.UpdateDoctor(doctor, dto.HospitalIds);
                if (!result) return NotFound(new { Message = "Doctor not found." });

                return Ok(new { Message = "Doctor updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            try
            {
                var result = await _doctorManager.DeleteDoctor(id);
                if (!result) return NotFound(new { Message = "Doctor not found." });

                return Ok(new { Message = "Doctor deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _doctorManager.GetDoctorById(id);
            if (doctor == null) return NotFound(new { Message = "Doctor not found." });

            var result = _mapper.Map<GetDoctorByIdDTO>(doctor);
            result.HospitalIds = doctor.DoctorHospitals.Select(dh => dh.HospitalId).ToList();
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _doctorManager.GetAllDoctors();
            var result = doctors.Select(doctor =>
            {
                var dto = _mapper.Map<GetDoctorDTO>(doctor);
                dto.HospitalDTOs = doctor.DoctorHospitals.Select(dh => dh.HospitalId).ToList();
                return dto;
            }).ToList();

            return Ok(result);
        }
    }
}
