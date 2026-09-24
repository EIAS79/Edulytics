using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Data.Contexts;
using Edulytics.Data.Identity;
using Edulytics.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Edulytics.Tests.Phase05;

public sealed class IdentitySchoolUserRepositoryTests
{
    [Fact]
    public async Task CreateAsync_PersistsSchoolRoleAndSetupToken()
    {
        using var provider = BuildProvider();

        using var scope =
            provider.CreateScope();

        var services =
            scope.ServiceProvider;

        var context =
            services.GetRequiredService<
                EdulyticsDbContext>();

        var roleManager =
            services.GetRequiredService<
                RoleManager<ApplicationRole>>();

        var userManager =
            services.GetRequiredService<
                UserManager<ApplicationUser>>();

        foreach (var role in new[]
                 {
                     RoleNames.SchoolAdmin,
                     RoleNames.SubjectSupervisor,
                     RoleNames.Teacher,
                     RoleNames.Student
                 })
        {
            await roleManager.CreateAsync(
                new ApplicationRole
                {
                    Name = role
                });
        }

        var school = NewSchool();

        context.Schools.Add(school);
        await context.SaveChangesAsync();

        var repository =
            new IdentitySchoolUserRepository(
                userManager,
                roleManager,
                context);

        var result =
            await repository.CreateAsync(
                school.Id,
                "teacher@example.com",
                RoleNames.Teacher);

        Assert.True(result.Succeeded);
        Assert.NotNull(result.User);

        Assert.Equal(
            school.Id,
            result.User!.SchoolId);

        Assert.Equal(
            RoleNames.Teacher,
            Assert.Single(result.User.Roles));

        Assert.False(
            string.IsNullOrWhiteSpace(
                result.PasswordSetupToken));

        var persisted =
            await userManager.FindByEmailAsync(
                "teacher@example.com");

        Assert.NotNull(persisted);
        Assert.False(
            await userManager.HasPasswordAsync(
                persisted!));
    }

