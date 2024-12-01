using HMS_Final.Manager.Departments.DTO_temp_;
using System.ComponentModel.DataAnnotations;

namespace HMS_Final.Controllers.Departments.DTO
{
    public class CreateDepartmentDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public List<HospitalDepartmentDTO> HospitalIds { get; set; }
    }
}
