using backend.Core.DataContext;
using backend.Core.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using CloudinaryDotNet;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using backend.Core.Interfaces;
using backend.Core.Services;
using backend.Core.Interfaces.IRepositories;
using backend.Repositories;
using backend.Core.Interfaces.IServices;
using backend.Core.Repositories;
using backend.Helpers;

namespace backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
       

            services.Configure<CloudinarySettings>(Configuration.GetSection("Cloudinary"));

            var cloudinarySettings = Configuration.GetSection("Cloudinary").Get<CloudinarySettings>();
            var account = new Account(cloudinarySettings.CloudName, cloudinarySettings.ApiKey, cloudinarySettings.ApiSecret);
            var cloudinary = new Cloudinary(account);

            services.AddSingleton(cloudinary);

            //Adding Dependency Injection
            services.AddScoped<IAcademicServices, AcademicServices>();
            services.AddScoped<IAcademicRepositories, AcademicRepositories>();

            services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<IAuthRepositories, AuthRepositories>();

            services.AddScoped<ILogServices, LogServices>();
            services.AddScoped<ILogRepositories, LogRepositories>();

            services.AddScoped<IJobServices, JobServices>();
            services.AddScoped<IJobRepositories, JobRepositories>();

            services.AddScoped<IProjectServices, ProjectServices>();
            services.AddScoped<IProjectRepositories, ProjectRepositories>();

            services.AddScoped<IExperienceServices, ExperienceServices>();
            services.AddScoped<IExperienceRepositories, ExperienceRepositories>();

            services.AddScoped<IJobApplicationServices, JobApplicationServices>();
            services.AddScoped<IJobApplicationRepositories, JobApplicationRepositories>();

            services.AddScoped<ISavedCandidateServices, SavedCandidateServices>();
            services.AddScoped<ISavedCandidateRepositories, SavedCandidateRepositories>();

            services.AddScoped<ISavedJobServices, SavedJobServices>();
            //services.AddScoped<ISavedJobRepositories, SavedJobRepositories>();

            services.AddScoped<ISkillServices, SkillServices>();
            services.AddScoped<ISkillRepositories, SkillRepositories>();

            services.AddScoped<IResumeServices, ResumeServices>();
            services.AddScoped<IResumeRepositories, ResumeRepositories>();

            services.AddScoped<ICandidateSkillServices, CandidateSkillServices>();
            services.AddScoped<ICandidateSkillRepositories, CandidateSkillRepositories>();

            services.AddSingleton<CloudinaryServices>();
            services.AddSingleton<DapperContext>();
            services.AddScoped<GenerateJWTToken>();

            services.AddAutoMapper(typeof(Startup));

            //Configuring Identity
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
            });

            //Adding Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDBContext>()
            .AddDefaultTokenProviders();


            //Adding authentication schema and JWT Bearer
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"])),
                    
                };
            });

            services.AddControllers().AddJsonOptions(options => {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "backend", Version = "v1" });

                var fileParam = new OpenApiParameter
                {
                    Name = "profilePhoto",
                    In = ParameterLocation.Query,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary"
                    }
                };

                var resumeFileParam = new OpenApiParameter
                {
                    Name = "resumeFile",
                    In = ParameterLocation.Query,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary"
                    }
                };


                //Allowing Swagger to authenticate users using JWT 
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Please enter you token in following format = 'Bearer YOUR_TOKEN'",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                //Applying Security Requirement to swagger
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "backend v1"));
            }

            app.UseCors(options =>
            {
                options
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
            }
);
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
