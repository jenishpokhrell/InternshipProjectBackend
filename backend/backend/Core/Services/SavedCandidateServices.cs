using AutoMapper;
using AutoMapper.QueryableExtensions;
using backend.Core.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Core.DTOs.SavedCandidates;
using backend.Core.Interfaces;
using backend.Core.Interfaces.IRepositories;

namespace backend.Core.Services
{
    public class SavedCandidateServices : ISavedCandidateServices
    {
        private readonly IMapper _mapper;
        private readonly ISavedCandidateRepositories _savedCandidateRepositories;

        public SavedCandidateServices(IMapper mapper, ISavedCandidateRepositories savedCandidateRepositories)
        {
            _mapper = mapper;
            _savedCandidateRepositories = savedCandidateRepositories;
        }

        //methhod for getting shortlisted candidates by employer
        public async Task<IEnumerable<SavedCandidateDto>> GetSavedCandidateAsync(ClaimsPrincipal User)
        {
            var savedCandidates = await _savedCandidateRepositories.GetSavedCandidates(User);
            if(savedCandidates is null)
            {
                throw new Exception("No candidate shotlisted yet.");
            }
            return _mapper.Map<IEnumerable<SavedCandidateDto>>(savedCandidates);
        }
    }
}
