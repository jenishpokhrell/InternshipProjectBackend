using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Entities
{
    public class Projects
    {
        [Key]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectURL { get; set; }
        public string CandidateId { get; set; }
    }
}
