using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Entities
{
    public class Experience
    {
        [Key]
        public int ExperienceId { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string JobDescription { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        public bool IsCurrentlyWoring { get; set; }
        public string UserId { get; set; }

    }
}
