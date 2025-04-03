using backend.Core.DataContext;
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
    public class JobApplicationRepositories : IJobApplicationRepositories
    {
        private readonly DapperContext _dContext;

        public JobApplicationRepositories(DapperContext dContext)
        {
            _dContext = dContext;
        }
        public async Task<IEnumerable<JobApplication>> GetMyJobApplications(ClaimsPrincipal User)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = "SELECT * FROM JobApplications WHERE CandidateId = @loggedInUserId";

            using (var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<JobApplication>(query, new { loggedInUserId });
            }
        }
    }
}
