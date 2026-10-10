using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using System.Threading.RateLimiting;
using Edulytics.Web;
using Edulytics.Services.LessonContent;
using Edulytics.Web.Bootstrap;
using Edulytics.Web.Extensions;
using Edulytics.Web.Health;
using Edulytics.Web.Hosting;
using Edulytics.Web.Hubs;
using Edulytics.Web.Localization;
using Edulytics.Web.Middleware;
using Edulytics.Web.Resilience;
using Edulytics.Web.Scale;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.RateLimiting;

var builder =
    WebApplication.CreateBuilder(args);

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
                    "pl");

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
                    new CustomRequestCultureProvider(
                        context =>
                        {
                            var path =
                                context.Request.Path.Value?
                                    .TrimEnd('/');

                            if (!string.Equals(
                                    path,
                                    "/account/login",
                                    StringComparison.OrdinalIgnoreCase) ||
                                !context.Request.Query.TryGetValue(
                                    "culture",
                                    out var requestedCulture))
                            {
                                return Task.FromResult<
                                    ProviderCultureResult?>(
                                    null);
                            }

                            var culture =
                                requestedCulture
                                    .ToString()
                                    .Trim()
                                    .ToLowerInvariant();

                            if (culture is not ("en" or "pl" or "ar"))
                            {
                                return Task.FromResult<
                                    ProviderCultureResult?>(
                                    null);
                            }

                            return Task.FromResult<
                                ProviderCultureResult?>(
                                new ProviderCultureResult(
                                    culture,
                                    culture));
                        }));

            options
                .RequestCultureProviders
                .Add(
                    new CustomRequestCultureProvider(
                        context =>
                        {
                            // Migrate visitors who selected Arabic through the
                            // legacy client-only public-language cookie. This
                            // preserves their choice on the first request after
                            // deployment and upgrades it to the canonical culture.
                            if (context.Request.Cookies.TryGetValue(
                                    "Edulytics.PublicLanguage",
                                    out var legacyPublicLanguage) &&
                                string.Equals(
                                    legacyPublicLanguage,
                                    "ar",
                                    StringComparison.OrdinalIgnoreCase))
                            {
                                context.Response.Cookies.Append(
                                    CultureCookie.Name,
                                    CultureCookie.CreateValue("ar"),
                                    new CookieOptions
                                    {
                                        Path = "/",
                                        Expires = DateTimeOffset.UtcNow.AddYears(1),
                                        IsEssential = true,
                                        HttpOnly = true,
                                        SameSite = SameSiteMode.Strict,
                                        Secure = context.Request.IsHttps
                                    });

                                return Task.FromResult<
                                    ProviderCultureResult?>(
                                    new ProviderCultureResult(
                                        "ar",
                                        "ar"));
                            }

                            if (CultureCookie.TryRead(
                                    context.Request,
                                    out var culture))
                            {
                                return Task.FromResult<
                                    ProviderCultureResult?>(
                                    new ProviderCultureResult(
                                        culture,
                                        culture));
                            }

                            return Task.FromResult<
                                ProviderCultureResult?>(
                                null);
                        }));
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
    .AddDirectStudentCommerce();

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
    .AddAdaptivePracticeV2(
        builder.Configuration);

builder.Services
    .AddSingleton<Edulytics.Web.GameRouting.Stage22ExactGameRuntime>();

builder.Services
    .AddAssessmentIntelligencePhase32();

builder.Services
    .AddMathematicsGenerationPhase33();

builder.Services
    .AddAdvancedMathematicsV3(
        builder.Configuration);

builder.Services
    .AddWeaknessRecoveryPhase36();

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

var app =
    builder.Build();

// Resolve and log YouTube configuration at application startup, even before
// an authenticated lesson request reaches the discovery service.
// The options factory logs presence flags only; it never logs the API key.
_ = app.Services.GetRequiredService<YouTubeLessonDiscoveryOptions>();

var cleanBootstrap =
    app.Configuration.GetValue<bool>("Edulytics:Deployment:CleanBootstrap");
var readCanonicalJson =
    app.Configuration.GetValue<bool>("Edulytics:LessonContent:ReadFromJson");
