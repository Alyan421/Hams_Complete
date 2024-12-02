using System.ComponentModel.DataAnnotations;

namespace HMS_Final.Controllers.Users.DTO
{
    public class UserGetDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
