using backend.Core.DTOs.Job;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Entities
{
    public class SavedJob
    {
        [Key]
        public int SavedJobId { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }
        public string CandidateId { get; set; }
    }
}
