using backend.Core.DTOs.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Core.DTOs.Academics;

namespace backend.Core.Interfaces.IServices
{
    public interface IAcademicServices
    {
        Task<GeneralServiceResponseDto> AddAcademicAsync(ClaimsPrincipal User, AcademicsDto addAcademicDto);

        Task<GetAcademicsDto> GetMyAcademicsAsync(ClaimsPrincipal User);

        Task<GetAcademicsDto> GetAcademicsByCandidateIdAsync(string candidateId);

        Task<IEnumerable<GetAllAcademicsDto>> GetAcademicsAsync();

        Task<GetAcademicsDto> GetAcademicsByIdAsync(int id);

        Task<GeneralServiceResponseDto> UpdateAcademicsAsync(ClaimsPrincipal User, AcademicsDto updateAcademicsDto, int id);

        Task<GeneralServiceResponseDto> DeleteAcademicsAsync(ClaimsPrincipal User, int id);

    }
}
