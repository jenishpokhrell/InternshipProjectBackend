using backend.Core.DataContext;
using backend.Core.Entities;
using backend.Core.Interfaces.IRepositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Repositories
{
    public class SkillRepositories : ISkillRepositories
    {
        private readonly DapperContext _dContext;

        public SkillRepositories(DapperContext dContext)
        {
            _dContext = dContext;
        }

        public async Task DeleteSkill(int id)
        {
            var query = "DELETE FROM Skills WHERE SkillId = @Id";

            using(var connection = _dContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }

        public async Task<IEnumerable<Skills>> GetAllSkills()
        {
            var query = "SELECT * FROM Skills";

            using (var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<Skills>(query);
            }
        }

        public async Task<Skills> GetSkillById(int id)
        {
            var query = "SELECT * FROM Skills WHERE SkillId = @Id";

            using (var connection = _dContext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Skills>(query, new { id });
            }
        }
    }
}
