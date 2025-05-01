using backend.Core.DataContext;
using backend.Core.DTOs.Projects;
using backend.Core.Entities;
using backend.Core.Interfaces.IRepositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
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

        //Repo method for adding project
        public async Task<Projects> AddProject(ClaimsPrincipal User, ProjectDto addProjectDto)
        {
            var candidateId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = "INSERT INTO Projects (ProjectName, ProjectDescription, ProjectURL, CandidateId) " +
                "VALUES (@ProjectName, @ProjectDescription, @ProjectURL, @candidateId) " +
                "SELECT CAST(SCOPE_IDENTITY() AS int)";

            var parameters = new DynamicParameters();
            parameters.Add("ProjectName", addProjectDto.ProjectName, DbType.String);
            parameters.Add("ProjectDescription", addProjectDto.ProjectDescription, DbType.String);
            parameters.Add("ProjectURL", addProjectDto.ProjectURL, DbType.String);
            parameters.Add("CandidateId", candidateId, DbType.String);

            using (var connection = _dContext.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);
                var addProject = new Projects
                {
                    ProjectName = addProjectDto.ProjectName,
                    ProjectDescription = addProjectDto.ProjectDescription,
                    ProjectURL = addProjectDto.ProjectURL,
                    CandidateId = candidateId
                };

                return addProject;
            }
        }

        //Repo method for getting all projects
        public async Task<IEnumerable<Projects>> GetAllProjects()
        {
            var query = "SELECT * FROM Projects";

            using(var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<Projects>(query);
            }
        }

        //Repo method for getting candidates projects using their id
        public async Task<IEnumerable<Projects>> GetProjectByCandidateId(string candidateId)
        {
            var query = "SELECT * FROM Projects WHERE CandidateId = @candidateId";

            using (var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<Projects>(query, new { candidateId });
            }
        }
        
        //Repo method for getting project by id
        public async Task<Projects> GetProjectById(int id)
        {
            var query = "SELECT * FROM Projects WHERE ProjectId = @Id";

            using (var connection = _dContext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Projects>(query, new { id });
            }
        }

        //Repo method for updating projects using id
        public async Task UpdateProjects(ProjectDto updateProjectDto, int id)
        {
            var query = "UPDATE Projects SET ProjectName = @ProjectName, ProjectDescription = @ProjectDescription, ProjectURL = @ProjectURL " +
                "WHERE ProjectId = @ProjectId";

            var parameters = new DynamicParameters();
            parameters.Add("ProjectId", id, DbType.Int32);
            parameters.Add("ProjectName", updateProjectDto.ProjectName, DbType.String);
            parameters.Add("ProjectDescription", updateProjectDto.ProjectDescription, DbType.String);
            parameters.Add("ProjectURL", updateProjectDto.ProjectURL, DbType.String);

            using(var connection = _dContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        //Repo method for deleting project by id
        public async Task DeleteProject(int id)
        {
            var query = "DELETE FROM Projects WHERE ProjectId = @ProjectId";

            using (var connection = _dContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }

    }
}
