using backend.Core.DataContext;
using backend.Core.DTOs.General;
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
    public class CandidateSkillRepositories : ICandidateSkillRepositories
    {
        private readonly DapperContext _dContext;

        public CandidateSkillRepositories(DapperContext dContext)
        {
            _dContext = dContext;
        }


        public async Task<IEnumerable<CandidateSkill>> GetCandidateSkills(string candidateId)
        {
            var query = "SELECT * FROM CandidateSkills WHERE CandidateId = @CandidateId";

            using(var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<CandidateSkill>(query, new { candidateId });
            }
        }

        public async Task<IEnumerable<CandidateSkill>> GetMySkills(ClaimsPrincipal User)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = "SELECT * FROM CandidateSkills WHERE CandidateId = @loggedInUserId";

            using(var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<CandidateSkill>(query, new { loggedInUserId });
            }
        }
        public async Task DeleteSkillById(int skillId)
        {
            var query = "DELETE FROM CandidateSkills WHERE SkillId = @skillId";

            using(var connection = _dContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { skillId });
            }
        }
    }
}
