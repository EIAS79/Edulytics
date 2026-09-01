using System.Globalization;
using System.Security.Claims;
using System.Threading.RateLimiting;
using Edulytics.Web;
using Edulytics.Web.Bootstrap;
using Edulytics.Web.Extensions;
using Edulytics.Web.Health;
using Edulytics.Web.Hubs;
using Edulytics.Web.Localization;
using Edulytics.Web.Middleware;
using Edulytics.Web.Resilience;
using Edulytics.Web.Scale;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.RateLimiting;

const string phase38StagingServiceId =
    "srv-da1o4url550s73aecsn0";

var builder =
    WebApplication.CreateBuilder(args);

var phase38MaintenanceFreeze =
    string.Equals(
        Environment.GetEnvironmentVariable(
            "RENDER_SERVICE_ID"),
        phase38StagingServiceId,
        StringComparison.Ordinal);

if (builder.Environment.IsProduction())
{
    builder.Logging.ClearProviders();

    builder.Logging.AddJsonConsole(
        options =>
        {
            options.IncludeScopes = true;
            options.UseUtcTimestamp = true;

            options.TimestampFormat =
                "yyyy-MM-dd'T'HH:mm:ss.fff'Z'";
        });
}

builder.Services.AddLocalization(
    options =>
    {
        options.ResourcesPath =
            "Resources";
    });

builder.Services
    .AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(
        options =>
        {
            options
                .DataAnnotationLocalizerProvider =
                (_, factory) =>
                    factory.Create(
                        typeof(
                            ValidationResource));
        });

builder.Services
    .AddEdulyticsIdentityAndData(
        builder.Configuration);

builder.Services
    .AddMultiInstanceScalePhase25(
        builder.Configuration,
        builder.Environment);

builder.Services
    .AddAuditCompliancePhase18();

var supportedCultures =
    CultureCookie
        .SupportedCultures
        .Select(
            x =>
                new CultureInfo(x))
        .ToArray();

builder.Services
    .Configure<
        RequestLocalizationOptions>(
        options =>
        {
            options
                .DefaultRequestCulture =
                new RequestCulture(
                    "en");

            options
                .SupportedCultures =
                supportedCultures;

            options
                .SupportedUICultures =
                supportedCultures;

            options
                .RequestCultureProviders
                .Clear();

            options
                .RequestCultureProviders
                .Add(
                    new CookieRequestCultureProvider
                    {
                        CookieName =
                            CultureCookie.Name
                    });
        });

builder.Services
    .AddSchoolManagementPhase04();

builder.Services
    .AddSchoolUserManagementPhase05();

builder.Services
    .AddCustomerOnboardingPhase25B();

builder.Services
    .AddSubscriptionsPhase25C();

builder.Services
    .AddBillingPhase25D();

builder.Services
    .AddSubjectSupervisorCompletionPhase19();

builder.Services
    .AddAcademicStructurePhase06();

builder.Services
    .AddCurriculumPhase07();

builder.Services
    .AddAssessmentsPhase08();

builder.Services
    .AddAnalyticsPhase09();

builder.Services
    .AddReportsPhase20(
        builder.Configuration);

builder.Services
    .AddRealtimeDashboardsPhase10(
        builder.Configuration,
        builder.Environment);

builder.Services
    .AddDataImportPhase11();

builder.Services
    .AddNotificationsPhase21();

builder.Services
    .AddStudentPortalPhase28();

builder.Services
    .AddLessonContentPhase29();

builder.Services
    .AddPracticePhase30();

builder.Services
    .AddAssessmentIntelligencePhase32();

builder.Services
    .AddMathematicsGenerationPhase33();

builder.Services
    .AddExamGenerationPhase34();

builder.Services
    .AddInvitationEmailDelivery(
        builder.Configuration);

builder.Services
    .AddOperationalAdminPhase22();

builder.Services
    .AddSecurityPrivacyHardeningPhase23(
        builder.Configuration,
        builder.Environment);

builder.Services
    .AddProductionHardeningPhase12(
        builder.Configuration,
        builder.Environment);

builder.AddBackendResiliencePhase14();

builder.Services
    .Configure<
        ForwardedHeadersOptions>(
        options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders
                    .XForwardedFor |
                ForwardedHeaders
                    .XForwardedProto |
                ForwardedHeaders
                    .XForwardedHost;

            var trustForwardedHeaders =
                builder.Configuration
                    .GetValue<bool>(
                        "Edulytics:Hosting:TrustForwardedHeaders");

            var isCodespaces =
                string.Equals(
                    Environment
                        .GetEnvironmentVariable(
                            "CODESPACES"),
                    "true",
                    StringComparison
                        .OrdinalIgnoreCase);

            if (trustForwardedHeaders ||
                isCodespaces)
            {
                options.KnownIPNetworks
                    .Clear();

                options.KnownProxies
                    .Clear();

                options.ForwardLimit = 1;
            }
        });

