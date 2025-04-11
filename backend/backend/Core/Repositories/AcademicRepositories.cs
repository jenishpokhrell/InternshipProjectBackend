using backend.Core.DataContext;
using backend.Core.DTOs.General;
using backend.Core.Entities;
using backend.Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Security.Claims;

namespace backend.Repositories
{
    public class AcademicRepositories : IAcademicRepositories
    {
        private readonly ApplicationDBContext _context;
        private readonly DapperContext _dContext;

        public AcademicRepositories(ApplicationDBContext context, DapperContext dContext)
        {
            _context = context;
            _dContext = dContext;
        }
        public async Task AddAcademics(Academic academics)
        {
            await _context.Academics.AddAsync(academics);
            await _context.SaveChangesAsync();   
        }

        public async Task<Academic> GetAcademicById(int id)
        {
            var query = "SELECT * FROM Academics WHERE Id = @Id";

            using(var connection = _dContext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Academic>(query, new { id });
            }
        }

        public async Task<IEnumerable<Academic>> GetAcademics()
        {
            var query = "SELECT * FROM Academics";

            using(var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<Academic>(query);
            }        
        }

        public async Task<Academic> GetMyAcademic(ClaimsPrincipal User)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = "SELECT * FROM Academics WHERE CandidateId = @loggedInUserId";

            using(var connection = _dContext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Academic>(query, new { loggedInUserId });
            }
        }

        public async Task UpdateAcademics(Academic academic)
        {
            _context.Academics.Update(academic);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAcademic(int id)
        {
            var query = "DELETE FROM Academics WHERE Id = @Id";

            using (var connection = _dContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }

        public async Task<Academic> GetAcademicsByCandidateId(string candidateId)
        {
            var query = "SELECT * FROM Academics WHERE CandidateId = @candidateId";

            using (var connection = _dContext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Academic>(query, new { candidateId });
            }
        }
    }   
}
