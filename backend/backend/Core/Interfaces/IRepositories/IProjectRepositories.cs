using backend.Core.DTOs.Projects;
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
        Task<Projects> AddProject(ClaimsPrincipal User, ProjectDto addProjectDto);

        Task<Projects> GetProjectById(int id);

        Task<IEnumerable<Projects>> GetProjectByCandidateId(string candidateId);

        Task<IEnumerable<Projects>> GetAllProjects();

        Task UpdateProjects(ProjectDto updateProjectDto, int id);

        Task DeleteProject(int id);
    }
}
