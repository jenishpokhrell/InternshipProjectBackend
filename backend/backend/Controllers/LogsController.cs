using backend.Core.Constants;
using backend.Core.DTOs.Log;
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
    public class LogsController : ControllerBase
    {
        private readonly ILogServices _logServices;

        public LogsController(ILogServices logServices)
        {
            _logServices = logServices;
        }

        [HttpGet]
        [Route("GetAllLogs")]
        [Authorize(Roles = StaticUserRole.ADMIN)] //Can be only accessed by admin
        public async Task<IActionResult> GetLogs()
        {
            var logs = await _logServices.GetLogsAsync();
            return Ok(logs);
        }

        [HttpGet]
        [Route("GetMyLogs")]
        [Authorize]
        public async Task<IActionResult> GetMyLogs()
        {
            var myLogs = await _logServices.GetMyLogsAsync(User);
            return Ok(myLogs);
        }
    }
}
