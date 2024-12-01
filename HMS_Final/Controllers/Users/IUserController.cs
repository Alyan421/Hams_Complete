using HMS_Final.Controllers.Users.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS_Final.Controllers.Users
{
    public interface IUserController
    {
        Task<IActionResult> CreateAsync(CreateUserDTO createUserDTO);
        Task<IActionResult> GetByIdAsync(int id);
        Task<IActionResult> UpdateAsync(UpdateUserDTO updateUserDTO);
        Task<IActionResult> DeleteUserAsync(int id);
        Task<IActionResult> GetAllAsync();
        Task<IActionResult> VerifyOTPAsync(int id, [FromBody] long otp);
        Task<IActionResult> GenerateOTP(int id);
    }
}
