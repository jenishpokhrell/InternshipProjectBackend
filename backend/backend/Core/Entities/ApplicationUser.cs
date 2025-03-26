using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string ProfilePhoto { get; set; }
        public string Gender { get; set; }
        public string JobTitle { get; set; }
        public string Years_Of_Experience { get; set; }

        [NotMapped]
        public IEnumerable<string> Roles { get; set; }
    }
}
