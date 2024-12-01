using HMS_Final.Manager.Departments.DTO_temp_;
using HMS_Final.Models;

namespace HMS_Final.Manager.Departments
{
    public interface IDepartmentManager
    {
        Task<Department>CreateDepartment(Department department, List<HospitalDepartmentDTO> hospitalIds);
        Task<bool> UpdateDepartment(Department department, List<HospitalDepartmentDTO> hospitalIds);
        Task<bool> DeleteDepartment(int id);
        Task<Department> GetDepartmentById(int id);
        Task<IEnumerable<Department>> GetAllDepartments();
    }
}
