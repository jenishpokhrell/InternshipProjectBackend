using AutoMapper;
using AutoMapper.QueryableExtensions;
using backend.Core.Constants;
using backend.Core.DataContext;
using backend.Core.DTOs.General;
using backend.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Core.DTOs.Projects;
using backend.Core.Interfaces;
using backend.Core.Interfaces.IRepositories;

namespace backend.Core.Services
{
    public class ProjectServices : IProjectServices
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogServices _logServices;
        private readonly IMapper _mapper;
        private readonly IProjectRepositories _projectRepositories;

        public ProjectServices(ApplicationDBContext context, ILogServices logServices, IMapper mapper, IProjectRepositories projectRepositories)
        {
            _context = context;
            _logServices = logServices;
            _mapper = mapper;
            _projectRepositories = projectRepositories;
        }

        //Method for adding project
        public async Task<GeneralServiceResponseDto> AddProjectAsync(ClaimsPrincipal User, AddProjectDto addProjectDto)
        {
            Projects projects = new Projects()
            {
                ProjectName = addProjectDto.ProjectName,
                ProjectDescription = addProjectDto.ProjectDescription,
                ProjectURL = addProjectDto.ProjectURL,
                CandidateId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };

            await _projectRepositories.AddProject(projects);
            await _logServices.SaveNewLog(User.Identity.Name, "Added a new project");

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Project added successfully"
            };

        }

        //Method for getting individuals projects
        public async Task<IEnumerable<GetMyProjectDto>> GetMyProjectsAsync(ClaimsPrincipal User)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var projects = await _projectRepositories.GetAllProjects();

            var myProjects = projects.Where(p => p.CandidateId == loggedInUserId);

            return _mapper.Map<IEnumerable<GetMyProjectDto>>(myProjects);
        }


        //Method for getting project by Id
        public async Task<GetProjectDto> GetProjectByIdAsync(ClaimsPrincipal User, int id)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUserRole = User.FindFirstValue(ClaimTypes.Role);

            var project = await _projectRepositories.GetProjectById(id);
            if(project is null)
            {
                throw new KeyNotFoundException("Project doesn't exist.");
            }

            if(loggedInUserRole != StaticUserRole.EMPLOYER && loggedInUserId != project.CandidateId)
            {
                throw new UnauthorizedAccessException("You are not authorized to access this project.");
            }

            return _mapper.Map<GetProjectDto>(project);
        }

        //Method for getting candidates projects by their Id
        public async Task<IEnumerable<GetProjectDto>> GetProjectsByCandidateIdAsync(string candidateId)
        {
            var candidateProjects = await _projectRepositories.GetProjectByCandidateId(candidateId);

            if (candidateProjects is null)
            {
                throw new Exception("No work experience found");
            }

            return _mapper.Map<IEnumerable<GetProjectDto>>(candidateProjects);
        }

        //Method for updating project
        public async Task<GeneralServiceResponseDto> UpdateProjectAsync(ClaimsPrincipal User, AddProjectDto addProjectDto, int id)
        {
            var project = await _projectRepositories.GetProjectById(id);
            if(project is null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Project doesn't exist"
                };
            }

            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(project.CandidateId != currentUser)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 403,
                    Message = "You are not authorized to update this project."
                };
            }

            project.ProjectName = addProjectDto.ProjectName;
            project.ProjectDescription = addProjectDto.ProjectDescription;
            project.ProjectURL = addProjectDto.ProjectURL;
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await _logServices.SaveNewLog(User.Identity.Name, "Updated their project");

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Project updated successfully."
            };

        }

        //Method for deleting project
        public async Task<GeneralServiceResponseDto> DeleteProjectAsync(ClaimsPrincipal User, int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if(project is null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "The project you're trying to delete doesn't exist"
                };
            }

            var loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(project.CandidateId != loggedInUser)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 403,
                    Message = "You're not authorized to delete this project."
                };
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Project removed successfully."
            };
        }

        
    }
}
