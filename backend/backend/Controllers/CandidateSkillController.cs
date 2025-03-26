using backend.Core.Constants;
using backend.Core.DTOs.Skill;
using backend.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateSkillController : ControllerBase
    {
        private readonly ICandidateSkillServices _candidateSkillServices;

        public CandidateSkillController(ICandidateSkillServices candidateSkillServices)
        {
            _candidateSkillServices = candidateSkillServices;
        }

        [HttpPost]
        [Route("Add-Skills")]
        [Authorize(Roles = StaticUserRole.CANDIDATE)]
        public async Task<IActionResult> AddCandidateSKill([FromBody] AddCandidateSkillDto addCandidateSkillDto)
        {
            var addSKills = await _candidateSkillServices.AddCandidateSkillAsync(User, addCandidateSkillDto);
            return Ok(addSKills);
        }

        [HttpGet]
        [Route("GetCandidateSkills")]
        [Authorize]
        public async Task<IActionResult> GetCandidateSKills(string candidateId)
        {
            var candidateSkills = await _candidateSkillServices.GetCandidateSkillsAsync(candidateId);
            return Ok(candidateSkills);
        }

        [HttpGet]
        [Route("GetAvaiblableSkills")]
        [Authorize]
        public async Task<IActionResult> GetAvailableSkills()
        {
            var skills = await _candidateSkillServices.GetAvailableSkillsAsync();
            return Ok(skills);
        }
    }
}
