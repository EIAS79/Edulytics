using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Data.Contexts;
using Edulytics.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Web.Bootstrap;

internal static class PresentationDemoProvisioner
{
    private const string StagingServiceId = "srv-da1o4url550s73aecsn0";
    private const string DemoSchoolCode = "EDULYTICS-DEMO";

    private static readonly (string Email, string Role)[] DemoUsers =
    [
        ("demo.admin@edulytiks.com", RoleNames.SchoolAdmin),
        ("demo.supervisor@edulytiks.com", RoleNames.SubjectSupervisor),
        ("demo.teacher@edulytiks.com", RoleNames.Teacher),
        ("demo.student@edulytiks.com", RoleNames.Student)
    ];

    public static async Task RunAsync(
        EdulyticsDbContext db,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        if (!string.Equals(
                Environment.GetEnvironmentVariable("RENDER_SERVICE_ID"),
                StagingServiceId,
                StringComparison.Ordinal))
        {
            return;
        }

        if (!bool.TryParse(
                configuration["Edulytics:PresentationDemo:Provision"],
                out var enabled) ||
            !enabled)
        {
            return;
        }

        var password =
            configuration["Edulytics:PresentationDemo:Password"];

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Presentation demo provisioning was enabled without a demo password.");
        }

        Console.WriteLine("PRESENTATION_DEMO_PROVISION_BEGIN");

        await using var transaction =
            await db.Database.BeginTransactionAsync();

