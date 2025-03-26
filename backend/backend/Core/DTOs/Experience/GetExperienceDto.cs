using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.DTOs.Experience
{
    public class GetExperienceDto
    {
        public int ExperienceId { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string JobDescription { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string UserId { get; set; }
    }
}
