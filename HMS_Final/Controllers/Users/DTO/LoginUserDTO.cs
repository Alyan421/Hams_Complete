using System.ComponentModel.DataAnnotations;

namespace HMS_Final.Controllers.Users.DTO
{
    public class LoginUserDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role {  get; set; }
        
    }
}
