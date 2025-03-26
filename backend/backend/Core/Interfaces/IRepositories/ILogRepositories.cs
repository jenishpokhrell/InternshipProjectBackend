using backend.Core.DTOs.Job;
using backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Core.Interfaces.IRepositories
{
    public interface ILogRepositories
    {
        Task<IEnumerable<Log>> GetAllLogs();
    }
}
