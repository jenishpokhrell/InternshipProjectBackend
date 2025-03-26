using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Entities
{
    public class Academic
    {
        [Key]
        public int Id { get; set; }
        public string InstitutionName { get; set; }
        public string Stream { get; set; }
        public string StartYear { get; set; }
        public string GraduationYear { get; set; }
        public string DegreeType { get; set; }
        public string CurrentSemester { get; set; }
        public string CandidateId { get; set; }
    }
}
