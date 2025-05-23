﻿using backend.Core.DTOs.Job;
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
        Task<Job> PostJob(ClaimsPrincipal User, JobDto postJobDto);

        Task<Job> GetJobById(int id);

        Task<IEnumerable<Job>> GetAllJobsAsync();

        Task<IEnumerable<JobApplication>> GetJobApplicationsByJobIdAsync(int jobId);

        Task UpdateJob(JobDto updateJobDto, int id);

        Task DeleteJobAsync(int id);
    }
}
