using AutoMapper;
using AutoMapper.QueryableExtensions;
using backend.Core.DataContext;
using backend.Core.DTOs.General;
using backend.Core.DTOs.Job;
using backend.Core.Entities;
using backend.Core.Interfaces.IRepositories;
using backend.Core.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Core.Services
{
    public class SavedJobServices : ISavedJobServices
    {
        private readonly ApplicationDBContext _context; 
        
        private readonly IMapper _mapper;

        public SavedJobServices(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GeneralServiceResponseDto> SaveJobAsync(ClaimsPrincipal User, int jobId)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var job = await _context.Jobs.FindAsync(jobId);

            if (job is null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Job doesn't exist."
                };
            }
         

            var savedJobsExists = await _context.SavedJobs.Where(sj => sj.JobId == jobId && sj.CandidateId == loggedInUserId).FirstOrDefaultAsync();

            if(savedJobsExists is not null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 201,
                    Message = "Job saved already."
                };
            }

            var saveJob = new SavedJob
            {
                JobId = job.Id,
                CandidateId = loggedInUserId
            };

            await _context.SavedJobs.AddAsync(saveJob);
            await _context.SaveChangesAsync();

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Job saved successfully."
            };
        }

        public async Task<IEnumerable<GetJobDtoForCandidate>> GetMySavedJobsAsync(ClaimsPrincipal User)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var savedJobs = await _context.SavedJobs.Where(sj => sj.CandidateId == loggedInUserId)
                .ProjectTo<GetJobDtoForCandidate>(_mapper.ConfigurationProvider).ToListAsync();

            return savedJobs;
        }

        public async Task<GetJobDtoForCandidate> GetSavedJobByIdAsync(ClaimsPrincipal User, int jobId)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var savedJob = await _context.SavedJobs.Where(sj => sj.CandidateId == loggedInUserId && sj.JobId == jobId)
                .ProjectTo<GetJobDtoForCandidate>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            return savedJob;
        }

        public async Task<GeneralServiceResponseDto> UnsaveJobsAsync(ClaimsPrincipal User, int jobId)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var job = await _context.SavedJobs.Where(sj => sj.JobId == jobId && sj.CandidateId == loggedInUserId).FirstOrDefaultAsync();

            if(job is null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Job isn't saved yet."
                };
            }

            _context.SavedJobs.Remove(job);
            await _context.SaveChangesAsync();

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Job unsaved successsfully"
            };

        }
    }
}
