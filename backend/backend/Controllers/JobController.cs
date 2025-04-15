using backend.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.DTOs.Job;
using backend.Core.Interfaces;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobServices _jobServices;
        public JobController(IJobServices jobServices)
        {
            _jobServices = jobServices;
        }

        [HttpPost]
        [Route("Post-Job")]
        [Authorize(Roles = StaticUserRole.EMPLOYER)]
        public async Task<IActionResult> PostJobAsync([FromBody] PostJobDto postJobDto)
        {
            var job = await _jobServices.PostJobAsync(User, postJobDto);
            if (job.IsSuccess)
            {
                return Ok(job);
            }
            return StatusCode(job.StatusCode, job.Message);
        }

        [HttpPost]
        [Route("ApplyForJob/{id}")]
        [Authorize]
        public async Task<IActionResult> ApplyForJob(int id)
        {
            var jobApply = await _jobServices.ApplyForJobAsync(User, id);
            return Ok(jobApply);
        }

        [HttpGet]
        [Route("GetAllJobsForCandidate")]
        [Authorize(Roles = StaticUserRole.CANDIDATE)]
        public async Task<IActionResult> GetAllJobsForCandidateAsync()
        {
            var jobs = await _jobServices.GetAllJobsForCandidateAsync();
            if(jobs is null)
            {
                return BadRequest();
            }
            return Ok(jobs);
        }

        [HttpGet]
        [Route("GetAllJobs")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<IActionResult> GetAllJobsAsync()
        {
            var jobs = await _jobServices.GetAllJobsAsync();
            if (jobs is null)
            {
                return BadRequest();
            }
            return Ok(jobs);
        }

        [HttpPut]
        [Route("Update-Job/{id}")]
        [Authorize(Roles = StaticUserRole.EMPLOYER)]
        public async Task<IActionResult> UpdateJob([FromBody] PostJobDto postJobDto, int id)
        {
            var updateJob = await _jobServices.UpdateJobAsync(User, postJobDto, id);
            return Ok(updateJob);
        }
        
        [HttpDelete]
        [Route("Delete-Job/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var deleteJob = await _jobServices.DeleteJobAsync(User, id);
            return Ok(deleteJob);
        }

        [HttpGet]
        [Route("GetJobByIdForCandidate/{id}")]
        [Authorize(Roles = StaticUserRole.CANDIDATE)]
        public async Task<IActionResult> GetJobByIdForCandidate(int id)
        {
            var job = await _jobServices.GetJobByIdForCandidatesAsync(id);
            return Ok(job);
        }

        [HttpGet]
        [Route("GetMyJobs")]
        [Authorize(Roles = StaticUserRole.EMPLOYER)]
        public async Task<IActionResult> GetMyJobs()
        {
            var jobs = await _jobServices.GetMyJobsAsync(User);
            return Ok(jobs);
        }


        [HttpGet]
        [Route("GetMyJobById/{id}")]
        [Authorize(Roles = StaticUserRole.EMPLOYER)]
        public async Task<IActionResult> GetMyJobById(int id)
        {
            var job = await _jobServices.GetMyJobByIdAsync(User, id);
            return Ok(job);
        }
    }
}
