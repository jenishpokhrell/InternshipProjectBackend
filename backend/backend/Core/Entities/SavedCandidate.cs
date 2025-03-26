using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Entities
{
    public class SavedCandidate
    {
        [Key]
        public int SavedCandidateId { get; set; }
        public string CandidateId { get; set; }
        public string CandidateName { get; set; }
        public string EmployerId { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public Job Job { get; set; }
    }
}
