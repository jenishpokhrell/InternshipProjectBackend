using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Entities
{
    public class JobApplication : BaseEntity<int>
    {
        public string JobStatus { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }
        public string CandidateId { get; set; }
        public string CandidateName { get; set; }
        public string Message { get; set; }
    }
}
