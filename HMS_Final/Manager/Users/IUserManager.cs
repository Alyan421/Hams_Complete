using HMS_Final.Models;

namespace HMS_Final.Manager.Users
{
    public interface IUserManager
    {
        Task<User> CreateAsync(User entity);
        Task<User> UpdateAsync(User entity);
        Task<User> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task DeleteAsync(int id);
        Task<bool> VerifyOTPAsync(int userId, long otp);
        Task GenerateOTPAsync(int userId);
        Task AddUserToHospitalAsync(int userId, int hospitalId);
        Task RemoveUserFromHospitalAsync(int userId, int hospitalId);
        Task<object> LoginAsync(string userName, string password);
    }
}
