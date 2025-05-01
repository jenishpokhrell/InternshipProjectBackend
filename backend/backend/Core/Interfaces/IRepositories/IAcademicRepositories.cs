using backend.Core.DTOs.Academics;
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
        Task<Academic> AddAcademics(ClaimsPrincipal User, AcademicsDto addAcademicsDto);

        Task<IEnumerable<Academic>> GetAcademics();

        Task<Academic> GetAcademicsByCandidateId(string candidateId);

        Task<Academic> GetAcademicById(int id);

        Task<Academic> GetMyAcademic(ClaimsPrincipal User);

        Task UpdateAcademics(AcademicsDto updateAcademicsDto, int id);

        Task DeleteAcademic(int id);
    }
}
