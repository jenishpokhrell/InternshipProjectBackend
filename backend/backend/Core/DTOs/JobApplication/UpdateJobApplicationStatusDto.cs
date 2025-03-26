using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.DTOs.JobApplication
{
    public class UpdateJobApplicationStatusDto
    {
        public string JobStatus { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
