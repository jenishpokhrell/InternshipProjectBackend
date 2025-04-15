using backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.DTOs.JobApplication;

namespace backend.Core.DTOs.Job
{
    public class GetJobDto
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string JobType { get; set; }
        public string JobLevel { get; set; }
        public int No_of_Openings { get; set; }
        public string Requirements { get; set; }
        public int Min_Years_of_Experience_Required { get; set; }
        public int Max_Years_of_Experience_Required { get; set; }
        public long MinimumSalary { get; set; }
        public long MaximumSalary { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }
        public string EmployerId { get; set; }
        public string PostedBy { get; set; }
    }
}
