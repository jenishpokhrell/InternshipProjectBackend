    using backend.Core.DTOs.JobApplication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Entities
{
    public class Job : BaseEntity<int>
    {
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
        public IList<JobApplication> JobApplications { get; set; }
    }
}
