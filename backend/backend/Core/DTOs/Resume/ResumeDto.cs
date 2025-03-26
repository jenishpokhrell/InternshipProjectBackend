using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.DTOs.Resume
{
    public class ResumeDto
    {
        //public string CandidateId { get; set; }
        public IFormFile CandidateResume { get; set; }
    }
}
