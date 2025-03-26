using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.DTOs.Job;

namespace backend.Core.DTOs.JobApplication
{
    public class MyJobApplicationsDto
    {
        public string JobStatus { get; set; }
        public string Message { get; set; }
        public GetJobDtoForCandidate Job { get; set; }
    }
}
