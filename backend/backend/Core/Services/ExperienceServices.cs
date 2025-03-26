using AutoMapper;
using AutoMapper.QueryableExtensions;
using backend.Core.Constants;
using backend.Core.DataContext;
using backend.Core.DTOs.General;
using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Core.DTOs.Experience;
using backend.Core.Interfaces;
using backend.Core.Interfaces.IRepositories;

namespace backend.Core.Services
{
    public class ExperienceServices : IExperienceServices
    {

        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly IExperienceRepositories _experienceRepositories;

        public ExperienceServices(ApplicationDBContext context, IMapper mapper, IExperienceRepositories experienceRepositories)
        {
            _context = context;
            _mapper = mapper;
            _experienceRepositories = experienceRepositories;
        }
        public async Task<GeneralServiceResponseDto> AddExperienceAsync(ClaimsPrincipal User, AddExperienceDto addExperienceDto)
        {
            if(!DateTime.TryParseExact(addExperienceDto.From, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fromDate))
            {
                return new GeneralServiceResponseDto()
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Message = "Invalid date format. Use yyyy-MM-dd instead",    
                };
            }

            DateTime? toDate = null;
            if(!string.IsNullOrEmpty(addExperienceDto.To) && addExperienceDto.To.ToLower() != "present")
            {
                if (!DateTime.TryParseExact(addExperienceDto.To, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedToDate))
                {
                    return new GeneralServiceResponseDto()
                    {
                        StatusCode = 400,
                        IsSuccess = false,
                        Message = "Invalid date format. Use 'yyyy-MM-dd' or 'Present' instead",
                    };
                }
                toDate = parsedToDate;
            }

            Experience experience = new Experience()
            {
                JobTitle = addExperienceDto.JobTitle,
                JobDescription = addExperienceDto.JobDescription,
                CompanyName = addExperienceDto.CompanyName,
                From = fromDate,
                To = toDate,
                IsCurrentlyWoring = addExperienceDto.To.ToLower() == "present",
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            await _experienceRepositories.AddExperience(experience);

            return new GeneralServiceResponseDto()
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Experience added successfully.",
            };
        }


        public async Task<GetExperienceDto> GetExperienceByIdAsync(ClaimsPrincipal User, int id)
        {
            var experience = await _experienceRepositories.GetExperienceById(id);
            if(experience is null)
            {
                throw new KeyNotFoundException("Experience doesn't exist");
            }

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUserRole = User.FindFirstValue(ClaimTypes.Role);

            if(experience.UserId != loggedInUserId && loggedInUserRole != StaticUserRole.EMPLOYER)
            {
                throw new UnauthorizedAccessException("You can't access this experience.");
            }

            return _mapper.Map<GetExperienceDto>(experience);
        }

        public async Task<IEnumerable<GetExperienceDto>> GetMyExperiencesAsync(ClaimsPrincipal User)
        {
            var experiences = await _experienceRepositories.GetMyExperiencesAsync(User);
            if(experiences is null)
            {
                throw new Exception("You haven't added you experience yet.");
            }

            return _mapper.Map<IEnumerable<GetExperienceDto>>(experiences);
        }

        public async Task<GeneralServiceResponseDto> UpdateExperienceAsync(ClaimsPrincipal User, AddExperienceDto addExperienceDto, int id)
        {
            if (!DateTime.TryParseExact(addExperienceDto.From, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fromDate))
            {
                return new GeneralServiceResponseDto()
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Message = "Invalid date format. Use yyyy-MM-dd instead",
                };
            }

            DateTime? toDate = null;
            if (!string.IsNullOrEmpty(addExperienceDto.To) && addExperienceDto.To.ToLower() != "present")
            {
                if (!DateTime.TryParseExact(addExperienceDto.To, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedToDate))
                {
                    return new GeneralServiceResponseDto()
                    {
                        StatusCode = 400,
                        IsSuccess = false,
                        Message = "Invalid date format. Use 'yyyy-MM-dd' or 'Present' instead",
                    };
                }
                toDate = parsedToDate;
            }

            var experience = await _experienceRepositories.GetExperienceById(id);

            if(experience is null)
            {
                throw new KeyNotFoundException("Experience doesn't exist");
            }

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(experience.UserId != loggedInUserId)
            {
                throw new UnauthorizedAccessException("You are not authorized to modify this experience.");
            }

            experience.JobTitle = addExperienceDto.JobTitle;
            experience.JobDescription = addExperienceDto.JobDescription;
            experience.CompanyName = addExperienceDto.CompanyName;
            experience.From = fromDate;
            experience.To = toDate;
            experience.IsCurrentlyWoring = addExperienceDto.To.ToLower() == "present";

            //EF Core for updating experience data
            /*_context.Entry(experience).State = EntityState.Modified;
            await _context.SaveChangesAsync();*/

            await _experienceRepositories.UpdateExperienceAsync(experience);

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Experience updated successsfully."
            };
        }

        public async Task<GeneralServiceResponseDto> DeleteExperienceAsync(ClaimsPrincipal User, int id)
        {
            var experience = await _experienceRepositories.GetExperienceById(id);

            if(experience is null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Experience not found."
                };
            }

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(experience.UserId != loggedInUserId)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "You are not authorized to delete this experience."
                };
            }

            await _experienceRepositories.DeleteExperienceAsync(id);

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Experience Deleted Successfully."
            };
        }
    }
}
