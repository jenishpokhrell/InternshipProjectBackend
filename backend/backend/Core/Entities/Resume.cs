using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Entities
{
    public class Resume
    {
        [Key]
        public int ResumeId { get; set; }
        public string CandidateId { get; set; }
        public string CandidateResume { get; set; }
        
    }
}
