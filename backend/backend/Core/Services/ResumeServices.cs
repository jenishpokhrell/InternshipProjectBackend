using AutoMapper;
using backend.Core.DataContext;
using backend.Core.DTOs.General;
using backend.Core.DTOs.Resume;
using backend.Core.Entities;
using backend.Core.Interfaces;
using backend.Core.Interfaces.IRepositories;
using backend.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace backend.Core.Services
{
    public class ResumeServices : IResumeServices
    {
        private readonly ApplicationDBContext _context;
        private readonly IResumeRepositories _resumeRepositories;
        private readonly IMapper _mapper;

        public ResumeServices(ApplicationDBContext context, IResumeRepositories resumeRepositories, IMapper mapper)
        {
            _context = context;
            _resumeRepositories = resumeRepositories;
            _mapper = mapper;
        }


        //Method for adding or updating existing resume
        public async Task<GeneralServiceResponseDto> AddorUpdateResumeAsync(ClaimsPrincipal User, ResumeDto resumeDto,
             CloudinaryServices cloudinaryServices)
        {
            if (resumeDto.CandidateResume == null || resumeDto.CandidateResume.Length == 0)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Invalid Resume File."
                };
            }

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingResume = await _context.Resumes.Where(r => r.CandidateId == loggedInUserId).FirstOrDefaultAsync();

            if (existingResume != null)
            {
                if (!string.IsNullOrEmpty(existingResume.CandidateResume))
                {
                    string publicId = cloudinaryServices.GetPublicId(existingResume.CandidateResume);
                    await cloudinaryServices.DeleteFileAsync(existingResume.CandidateResume);
                }

                existingResume.CandidateResume = await cloudinaryServices.UploadResumeAsync(resumeDto.CandidateResume);
                _context.Resumes.Update(existingResume);
            }
            else
            {
                var resume = new Resume()
                {
                    CandidateId = loggedInUserId,
                    CandidateResume = await cloudinaryServices.UploadResumeAsync(resumeDto.CandidateResume)
                };
                await _context.Resumes.AddAsync(resume);
            }
            await _context.SaveChangesAsync();

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Resume Uploaded Successfully.",
            };
        }  


        //Method for getting my resume
        public async Task<GetResumeDto> GetMyResumeAsync(ClaimsPrincipal User)
        {
            var resume = await _resumeRepositories.GetMyResume(User);
            return _mapper.Map<GetResumeDto>(resume);
        }

        //Method for getting candidates resume by their id
        public async Task<GetResumeDto> GetResumeByCandidateIdAsync(string candidateId)
        {
            var candidateResume = await _resumeRepositories.GetResumeByCandidateId(candidateId);

            if(candidateResume is null)
            {
                throw new Exception("Candidate haven't added their resume yet.");
            }

            return _mapper.Map<GetResumeDto>(candidateResume);
        }


        //Method for deleting resume
        public async Task<GeneralServiceResponseDto> DeleteResumeAsync(ClaimsPrincipal User)
        {
            await _resumeRepositories.DeleteResume(User);
            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Resume Deleted Successfully.",
            };
        }
    }
}

