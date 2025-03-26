using backend.Core.Constants;
using backend.Core.Interfaces.IServices;
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
    public class SavedJobController : ControllerBase
    {
        private readonly ISavedJobServices _savedJobServices;

        public SavedJobController(ISavedJobServices savedJobServices)
        {
            _savedJobServices = savedJobServices;
        }

        [HttpPost]
        [Route("SaveJobs/{jobId}")]
        [Authorize(Roles = StaticUserRole.CANDIDATE)]
        public async Task<IActionResult> SaveJobAsync(int jobId)
        {
            var saveJob = await _savedJobServices.SaveJobAsync(User, jobId);
            return Ok(saveJob);
        }

        [HttpGet]
        [Route("GetMySavedJobs")]
        [Authorize(Roles = StaticUserRole.CANDIDATE)]
        public async Task<IActionResult> GetMySavedJobs()
        {
            var savedJobs = await _savedJobServices.GetMySavedJobsAsync(User);
            return Ok(savedJobs);
        }

        [HttpGet]
        [Route("GetSavedJob/{jobId}")]
        [Authorize(Roles = StaticUserRole.CANDIDATE)]
        public async Task<IActionResult> GetSavedJobByIdAsync(int jobId)
        {
            var savedJob = await _savedJobServices.GetSavedJobByIdAsync(User, jobId);
            return Ok(savedJob);
        }
    }
}