var runStartupDataMaintenance =
    app.Configuration.GetValue<bool?>(
        "Edulytics:Deployment:RunStartupDataMaintenance") ??
    !cleanBootstrap;

if (cleanBootstrap && !readCanonicalJson)
{
    throw new InvalidOperationException(
        "CleanBootstrap requires ReadFromJson=true to avoid persisting lesson prose.");
}

using (var scope =
       app.Services.CreateScope())
{
    var bootstrapTimer =
        Stopwatch.StartNew();

    var bootstrapper =
        scope.ServiceProvider
            .GetRequiredService<
                EdulyticsDatabaseBootstrapper>();

    await bootstrapper
        .InitializeAsync();

    bootstrapTimer.Stop();

    Console.WriteLine(
        $"STARTUP_BOOTSTRAP_COMPLETED elapsedMs={bootstrapTimer.ElapsedMilliseconds}");

    var mathematicsCanonicalLessonContentSeeder =
        scope.ServiceProvider
            .GetRequiredService<
                Edulytics.Data.Seeding.MathematicsCanonicalLessonContentSeeder>();

    if (runStartupDataMaintenance)
    {
        var maintenanceTimer =
            Stopwatch.StartNew();

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

        if (readCanonicalJson)
        {
            await mathematicsCanonicalLessonContentSeeder
                .SeedMetadataOnlyAsync();

            Console.WriteLine("STARTUP_CANONICAL_JSON_METADATA_ONLY_SEEDED");
        }
        else
        {
            var approvedCorrectionTimer = Stopwatch.StartNew();
            await mathematicsCanonicalLessonContentSeeder
                .SeedApprovedProductionCorrectionsAsync();
            approvedCorrectionTimer.Stop();
            Console.WriteLine(
                $"STARTUP_APPROVED_CONTENT_CORRECTIONS_COMPLETED elapsedMs={approvedCorrectionTimer.ElapsedMilliseconds}");
            await mathematicsCanonicalLessonContentSeeder.SeedAsync();
        }

        maintenanceTimer.Stop();

        Console.WriteLine(
            $"STARTUP_DATA_MAINTENANCE_COMPLETED elapsedMs={maintenanceTimer.ElapsedMilliseconds}");
    }
    else
    {
        if (!readCanonicalJson)
        {
            var approvedCorrectionTimer = Stopwatch.StartNew();
            await mathematicsCanonicalLessonContentSeeder
                .SeedApprovedProductionCorrectionsAsync();
            approvedCorrectionTimer.Stop();
            Console.WriteLine(
                $"STARTUP_APPROVED_CONTENT_CORRECTIONS_COMPLETED elapsedMs={approvedCorrectionTimer.ElapsedMilliseconds}");
        }
        Console.WriteLine("STARTUP_DATA_MAINTENANCE_SKIPPED");
    }
}

app.UseForwardedHeaders();

// The public static front door already routes /__frontdoor-live/* to this
// application. Keep the demo tenant on that SAME public origin while proxying
// only from the approved main service to the dedicated, isolated demo service.
// Never activate on the demo service itself or on a developer machine.
app.Use(async (context, next) =>
{
    if (app.Configuration.GetValue<bool>("Edulytics:DemoGateway:Enabled") &&
        string.Equals(
            Environment.GetEnvironmentVariable("RENDER_SERVICE_ID"),
            "srv-dakq5n2fngtc73a62i10",
            StringComparison.Ordinal))
    {
        if (context.Request.Path.StartsWithSegments(
                "/__frontdoor-live/demo", out var demoPath))
        {
            await DemoSameOriginGateway.ForwardAsync(context, demoPath);
            return;
        }

        // Demo access is only through the explicit /__frontdoor-live/demo path.
        // Never infer the destination from a browser cookie: an old demo
        // cookie can coexist with a real school's production session.

    }

    await next();
});

app.UseMiddleware<
    CorrelationIdMiddleware>();

app.UseMiddleware<
    SecurityHeadersMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(
        "/system/error");

    app.UseHsts();
}

