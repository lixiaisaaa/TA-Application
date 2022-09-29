using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TAApplication.Areas.Data;

namespace TAApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<TAUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
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
        }
    }
}