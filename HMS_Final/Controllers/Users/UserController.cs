using AutoMapper;
using HMS_Final.Models;
using Microsoft.AspNetCore.Mvc;
using HMS_Final.Manager.Users;
using System.Collections.Generic;
using System.Threading.Tasks;
using HMS_Final.Services;
using HMS_Final.Controllers.Users.DTO;
using System;

namespace HMS_Final.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase, IUserController
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        public UserController(IUserManager userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateUserDTO createUserDTO)
        {
            try
            {
                if (createUserDTO == null)
                {
                    return BadRequest("User data is required.");
                }

                var user = _mapper.Map<User>(createUserDTO);
                var createdUser = await _userManager.CreateAsync(user);

                if (createdUser == null)
                {
                    return StatusCode(500, "An error occurred while creating the user.");
                }

                var userDTO = _mapper.Map<UserDTO>(createdUser);
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid user ID.");
                }

                var user = await _userManager.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                var userDTO = _mapper.Map<UserDTO>(user);
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UpdateUserDTO updateUserDTO)
        {
            try
            {
                if (updateUserDTO == null || updateUserDTO.Id <= 0)
                {
                    return BadRequest("Valid user data is required.");
                }

                var user = _mapper.Map<User>(updateUserDTO);
                var updatedUser = await _userManager.UpdateAsync(user);

                if (updatedUser == null)
                {
                    return NotFound($"User with ID {updateUserDTO.Id} not found.");
                }

                var userDTO = _mapper.Map<UpdateUserDTO>(updatedUser);
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid user ID.");
                }

                var user = await _userManager.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                await _userManager.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var users = await _userManager.GetAllAsync();
                if (users == null || !users.Any())
                {
                    return NotFound("No users found.");
                }

                var userDTOs = _mapper.Map<IEnumerable<UserGetDTO>>(users);
                return Ok(userDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{id}/verify-otp")]
        public async Task<IActionResult> VerifyOTPAsync(int id, [FromBody] long otp)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid user ID.");
                }

                bool isVerified = await _userManager.VerifyOTPAsync(id, otp);
                if (isVerified)
                {
                    return Ok("OTP verified successfully.");
                }

                return BadRequest("Invalid OTP or OTP expired.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{id}/generate-otp")]
        public async Task<IActionResult> GenerateOTP(int id)
        {
            try
            {
                await _userManager.GenerateOTPAsync(id);
                return Ok(new { message = "OTP generated and sent to the user's email." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{userId}/hospitals/{hospitalId}")]
        public async Task<IActionResult> AddUserToHospital(int userId, int hospitalId)
        {
            try
            {
                await _userManager.AddUserToHospitalAsync(userId, hospitalId);
                return Ok(new { message = "User added to hospital successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{userId}/hospitals/{hospitalId}")]
        public async Task<IActionResult> RemoveUserFromHospital(int userId, int hospitalId)
        {
            try
            {
                await _userManager.RemoveUserFromHospitalAsync(userId, hospitalId);
                return Ok(new { message = "User removed from hospital successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignupAsync(CreateUserDTO createUserDTO)
        {
            try
            {
                if (createUserDTO == null)
                {
                    return BadRequest("User data is required.");
                }

                var user = _mapper.Map<User>(createUserDTO);
                var createdUser = await _userManager.CreateAsync(user);

                if (createdUser == null)
                {
                    return StatusCode(500, "An error occurred while creating the user.");
                }

                var userDTO = _mapper.Map<UserDTO>(createdUser);
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginUserDTO loginUserDTO)
        {
            try
            {
                if (loginUserDTO == null)
                {
                    return BadRequest("Login data is required.");
                }

                var loginResult = await _userManager.LoginAsync(loginUserDTO.UserName, loginUserDTO.Password);

                if (loginResult == null)
                {
                    return Unauthorized("Invalid username or password, or user not verified.");
                }

                return Ok(loginResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}