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
    public class ExperienceRepositories : IExperienceRepositories
    {
        private readonly ApplicationDBContext _context;
        private readonly DapperContext _dContext;

        public ExperienceRepositories(ApplicationDBContext context, DapperContext dContext)
        {
            _context = context;
            _dContext = dContext;
        }

        //Repo method for adding work experience
        public async Task AddExperience(Experience experience)
        {
            await _context.Experiences.AddAsync(experience);
            await _context.SaveChangesAsync();
        }

        //Repo method for getting individuals work experience
        public async Task<IEnumerable<Experience>> GetMyExperiencesAsync(ClaimsPrincipal User)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = "SELECT * FROM Experiences WHERE UserId = @loggedInUserId";

            using (var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<Experience>(query, new { loggedInUserId });
            }
        }

        //Repo method for getting work experience by id
        public async Task<Experience> GetExperienceById(int id)
        {
            var query = "SELECT * FROM Experiences WHERE ExperienceId = @Id";

            using (var connection = _dContext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Experience>(query, new { id });
            }
        }


        //Repo method for getting work all experiences
        public async Task<IEnumerable<Experience>> GetAllExperiences()
        {
            var query = "SELECT * FROM Experiences";

            using (var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<Experience>(query);
            }
        }

        //Repo method for getting candidates experiences by their id
        public async Task<IEnumerable<Experience>> GetExperienceByCandidateId(string candidateId)
        {
            var query = "SELECT * FROM Experiences WHERE UserId = @candidateId";

            using (var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<Experience>(query, new { candidateId });
            }
        }

        //Repo method updating work experience
        public async Task UpdateExperienceAsync(Experience experience)
        {
            /*var query = "UPDATE Experiences SET JobTitle = @JobTitle," +
                "CompanyName = @CompanyName," +
                "JobDescription = @JobDescription," +
                "From = @From," +
                "To = @To," +
                "IsCurrentlyWorking = @IsCurrentlyWorking WHERE ExperienceId = @Id";

            using (var connection = _dContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, experience);
            }*/

            _context.Experiences.Update(experience);
            await _context.SaveChangesAsync();
        }

        //Repo method for deleting experience by id
        public async Task DeleteExperienceAsync(int id)
        {
            var query = "DELETE FROM Experiences WHERE ExperienceId = @Id";

            using(var connection = _dContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }

        
    }
}
