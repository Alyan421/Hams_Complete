﻿using System.ComponentModel.DataAnnotations;

namespace HMS_Final.Controllers.Users.DTO
{
    public class CreateUserDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
