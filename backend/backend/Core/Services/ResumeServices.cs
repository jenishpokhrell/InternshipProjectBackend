using backend.Core.DataContext;
using backend.Core.DTOs.General;
using backend.Core.DTOs.Resume;
using backend.Core.Entities;
using backend.Core.Interfaces;
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

        public ResumeServices(ApplicationDBContext context)
        {
            _context = context;
        }

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
    }
}

