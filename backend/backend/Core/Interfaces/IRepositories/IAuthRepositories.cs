using backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Interfaces.IRepositories
{
    public interface IAuthRepositories
    {
        Task<ApplicationUser> GetUserById(string id);

        Task<IEnumerable<ApplicationUser>> GetAllUsers();
    }
}
