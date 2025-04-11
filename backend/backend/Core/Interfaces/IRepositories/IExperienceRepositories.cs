using backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Core.Interfaces.IRepositories
{
    public interface IExperienceRepositories
    {
        Task AddExperience(Experience experience);

        Task<IEnumerable<Experience>> GetAllExperiences();

        Task<Experience> GetExperienceById(int id);

        Task<IEnumerable<Experience>> GetExperienceByCandidateId(string candidateId);

        Task<IEnumerable<Experience>> GetMyExperiencesAsync(ClaimsPrincipal User);

        Task UpdateExperienceAsync(Experience experience);

        Task DeleteExperienceAsync(int id);
    }
}
