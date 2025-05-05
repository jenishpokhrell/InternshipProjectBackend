using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.DTOs.Auth
{
    public class LoginDto
    {
        [Display(Name = "Email or Username")]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
