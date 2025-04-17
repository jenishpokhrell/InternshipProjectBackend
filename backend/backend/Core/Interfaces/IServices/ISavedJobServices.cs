using backend.Core.DTOs.General;
using backend.Core.DTOs.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Core.Interfaces.IServices
{
    public interface ISavedJobServices
    {
        Task<GeneralServiceResponseDto> SaveJobAsync(ClaimsPrincipal User, int jobId);

        Task<IEnumerable<GetJobDtoForCandidate>> GetMySavedJobsAsync(ClaimsPrincipal User);

        Task<GeneralServiceResponseDto> UnsaveJobsAsync(ClaimsPrincipal User, int jobId);

        Task<GetJobDtoForCandidate> GetSavedJobByIdAsync(ClaimsPrincipal User, int jobId);
    }
}
