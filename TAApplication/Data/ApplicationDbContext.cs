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
using FileHelpers;
using System;
using Xunit;
using System.Text;
using System.Web;
using Microsoft.VisualBasic.FileIO;
using ZendeskApi_v2.Models.Shared;

namespace TAApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<TAUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _environment;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
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

            Application a = new() {Pursuing = Pursuing.MS, GPA = 4, Department = "CS", avaiableBefore = true, numberOfHour = 10 ,SemestersCount = 10, PersonalStatement = "hello" ,TransferSchool = "SLCC",LinkedinURL="https://google.com",User = s, resumePath= "fbabecad-3a29-4a4c-866c-5692c854810f_Wenlin_Li_Resume.pdf", photoPath= "f0550039-4282-4685-9205-edd8915198b0_名片 2022年8月22日-1.jpg" };
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

            Slot slot = new Slot() {User = s, IsActive = true, time = "Monday 8:00am Monday 8:15am Monday 8:30am Monday 8:45am Monday 9:00am Monday 9:15am Monday 9:30am Monday 9:45am Monday 10:00am Monday 10:15am Monday 10:30am Monday 10:45am Monday 11:00am Monday 11:15am Monday 11:30am Monday 11:45am Friday 8:00am Friday 8:15am Friday 8:30am Friday 8:45am Friday 9:00am Friday 9:15am Friday 9:30am Friday 9:45am Friday 10:00am Friday 10:15am Friday 10:30am Friday 10:45am Friday 11:00am Friday 11:15am Friday 11:30am Friday 11:45am Tuesday 12:00pm Tuesday 12:15pm Tuesday 12:30pm Tuesday 12:45pm Tuesday 1:00pm Tuesday 1:15pm Tuesday 1:30pm Tuesday 1:45pm Tuesday 2:00pm Tuesday 2:15pm Tuesday 2:30pm Tuesday 2:45pm Tuesday 3:00pm Tuesday 3:15pm Tuesday 3:30pm Tuesday 3:45pm Tuesday 4:00pm Tuesday 4:15pm Tuesday 4:30pm Tuesday 4:45pm Thursday 12:00pm Thursday 12:15pm Thursday 12:30pm Thursday 12:45pm Thursday 1:00pm Thursday 1:15pm Thursday 1:30pm Thursday 1:45pm Thursday 2:00pm Thursday 2:15pm Thursday 2:30pm Thursday 2:45pm Thursday 3:00pm Thursday 3:15pm Thursday 3:30pm Thursday 3:45pm Thursday 4:00pm Thursday 4:15pm Thursday 4:30pm Thursday 4:45pm" , timeArray = "50 50#50 60#50 70#50 80#50 90#50 100#50 110#50 120#50 130#50 140#50 150#50 160#50 170#50 180#50 190#50 200#570 50#570 60#570 70#570 80#570 90#570 100#570 110#570 120#570 130#570 140#570 150#570 160#570 170#570 180#570 190#570 200#180 210#180 220#180 230#180 240#180 250#180 260#180 270#180 280#180 290#180 300#180 310#180 320#180 330#180 340#180 350#180 360#180 370#180 380#180 390#180 400#440 210#440 220#440 230#440 240#440 250#440 260#440 270#440 280#440 290#440 300#440 310#440 320#440 330#440 340#440 350#440 360#440 370#440 380#440 390#440 400#"};
            Add(slot);

            string Banner1ImagePath = Path.Combine(_environment.ContentRootPath, @"wwwroot\temp.csv");
            using (TextFieldParser textFieldParser = new TextFieldParser(Banner1ImagePath))
            {
                textFieldParser.TextFieldType = FieldType.Delimited;
                textFieldParser.SetDelimiters(",");
                int index = 0;
                string[] date = null;
                while (!textFieldParser.EndOfData)
                {
                    if (index == 0)
                    {
                        date = textFieldParser.ReadFields();
                    }
                    else
                    {
                        string[] rows = textFieldParser.ReadFields();
                        if (rows is not null && date is not null)
                        {
                            string course = rows[0];
                            for (int i = 1; i < rows.Length; i++)
                            {
                                Enrollment e = new Enrollment(){ Course = course, Date = date[i], enrollment = Int32.Parse(rows[i]) };
                                Add(e);
                            }
                        }
                    }
                    index++;
                }
            }

            SaveChanges();

        }

        public async Task addEnrollments() {
            string Banner1ImagePath = Path.Combine(_environment.ContentRootPath, @"wwwroot\temp.csv");
            using (TextFieldParser textFieldParser = new TextFieldParser(Banner1ImagePath))
            {
                textFieldParser.TextFieldType = FieldType.Delimited;
                textFieldParser.SetDelimiters(",");
                int index = 0;
                string[] date = null;
                while (!textFieldParser.EndOfData)
                {
                    if (index == 0)
                    {
                        date = textFieldParser.ReadFields();
                    }
                    else {
                        string[] rows = textFieldParser.ReadFields();
                        if (rows is not null && date is not null)
                        {
                            string course = rows[0];
                            for (int i = 1; i < rows.Length; i++)
                            {
                                Enrollment e = new Enrollment() { Course = course, Date = date[i], enrollment = Int32.Parse(rows[i]) };
                                Add(e);
                            }
                        }
                    }
                    index++;
                }
            }
        }


        public DbSet<TAApplication.Models.Application> Application { 
            get; set; }
        public DbSet<TAApplication.Models.Course> Course { get; set; }
        public DbSet<TAApplication.Models.Slot> Slot { get; set; }
        public DbSet<TAApplication.Models.Enrollment> Enrollment { get; set; }
    }
}