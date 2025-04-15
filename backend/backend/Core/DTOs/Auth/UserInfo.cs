using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.DTOs.Auth
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string ProfilePhoto { get; set; }
        public string Gender { get; set; }
        public string JobTitle { get; set; }
        public int Years_Of_Experience { get; set; }
        public bool isApproved { get; set; }
        public List<string> Roles { get; set; }
    }
}
