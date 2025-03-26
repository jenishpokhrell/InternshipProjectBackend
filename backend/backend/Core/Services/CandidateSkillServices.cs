using AutoMapper;
using AutoMapper.QueryableExtensions;
using backend.Core.DataContext;
using backend.Core.DTOs.General;
using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Core.Interfaces;
using backend.Core.DTOs.Skill;
using backend.Core.Interfaces.IRepositories;

namespace backend.Core.Services
{
    public class CandidateSkillServices : ICandidateSkillServices
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly ICandidateSkillRepositories _candidateSkillRepositories;
        private readonly ISkillRepositories _skillRepositories;

        public CandidateSkillServices(ApplicationDBContext context, IMapper mapper, ICandidateSkillRepositories candidateSkillRepositories,
            ISkillRepositories skillRepositories)
        {
            _context = context;
            _mapper = mapper;
            _candidateSkillRepositories = candidateSkillRepositories;
            _skillRepositories = skillRepositories;
        }

        public async Task<GeneralServiceResponseDto> AddCandidateSkillAsync(ClaimsPrincipal User, AddCandidateSkillDto addCandidateSkillDto)
        {
            var existingSkills = await _context.Skills.Where(s => addCandidateSkillDto.SkillId.Contains(s.SkillId)).ToListAsync();
            if (existingSkills.Count != addCandidateSkillDto.SkillId.Count)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Skill doesn't exist."
                };
            } 

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach(var skillId in addCandidateSkillDto.SkillId)
            {
                if(!_context.CandidateSkills.Any(cs => cs.CandidateId == loggedInUserId && cs.SkillId == skillId))
                {
                    _context.CandidateSkills.Add(new CandidateSkill { CandidateId = loggedInUserId, SkillId = skillId });
                }
            }

            await _context.SaveChangesAsync();
            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Skill added successfully."
            };
        }

        public async Task<IEnumerable<GetSkillDto>> GetAvailableSkillsAsync()
        {
            var skills = await _skillRepositories.GetAllSkills();
            return _mapper.Map<IEnumerable<GetSkillDto>>(skills);
        }

        public async Task<IEnumerable<GetCandidateSkillsDto>> GetCandidateSkillsAsync(string candidateId)
        {
            var candidateSkills = await _candidateSkillRepositories.GetCandidateSkills(candidateId);

            if(candidateSkills is null || !candidateSkills.Any())
            {
                throw new Exception("Candidate haven't added their skills yet.");
            }

            return _mapper.Map<IEnumerable<GetCandidateSkillsDto>>(candidateSkills);
        }
    }
}
