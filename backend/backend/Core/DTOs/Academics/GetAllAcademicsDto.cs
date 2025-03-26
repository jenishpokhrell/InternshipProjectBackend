using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.DTOs.Academics
{
    public class GetAllAcademicsDto
    {
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
