using backend.Core.DataContext;
using backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.Interfaces;
using AutoMapper;
using backend.Core.DTOs.Log;
using System.Security.Claims;
using backend.Core.Interfaces.IRepositories;

namespace backend.Core.Services
{
    public class LogServices : ILogServices
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogRepositories _logRepositories;
        private readonly IMapper _mapper;
        public LogServices(ApplicationDBContext context, IMapper mapper, ILogRepositories logRepositories)
        {
            _context = context;
            _mapper = mapper;
            _logRepositories = logRepositories;
        }

        public async Task SaveNewLog(string Username, string Description)
        {
            var newLog = new Log()
            {
                Username = Username,
                Description = Description
            };

            await _context.Logs.AddAsync(newLog);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GetLogDto>> GetLogsAsync()
        {
            var logs = await _logRepositories.GetAllLogs();
            return _mapper.Map<IEnumerable<GetLogDto>>(logs);
        }

        public async Task<IEnumerable<GetLogDto>> GetMyLogsAsync(ClaimsPrincipal User)
        {
            var loggedInUser = User.Identity.Name;

            var logs = await _logRepositories.GetAllLogs();

            var myLogs = logs.Where(l => l.Username == loggedInUser);

            return _mapper.Map<IEnumerable<GetLogDto>>(myLogs);
        }
    }
}
