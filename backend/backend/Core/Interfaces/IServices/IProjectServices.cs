using backend.Core.DTOs.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Core.DTOs.Projects;

namespace backend.Core.Interfaces
{
    public interface IProjectServices
    {
        Task<GeneralServiceResponseDto> AddProjectAsync(ClaimsPrincipal User, ProjectDto addProjectDto);


        Task<IEnumerable<GetMyProjectDto>> GetMyProjectsAsync(ClaimsPrincipal User);

        Task<IEnumerable<GetProjectDto>> GetProjectsByCandidateIdAsync(string candidateId);

        Task<GetProjectDto> GetProjectByIdAsync(ClaimsPrincipal User, int id);
        Task<GeneralServiceResponseDto> UpdateProjectAsync(ClaimsPrincipal User, ProjectDto updateProjectDto, int id);

        Task<GeneralServiceResponseDto> DeleteProjectAsync(ClaimsPrincipal User, int id);
    }
}
