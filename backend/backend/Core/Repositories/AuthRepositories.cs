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
    public class AuthRepositories : IAuthRepositories
    {

        private readonly ApplicationDBContext _context;
        private readonly DapperContext _dContext;

        public AuthRepositories(ApplicationDBContext context, DapperContext dContext)
        {
            _context = context;
            _dContext = dContext;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            var query = "SELECT * FROM Users";

            using(var connection = _dContext.CreateConnection())
            {
                return await connection.QueryAsync<ApplicationUser>(query);
            }
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            var query = "SELECT * FROM Users WHERE Id = @Id";

            using(var connection = _dContext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<ApplicationUser>(query, new { id });
            }
        }
    }
}
