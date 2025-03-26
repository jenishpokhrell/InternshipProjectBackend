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
    public class SavedCandidateRepositories : ISavedCandidateRepositories
    {
        private readonly DapperContext _dContext;

        public SavedCandidateRepositories(DapperContext dContext)
        {
            _dContext = dContext;
        }
        public async Task<IEnumerable<SavedCandidate>> GetSavedCandidates(ClaimsPrincipal User)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = "SELECT * FROM SavedCandidates Where EmployerId = @loggedInUserId";

            using(var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<SavedCandidate>(query, new { loggedInUserId });
            }
        }
    }
}
