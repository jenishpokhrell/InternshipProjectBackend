using backend.Core.DTOs.Job;
using backend.Core.DTOs.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Core.Interfaces
{
    public interface ILogServices
    {
        Task SaveNewLog(string Username, string Description);

        Task<IEnumerable<GetLogDto>> GetLogsAsync();

        Task<IEnumerable<GetLogDto>> GetMyLogsAsync(ClaimsPrincipal User);
    }
}
