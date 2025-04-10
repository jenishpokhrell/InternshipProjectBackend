using backend.Core.DTOs.JobApplication;
using backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Core.Interfaces.IRepositories
{
    public interface IJobRepositories
    {
        Task PostJob(Job job);

        Task<Job> GetJobById(int id);

        Task<IEnumerable<Job>> GetAllJobsAsync();

        Task<IEnumerable<JobApplication>> GetJobApplicationsByJobIdAsync(int jobId);

        Task ApplyForJobAsync(ClaimsPrincipal User, int id);

        Task UpdateJob(Job job);

        Task DeleteJobAsync(int id);
    }
}
