using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.DTOs.SavedCandidates
{
    public class SavedCandidateDto
    {
        public string CandidateId { get; set; }
        public string CandidateName { get; set; }
        public string EmployerId { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; }
    }
}
