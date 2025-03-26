using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Entities
{
    public class CandidateSkill
    {
        [Key]
        public int CandidateSkillId { get; set; }
        public string CandidateId { get; set; }
        public int SkillId { get; set; }
        public string Skill { get; set; }
        public Skills Skills { get; set; }
    }
}
