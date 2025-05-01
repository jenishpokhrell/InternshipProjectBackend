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
using backend.Core.DTOs.Academics;
using System.Data;

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

        //Repo method for ading academic
        public async Task<Academic> AddAcademics(ClaimsPrincipal User, AcademicsDto addAcademicsDto)
        {
            var candidateId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = "INSERT INTO Academics (InstitutionName, Stream, StartYear, GraduationYear, DegreeType, CurrentSemester, CandidateId) " +
                "VALUES (@InstitutionName, @Stream, @StartYear, @GraduationYear, @DegreeType, @CurrentSemester, @candidateId) " +
                "SELECT CAST(SCOPE_IDENTITY() AS int)";

            var parameters = new DynamicParameters();
            parameters.Add("InstitutionName", addAcademicsDto.InstitutionName, DbType.String);
            parameters.Add("Stream", addAcademicsDto.Stream, DbType.String);
            parameters.Add("StartYear", addAcademicsDto.StartYear, DbType.String);
            parameters.Add("GraduationYear", addAcademicsDto.GraduationYear, DbType.String);
            parameters.Add("DegreeType", addAcademicsDto.DegreeType, DbType.String);
            parameters.Add("CurrentSemester", addAcademicsDto.CurrentSemester, DbType.String);
            parameters.Add("CandidateId", candidateId, DbType.String);

            using(var connection = _dContext.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);
                var addAcademics = new Academic
                {
                    Id = id,
                    InstitutionName = addAcademicsDto.InstitutionName,
                    Stream = addAcademicsDto.Stream,
                    DegreeType = addAcademicsDto.DegreeType,
                    CurrentSemester = addAcademicsDto.CurrentSemester,
                    StartYear = addAcademicsDto.StartYear,
                    GraduationYear = addAcademicsDto.GraduationYear,
                    CandidateId = candidateId
                };

                return addAcademics;
            }
        }

        //Repo method for getting academic by id
        public async Task<Academic> GetAcademicById(int id)
        {
            var query = "SELECT * FROM Academics WHERE Id = @Id";

            using(var connection = _dContext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Academic>(query, new { id });
            }
        }

        //Repo method for getting all academics
        public async Task<IEnumerable<Academic>> GetAcademics()
        {
            var query = "SELECT * FROM Academics";

            using(var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<Academic>(query);
            }        
        }

        //Repo method for getting individuals academic experience
        public async Task<Academic> GetMyAcademic(ClaimsPrincipal User)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = "SELECT * FROM Academics WHERE CandidateId = @loggedInUserId";

            using(var connection = _dContext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Academic>(query, new { loggedInUserId });
            }
        }

        //Repo method for getting candidates academic experience by their id
        public async Task<Academic> GetAcademicsByCandidateId(string candidateId)
        {
            var query = "SELECT * FROM Academics WHERE CandidateId = @candidateId";

            using (var connection = _dContext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Academic>(query, new { candidateId });
            }
        }

        //Repo method for updating academic
        public async Task UpdateAcademics(AcademicsDto updateAcademicsDto, int id)
        {
            var query = "UPDATE Academics SET InstitutionName = @InstitutionName, Stream = @Stream, StartYear = @StartYear, " +
                "GraduationYear = @GraduationYear, DegreeType = @DegreeType, CurrentSemester = @CurrentSemester " +
                "WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("InstitutionName", updateAcademicsDto.InstitutionName, DbType.String);
            parameters.Add("Stream", updateAcademicsDto.Stream, DbType.String);
            parameters.Add("StartYear", updateAcademicsDto.StartYear, DbType.String);
            parameters.Add("GraduationYear", updateAcademicsDto.GraduationYear, DbType.String);
            parameters.Add("DegreeType", updateAcademicsDto.DegreeType, DbType.String);
            parameters.Add("CurrentSemester", updateAcademicsDto.CurrentSemester, DbType.String);

            using(var connection = _dContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        //Repo method for deleting academic by id
        public async Task DeleteAcademic(int id)
        {
            var query = "DELETE FROM Academics WHERE Id = @Id";

            using (var connection = _dContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }

        
    }   
}
