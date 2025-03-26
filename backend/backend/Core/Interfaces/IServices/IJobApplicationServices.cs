using backend.Core.DTOs.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Core.DTOs.JobApplication;

namespace backend.Core.Interfaces
{
    public interface IJobApplicationServices
    {
        Task<IEnumerable<MyJobApplicationsDto>> GetMyJobApplicationAsync(ClaimsPrincipal User);

        Task<GeneralServiceResponseDto> UpdateJobApplicationAsync(ClaimsPrincipal User,
            UpdateJobApplicationStatusDto updateJobApplicationStatusDto, int id, int jobId);
    }
}
