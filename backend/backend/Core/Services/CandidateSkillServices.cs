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

        //Method for adding candidate skills
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
                    var skillName = existingSkills.FirstOrDefault(s => s.SkillId == skillId)?.Skill;
                    _context.CandidateSkills.Add(new CandidateSkill { CandidateId = loggedInUserId, SkillId = skillId, Skill = skillName  });
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

        //Method for getting all available skills
        public async Task<IEnumerable<GetSkillDto>> GetAvailableSkillsAsync()
        {
            var skills = await _skillRepositories.GetAllSkills();
            return _mapper.Map<IEnumerable<GetSkillDto>>(skills);
        }

        //Method for getting candidate skills using their Id
        public async Task<IEnumerable<GetCandidateSkillsDto>> GetCandidateSkillsAsync(string candidateId)
        {
            var candidateSkills = await _candidateSkillRepositories.GetCandidateSkills(candidateId);

            if(candidateSkills is null || !candidateSkills.Any())
            {
                throw new Exception("Candidate haven't added their skills yet.");
            }

            return _mapper.Map<IEnumerable<GetCandidateSkillsDto>>(candidateSkills);
        }

        //Method for getting individual skills
        public async Task<IEnumerable<GetSkillDto>> GetMySkillsAsync(ClaimsPrincipal User)
        {
            var mySkills = await _candidateSkillRepositories.GetMySkills(User);

            if(mySkills is null || !mySkills.Any())
            {
                throw new Exception("You haven't added your skills yet.");
            }

            return _mapper.Map<IEnumerable<GetSkillDto>>(mySkills);
        }

        //Method for deleting skill using skillId
        public async Task<GeneralServiceResponseDto> DeleteSkillAsync(ClaimsPrincipal User, int skillId)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var skill = await _context.CandidateSkills.FirstOrDefaultAsync(s => s.SkillId == skillId);

            if (skill is null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Skill not found"
                };
            }

            if (skill.CandidateId != loggedInUserId)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "You are not authorized to delete other candidate skills"
                };
            }

            await _candidateSkillRepositories.DeleteSkillById(skillId);

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Skill Deleted Successfully."
            };
        }
    }
}
