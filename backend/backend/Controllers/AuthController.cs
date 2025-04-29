using backend.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.DTOs.Auth;
using backend.Core.Interfaces;
using backend.Core.Services;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost]
        [Route("Seed-Roles")]
        public async Task<IActionResult> SeedRoles()
        {
            var seedRoles = await _authServices.SeedRolesAsync();
            return StatusCode(seedRoles.StatusCode, seedRoles.Message);
        }


        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> RegisterAync([FromForm] RegisterDto registerDto,
            [FromForm] IFormFile profilePhoto,
            [FromServices] CloudinaryServices cloudinaryServices)
        {
            var createUser = await _authServices.RegisterAsync(registerDto, profilePhoto, cloudinaryServices);
            return Ok(createUser);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            var user = await _authServices.LoginAsync(loginDto);
            if(user is null)
            {
                return Unauthorized("Invalid Credentials.");
            }

            if (user is not null && user.UserInfo.isApproved == false)
            {
                return StatusCode(403, new { message = "You have not been approved by admin yet." });
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("ApproveEmployer/{employerId}")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<IActionResult> ApproveEmployerAsync(string employerId)
        {
            var approveEmployer = await _authServices.ApproveEmployerAsync(employerId);
            return Ok(approveEmployer);
        }

        [HttpPost]
        [Route("Me")]
        public async Task<IActionResult> Me([FromBody] MeDto meDto)
        {
            try
            {
                var me = await _authServices.MeAsync(meDto);
                if(me is null)
                {
                    return Unauthorized("Invalid Token");
                }
                else
                {
                    return Ok(me);
                }
            }
            catch(Exception)
            {
                return Unauthorized("Invalid Token");
            }

        }

        [HttpGet]
        [Route("GetAllUsers")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetAllUsers()
        {
            var users = await _authServices.GetAllUser();
            return Ok(users);
        }

        [HttpGet]
        [Route("GetUserById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _authServices.GetUserByIdAsync(User, id);
            return Ok(user);
        }

        [HttpGet]
        [Route("GetPendingEmployers")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<IActionResult> GetPendingEmployersAsync()
        {
            var employers = await _authServices.GetPendingEmployersAsync();
            return Ok(employers);
        }

        [HttpPut]
        [Route("Update-User/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(string id,
                    [FromForm] UpdateUserDto updateUserDto,
                    [FromForm] IFormFile profilePhoto,
                    [FromServices] CloudinaryServices cloudinaryServices)
        {
            var updateUser = await _authServices.UpdateAsync(User, updateUserDto, id, profilePhoto, cloudinaryServices);
            if (!updateUser.IsSuccess)
            {
                return BadRequest();
            }
            return Ok(updateUser);
        }

       

        [HttpPut]
        [Route("ChangePassword/{id}")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordDto passwordDto, string id)
        {
            var changePassword = await _authServices.ChangePasswordAsync(User, passwordDto, id);
            return Ok(changePassword);
        }


        [HttpDelete]
        [Route("DeleteUser/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _authServices.DeleteUserAsync(User, id);
            return StatusCode(user.StatusCode, user.Message);
        }
    }
}
