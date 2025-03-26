using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.Constants;

namespace backend.Core.DTOs.JobApplication
{
    public class JobApplicationsDto
    {
        public string JobStatus { get; set; }
        public string CandidateId { get; set; }
        public string CandidateName { get; set; }
    }
}
