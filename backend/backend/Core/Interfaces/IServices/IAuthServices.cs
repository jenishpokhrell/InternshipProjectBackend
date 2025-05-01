using backend.Core.DTOs.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Core.DTOs.Auth;
using backend.Core.Services;
using Microsoft.AspNetCore.Http;
using backend.Helpers;

namespace backend.Core.Interfaces
{
    public interface IAuthServices
    {
        Task<GeneralServiceResponseDto> SeedRolesAsync();

        Task<GeneralServiceResponseDto> RegisterAsync(RegisterDto registerDto, IFormFile profilePhoto, CloudinaryServices cloudinaryServices);

        Task<LoginServiceResponseDto> LoginAsync(LoginDto loginDto);

        Task<IEnumerable<UserInfo>> GetPendingEmployersAsync();

        Task<GeneralServiceResponseDto> ApproveEmployerAsync(string employerId);

        Task<LoginServiceResponseDto> MeAsync(MeDto meDto);

        Task<GeneralServiceResponseDto> UpdateAsync(ClaimsPrincipal User, UpdateUserDto updateUserDto, string id, IFormFile profilePhoto,
            CloudinaryServices cloudinaryServices);

        Task<GeneralServiceResponseDto> ChangePasswordAsync(ClaimsPrincipal User, PasswordDto passwordDto, string id);

        Task<IEnumerable<UserInfo>> GetAllUser();

        Task<UserInfo> GetUserByIdAsync(ClaimsPrincipal User,string id);

        Task<GeneralServiceResponseDto> DeleteUserAsync(ClaimsPrincipal User, string id);
    }
}
