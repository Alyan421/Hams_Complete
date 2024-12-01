using HMS_Final.Controllers.Insurances.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HMS_Final.Controllers.Insurances
{
    public interface IInsuranceController
    {
        Task<IActionResult> CreateInsurance(InsuranceCreateDTO dto);
        Task<IActionResult> UpdateInsurance(InsuranceUpdateDTO dto);
        Task<IActionResult> DeleteInsurance(int id);
        Task<IActionResult> GetInsuranceById(int id);
        Task<IActionResult> GetAllInsurances();
    }
}
