using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.DTOs.Auth
{
    public enum Roles
    {
        Employer,
        Candidate
    }
    public class RegisterDto
    {
        [Required(ErrorMessage = "Firstname is Required.")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Lastname is Required.")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Username is Required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Address is Required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "PhoneNumber is Required.")]
        public string Contact { get; set; }

        [Required(ErrorMessage = "Required Valid Email.")]
        public string Email { get; set; }

        public IFormFile ProfilePhoto { get; set; }

        [Required(ErrorMessage = "Required Gender")]
        public string Gender { get; set; }

        public string JobTitle { get; set; }

        public string Years_Of_Experience { get; set; }

        [Required(ErrorMessage ="Valid Roles Required")]
        [EnumDataType(typeof(Roles))]
        public string roles { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

    }
}
