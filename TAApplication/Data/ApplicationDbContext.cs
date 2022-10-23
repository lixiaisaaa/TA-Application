using Microsoft.AspNetCore.Http;
﻿/**
* Author:    Xia Li
* Partner:   Wenlin Li
* Date:      10/17/2022
* Course:    CS 4540, University of Utah, School of Computing
* Copyright: CS 4540 and Xia Li and Wenlin Li - This work may not be copied for use in Academic Coursework.
*
* I, Xia Li and Wenlin Li, certify that I wrote this code from scratch and did 
* not copy it in part or whole from another source.  Any references used 
* in the completion of the assignment are cited in my README file and in
* the appropriate method header.
*
* File Contents
*
* This db file controller for db.
*/
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Security.Claims;
using TAApplication.Areas.Data;
using TAApplication.Models;
using static System.Net.Mime.MediaTypeNames;
using Application = TAApplication.Models.Application;

namespace TAApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<TAUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Every time Save Changes is called, add timestamps
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()  // JIM: Override save changes to add timestamps
        {
            AddTimestamps();
            return base.SaveChanges();
        }
        /// <summary>
        /// Every time Save Changes (Async) is called, add timestamps
        /// </summary>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            AddTimestamps();   // JIM: Override save changes async to add timestamps
            return await base.SaveChangesAsync(cancellationToken);
        }
        /// <summary>
        /// JIM: this code adds time/user to DB entry
        /// 
        /// Check the DB tracker to see what has been modified, and add timestamps/names as appropriate.
        /// 
        /// </summary>
        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is ModificationTracking
                        && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var currentUsername = "";

            if (_httpContextAccessor.HttpContext == null) // happens during startup/initialization code
            {
                currentUsername = "DBSeeder";
            }
            else
            {
                currentUsername = _httpContextAccessor.HttpContext.User.Identity?.Name;
            }

            currentUsername ??= "Sadness"; // JIM: compound assignment magic... test for null, and if so, assign value

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((ModificationTracking)entity.Entity).CreationDate = DateTime.UtcNow;
                    ((ModificationTracking)entity.Entity).CreatedBy = currentUsername;
                }
                ((ModificationTracking)entity.Entity).ModificationDate = DateTime.UtcNow;
                ((ModificationTracking)entity.Entity).ModifiedBy = currentUsername;
            }
        }

        public async Task InitializeUsers(UserManager<TAUser> u, RoleManager<IdentityRole> r)
        {
            Database.Migrate();
            if (u.Users.ToArray().Length > 0) {
                return;
            }

            IdentityRole Admin = new IdentityRole("Admin");
            await r.CreateAsync(Admin);
            IdentityRole Professor = new IdentityRole("professor");
            await r.CreateAsync(Professor);
            IdentityRole Applicant = new IdentityRole("Applicant");
            await r.CreateAsync(Applicant);


            var user = new TAUser { UserName = "admin@utah.edu", Unid = "u1234567", Name = "admin", EmailConfirmed = true, RefferedTo = "" };
            await u.CreateAsync(user, "123ABC!@#def");
            await u.AddToRoleAsync(user, "Admin");

            var prof = new TAUser { UserName = "professor@utah.edu", Unid = "u7654321", Name = "professor", EmailConfirmed = true, RefferedTo = "" };
            await u.CreateAsync(prof, "123ABC!@#def");
            await u.AddToRoleAsync(prof, "professor");

            var s = new TAUser { UserName = "u0000000@utah.edu", Unid = "u0000000", Name = "u0", EmailConfirmed = true, RefferedTo = "" };
            await u.CreateAsync(s, "123ABC!@#def");
            await u.AddToRoleAsync(s, "Applicant");

            var s1 = new TAUser { UserName = "u0000001@utah.edu", Unid = "u0000001", Name = "u1", EmailConfirmed = true, RefferedTo = "" };
            await u.CreateAsync(s1, "123ABC!@#def");
            await u.AddToRoleAsync(s1, "Applicant");

            var s2 = new TAUser { UserName = "u0000002@utah.edu", Unid = "u0000002", Name = "u2", EmailConfirmed = true, RefferedTo = "" };
            await u.CreateAsync(s2, "123ABC!@#def");
            await u.AddToRoleAsync(s2, "Applicant");

            Application a = new() { Pursuing = Pursuing.MS, GPA = 4, Department = "CS", avaiableBefore = true, numberOfHour = 10 ,SemestersCount = 10, PersonalStatement = "hello" ,TransferSchool = "SLCC",LinkedinURL="https://google.com",User = s, resumePath= "fbabecad-3a29-4a4c-866c-5692c854810f_Wenlin_Li_Resume.pdf", photoPath= "f0550039-4282-4685-9205-edd8915198b0_名片 2022年8月22日-1.jpg" };
            Add(a);

            Application a1 = new() { Pursuing = Pursuing.MS, GPA = 4, Department = "CS", avaiableBefore = true, numberOfHour = 10, SemestersCount = 10, User = s1 };
            Add(a1);

            Course c1 = new() { Semester = "Spring", Year = 2023, CourseNumber = 1400, note = "Need 2 TAs" };
            Add(c1);

            Course c2 = new() { Semester = "Spring", Year = 2023, CourseNumber = 1420, note = "Need 2 TAs" };
            Add(c2);

            Course c3 = new() { Semester = "Spring", Year = 2023, CourseNumber = 2420, note = "Need 2 TAs" };
            Add(c3);

            Course c4 = new() { Semester = "Spring", Year = 2023, CourseNumber = 4150, note = "Need 2 TAs" };
            Add(c4);

            Course c5 = new() { Semester = "Spring", Year = 2023, CourseNumber = 4400, note = "Need 2 TAs" };
            Add(c5);
            SaveChanges();

        }
        public DbSet<TAApplication.Models.Application> Application { 
            get; set; }
        public DbSet<TAApplication.Models.Course> Course { get; set; }
    }
}