    [Fact]
    public async Task QueryBySchoolAsync_FiltersRolesAndPagesOnTheServerContract()
    {
        using var provider = BuildProvider();
        using var scope = provider.CreateScope();

        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<EdulyticsDbContext>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in new[]
                 {
                     RoleNames.SchoolAdmin,
                     RoleNames.Teacher,
                     RoleNames.Student
                 })
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = role });
        }

        var school = NewSchool();
        context.Schools.Add(school);
        await context.SaveChangesAsync();

        var repository = new IdentitySchoolUserRepository(
            userManager,
            roleManager,
            context);

        await repository.CreateAsync(
            school.Id,
            "teacher-a@example.com",
            RoleNames.Teacher);
        await repository.CreateAsync(
            school.Id,
            "teacher-b@example.com",
            RoleNames.Teacher);
        await repository.CreateAsync(
            school.Id,
            "student@example.com",
            RoleNames.Student);

        var secondTeacherPage = await repository.QueryBySchoolAsync(
            school.Id,
            new Edulytics.Core.Users.SchoolUserListQuery(
                Role: RoleNames.Teacher,
                Page: 2,
                PageSize: 1));

        Assert.Equal(2, secondTeacherPage.TotalCount);
        Assert.Equal(2, secondTeacherPage.Page);
        Assert.Equal(1, secondTeacherPage.PageSize);
        Assert.Equal(
            "teacher-b@example.com",
            Assert.Single(secondTeacherPage.Users).Email);

        var searched = await repository.QueryBySchoolAsync(
            school.Id,
            new Edulytics.Core.Users.SchoolUserListQuery(
                Search: "student@",
                Page: 99,
                PageSize: 25));

        Assert.Equal(1, searched.TotalCount);
        Assert.Equal(1, searched.Page);
        Assert.Equal(
            RoleNames.Student,
            Assert.Single(Assert.Single(searched.Users).Roles));
    }

    [Fact]
    public async Task QueryBySchoolAsync_FiltersByIdentityAndAcademicContext()
    {
        using var provider = BuildProvider();
        using var scope = provider.CreateScope();

        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<EdulyticsDbContext>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in new[]
                 {
                     RoleNames.Teacher,
                     RoleNames.Student
                 })
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = role });
        }

        var school = NewSchool();
        context.Schools.Add(school);
        await context.SaveChangesAsync();

        var repository = new IdentitySchoolUserRepository(
            userManager,
            roleManager,
            context);

        var teacherResult = await repository.CreateAsync(
            school.Id,
            "teacher.stage6@example.com",
            RoleNames.Teacher);
        var studentResult = await repository.CreateAsync(
            school.Id,
            "alice.student@example.com",
            RoleNames.Student);

        Assert.NotNull(teacherResult.User);
        Assert.NotNull(studentResult.User);

        var now = DateTime.UtcNow;
        var year = new AcademicYear
        {
            Id = Guid.NewGuid(),
            SchoolId = school.Id,
            Name = "2026/2027",
            StartsOn = new DateOnly(2026, 9, 1),
            EndsOn = new DateOnly(2027, 6, 30),
            Status = AcademicStructureStatus.Active,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };
        var program = new AcademicProgram
        {
            Id = Guid.NewGuid(),
            SchoolId = school.Id,
            Name = "British Programme",
            Code = "BRITISH",
            NormalizedCode = "BRITISH",
            Status = AcademicStructureStatus.Active,
            IsDefault = false,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };
        var grade = new GradeLevel
        {
            Id = Guid.NewGuid(),
            SchoolId = school.Id,
            Name = "Grade 6",
            Order = 6
        };
        var classGroup = new ClassGroup
        {
            Id = Guid.NewGuid(),
            SchoolId = school.Id,
            AcademicYearId = year.Id,
            AcademicProgramId = program.Id,
            GradeLevelId = grade.Id,
            Name = "Cambridge Primary Stage 6 — A",
            NormalizedName = "CAMBRIDGE PRIMARY STAGE 6 — A",
            Code = "CAM6A",
            NormalizedCode = "CAM6A",
            Status = AcademicStructureStatus.Active
        };
        var subject = new Subject
        {
            Id = Guid.NewGuid(),
            SchoolId = school.Id,
            Name = "Mathematics",
            Code = "MATH",
            NormalizedCode = "MATH",
            Status = AcademicStructureStatus.Active
        };
        var profile = new StudentProfile
        {
            Id = Guid.NewGuid(),
            SchoolId = school.Id,
            UserId = studentResult.User!.Id,
            StudentNumber = "CAMB-9001",
            NormalizedStudentNumber = "CAMB-9001",
            FirstName = "Alice",
            LastName = "Bennett",
            DisplayName = "Alice Bennett",
            Status = AcademicStructureStatus.Active,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        context.AddRange(
            year,
            program,
            grade,
            classGroup,
            subject,
            profile,
            new StudentEnrollment
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                StudentProfileId = profile.Id,
                ClassGroupId = classGroup.Id,
                AcademicYearId = year.Id,
                EnrolledAtUtc = now
            },
            new TeacherAssignment
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                TeacherUserId = teacherResult.User!.Id,
                ClassGroupId = classGroup.Id,
                SubjectId = subject.Id,
                AcademicYearId = year.Id,
                CreatedAtUtc = now
            });

        await context.SaveChangesAsync();

        var byClass = await repository.QueryBySchoolAsync(
            school.Id,
            new Edulytics.Core.Users.SchoolUserListQuery(
                AcademicYearId: year.Id,
                AcademicProgramId: program.Id,
                ClassGroupId: classGroup.Id,
                PageSize: 25));

        Assert.Equal(2, byClass.TotalCount);
        Assert.Contains(byClass.Users, x => x.Id == teacherResult.User.Id);
        Assert.Contains(byClass.Users, x => x.Id == studentResult.User.Id);

        var byName = await repository.QueryBySchoolAsync(
            school.Id,
            new Edulytics.Core.Users.SchoolUserListQuery(
                Name: "Alice",
                PageSize: 25));

        var named = Assert.Single(byName.Users);
        Assert.Equal(studentResult.User.Id, named.Id);
        Assert.Equal("Alice Bennett", named.DisplayName);
        Assert.Equal("CAMB-9001", named.StudentNumber);
        Assert.Contains(
            named.AcademicContexts!,
            contextItem => contextItem.ClassGroupId == classGroup.Id);

        var byUserId = await repository.QueryBySchoolAsync(
            school.Id,
            new Edulytics.Core.Users.SchoolUserListQuery(
                UserId: teacherResult.User.Id.ToString(),
                PageSize: 25));

        Assert.Equal(
            teacherResult.User.Id,
            Assert.Single(byUserId.Users).Id);

        var byEmail = await repository.QueryBySchoolAsync(
            school.Id,
            new Edulytics.Core.Users.SchoolUserListQuery(
                Email: "alice.student",
                PageSize: 25));

        Assert.Equal(
            studentResult.User.Id,
            Assert.Single(byEmail.Users).Id);

        var options = await repository.GetDirectoryFilterOptionsAsync(
            school.Id);

        Assert.Contains(options.AcademicYears, x => x.Id == year.Id);
        Assert.Contains(options.AcademicPrograms, x => x.Id == program.Id);
        Assert.Contains(options.Classes, x => x.Id == classGroup.Id);
    }

    [Fact]
    public async Task GetBySchoolAndIdAsync_DoesNotCrossTenant()
    {
        using var provider = BuildProvider();

        using var scope =
            provider.CreateScope();

        var services =
            scope.ServiceProvider;

        var context =
            services.GetRequiredService<
                EdulyticsDbContext>();

        var roleManager =
            services.GetRequiredService<
                RoleManager<ApplicationRole>>();

        var userManager =
            services.GetRequiredService<
                UserManager<ApplicationUser>>();

        await roleManager.CreateAsync(
            new ApplicationRole
            {
                Name = RoleNames.Teacher
            });

        var schoolA = NewSchool();
        var schoolB = NewSchool();

        context.Schools.AddRange(
            schoolA,
            schoolB);

        await context.SaveChangesAsync();

        var repository =
            new IdentitySchoolUserRepository(
                userManager,
                roleManager,
                context);

        var created =
            await repository.CreateAsync(
                schoolA.Id,
                "teacher@example.com",
                RoleNames.Teacher);

        Assert.NotNull(created.User);

        var crossTenant =
            await repository.GetBySchoolAndIdAsync(
                schoolB.Id,
                created.User!.Id);

        Assert.Null(crossTenant);
    }

    private static ServiceProvider BuildProvider()
    {
        var services =
            new ServiceCollection();

        services.AddLogging();
        services.AddDataProtection();

        services.AddDbContext<
            EdulyticsDbContext>(
            options =>
                options.UseInMemoryDatabase(
                    Guid.NewGuid()
                        .ToString("N")));

        services
            .AddIdentityCore<ApplicationUser>(
                options =>
                {
                    options.User.RequireUniqueEmail =
                        true;

                    options.Password.RequiredLength =
                        12;

                    options.Password
                        .RequireNonAlphanumeric =
                        true;

                    options.Password.RequireDigit =
                        true;

                    options.Password.RequireLowercase =
                        true;

                    options.Password.RequireUppercase =
                        true;
                })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<
                EdulyticsDbContext>()
            .AddDefaultTokenProviders();

        return services.BuildServiceProvider();
    }

    private static School NewSchool() =>
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Test School",
            SchoolCode =
                Guid.NewGuid()
                    .ToString("N")[..8]
                    .ToUpperInvariant(),
            NormalizedSchoolCode =
                Guid.NewGuid()
                    .ToString("N")[..8]
                    .ToUpperInvariant(),
            Status = SchoolStatus.Active,
            CountryCode = "PL",
            City = "Warsaw",
            ContactEmail =
                $"{Guid.NewGuid():N}@example.com",
            DefaultCulture = "en",
            TimeZoneId = "Europe/Warsaw",
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            RowVersion =
                BitConverter.GetBytes(1L)
        };
}
