using backend.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.DataContext
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<Admin> Admin { get; set; }
        public DbSet<Academic> Academics { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobApplication> JobApplications{ get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<SavedCandidate> SavedCandidates { get; set; }
        public DbSet<SavedJob> SavedJobs { get; set; }
        public DbSet<CandidateSkill> CandidateSkills { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(e =>
            {
                e.ToTable("Users");
            });

            builder.Entity<IdentityUserClaim<string>>(e =>
            {
                e.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(e =>
            {
                e.ToTable("UserLogins");
            });
   
            builder.Entity<IdentityUserToken<string>>(e =>
            {
                e.ToTable("UserTokens");
            });

            builder.Entity<IdentityRole>(e =>
            {
                e.ToTable("Roles");
            });

            builder.Entity<IdentityRoleClaim<string>>(e =>
            {
                e.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserRole<string>>(e =>
            {
                e.ToTable("UserRoles")
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            });

        }
    }
}
