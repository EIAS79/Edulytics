using Edulytics.Core.Interfaces;
using Edulytics.Data.Repositories;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Web.Extensions;

public static class LessonContentRegistrationExtensions
{
    public static IServiceCollection AddLessonContentPhase29(this IServiceCollection services)
    {
        services.AddScoped<ILessonContentRepository, LessonContentRepository>();
        services.AddScoped<ILessonContentService, LessonContentService>();
        services.AddScoped<MathematicsPedagogicalLessonSeeder>();
        services.AddScoped<MathematicsCanonicalLessonContentSeeder>();

        services.AddSingleton(sp =>
        {
            var configuration =
                sp.GetRequiredService<IConfiguration>()
                    .GetSection("Edulytics:YouTubeLessons");

            return new YouTubeLessonDiscoveryOptions
            {
                Enabled = ReadBool(configuration["Enabled"], true),
                ApiKey = configuration["ApiKey"] ?? string.Empty,
                CacheMinutes = ReadInt(configuration["CacheMinutes"], 720),
                SearchResultCount = ReadInt(configuration["SearchResultCount"], 25),
                RelatedResultCount = ReadInt(configuration["RelatedResultCount"], 6),
                MinimumRelevancePercent =
                    ReadInt(configuration["MinimumRelevancePercent"], 34)
            };
        });

        services.AddHttpClient<
                IYouTubeLessonDiscoveryService,
                YouTubeLessonDiscoveryService>(
                client =>
                {
                    client.Timeout = TimeSpan.FromSeconds(8);
                    client.DefaultRequestHeaders.UserAgent.ParseAdd(
                        "Edulytics-Lesson-YouTube/2.0");
                });

        return services;
    }

    private static bool ReadBool(string? value, bool fallback) =>
        bool.TryParse(value, out var parsed)
            ? parsed
            : fallback;

    private static int ReadInt(string? value, int fallback) =>
        int.TryParse(value, out var parsed)
            ? parsed
            : fallback;
}
