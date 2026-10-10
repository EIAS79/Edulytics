using Edulytics.Core.Constants;
using Edulytics.Data.Contexts;
using Edulytics.Data.Identity;
using Edulytics.Web.Bootstrap;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

// Disposable CI test data only. This program is not referenced by the Web app
// or included in its production Docker publish. Refuse real databases.
if (!string.Equals(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"), "true",
        StringComparison.OrdinalIgnoreCase))
    throw new InvalidOperationException("CI fixtures may only run in GitHub Actions.");

if (args.Length != 1 || args[0] is not ("roles" or "academic"))
    throw new ArgumentException("Expected one fixture mode: roles or academic.");

var connection = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? throw new InvalidOperationException("Disposable CI connection not configured.");
var parsed = new NpgsqlConnectionStringBuilder(connection);
if (!string.Equals(parsed.Host, "127.0.0.1", StringComparison.Ordinal) ||
    !string.Equals(parsed.Database, "edulytics_clean_ci", StringComparison.Ordinal) ||
    parsed.Port != 5432)
    throw new InvalidOperationException("Refusing fixture writes outside disposable localhost CI PostgreSQL.");

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();
var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.AddLogging();
services.AddDbContext<EdulyticsDbContext>(
    options => options.UseNpgsql(connection));
services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
})
.AddEntityFrameworkStores<EdulyticsDbContext>()
.AddDefaultTokenProviders();
using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<EdulyticsDbContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
foreach (var roleName in new[]
{
    RoleNames.SuperAdmin, RoleNames.SchoolAdmin, RoleNames.SubjectSupervisor,
    RoleNames.Teacher, RoleNames.Student
})
{
    if (await roleManager.RoleExistsAsync(roleName))
        continue;
    var created = await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
    if (!created.Succeeded)
        throw new InvalidOperationException(
            "CI role setup failed: " + string.Join("; ", created.Errors.Select(x => x.Code)));
}
if (args[0] == "roles")
{
    await PresentationDemoProvisioner.RunAsync(db, userManager, configuration);
    Console.WriteLine("CI_ROLE_FIXTURES_READY");
}
else
{
    await MeetingDemoProvisioner.RunAsync(db, userManager, configuration);
    Console.WriteLine("CI_ACADEMIC_FIXTURES_READY");
}
