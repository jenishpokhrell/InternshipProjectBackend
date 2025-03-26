using backend.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.DTOs.Experience;
using backend.Core.Interfaces;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperienceController : ControllerBase
    {
        private readonly IExperienceServices _experiencesServices;

        public ExperienceController(IExperienceServices experienceServices)
        {
            _experiencesServices = experienceServices;
        }

        [HttpPost]
        [Route("Add-Experience")]
        [Authorize(Roles = StaticUserRole.CANDIDATE_EMPLOYER)]
        public async Task<IActionResult> AddExperience([FromBody] AddExperienceDto addExperienceDto)
        {
            var addExperience = await _experiencesServices.AddExperienceAsync(User, addExperienceDto);
            return Ok(addExperience);
        }

        [HttpGet]
        [Route("GetExperienceById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetExperienceById(int id)
        {
            var experience = await _experiencesServices.GetExperienceByIdAsync(User, id);
            return Ok(experience);
        }

        [HttpGet]
        [Route("GetMyExperiences")]
        [Authorize]
        public async Task<IActionResult> GetMyExperiences()
        {
            var experiences = await _experiencesServices.GetMyExperiencesAsync(User);
            return Ok(experiences);
        }

        [HttpPut]
        [Route("Update-Experience/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateExperience([FromBody] AddExperienceDto addExperienceDto, int id)
        {
            var updateExperience = await _experiencesServices.UpdateExperienceAsync(User, addExperienceDto, id);
            return Ok(updateExperience);
        }

        [HttpDelete]
        [Route("Delete-Experience/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteExperience(int id)
        {
            var deleteExperience = await _experiencesServices.DeleteExperienceAsync(User, id);
            if (!deleteExperience.IsSuccess)
            {
                return StatusCode(deleteExperience.StatusCode, deleteExperience.Message);
            }
            return Ok(deleteExperience);
        }
    }
}
