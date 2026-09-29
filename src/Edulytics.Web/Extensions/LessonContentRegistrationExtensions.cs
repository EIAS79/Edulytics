using Edulytics.Core.Interfaces;
using Edulytics.Data.Repositories;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;
using Edulytics.Web.YouTubeLearning;

namespace Edulytics.Web.Extensions;

public static class LessonContentRegistrationExtensions
{
    public static IServiceCollection AddLessonContentPhase29(this IServiceCollection services)
    {
        services.AddScoped<ILessonContentRepository, LessonContentRepository>();
        services.AddScoped<ILessonContentService, LessonContentService>();
        services.AddMemoryCache();
        services.AddHttpClient("YouTubeLearning", client =>
        {
            client.BaseAddress = new Uri("https://www.googleapis.com/youtube/v3/");
            client.Timeout = TimeSpan.FromSeconds(8);
        });
        services.AddScoped<IYouTubeLearningService, YouTubeLearningService>();
        services.AddScoped<MathematicsPedagogicalLessonSeeder>();
        services.AddScoped<MathematicsCanonicalLessonContentSeeder>();
        return services;
    }
}