var edgeEnforcesHttps =
    app.Configuration
        .GetValue<bool>(
            "Edulytics:Hosting:EdgeEnforcesHttps");

// Some trusted edge proxies, including Render, terminate TLS and enforce the
// public HTTP -> HTTPS redirect before forwarding traffic over the private
// network. Keep this separate from forwarded-header trust because accepting
// proxy metadata alone does not prove that the proxy enforces HTTPS.
if (!edgeEnforcesHttps)
{
    app.UseHttpsRedirection();
}


if (!app.Environment.IsDevelopment())
{
    app.UseStatusCodePagesWithReExecute(
        "/system/status/{0}");
}

// Static snapshots use /__frontdoor-live as a same-origin bridge after the
// browser has confirmed that the free application is ready. Render's static
// rewrite forwards this otherwise-missing path to the web service; strip the
// bridge prefix before MVC routing so the real anonymous endpoint handles the
// request and emits fresh cookies / anti-forgery tokens.
app.Use(async (context, next) =>
{
    const string frontDoorLivePrefix = "/__frontdoor-live";

    // The dedicated demo application needs PathBase so MVC links, forms,
    // redirects and auth-cookie paths stay under the same-origin /demo bridge.
    if (string.Equals(
            Environment.GetEnvironmentVariable("RENDER_SERVICE_ID"),
            "srv-db4jtht9fdbs73fioa20",
            StringComparison.Ordinal) &&
        context.Request.Path.StartsWithSegments(
            "/__frontdoor-live/demo", out var demoRemaining))
    {
        context.Request.PathBase = "/__frontdoor-live/demo";
        context.Request.Path = demoRemaining.HasValue ? demoRemaining : "/";
    }
    else if (context.Request.Path.StartsWithSegments(
            frontDoorLivePrefix,
            out var remainingPath))
    {
        context.Request.Path =
            remainingPath.HasValue
                ? remainingPath
                : "/";
    }

    await next();
});

// Parse culture AFTER normalizing the same-origin demo PathBase so EN, PL
// and AR apply to both localized content and the document's html/dir tags.
app.UseRequestLocalization();

app.UseRouting();

// The always-on static public front door wakes this free web service in the
// background and polls readiness before sending a visitor into the app. Keep
// readiness anonymous, non-cacheable and readable cross-origin so visitors
// never need to see Render's cold-start loading page.
app.Use(async (context, next) =>
{
    if (context.Request.Path.Equals(
            "/health/ready",
            StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Headers.AccessControlAllowOrigin = "*";
        context.Response.Headers.CacheControl = "no-store";

        if (HttpMethods.IsOptions(context.Request.Method))
        {
            context.Response.StatusCode = StatusCodes.Status204NoContent;
            return;
        }
    }

    await next();
});

app.UseRequestTimeouts();

app.UseAuthentication();

app.UseMiddleware<
    DistributedSensitiveRateLimitMiddleware>();

app.UseRateLimiter();

app.UseAuthorization();

app.UseMiddleware<IdempotencyMiddleware>();

// Diagnostic endpoint is authenticated, returns presence flags only, and reads
// the exact singleton instance used by YouTubeLessonDiscoveryService.
app.MapGet(
        "/internal/diagnostics/youtube",
        (YouTubeLessonDiscoveryOptions options) =>
            Results.Json(new
            {
                enabled = options.Enabled,
                apiKeyConfigured = !string.IsNullOrWhiteSpace(options.ApiKey)
            }))
    .RequireAuthorization();

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

await app.StartAsync();

try
{
    using var meetingDemoScope =
        app.Services.CreateScope();

    await MeetingDemoProvisioner.RunAsync(
        meetingDemoScope.ServiceProvider
            .GetRequiredService<Edulytics.Data.Contexts.EdulyticsDbContext>(),
        meetingDemoScope.ServiceProvider
            .GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Edulytics.Data.Identity.ApplicationUser>>(),
        app.Configuration);
}
catch (Exception exception)
{
    Console.Error.WriteLine(
        $"MEETING_DEMO_POST_START_FAILED type={exception.GetType().Name} message={exception.Message}");
}

await app.WaitForShutdownAsync();

public partial class Program
{
}