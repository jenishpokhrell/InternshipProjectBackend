using backend.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.DTOs.Projects;
using backend.Core.Interfaces;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectServices _projectServices;

        public ProjectsController(IProjectServices projectServices)
        {
            _projectServices = projectServices;
        }

        [HttpPost]
        [Route("Add-Project")]
        [Authorize(Roles = StaticUserRole.CANDIDATE)]
        public async Task<IActionResult> AddProjectAsync([FromBody] AddProjectDto addProjectDto)
        {
            var project = await _projectServices.AddProjectAsync(User, addProjectDto);
            if (!project.IsSuccess)
            {
                return Unauthorized("Only candidate can add projects.");
            }
            return Ok(project);
        }

        [HttpPut]
        [Route("Update-Project/{id}")]
        [Authorize(Roles = StaticUserRole.CANDIDATE)]
        public async Task<IActionResult> UpdateProjectAsync([FromBody] AddProjectDto addProjectDto, int id)
        {
            var updatedProject = await _projectServices.UpdateProjectAsync(User, addProjectDto, id);
            return Ok(updatedProject);
        }

        [HttpGet]
        [Route("GetMyProjects")]
        [Authorize]
        public async Task<IActionResult> GetMyProjectsAsync()
        {
            var projects = await _projectServices.GetMyProjectsAsync(User);
            return Ok(projects);
        }

        [HttpGet]
        [Route("GetProjectById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _projectServices.GetProjectByIdAsync(User, id);
            return Ok(project);
        }

        [HttpGet]
        [Route("GetProjectByCandidateId/{candidateId}")]
        [Authorize]
        public async Task<IActionResult> GetProjecyByCandidateId(string candidateId)
        {
            var project = await _projectServices.GetProjectsByCandidateIdAsync(candidateId);
            return Ok(project);
        }

        [HttpDelete]
        [Route("DeleteProject/{id}")]
        [Authorize(Roles = StaticUserRole.CANDIDATE)]
        public async Task<IActionResult> DeleteProjectAsync(int id)
        {
            var deleteProject = await _projectServices.DeleteProjectAsync(User, id);
            if (deleteProject.IsSuccess)
            {
                return Ok(deleteProject);
            }
            else
            {
                return StatusCode(deleteProject.StatusCode, deleteProject.Message);
            }
        }
    }
}
