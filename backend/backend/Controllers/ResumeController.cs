using backend.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.DTOs.Resume;
using backend.Core.Interfaces;
using backend.Core.Services;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeServices _resumeServices;

        public ResumeController(IResumeServices resumeServices)
        {
            _resumeServices = resumeServices;
        }

        [HttpPost]
        [Route("Add-Resume")]
        [Authorize(Roles = StaticUserRole.CANDIDATE)]
        public async Task<IActionResult> AddResume([FromForm] ResumeDto resumeDto,
            [FromServices] CloudinaryServices cloudinaryServices)
        {
            var addResume = await _resumeServices.AddorUpdateResumeAsync(User, resumeDto, cloudinaryServices);
            return Ok(addResume);
        }

        [HttpGet]
        [Route("GetResumeByCandidateId/{candidateId}")]
        [Authorize(Roles = StaticUserRole.ADMIN_EMPLOYER)]
        public async Task<IActionResult> GetResumeByCandidateId(string candidateId)
        {
            var resume = await _resumeServices.GetResumeByCandidateIdAsync(candidateId);
            return Ok(resume);
        }
    }
}
