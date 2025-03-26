using backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Interfaces.IRepositories
{
    public interface ISkillRepositories
    {
        Task<Skills> GetSkillById(int id);

        Task<IEnumerable<Skills>> GetAllSkills();

        Task DeleteSkill(int id);
    }
}
