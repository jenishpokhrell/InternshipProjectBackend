using AutoMapper;
using AutoMapper.QueryableExtensions;
using backend.Core.DataContext;
using backend.Core.DTOs.General;
using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Core.Constants;
using backend.Core.DTOs.JobApplication;
using backend.Core.Interfaces;
using backend.Core.Interfaces.IRepositories;
using backend.Core.DTOs.Job;

namespace backend.Core.Services
{
    public class JobApplicationServices : IJobApplicationServices
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly IJobApplicationRepositories _jobApplicationRespositories;

        public JobApplicationServices(ApplicationDBContext context, IMapper mapper, IJobApplicationRepositories jobApplicationRespositories)
        {
            _context = context;
            _mapper = mapper;
            _jobApplicationRespositories = jobApplicationRespositories;
        }

        public Task<GetJobDtoForCandidate> GetJobDetails()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MyJobApplicationsDto>> GetMyJobApplicationAsync(ClaimsPrincipal User)
        {
            var jobApplications = await _jobApplicationRespositories.GetMyJobApplications(User);
            if(jobApplications is null)
            {
                throw new Exception("You haven't applied for any jobs yet.");
            }
            return _mapper.Map<IEnumerable<MyJobApplicationsDto>>(jobApplications);
        }

        public async Task<GeneralServiceResponseDto> UpdateJobApplicationAsync(ClaimsPrincipal User, 
            UpdateJobApplicationStatusDto updateJobApplicationStatusDto, int id)
        {
            var jobApplication = await _context.JobApplications.FindAsync(id);
            if(jobApplication is null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Job Application doesn't exist."
                };
            }

            var job = await _context.Jobs.FindAsync(jobApplication.JobId);

            if (jobApplication.JobId != job.Id)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "The JobId doesn't match with this job application."
                };
            }

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(job.EmployerId != loggedInUserId)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "You are not authorized to modify this job status."
                };
            }

            if(updateJobApplicationStatusDto.JobStatus != "Shortlisted" && updateJobApplicationStatusDto.JobStatus != "Rejected")
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Invalid Job Status. Should be either 'Shortlisted' or 'Rejected'"
                };
            }

            jobApplication.JobStatus = updateJobApplicationStatusDto.JobStatus;
            jobApplication.UpdatedAt = updateJobApplicationStatusDto.UpdatedAt = DateTime.Now;
            
            if(updateJobApplicationStatusDto.JobStatus == "Shortlisted")
            {
                jobApplication.Message = JobApplicationStatusMessage.Shortlisted;
            }
            else
            {
                jobApplication.Message = JobApplicationStatusMessage.Rejected;
            }

            if(updateJobApplicationStatusDto.JobStatus == "Shortlisted")
            {
                var savedCandidate = new SavedCandidate()
                {
                    EmployerId = loggedInUserId,
                    CandidateId = jobApplication.CandidateId,
                    CandidateName = jobApplication.CandidateName,
                    JobId = job.Id,
                    JobTitle = job.JobTitle
                };
                await _context.SavedCandidates.AddAsync(savedCandidate);
            }

            _context.Entry(jobApplication).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Job Status Modified Successfully"
            };
        }
    }
}
