using backend.Core.DataContext;
using backend.Core.DTOs.Job;
using backend.Core.Entities;
using backend.Core.Interfaces.IRepositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Repositories
{
    public class LogRepositories : ILogRepositories
    {
        private readonly DapperContext _dContext;

        public LogRepositories(DapperContext dContext)
        {
            _dContext = dContext;
        }
        public async Task<IEnumerable<Log>> GetAllLogs()
        {
            var query = "SELECT * FROM Logs";

            using(var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<Log>(query);
            }
        }
    }
}
