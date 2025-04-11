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
    public class ProjectRepositories : IProjectRepositories
    {
        private readonly ApplicationDBContext _context;
        private readonly DapperContext _dContext;

        public ProjectRepositories(ApplicationDBContext context, DapperContext dContext)
        {
            _context = context;
            _dContext = dContext;
        }
        public async Task AddProject(Projects projects)
        {
            await _context.AddAsync(projects);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Projects>> GetAllProjects()
        {
            var query = "SELECT * FROM Projects";

            using(var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<Projects>(query);
            }
        }

        public async Task<IEnumerable<Projects>> GetProjectByCandidateId(string candidateId)
        {
            var query = "SELECT * FROM Projects WHERE CandidateId = @candidateId";

            using (var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<Projects>(query, new { candidateId });
            }
        }

        public async Task<Projects> GetProjectById(int id)
        {
            var query = "SELECT * FROM Projects WHERE ProjectId = @Id";

            using (var connection = _dContext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Projects>(query, new { id });
            }
        }
    }
}
