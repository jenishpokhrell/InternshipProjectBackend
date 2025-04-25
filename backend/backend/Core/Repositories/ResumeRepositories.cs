using backend.Core.DataContext;
using backend.Core.Entities;
using backend.Core.Interfaces.IRepositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Core.Repositories
{
    public class ResumeRepositories : IResumeRepositories
    {
        private readonly ApplicationDBContext _context;
        private readonly DapperContext _dContext;

        public ResumeRepositories(ApplicationDBContext context, DapperContext dContext)
        {
            _context = context;
            _dContext = dContext;
        }

        public async Task DeleteResume(ClaimsPrincipal User)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = "DELETE FROM Resumes WHERE CandidateId = @loggedInUserId";

            using(var connection = _dContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { loggedInUserId });
            }
        }

        public async Task<Resume> GetMyResume(ClaimsPrincipal User)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = "SELECT * FROM Resumes WHERE CandidateId = @loggedInUserId";

            using(var connection = _dContext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Resume>(query, new { loggedInUserId });
            }
        }

        public async Task<Resume> GetResumeByCandidateId(string candidateId)
        {
            var query = "SELECT * FROM Resumes WHERE CandidateId = @candidateId";

            using(var connection = _dContext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Resume>(query, new { candidateId } );
            }
        }
    }
}
