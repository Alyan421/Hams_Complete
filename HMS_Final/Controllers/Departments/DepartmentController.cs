using AutoMapper;
using HMS_Final.Controllers.Departments.DTO;
using HMS_Final.Manager.Departments;
using HMS_Final.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS_Final.Controllers.Departments
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase, IDepartmentController
    {
        private readonly IDepartmentManager _departmentManager;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentManager departmentManager, IMapper mapper)
        {
            _departmentManager = departmentManager;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDTO dto)
        {
            try
            {
                var department = _mapper.Map<Department>(dto);
                var createdDepartment = await _departmentManager.CreateDepartment(department, dto.HospitalIds);
                return Ok(new { Message = "Department created successfully.", Department = createdDepartment });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateDepartment([FromBody] UpdateDepartmentDTO dto)
        {
            try
            {
                var department = _mapper.Map<Department>(dto);
                var result = await _departmentManager.UpdateDepartment(department, dto.HospitalIds);
                if (!result) return NotFound(new { Message = "Department not found." });

                return Ok(new { Message = "Department updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var result = await _departmentManager.DeleteDepartment(id);
                if (!result) return NotFound(new { Message = "Department not found." });

                return Ok(new { Message = "Department deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var department = await _departmentManager.GetDepartmentById(id);
            if (department == null) return NotFound(new { Message = "Department not found." });

            var result = _mapper.Map<GetDepartmentById>(department);
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentManager.GetAllDepartments();
            var result = _mapper.Map<IEnumerable<GetAllDepartmentDTO>>(departments);
            return Ok(result);
        }
    }

}
