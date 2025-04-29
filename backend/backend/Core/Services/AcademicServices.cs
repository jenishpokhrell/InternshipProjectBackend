using AutoMapper;
using AutoMapper.QueryableExtensions;
using backend.Core.DataContext;
using backend.Core.DTOs.General;
using backend.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Core.DTOs.Academics;
using backend.Core.Interfaces.IServices;
using backend.Core.Interfaces.IRepositories;

namespace backend.Core.Services
{
    public class AcademicServices : IAcademicServices
    {
        private readonly IMapper _mapper;
        private readonly IAcademicRepositories _academicrepositories;

        public AcademicServices(IMapper mapper, IAcademicRepositories academicRepositories)
        {
            _mapper = mapper;
            _academicrepositories = academicRepositories;
        }

        //Adding new academic
        public async Task<GeneralServiceResponseDto> AddAcademicAsync(ClaimsPrincipal User, AddAcademicsDto addAcademicsDto)
        {
            /*var academics = new Academic()
            {
                InstitutionName = addAcademicDto.InstitutionName,
                Stream = addAcademicDto.Stream,
                DegreeType = addAcademicDto.DegreeType,
                CurrentSemester = addAcademicDto.CurrentSemester,
                StartYear = addAcademicDto.StartYear,
                GraduationYear = addAcademicDto.GraduationYear,
                CandidateId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };*/

            await _academicrepositories.AddAcademics(User, addAcademicsDto);

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Academic experience added successfully"
            };
        }


        //Method for Getting all academics
        public async Task<IEnumerable<GetAllAcademicsDto>> GetAcademicsAsync()
        {
            var academics = await _academicrepositories.GetAcademics();
            return _mapper.Map<IEnumerable<GetAllAcademicsDto>>(academics);
        }

        //Method for getting academic by Id
        public async Task<GetAcademicsDto> GetAcademicsByIdAsync(int id)
        {
            var academic = await _academicrepositories.GetAcademicById(id);

            if(academic is null)
            {
                throw new Exception("Academic experience not found");
            }

            return _mapper.Map<GetAcademicsDto>(academic);
        }

        //Method for getting individuals academic
        public async Task<GetAcademicsDto> GetMyAcademicsAsync(ClaimsPrincipal User)
        {
            var academic = await _academicrepositories.GetMyAcademic(User);

            if(academic is null)
            {
                throw new Exception("You haven't added your academics yet.");
            }
            return _mapper.Map<GetAcademicsDto>(academic);
        }

        //Method for getting candidates academic experience by Candidate Ids
        public async Task<GetAcademicsDto> GetAcademicsByCandidateIdAsync(string candidateId)
        {
            var candidateAcademics = await _academicrepositories.GetAcademicsByCandidateId(candidateId);

            if (candidateAcademics is null)
            {
                throw new Exception("Candidate hasn't added any academics yet.");
            }

            return _mapper.Map<GetAcademicsDto>(candidateAcademics);
        }


        //Method for updating individuals academic
        public async Task<GeneralServiceResponseDto> UpdateAcademicsAsync(ClaimsPrincipal User, AddAcademicsDto addAcademicsDto, int id)
        {
            var loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var academic = await _academicrepositories.GetAcademicById(id);

            if(academic is null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Academic experience you're trying to find seems to be missing or deleted by the user."
                };
            }

            if(loggedInUser != academic.CandidateId)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 403,
                    Message = "You are not authorized to update this academic experience."
                };
            }

            academic.InstitutionName = addAcademicsDto.InstitutionName;
            academic.Stream = addAcademicsDto.Stream;
            academic.GraduationYear = addAcademicsDto.GraduationYear;
            academic.StartYear = addAcademicsDto.StartYear;
            academic.DegreeType = addAcademicsDto.DegreeType;
            academic.CurrentSemester = addAcademicsDto.CurrentSemester;

            await _academicrepositories.UpdateAcademics(academic);

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 400,
                Message = "Academic experience updated successfully."
            };
        }

        //Method for deleting academic
        public async Task<GeneralServiceResponseDto> DeleteAcademicsAsync(ClaimsPrincipal User, int id)
        {
            var loggedinInUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var academic = await _academicrepositories.GetAcademicById(id);

            if (academic.CandidateId != loggedinInUser)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "You are not authorized to delete this academics"
                };
            }

            await _academicrepositories.DeleteAcademic(id);

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Academics removed successfully."
            };
        }     
    }
}
