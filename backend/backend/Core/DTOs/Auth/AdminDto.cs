using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.DTOs.Auth
{
    public class AdminDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(7)]
        public string Password { get; set; }
    }
}