if (phase38MaintenanceFreeze)
{
    var webAssembly =
        typeof(EdulyticsDatabaseBootstrapper)
            .Assembly;

    var backgroundWorkers =
        builder.Services
            .Where(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(IHostedService) &&
                    descriptor.ImplementationType?
                        .Assembly == webAssembly)
            .ToArray();

    foreach (var descriptor in
             backgroundWorkers)
    {
        builder.Services.Remove(
            descriptor);
    }

    Console.WriteLine(
        "PHASE38_MAINTENANCE_BACKGROUND_WORKERS_DISABLED count={0} workers={1}",
        backgroundWorkers.Length,
        string.Join(
            ',',
            backgroundWorkers
                .Select(
                    x =>
                        x.ImplementationType?
                            .Name ??
                        "unknown")
                .OrderBy(
                    x => x,
                    StringComparer.Ordinal)));
}

var app =
    builder.Build();

using (var scope =
       app.Services.CreateScope())
{
    var bootstrapper =
        scope.ServiceProvider
            .GetRequiredService<
                EdulyticsDatabaseBootstrapper>();

    await bootstrapper
        .InitializeAsync();

    var curriculumLevelIdentityBackfill =
        scope.ServiceProvider
            .GetRequiredService<
                Edulytics.Data.Seeding.CurriculumLevelIdentityBackfill>();

    await curriculumLevelIdentityBackfill
        .RunAsync();

    var mathematicsCurriculumPackSeeder =
        scope.ServiceProvider
            .GetRequiredService<
                Edulytics.Data.Seeding.MathematicsCurriculumPackSeeder>();

    await mathematicsCurriculumPackSeeder
        .SeedAsync();

    var mathematicsPedagogicalLessonSeeder =
        scope.ServiceProvider
            .GetRequiredService<
                Edulytics.Data.Seeding.MathematicsPedagogicalLessonSeeder>();

    await mathematicsPedagogicalLessonSeeder
        .SeedAsync();

    var mathematicsCanonicalLessonContentSeeder =
        scope.ServiceProvider
            .GetRequiredService<
                Edulytics.Data.Seeding.MathematicsCanonicalLessonContentSeeder>();

    await mathematicsCanonicalLessonContentSeeder
        .SeedAsync();
}

app.UseForwardedHeaders();

app.UseMiddleware<
    CorrelationIdMiddleware>();

app.UseMiddleware<
    SecurityHeadersMiddleware>();

if (phase38MaintenanceFreeze)
{
    app.Use(
        async (context, next) =>
        {
            if (context.Request.Path
                .StartsWithSegments(
                    "/health"))
            {
                await next();
                return;
            }

            context.Response.StatusCode =
                StatusCodes
                    .Status503ServiceUnavailable;

            context.Response.ContentType =
                "text/plain; charset=utf-8";

            context.Response.Headers
                .RetryAfter =
                "3600";

            await context.Response.WriteAsync(
                "Edulytics staging is temporarily unavailable during the Phase 38 launch-gate maintenance window.");
        });
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(
        "/system/error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRequestLocalization();

if (!app.Environment.IsDevelopment())
{
    app.UseStatusCodePagesWithReExecute(
        "/system/status/{0}");
}

app.UseRouting();

app.UseRequestTimeouts();

app.UseAuthentication();

app.UseMiddleware<
    DistributedSensitiveRateLimitMiddleware>();

app.UseRateLimiter();

app.UseAuthorization();

app.UseMiddleware<IdempotencyMiddleware>();

app.MapHealthChecks(
        "/health/live",
        new HealthCheckOptions
        {
            Predicate =
                registration =>
                    registration.Tags
                        .Contains(
                            "live"),

            ResponseWriter =
                HealthResponseWriter
                    .WriteAsync
        })
    .AllowAnonymous();

app.MapHealthChecks(
        "/health/ready",
        new HealthCheckOptions
        {
            Predicate =
                registration =>
                    registration.Tags
                        .Contains(
                            "ready"),

            ResponseWriter =
                HealthResponseWriter
                    .WriteAsync
        })
    .AllowAnonymous();

app.MapStaticAssets()
    .Add(
        endpointBuilder =>
            endpointBuilder.Metadata
                .Add(
                    new Microsoft
                        .AspNetCore
                        .Authorization
                        .AllowAnonymousAttribute()));

app.MapControllerRoute(
        name:
            "default",
        pattern:
            "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<AnalyticsHub>(
    "/hubs/analytics");

app.MapFallback(
        context =>
        {
            context.Response.StatusCode =
                StatusCodes.Status404NotFound;

            return Task.CompletedTask;
        })
    .AllowAnonymous();

app.Run();

public partial class Program
{
}
