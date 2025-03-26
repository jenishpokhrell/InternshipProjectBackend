using backend.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.Interfaces;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavedCandidateController : ControllerBase
    {
        private readonly ISavedCandidateServices _savedCandidateServices;

        public SavedCandidateController(ISavedCandidateServices savedCandidateServices)
        {
            _savedCandidateServices = savedCandidateServices;
        }

        [HttpGet]
        [Route("GetSavedCandidates")]
        [Authorize(Roles = StaticUserRole.EMPLOYER)]
        public async Task<IActionResult> GetSavedCandidate()
        {
            var savedCandidates = await _savedCandidateServices.GetSavedCandidateAsync(User);
            return Ok(savedCandidates);
        }
    }
}
