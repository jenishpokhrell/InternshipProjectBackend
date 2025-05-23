﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Core.DTOs.General;
using backend.Core.DTOs.Resume;
using backend.Core.Services;
using backend.Helpers;

namespace backend.Core.Interfaces
{
    public interface IResumeServices
    {
        Task<GeneralServiceResponseDto> AddorUpdateResumeAsync(ClaimsPrincipal User, ResumeDto resumeDto, CloudinaryServices cloudinaryServices);

        Task<GetResumeDto> GetResumeByCandidateIdAsync(string candidateId);

        Task<GetResumeDto> GetMyResumeAsync(ClaimsPrincipal User);

        Task<GeneralServiceResponseDto> DeleteResumeAsync(ClaimsPrincipal User);
    }
}
