using backend.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.DTOs.JobApplication;
using backend.Core.Interfaces;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        private readonly IJobApplicationServices _jobApplicationServices;

        public JobApplicationController(IJobApplicationServices jobApplicationServices)
        {
            _jobApplicationServices = jobApplicationServices;
        }

        [HttpGet]
        [Route("GetMyJobApplications")]
        [Authorize(Roles = StaticUserRole.CANDIDATE)]
        public async Task<IActionResult> GetMyJobApplication()
        {
            var myApplications = await _jobApplicationServices.GetMyJobApplicationAsync(User);
            return Ok(myApplications);
        }

        [HttpPut]
        [Route("UpdateJobApplicationStatus/{jobApplicationId}")]
        [Authorize(Roles = StaticUserRole.EMPLOYER)]
        public async Task<IActionResult> UpdateJobApplicationStatus([FromBody] UpdateJobApplicationStatusDto updateJobApplicationStatusDto, int jobApplicationId
             )
        {
            var updateStatus = await _jobApplicationServices.UpdateJobApplicationAsync(User, updateJobApplicationStatusDto, jobApplicationId);
            return Ok(updateStatus);
        }
    }
}
