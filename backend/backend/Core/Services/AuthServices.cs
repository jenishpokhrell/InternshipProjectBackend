using AutoMapper;
using AutoMapper.QueryableExtensions;
using backend.Core.Constants;
using backend.Core.DataContext;
using backend.Core.DTOs.General;
using backend.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using backend.Core.DTOs.Auth;
using backend.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using backend.Core.Interfaces.IRepositories;

namespace backend.Core.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDBContext _context;
        private readonly ILogServices _logServices;
        private readonly IMapper _mapper;
        private readonly IAuthRepositories _authRepositories;

        public AuthServices(ApplicationDBContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ILogServices logServices, 
            IMapper mapper, IAuthRepositories authRepositories)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logServices = logServices;
            _mapper = mapper;
            _authRepositories = authRepositories;
        }

        //Seeding roles into the database
        public async Task<GeneralServiceResponseDto> SeedRolesAsync()
        {
            bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRole.ADMIN);
            bool isEmployerRoleExists = await _roleManager.RoleExistsAsync(StaticUserRole.EMPLOYER);
            bool isCandidateRoleExists = await _roleManager.RoleExistsAsync(StaticUserRole.CANDIDATE);

            if (isAdminRoleExists && isEmployerRoleExists && isCandidateRoleExists)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "Role Seeding Already Done"
                };
            }

            await _roleManager.CreateAsync(new IdentityRole(StaticUserRole.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRole.EMPLOYER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRole.CANDIDATE));

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Role Seeding Done Successfully"
            };
        }

        //Registering user
        public async Task<GeneralServiceResponseDto> RegisterAsync(RegisterDto registerDto, IFormFile profilePhoto, CloudinaryServices cloudinaryServices)
        {
            var isUserExists = await _userManager.FindByNameAsync(registerDto.Username);
            if (isUserExists is not null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 201,
                    Message = "Username Already Exists"
                };
            }

            string photoUrl = null;
            if(profilePhoto != null)
            {
                photoUrl = await cloudinaryServices.UploadImageAsync(profilePhoto);
            }

            bool isApproved = registerDto.roles == "Candidate" && registerDto.roles == "Admin";

            ApplicationUser newUser = new ApplicationUser()
            {
                Firstname = registerDto.Firstname,
                Lastname = registerDto.Lastname,
                UserName = registerDto.Username,
                Address = registerDto.Address,
                Contact = registerDto.Contact,
                Email = registerDto.Email,
                ProfilePhoto = photoUrl,
                Gender = registerDto.Gender,
                JobTitle = registerDto.JobTitle,
                Years_Of_Experience = registerDto.Years_Of_Experience,
                isApproved = registerDto.roles == "Employer" ? false : true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            if(registerDto.roles != "Employer" && registerDto.roles != "Candidate" && registerDto.roles != "Admin")
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Valid Role Required."
                };
            }

            if (registerDto.Gender != "Male" && registerDto.Gender != "Female" && registerDto.Gender != "Other")
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = true,
                    StatusCode = 201,
                    Message = "Enter valid gender. Male or Female or Other"
                };
            }

            var createUser = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!createUser.Succeeded)
            {
                return new GeneralServiceResponseDto()
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Message = "User Creation Failed"
                };
            }

            var candidateRoleExists = await _roleManager.RoleExistsAsync(StaticUserRole.CANDIDATE);
            var employerRoleExists = await _roleManager.RoleExistsAsync(StaticUserRole.EMPLOYER);
            if (!employerRoleExists && !candidateRoleExists)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "Role Doesn't exist"
                };
            }
            if (createUser.Succeeded)
            {
                if(registerDto.roles == "Employer")
                {
                    await _userManager.AddToRolesAsync(newUser, new List<string> { StaticUserRole.EMPLOYER });
                }
                else if (registerDto.roles == "Candidate")
                {
                    await _userManager.AddToRolesAsync(newUser, new List<string> { StaticUserRole.CANDIDATE });
                }
                else if (registerDto.roles == "Admin")
                {
                    await _userManager.AddToRolesAsync(newUser, new List<string> { StaticUserRole.ADMIN });
                }
            }
            await _logServices.SaveNewLog(newUser.UserName, "Created an account.");

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = registerDto.roles == "Employer" ? "Employer registered successfully. Waiting for admin approval. This may take few minutes."
                : registerDto.roles == "Admin" ? "Admin Created Successfully" :
                "Candidate registered successfully. You can proceed to login."
            };
        }

        //Logging In User
        public async Task<LoginServiceResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user is null)
                return null;

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordCorrect)
                return null;

           

            var newToken = await GenerateJWTToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            var userInfo = GenerateUserInfo(user, roles);

            return new LoginServiceResponseDto()
            {
                NewToken = newToken,
                UserInfo = userInfo
            };

        }

        public async Task<IEnumerable<UserInfo>> GetPendingEmployersAsync()
        {
            var pendingEmployers = await _userManager.Users.Where(u => u.isApproved == false).ToListAsync();
            List<UserInfo> employersInfo = new List<UserInfo>();
            foreach(var employers in pendingEmployers)
            {
                var roles = await _userManager.GetRolesAsync(employers);
                var employersInfoResult = GenerateUserInfo(employers, roles);
                employersInfo.Add(employersInfoResult);
            }
            if (pendingEmployers is null)
            {
                throw new Exception("No pending employers.");
            }

            return _mapper.Map<IEnumerable<UserInfo>>(employersInfo);
        }

        public async Task<GeneralServiceResponseDto> ApproveEmployerAsync(string employerId)
        {
            var employer = await _userManager.FindByIdAsync(employerId);
            if (employer is null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Employer not found."
                };
            }

            if (employer.isApproved != false)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "Employer already approved."
                };
            }

            employer.isApproved = true;
            await _userManager.UpdateAsync(employer);

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Employer approved successfully."
            };

        }

        public async Task<LoginServiceResponseDto> MeAsync(MeDto meDto)
        {
            ClaimsPrincipal handler = new JwtSecurityTokenHandler().ValidateToken(meDto.Token, new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidAudience = _configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))
            }, out SecurityToken securityToken);

            string decodedUserName = handler.Claims.First(q => q.Type == ClaimTypes.Name).Value;

            if(decodedUserName is null)
            {
                return null;
            }

            var user = await _userManager.FindByNameAsync(decodedUserName);
            if (user is null)
                return null;

            var newToken = await GenerateJWTToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            var userInfo = GenerateUserInfo(user, roles);

            return new LoginServiceResponseDto()
            {
                NewToken = newToken,
                UserInfo = userInfo
            };
        }

        //Updating User
        public async Task<GeneralServiceResponseDto> UpdateAsync(ClaimsPrincipal User, UpdateUserDto updateUserDto, string id, IFormFile profilePhoto,
            CloudinaryServices cloudinaryServices)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user is null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "User doesn't exists."
                };
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != user.Id)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 403,
                    Message = "You are not authorized to update this data"
                };
            }

            if (profilePhoto != null)
            {
                if (!string.IsNullOrEmpty(user.ProfilePhoto))
                {
                    await cloudinaryServices.DeleteFileAsync(user.ProfilePhoto);
                }

                user.ProfilePhoto = await cloudinaryServices.UploadImageAsync(profilePhoto);
            }

            user.Firstname = updateUserDto.Firstname;
            user.Lastname = updateUserDto.Lastname;
            user.UserName = updateUserDto.Username;
            user.Address = updateUserDto.Address;
            user.Contact= updateUserDto.Contact;
            user.Email = updateUserDto.Email;
            //user.ProfilePhoto = user.ProfilePhoto;
            user.Gender = updateUserDto.Gender;
            user.JobTitle = updateUserDto.JobTitle;
            user.Years_Of_Experience = updateUserDto.Years_Of_Experience;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Failed to update user " + string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, isPersistent: false);
            await _logServices.SaveNewLog(User.Identity.Name, "Updated Their Profile");

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Profile Updated Successfully"
            };
        }

        //Change Password
        public async Task<GeneralServiceResponseDto> ChangePasswordAsync(ClaimsPrincipal User, PasswordDto passwordDto, string id)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(id);

            if(user is null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "User doesn't exist."
                };
            }

            if(user.Id != loggedInUserId)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "You're not authorized to change the password of other users."
                };
            }

            var result = await _userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);
            if (!result.Succeeded)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = string.Join(", ",result.Errors.Select(cp => cp.Description))
                };
            }

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, isPersistent: false);
            await _logServices.SaveNewLog(User.Identity.Name, "Changed their password.");

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Password Updated Successfully"
            };
        }

        //Getting all users data
        public async Task<IEnumerable<UserInfo>> GetAllUser()
        {
            var users = await _authRepositories.GetAllUsers();
            List<UserInfo> userInfo = new List<UserInfo>();
            foreach(var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userInfoResult = GenerateUserInfo(user, roles);
                userInfo.Add(userInfoResult);
            }
            return userInfo;
        }

        //Getting user by id
        public async Task<UserInfo> GetUserByIdAsync(ClaimsPrincipal User,string id)
        {
            var loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUserRole = User.FindFirstValue(ClaimTypes.Role);
            var user = await _authRepositories.GetUserById(id);
            if(user is null)
            {
                throw new Exception("User not found");
            }
            var roles = await _userManager.GetRolesAsync(user);
            if(roles.Contains(StaticUserRole.ADMIN) || roles.Contains(loggedInUserRole)  && loggedInUser != user.Id)
            {
                throw new UnauthorizedAccessException("You are not authorized to access this data");
            }
            var userInfo = GenerateUserInfo(user, roles);
            return userInfo;
        }

        //Delete User
        public async Task<GeneralServiceResponseDto> DeleteUserAsync(ClaimsPrincipal User, string id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = await _authRepositories.GetUserById(id);

                    if (user is null)
                    {
                        throw new KeyNotFoundException("User not found.");
                    }

                    var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if(loggedInUserId != user.Id)
                    {
                        throw new UnauthorizedAccessException("You are not authorized to delete this user.");
                    }

                    var jobs = await _context.Jobs.Where(c => c.EmployerId == id).ToListAsync();
                    var savedCandidates = await _context.SavedCandidates.Where(sc => sc.EmployerId == id && sc.CandidateId == id).ToListAsync();
                    var projects = await _context.Projects.Where(c => c.CandidateId == id).ToListAsync();
                    var academic = await _context.Academics.Where(c => c.CandidateId == id).FirstOrDefaultAsync();
                    var experiences = await _context.Experiences.Where(e => e.UserId == id).ToListAsync();
                    var jobApplications = await _context.JobApplications.Where(ja => ja.CandidateId == id).ToListAsync();
                    var resumes = await _context.Resumes.Where(r => r.CandidateId == id).FirstOrDefaultAsync();
                    var candidateSkills = await _context.CandidateSkills.Where(cs => cs.CandidateId == id).ToListAsync();
                    var savedJobs = await _context.SavedJobs.Where(sj => sj.CandidateId == id).ToListAsync();

                    _context.Users.Remove(user);
                    _context.Jobs.RemoveRange(jobs);
                    _context.Projects.RemoveRange(projects);
                    _context.Experiences.RemoveRange(experiences);
                    _context.JobApplications.RemoveRange(jobApplications);
                    _context.CandidateSkills.RemoveRange(candidateSkills);
                    _context.SavedJobs.RemoveRange(savedJobs);
                    if(academic != null)
                    {
                        _context.Academics.Remove(academic);
                    }

                    if(resumes != null)
                    {
                        _context.Resumes.Remove(resumes);   
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new GeneralServiceResponseDto()
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Message = "User removed successfully."
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new GeneralServiceResponseDto()
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Message = "An error occured while deleting a user" + ex
                    };
                }

            }
        }

        //Generating jwt token
        private async Task<string> GenerateJWTToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            if (!userRoles.Contains("ADMIN"))
            {
                if (!string.IsNullOrEmpty(user.Firstname))
                    authClaims.Add(new Claim("Firstname", user.Firstname));

                if (!string.IsNullOrEmpty(user.Lastname))
                    authClaims.Add(new Claim("Lastname", user.Lastname));
            }

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var signingCredentials = new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256);

            var tokenObject = new JwtSecurityToken(
                issuer : _configuration["JWT:ValidIssuer"],
                audience : _configuration["JWT:ValidAudience"],
                notBefore : DateTime.Now,
                expires : DateTime.Now.AddHours(3),
                claims : authClaims,
                signingCredentials : signingCredentials
            );
            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
            return token;

        }

        //Generating user info to return after user logs in
        private UserInfo GenerateUserInfo(ApplicationUser User, IEnumerable<string> Roles)
        {
            var userInfo = _mapper.Map<UserInfo>(User);
            userInfo.Roles = Roles.ToList();
            return userInfo;
        }

    }
}
