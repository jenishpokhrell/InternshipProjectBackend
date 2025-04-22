using backend.Core.DTOs.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Core.DTOs.Skill;

namespace backend.Core.Interfaces
{
    public interface ICandidateSkillServices
    {
        Task<IEnumerable<GetSkillDto>> GetAvailableSkillsAsync();

        Task<IEnumerable<GetCandidateSkillsDto>> GetCandidateSkillsAsync(string candidateId);

        Task<IEnumerable<GetSkillDto>> GetMySkillsAsync(ClaimsPrincipal User);

        Task<GeneralServiceResponseDto> AddCandidateSkillAsync(ClaimsPrincipal User, AddCandidateSkillDto addCandidateSkillDto);

        Task<GeneralServiceResponseDto> DeleteSkillAsync(ClaimsPrincipal User, int skillId);
    }
}
