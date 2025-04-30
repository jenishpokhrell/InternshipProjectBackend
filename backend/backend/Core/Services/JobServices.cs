using AutoMapper;
using AutoMapper.QueryableExtensions;
using backend.Core.Constants;
using backend.Core.DataContext;
using backend.Core.DTOs.General;
using backend.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Core.DTOs.Job;
using backend.Core.Interfaces;
using backend.Core.Interfaces.IRepositories;

namespace backend.Core.Services
{
    public class JobServices : IJobServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJobRepositories _jobrepositories;
        private readonly ApplicationDBContext _context;
        private readonly ILogServices _logServices;
        private readonly IMapper _mapper;
        public JobServices(ApplicationDBContext context, ILogServices logServices, UserManager<ApplicationUser> userManager,
            IMapper mapper, IJobRepositories jobrepositories)
        {
            _context = context;
            _jobrepositories = jobrepositories;
            _logServices = logServices;
            _userManager = userManager;
            _mapper = mapper;
        }

        //Method Posting job
        public async Task<GeneralServiceResponseDto> PostJobAsync(ClaimsPrincipal User, PostJobDto postJobDto)
        {
            await _jobrepositories.PostJob(User, postJobDto);
            await _logServices.SaveNewLog(User.Identity.Name, "Posted a new job");

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Job Posted Successfully"
            };
        }

        //Method for applying for job
        public async Task<GeneralServiceResponseDto> ApplyForJobAsync(ClaimsPrincipal User, int id)
        {
            var job = await _jobrepositories.GetJobById(id);
            if(job is null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "The job you're trying to apply for doesnt exist."
                };
            }

            var loggedInUserRole = User.FindFirstValue(ClaimTypes.Role);
            if(loggedInUserRole != StaticUserRole.CANDIDATE)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 403,
                    Message = "Only candidate can apply for the job."
                };
            }

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingApplication = await _context.JobApplications.Where(j => j.CandidateId == loggedInUserId && j.JobId == id).FirstOrDefaultAsync();
            if(existingApplication is not null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "You've already applied for this job."
                };
            }

            if(job.IsActive != true)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "This job has already expired."
                };
            }

            var jobApplication = new JobApplication()
            {
                JobStatus = "Pending",
                JobId = id,
                CandidateId = loggedInUserId,
                CandidateName = User.Identity.Name,
                Message = JobApplicationStatusMessage.Pending
            };

            await _context.JobApplications.AddAsync(jobApplication);
            await _context.SaveChangesAsync();
            await _logServices.SaveNewLog(User.Identity.Name, "Applied for the job");

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Job Applied Successfully. Good Luck!!"
            };
        }

        //Getting all jobs for candidate
        public async Task<IEnumerable<GetJobDtoForCandidate>> GetAllJobsForCandidateAsync()
        {
            var jobs = await _jobrepositories.GetAllJobsAsync();
            return _mapper.Map<IEnumerable<GetJobDtoForCandidate>>(jobs);
        }

        //Getting all jobs for admin
        public async Task<IEnumerable<GetJobDto>> GetAllJobsAsync()
        {
            var jobs = await _jobrepositories.GetAllJobsAsync();
            return _mapper.Map<IEnumerable<GetJobDto>>(jobs);
        }

        // Getting job by Id candidate to view
        public async Task<GetJobDtoForCandidate> GetJobByIdForCandidatesAsync(int id)
        {
            var job = await _jobrepositories.GetJobById(id);
            if (job is null)
            {
                throw new NullReferenceException("The job doesn't exist.");
            }

            return _mapper.Map<GetJobDtoForCandidate>(job);
        }

        //Getting all the jobs posted by employers to view their jobs
        public async Task<IEnumerable<GetMyJobDto>> GetMyJobsAsync(ClaimsPrincipal User)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobs = await _jobrepositories.GetAllJobsAsync();
            var myjobs = jobs.Where(j => j.EmployerId == loggedInUserId).ToList();
            if (myjobs is null)
            {
                throw new Exception("You haven't posted any jobs yet.");
            }

            foreach (var job in myjobs)
            {
                var applications = await _jobrepositories.GetJobApplicationsByJobIdAsync(job.Id);
                job.JobApplications = applications.ToList();
            }
            return _mapper.Map<IEnumerable<GetMyJobDto>>(myjobs);
        }

        //getting a single job for employer to view 
        public async Task<GetMyJobDto> GetMyJobByIdAsync(ClaimsPrincipal User, int id)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobs = await _jobrepositories.GetAllJobsAsync();
            var job = jobs.Where(j => j.Id == id).FirstOrDefault();
            if (job is null)
                throw new KeyNotFoundException("Job doesn't exist.");

            if (job.EmployerId != loggedInUserId)
                throw new UnauthorizedAccessException("You are not authorized to access this job posting.");

            var applications = await _jobrepositories.GetJobApplicationsByJobIdAsync(job.Id);
            job.JobApplications = applications.ToList();


            return _mapper.Map<GetMyJobDto>(job);
        }

        //Getting job by Id
        public async Task<GetJobDto> GetJobByIdAsync(int id)
        {
            var jobs = await _jobrepositories.GetJobById(id);

            return _mapper.Map<GetJobDto>(jobs);
        }

        //Updating the jobs
        public async Task<GeneralServiceResponseDto> UpdateJobAsync(ClaimsPrincipal User, PostJobDto postJobDto, int id)
        {
            var job = await _jobrepositories.GetJobById(id);
            if(job is null)
            {
                throw new KeyNotFoundException("Job doesn't exist");
            }

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(job.EmployerId != loggedInUserId)
            {
                throw new UnauthorizedAccessException("You can not authorized to modify this job.");
            }

            await _jobrepositories.UpdateJob(postJobDto, id);
            await _logServices.SaveNewLog(User.Identity.Name, "Updated their job posting.");

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Job modified successfully."
            };
        }

        //Deleting Jobs
        public async Task<GeneralServiceResponseDto> DeleteJobAsync(ClaimsPrincipal User, int id)
        {
            var job = await _jobrepositories.GetJobById(id);
            if(job is null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "The job you're trying to remove doesnt exist."
                };
            }

            var loggedInUser = User.FindFirstValue(ClaimTypes.Role);

            if(loggedInUser == StaticUserRole.CANDIDATE)
            {
                throw new UnauthorizedAccessException("Candidates are not authorized to delete the jobs.");
            }

            else if(loggedInUser == StaticUserRole.EMPLOYER)
            {
                throw new UnauthorizedAccessException("Employers can't delete job, they can only disable it.");
            }

            else if(job.IsActive == true)
            {
                throw new Exception("Active jobs cant be deleted. Employer need to disable the job.");
            }


            else
            {
                var jobApplications = await _jobrepositories.GetJobApplicationsByJobIdAsync(id);            
                await _jobrepositories.DeleteJobAsync(id);
                _context.RemoveRange(jobApplications);
            }


            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Job Deleted Successfully."
            };
        }

        
    }
}