        try
        {
            var school = await EnsureDemoSchoolAsync(db);

            var createdUsers = new Dictionary<string, ApplicationUser>(
                StringComparer.OrdinalIgnoreCase);

            foreach (var definition in DemoUsers)
            {
                var user = await EnsureDemoUserAsync(
                    userManager,
                    school.Id,
                    definition.Email,
                    definition.Role,
                    password);

                createdUsers[definition.Role] = user;

                Console.WriteLine(
                    $"PRESENTATION_DEMO_USER_VERIFIED role={definition.Role} email={definition.Email}");
            }

            await EnsureStudentProfileAsync(
                db,
                school.Id,
                createdUsers[RoleNames.Student].Id);

            await transaction.CommitAsync();

            Console.WriteLine(
                $"PRESENTATION_DEMO_PROVISION_COMMITTED schoolId={school.Id:D} users=4 studentProfiles=1");
        }
        catch
        {
            await transaction.RollbackAsync();
            Console.WriteLine("PRESENTATION_DEMO_PROVISION_ROLLED_BACK");
            throw;
        }
    }

    private static async Task<School> EnsureDemoSchoolAsync(
        EdulyticsDbContext db)
    {
        var school = await db.Schools.SingleOrDefaultAsync(
            x => x.NormalizedSchoolCode == DemoSchoolCode);

        var now = DateTime.UtcNow;

        if (school is null)
        {
            school = new School
            {
                Id = Guid.NewGuid(),
                Name = "Edulytics Demo School",
                SchoolCode = DemoSchoolCode,
                NormalizedSchoolCode = DemoSchoolCode,
                Status = SchoolStatus.Active,
                CountryCode = "PL",
                City = "Warsaw",
                ContactEmail = "demo@edulytiks.com",
                DefaultCulture = "en",
                TimeZoneId = "Europe/Warsaw",
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
                ArchivedAtUtc = null,
                RowVersion = []
            };

            db.Schools.Add(school);
            await db.SaveChangesAsync();
        }
        else
        {
            school.Name = "Edulytics Demo School";
            school.Status = SchoolStatus.Active;
            school.CountryCode = "PL";
            school.City = "Warsaw";
            school.ContactEmail = "demo@edulytiks.com";
            school.DefaultCulture = "en";
            school.TimeZoneId = "Europe/Warsaw";
            school.ArchivedAtUtc = null;
            school.UpdatedAtUtc = now;
            await db.SaveChangesAsync();
        }

        Console.WriteLine(
            $"PRESENTATION_DEMO_SCHOOL_VERIFIED schoolId={school.Id:D} code={DemoSchoolCode}");

        return school;
    }

    private static async Task<ApplicationUser> EnsureDemoUserAsync(
        UserManager<ApplicationUser> userManager,
        Guid schoolId,
        string email,
        string role,
        string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        var now = DateTime.UtcNow;

        if (user is null)
        {
            user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                IsActive = true,
                SchoolId = schoolId,
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
                LockoutEnd = null,
                AccessFailedCount = 0
            };

            var create = await userManager.CreateAsync(user, password);
            EnsureIdentitySucceeded(create, $"create {email}");
        }
        else
        {
            if (user.SchoolId != schoolId)
            {
                throw new InvalidOperationException(
                    $"Demo account '{email}' already exists outside the demo school.");
            }

            user.UserName = email;
            user.Email = email;
            user.EmailConfirmed = true;
            user.IsActive = true;
            user.LockoutEnd = null;
            user.AccessFailedCount = 0;
            user.UpdatedAtUtc = now;

            var update = await userManager.UpdateAsync(user);
            EnsureIdentitySucceeded(update, $"update {email}");

            var resetToken =
                await userManager.GeneratePasswordResetTokenAsync(user);
            var reset =
                await userManager.ResetPasswordAsync(
                    user,
                    resetToken,
                    password);
            EnsureIdentitySucceeded(reset, $"reset password for {email}");
        }

        var currentRoles = await userManager.GetRolesAsync(user);

        foreach (var currentRole in currentRoles)
        {
            if (!string.Equals(currentRole, role, StringComparison.Ordinal))
            {
                var remove =
                    await userManager.RemoveFromRoleAsync(
                        user,
                        currentRole);
                EnsureIdentitySucceeded(
                    remove,
                    $"remove role {currentRole} from {email}");
            }
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            var add = await userManager.AddToRoleAsync(user, role);
            EnsureIdentitySucceeded(add, $"add role {role} to {email}");
        }

        return user;
    }

    private static async Task EnsureStudentProfileAsync(
        EdulyticsDbContext db,
        Guid schoolId,
        Guid studentUserId)
    {
        var profile = await db.StudentProfiles.SingleOrDefaultAsync(
            x => x.UserId == studentUserId);

        var now = DateTime.UtcNow;

        if (profile is null)
        {
            profile = new StudentProfile
            {
                Id = Guid.NewGuid(),
                SchoolId = schoolId,
                UserId = studentUserId,
                StudentNumber = "DEMO-STU-001",
                NormalizedStudentNumber = "DEMO-STU-001",
                FirstName = "Alex",
                LastName = "Morgan",
                DisplayName = "Alex Morgan",
                Status = AcademicStructureStatus.Active,
                IsArchived = false,
                ArchivedAtUtc = null,
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
                RowVersion = []
            };

            db.StudentProfiles.Add(profile);
        }
        else
        {
            if (profile.SchoolId != schoolId)
            {
                throw new InvalidOperationException(
                    "The demo student profile is linked to a different school.");
            }

            profile.StudentNumber = "DEMO-STU-001";
            profile.NormalizedStudentNumber = "DEMO-STU-001";
            profile.FirstName = "Alex";
            profile.LastName = "Morgan";
            profile.DisplayName = "Alex Morgan";
            profile.Status = AcademicStructureStatus.Active;
            profile.IsArchived = false;
            profile.ArchivedAtUtc = null;
            profile.UpdatedAtUtc = now;
        }

        await db.SaveChangesAsync();
    }

    private static void EnsureIdentitySucceeded(
        IdentityResult result,
        string operation)
    {
        if (result.Succeeded)
        {
            return;
        }

        throw new InvalidOperationException(
            $"Presentation demo provisioning failed to {operation}: " +
            string.Join(
                "; ",
                result.Errors.Select(x => x.Description)));
    }
}
