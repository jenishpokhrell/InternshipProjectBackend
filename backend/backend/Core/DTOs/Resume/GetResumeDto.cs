using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.DTOs.Resume
{
    public class GetResumeDto
    {
        public string CandidateId { get; set; }
        public string CandidateResume { get; set; }
    }
}
