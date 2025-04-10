using backend.Core.DTOs.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Core.DTOs.Experience;

namespace backend.Core.Interfaces
{
    public interface IExperienceServices
    {
        Task<GeneralServiceResponseDto> AddExperienceAsync(ClaimsPrincipal User, AddExperienceDto addExperienceDto);

        Task<IEnumerable<GetExperienceDto>> GetAllExperiencesAsync();

        Task<GetExperienceDto> GetExperienceByIdAsync(ClaimsPrincipal User, int id);

        Task<IEnumerable<GetExperienceDto>> GetMyExperiencesAsync(ClaimsPrincipal User);

        Task<GeneralServiceResponseDto> UpdateExperienceAsync(ClaimsPrincipal User, AddExperienceDto addExperienceDto, int id);

        Task<GeneralServiceResponseDto> DeleteExperienceAsync(ClaimsPrincipal User, int id);
    }
}
