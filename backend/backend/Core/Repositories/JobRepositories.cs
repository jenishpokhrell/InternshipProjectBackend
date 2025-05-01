using backend.Core.DataContext;
using backend.Core.DTOs.Job;
using backend.Core.DTOs.JobApplication;
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
    public class JobRepositories : IJobRepositories
    {
        private readonly ApplicationDBContext _context;
        private readonly DapperContext _dContext;

        public JobRepositories(ApplicationDBContext context, DapperContext dContext)
        {
            _context = context;
            _dContext = dContext;
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
            var query = "SELECT Id, JobStatus, CandidateId, CandidateName FROM JobApplications WHERE JobId = @jobId";

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

        public async Task<Job> PostJob(ClaimsPrincipal User, JobDto postJobDto)
        {
            var postedBy = User.Identity.Name;
            var employerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var now = DateTime.Now;

            var query = "INSERT INTO Jobs (JobTitle, JobDescription, JobType, JobLevel, No_of_Openings, Requirements, Min_Years_of_Experience_Required, " +
                "Max_Years_of_Experience_Required, MinimumSalary, MaximumSalary, Location, IsActive, EmployerId, PostedBy, CreatedAt, UpdatedAt) VALUES " +
                "(@JobTitle, @JobDescription, @JobType, @JobLevel, @No_of_Openings, @Requirements, @Min_Years_of_Experience_Required, " +
                "@Max_Years_of_Experience_Required, @MinimumSalary, @MaximumSalary, @Location, @IsActive, @EmployerId, @PostedBy, @CreatedAt, @UpdatedAt) " +
                "SELECT CAST(SCOPE_IDENTITY() AS int)";

            var parameters = new DynamicParameters();
            parameters.Add("JobTitle", postJobDto.JobTitle, DbType.String);
            parameters.Add("JobDescription", postJobDto.JobDescription, DbType.String);
            parameters.Add("JobType", postJobDto.JobType, DbType.String);
            parameters.Add("JobLevel", postJobDto.JobLevel, DbType.String);
            parameters.Add("No_of_Openings", postJobDto.No_of_Openings, DbType.String);
            parameters.Add("Requirements", postJobDto.Requirements, DbType.String);
            parameters.Add("Min_Years_of_Experience_Required", postJobDto.Min_Years_of_Experience_Required, DbType.Int32);
            parameters.Add("Max_Years_of_Experience_Required", postJobDto.Max_Years_of_Experience_Required, DbType.Int32);
            parameters.Add("MinimumSalary", postJobDto.MinimumSalary, DbType.Int64);
            parameters.Add("MaximumSalary", postJobDto.MaximumSalary, DbType.Int64);
            parameters.Add("Location", postJobDto.Location, DbType.String);
            parameters.Add("IsActive", postJobDto.IsActive, DbType.Boolean);
            parameters.Add("EmployerId", employerId, DbType.String);
            parameters.Add("PostedBy", postedBy, DbType.String);
            parameters.Add("CreatedAt", now, DbType.DateTime);
            parameters.Add("UpdatedAt", now, DbType.DateTime);

            using (var connection = _dContext.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);
                var job = new Job()
                {
                    JobTitle = postJobDto.JobTitle,
                    JobDescription = postJobDto.JobDescription,
                    No_of_Openings = postJobDto.No_of_Openings,
                    JobLevel = postJobDto.JobLevel,
                    JobType = postJobDto.JobType,
                    Requirements = postJobDto.Requirements,
                    Min_Years_of_Experience_Required = postJobDto.Min_Years_of_Experience_Required,
                    Max_Years_of_Experience_Required = postJobDto.Max_Years_of_Experience_Required,
                    MinimumSalary = postJobDto.MinimumSalary,
                    MaximumSalary = postJobDto.MaximumSalary,
                    Location = postJobDto.Location,
                    IsActive = postJobDto.IsActive,
                    EmployerId = employerId,
                    PostedBy = postedBy,
                    CreatedAt = now,
                    UpdatedAt = now,
                };

                return job;
            }
        }

        public async Task UpdateJob(JobDto updateJobDto, int id)
        {
            var updatedAt = DateTime.Now;

            var query = "UPDATE Jobs SET JobTitle = @JobTitle, JobDescription = @JobDescription, JobType = @JobType, JobLevel = @JobLevel, " +
                 "No_of_Openings = @No_of_Openings, " +
                 "Requirements = @Requirements, Min_Years_of_Experience_Required = @Min_Years_of_Experience_Required, " +
                 "Max_Years_of_Experience_Required = @Max_Years_of_Experience_Required, MinimumSalary = @MinimumSalary, " +
                 "MaximumSalary = @MaximumSalary, Location = @Location, " +
                 "IsActive = @IsActive, UpdatedAt = @UpdatedAt WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("JobTitle", updateJobDto.JobTitle, DbType.String);
            parameters.Add("JobDescription", updateJobDto.JobDescription, DbType.String);
            parameters.Add("JobType", updateJobDto.JobType, DbType.String);
            parameters.Add("JobLevel", updateJobDto.JobLevel, DbType.String);
            parameters.Add("No_of_Openings", updateJobDto.No_of_Openings, DbType.String);
            parameters.Add("Requirements", updateJobDto.Requirements, DbType.String);
            parameters.Add("Min_Years_of_Experience_Required", updateJobDto.Min_Years_of_Experience_Required, DbType.String);
            parameters.Add("Max_Years_of_Experience_Required", updateJobDto.Max_Years_of_Experience_Required, DbType.String);
            parameters.Add("MinimumSalary", updateJobDto.MinimumSalary, DbType.String);
            parameters.Add("MaximumSalary", updateJobDto.MaximumSalary, DbType.String);
            parameters.Add("Location", updateJobDto.Location, DbType.String);
            parameters.Add("IsActive", updateJobDto.IsActive, DbType.Boolean);
            parameters.Add("UpdatedAt", updatedAt, DbType.DateTime);

            using(var connection = _dContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
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
