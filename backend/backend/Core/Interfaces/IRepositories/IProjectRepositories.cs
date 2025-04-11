using backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Core.Interfaces.IRepositories
{
    public interface IProjectRepositories
    {
        Task AddProject(Projects projects);

        Task<Projects> GetProjectById(int id);

        Task<IEnumerable<Projects>> GetProjectByCandidateId(string candidateId);

        Task<IEnumerable<Projects>> GetAllProjects();
    }
}
