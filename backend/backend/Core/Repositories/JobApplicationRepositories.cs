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
        
        //Repo method for getting candidates job applications
        public async Task<IEnumerable<JobApplication>> GetMyJobApplications(ClaimsPrincipal User)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = "SELECT ja.JobStatus, ja.Message, j.* FROM JobApplications ja INNER JOIN Jobs j ON ja.JobId = j.Id WHERE ja.CandidateId = @loggedInUserId";

            using (var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<JobApplication, Job, JobApplication>(query,
                    (ja, job) =>
                    {
                        ja.Job = job;
                        return ja;
                    },
                    new { loggedInUserId },
                    splitOn: "Id"
                    );
            }
        }
    }
}
