using backend.Core.DTOs.General;
using backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Core.Interfaces.IRepositories
{
    public interface IAcademicRepositories
    {
        Task AddAcademics(Academic academics);

        Task<IEnumerable<Academic>> GetAcademics();

        Task<Academic> GetAcademicsByCandidateId(string candidateId);

        Task<Academic> GetAcademicById(int id);

        Task<Academic> GetMyAcademic(ClaimsPrincipal User);

        Task UpdateAcademics(Academic academic);

        Task DeleteAcademic(int id);
    }
}
