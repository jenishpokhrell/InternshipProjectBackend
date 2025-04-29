using AutoMapper;
using AutoMapper.QueryableExtensions;
using backend.Core.DataContext;
using backend.Core.DTOs.General;
using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.DTOs.Skill;
using backend.Core.Interfaces;
using backend.Core.Interfaces.IRepositories;

namespace backend.Core.Services
{
    public class SkillServices : ISkillServices
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly ISkillRepositories _skillRepositories;

        public SkillServices(ApplicationDBContext context, IMapper mapper, ISkillRepositories skillRepositories)
        {
            _context = context;
            _mapper = mapper;
            _skillRepositories = skillRepositories;
        }

        //method for adding skills 
        public async Task<GeneralServiceResponseDto> AddSkillsAsync(SkillDto skillDto)
        {
            Skills skill = new Skills()
            {
                Skill = skillDto.Skill
            };

            await _context.AddAsync(skill);
            await _context.SaveChangesAsync();

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Skill added successfully."
            };
        }

        //method for getting all skills
        public async Task<IEnumerable<GetSkillDto>> GetAllSkillsAsync()
        {
            var skills = await _context.Skills.ProjectTo<GetSkillDto>(_mapper.ConfigurationProvider).ToListAsync();
            if(skills is null)
            {
                throw new Exception("No skills added yet.");
            }
            return skills;
        }

        //method for getting skill by id
        public async Task<GetSkillDto> GetSkillByIdAsync(int id)
        {
            var skill = await _skillRepositories.GetSkillById(id);
            if(skill is null)
            {
                throw new Exception("No skill found");
            }
            return _mapper.Map<GetSkillDto>(skill);
        }

        //method for deleting skill by id
        public async Task<GeneralServiceResponseDto> DeleteSkillAsync(int id)
        {
            var skill = await _skillRepositories.GetSkillById(id);

            if (skill is null)
            {
                throw new Exception("No skill found");
            }

            await _skillRepositories.DeleteSkill(id);

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Skill Deleted Successfully."
            };

        }
    }
}
