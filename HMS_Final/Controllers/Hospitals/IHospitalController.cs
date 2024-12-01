using HMS_Final.Controllers.Hospitals.DTO;
using HMS_Final.Manager.Hospitals;
using Microsoft.AspNetCore.Mvc;

namespace HMS_Final.Controllers.Hospitals
{

    public interface IHospitalController
    {
        Task<IActionResult> CreateHospital(CreateHospitalDTO dto);
       // Task<IActionResult> CreateHospital(CreateHospitalRequestDTO request);
        Task<IActionResult> GetHospitalById(int id);
        Task<IActionResult> GetAllHospitals();
          Task<IActionResult> UpdateHospital(UpdateHospitalDTO dto);
        //Task<IActionResult> UpdateHospital(UpdateHospitalDTO dto, List<HospitalDepartmentDTO1> departmentDTOs);

        Task<IActionResult> DeleteHospital(int id);
    }
}
