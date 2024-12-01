using HMS_Final.Controllers.Departments.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HMS_Final.Controllers.Departments
{
    public interface IDepartmentController
    {
        Task<IActionResult> CreateDepartment(CreateDepartmentDTO dto);
        Task<IActionResult> UpdateDepartment(UpdateDepartmentDTO dto);
        Task<IActionResult> DeleteDepartment(int id);
        Task<IActionResult> GetDepartmentById(int id);
        Task<IActionResult> GetAllDepartments();
    }
}
