using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Entities
{
    public class Skills
    {
        [Key]
        public int SkillId { get; set; }
        public string Skill { get; set; }
    }
}
