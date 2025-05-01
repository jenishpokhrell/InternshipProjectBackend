using backend.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.DTOs.Academics;
using backend.Core.Interfaces.IServices;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcademicsController : ControllerBase
    {
        private readonly IAcademicServices _academicServices;

        public AcademicsController(IAcademicServices academicServices)
        {
            _academicServices = academicServices;
        }

        [HttpPost]
        [Route("Add-Academics")]
        [Authorize(Roles = StaticUserRole.CANDIDATE)]
        public async Task<IActionResult> AddAcademicAsync([FromBody] AcademicsDto addAcademicDto)
        {
            var addAcademics = await _academicServices.AddAcademicAsync(User, addAcademicDto);
            if (!addAcademics.IsSuccess)
            {
                return Unauthorized("Only candidate can add academic experience");
            }
            return Ok(addAcademics);
        }


        [HttpGet]
        [Route("GetMyAcademics")]
        [Authorize]
        public async Task<IActionResult> GetMyAcademicsAsync()
        {
            var myAcademics = await _academicServices.GetMyAcademicsAsync(User);
            return Ok(myAcademics);
        }

        [HttpGet]
        [Route("GetAcademics")]
        [Authorize(Roles = StaticUserRole.ADMIN_EMPLOYER)]
        public async Task<ActionResult<IEnumerable<GetAllAcademicsDto>>> GetAcademicsAsync()
        {
            var academics = await _academicServices.GetAcademicsAsync();
            return Ok(academics);
        }

        [HttpGet]
        [Route("GetAcademicsById/{id}")]
        [Authorize(Roles = StaticUserRole.ADMIN_EMPLOYER)]
        public async Task<IActionResult> GetAcademicsById(int id)
        {
            var academic = await _academicServices.GetAcademicsByIdAsync(id);
            return Ok(academic); 
        }

        [HttpGet]
        [Route("GetAcademicsByCandidateId/{candidateId}")]
        [Authorize(Roles = StaticUserRole.ADMIN_EMPLOYER)]
        public async Task<IActionResult> GetAcademicsByCandidateId(string candidateId)
        {
            var academic = await _academicServices.GetAcademicsByCandidateIdAsync(candidateId);
            return Ok(academic);
        }

        

        [HttpPut]
        [Route("UpdateAcademics/{id}")]
        [Authorize(Roles = StaticUserRole.CANDIDATE)]
        public async Task<IActionResult> UpdateAcademics([FromBody] AcademicsDto updateAcademicsDto, int id)
        {
            var updatedAcademic = await _academicServices.UpdateAcademicsAsync(User, updateAcademicsDto, id);
            if (updatedAcademic.IsSuccess)
            {
                return Ok(updatedAcademic);
            }
            return StatusCode(updatedAcademic.StatusCode, updatedAcademic.Message);
        }

        [HttpDelete]
        [Route("DeleteAcademics/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAcademics(int id)
        {
            var deleteAcademics = await _academicServices.DeleteAcademicsAsync(User, id);
            if (!deleteAcademics.IsSuccess)
            {
                return StatusCode(deleteAcademics.StatusCode, deleteAcademics.Message);
            }

            return Ok(deleteAcademics);
        }
    }
}
