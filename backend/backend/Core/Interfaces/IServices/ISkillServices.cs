using backend.Core.DTOs.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.DTOs.Skill;

namespace backend.Core.Interfaces
{
    public interface ISkillServices
    {
        Task<GeneralServiceResponseDto> AddSkillsAsync(SkillDto skillDto);

        Task<IEnumerable<GetSkillDto>> GetAllSkillsAsync();

        Task<GetSkillDto> GetSkillByIdAsync(int id);

        Task<GeneralServiceResponseDto> DeleteSkillAsync(int id);
    }
}
