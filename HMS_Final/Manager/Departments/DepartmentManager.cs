using HMS_Final.Manager.Departments.DTO_temp_;
using HMS_Final.Models;
using HMS_Final.Repository;
using Microsoft.EntityFrameworkCore;


namespace HMS_Final.Manager.Departments
{
    public class DepartmentManager: IDepartmentManager
    {
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IGenericRepository<HospitalDepartment> _hospitalDepartmentRepository;
        private readonly ILogger<DepartmentManager> _logger;

        public DepartmentManager(
            IGenericRepository<Department> departmentRepository,
            IGenericRepository<HospitalDepartment> hospitalDepartmentRepository,
            ILogger<DepartmentManager> logger)
        {
            _departmentRepository = departmentRepository;
            _hospitalDepartmentRepository = hospitalDepartmentRepository;
            _logger = logger;
        }

        public async Task<Department> CreateDepartment(Department department, List<HospitalDepartmentDTO> hospitalDepartments)
        {
            try
            {
                // Check if a department with the same name already exists
                var existingDepartment = await _departmentRepository.GetDbSet()
                    .FirstOrDefaultAsync(d => d.Name.ToLower() == department.Name.ToLower());

                if (existingDepartment != null)
                {
                    throw new Exception($"A department with the name '{department.Name}' already exists.");
                }

                // Add the department
                await _departmentRepository.AddAsync(department);
                await _departmentRepository.SaveChangesAsync();

                // Add relationships in HospitalDepartment
                foreach (var hospitalDepartmentDto in hospitalDepartments)
                {
                    // Check if the relationship already exists
                    var existingRelationship = await _hospitalDepartmentRepository.GetDbSet()
                        .FirstOrDefaultAsync(hd =>
                            hd.HospitalId == hospitalDepartmentDto.HospitalId &&
                            hd.DepartmentId == department.Id);

                    if (existingRelationship == null)
                    {
                        var hospitalDepartment = new HospitalDepartment
                        {
                            HospitalId = hospitalDepartmentDto.HospitalId,
                            DepartmentId = department.Id
                        };
                        await _hospitalDepartmentRepository.AddAsync(hospitalDepartment);
                    }
                }

                await _hospitalDepartmentRepository.SaveChangesAsync();
                return department;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating department.");
                throw new Exception("An error occurred while creating the department. Please try again.");
            }
        }



        public async Task<bool> UpdateDepartment(Department department, List<HospitalDepartmentDTO> hospitalDepartmentDTOs)
        {
            try
            {
                // Check if the department exists
                var existingDepartment = await _departmentRepository.GetByIdAsync(department.Id);
                if (existingDepartment == null)
                {
                    _logger.LogWarning($"Department with Id {department.Id} not found.");
                    return false;
                }

                // **Check if another department with the same name already exists**
                var duplicateDepartment = await _departmentRepository.GetDbSet()
                    .FirstOrDefaultAsync(d => d.Name.ToLower() == department.Name.ToLower() && d.Id != department.Id);
                if (duplicateDepartment != null)
                {
                    throw new Exception($"Another department with the name '{department.Name}' already exists.");
                }

                // Update department details
                existingDepartment.Name = department.Name;
                await _departmentRepository.UpdateAsync(existingDepartment);

                // Remove old relationships
                var existingAssociations = await _hospitalDepartmentRepository.GetDbSet()
                    .Where(hd => hd.DepartmentId == department.Id)
                    .ToListAsync();
                foreach (var association in existingAssociations)
                {
                    await _hospitalDepartmentRepository.DeleteAsync(association);
                }

                // Add new relationships using HospitalDepartmentDTO
                foreach (var hospitalDepartmentDTO in hospitalDepartmentDTOs)
                {
                    var hospitalDepartment = new HospitalDepartment
                    {
                        HospitalId = hospitalDepartmentDTO.HospitalId,
                        DepartmentId = department.Id
                    };
                    await _hospitalDepartmentRepository.AddAsync(hospitalDepartment);
                }

                await _hospitalDepartmentRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating department with Id {department.Id}.");
                throw new Exception("An error occurred while updating the department. Please try again.");
            }
        }



        public async Task<bool> DeleteDepartment(int id)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(id);
                if (department == null) return false;

                // Remove related HospitalDepartment records
                var associatedRecords = await _hospitalDepartmentRepository.GetDbSet()
                    .Where(hd => hd.DepartmentId == id)
                    .ToListAsync();

                foreach (var record in associatedRecords)
                {
                    await _hospitalDepartmentRepository.DeleteAsync(record);
                }

                // Delete the department
                await _departmentRepository.DeleteAsync(department);
                await _departmentRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting department.");
                throw new Exception("An error occurred while deleting the department. Please try again.");
            }
        }

        public async Task<Department> GetDepartmentById(int id)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(id);
                if (department == null)
                {
                    throw new Exception($"Department with Id {id} not found.");
                }
                return department;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving department with Id {id}.");
                throw new Exception($"An error occurred while fetching the department: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Department>> GetAllDepartments()
        {
            try
            {
                var departments = await _departmentRepository.GetAllAsync();
                if (!departments.Any())
                {
                    throw new Exception("No departments found.");
                }
                return departments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all departments.");
                throw new Exception($"An error occurred while fetching departments: {ex.Message}");
            }
        }


    }
}
