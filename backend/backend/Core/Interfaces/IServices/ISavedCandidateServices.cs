using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Core.DTOs.SavedCandidates;

namespace backend.Core.Interfaces
{
    public interface ISavedCandidateServices
    {
        Task<IEnumerable<SavedCandidateDto>> GetSavedCandidateAsync(ClaimsPrincipal User);
    }
}
