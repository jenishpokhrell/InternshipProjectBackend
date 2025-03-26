using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.DTOs.Projects
{
    public class GetProjectDto
    {
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectURL { get; set; }
        public string CandidateId { get; set; }
    }
}
