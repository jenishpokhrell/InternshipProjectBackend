using backend.Core.DataContext;
using backend.Core.DTOs.JobApplication;
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
    public class JobRepositories : IJobRepositories
    {
        private readonly ApplicationDBContext _context;
        private readonly DapperContext _dContext;

        public JobRepositories(ApplicationDBContext context, DapperContext dContext)
        {
            _context = context;
            _dContext = dContext;
        }

        public Task ApplyForJobAsync(ClaimsPrincipal User, int id)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            var query = "SELECT * FROM Jobs";

            using(var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<Job>(query);
            }
        }

        public async Task<IEnumerable<JobApplication>> GetJobApplicationsByJobIdAsync(int jobId)
        {
            var query = "SELECT JobStatus, CandidateId, CandidateName FROM JobApplications WHERE JobId = @jobId";

            using (var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<JobApplication>(query, new { jobId });
            }
        }

        public async Task<Job> GetJobById(int id)
        {
            var query = "SELECT * FROM Jobs WHERE Id = @Id";

            using(var connection = _dContext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Job>(query, new { id });
            }
        }

        public async Task PostJob(Job job)
        {
            await _context.AddAsync(job);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateJob(Job job)
        {
            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteJobAsync(int id)
        {
            var query = "DELETE FROM Jobs WHERE Id = @Id";

            using(var connection = _dContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }
    }
}
