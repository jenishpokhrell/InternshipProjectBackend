using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.DTOs.Skill;
using backend.Core.Interfaces;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillServices _skillServices;

        public SkillController(ISkillServices skillServices)
        {
            _skillServices = skillServices;
        }

        [HttpPost]
        [Route("AddSkill")]
        [Authorize]
        public async Task<IActionResult> AddSkill([FromBody] SkillDto skillDto)
        {
            var skill = await _skillServices.AddSkillsAsync(skillDto);
            return Ok(skill);
        }

        [HttpGet]
        [Route("GetAllSkills")]
        [Authorize]
        public async Task<IActionResult> GetAllSkills()
        {
            var skills = await _skillServices.GetAllSkillsAsync();
            return Ok(skills);
        }

        [HttpGet]
        [Route("GetSkillById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetSkillById(int id)
        {
            var skill = await _skillServices.GetSkillByIdAsync(id);
            return Ok(skill);
        }

        [HttpDelete]
        [Route("DeleteSkill/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSkillAsync(int id)
        {
            var deleteSkill = await _skillServices.DeleteSkillAsync(id);
            return Ok(deleteSkill);
        }
    }
}
