using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.DTOs.Projects
{
    public class GetMyProjectDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectURL { get; set; }
    }
}
