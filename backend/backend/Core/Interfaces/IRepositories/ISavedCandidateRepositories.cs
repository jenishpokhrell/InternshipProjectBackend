using backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Core.Interfaces.IRepositories
{
    public interface ISavedCandidateRepositories
    {
        Task<IEnumerable<SavedCandidate>> GetSavedCandidates(ClaimsPrincipal User);
    }
}
