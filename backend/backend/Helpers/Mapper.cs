using AutoMapper;
using backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.DTOs.Academics;
using backend.Core.DTOs.Auth;
using backend.Core.DTOs.Experience;
using backend.Core.DTOs.Job;
using backend.Core.DTOs.JobApplication;
using backend.Core.DTOs.Projects;
using backend.Core.DTOs.SavedCandidates;
using backend.Core.DTOs.Skill;
using backend.Core.DTOs.Log;

namespace backend.Helpers
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<ApplicationUser, UserInfo>().ReverseMap();
            CreateMap<Job, GetJobDto>().ReverseMap();
            CreateMap<Job, GetMyJobDto>().ReverseMap();
            CreateMap<Job, GetJobDtoForCandidate>().ReverseMap();
            CreateMap<SavedJob, GetJobDtoForCandidate>()
                .IncludeMembers(src => src.Job).ReverseMap();
            CreateMap<JobApplication, JobApplicationsDto>().ReverseMap();
            CreateMap<Projects, GetMyProjectDto>().ReverseMap();
            CreateMap<Projects, GetProjectDto>().ReverseMap();
            CreateMap<Academic, GetAcademicsDto>().ReverseMap();
            CreateMap<Academic, GetAllAcademicsDto>().ReverseMap();
            CreateMap<Experience, GetExperienceDto>()
                .ForMember(f => f.From, frm => frm.MapFrom(src => src.From.ToString("yyyy-MM-dd")))
                .ForMember(d => d.To, opt => opt.MapFrom(src => src.To.HasValue ? src.To.Value.ToString("yyyy-MM-dd") : "Present"))
                .ReverseMap();
            CreateMap<Log, GetLogDto>().ReverseMap();
            CreateMap<JobApplication, MyJobApplicationsDto>().ReverseMap();
            CreateMap<SavedCandidate, SavedCandidateDto>().ReverseMap();
            CreateMap<Skills, GetSkillDto>().ReverseMap();
            CreateMap<CandidateSkill, GetCandidateSkillsDto>().ReverseMap();
        }
    }
}
