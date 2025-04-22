using backend.Core.DTOs.General;
using backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Core.DTOs.Job;

namespace backend.Core.Interfaces
{
    public interface IJobServices
    {
        Task<GeneralServiceResponseDto> PostJobAsync(ClaimsPrincipal User, PostJobDto postJobDto);

        Task<IEnumerable<GetJobDtoForCandidate>> GetAllJobsForCandidateAsync();

        Task<IEnumerable<GetJobDto>> GetAllJobsAsync();

        Task<GetJobDto> GetJobByIdAsync(int id);

        Task<GetJobDtoForCandidate> GetJobByIdForCandidatesAsync(int id);

        Task<IEnumerable<GetMyJobDto>> GetMyJobsAsync(ClaimsPrincipal User);

        Task<GetMyJobDto> GetMyJobByIdAsync(ClaimsPrincipal User, int id);

        Task<GeneralServiceResponseDto> ApplyForJobAsync(ClaimsPrincipal User, int id);

        Task<GeneralServiceResponseDto> UpdateJobAsync(ClaimsPrincipal User, PostJobDto postJobDto, int id);

        Task<GeneralServiceResponseDto> DeleteJobAsync(ClaimsPrincipal User, int id);

    }
}
