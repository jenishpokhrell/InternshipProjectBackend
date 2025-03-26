using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.DTOs.Auth
{
    public class PasswordDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
