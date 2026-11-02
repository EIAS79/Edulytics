
using Edulytics.Core.Practice;
using Edulytics.Data.Repositories;
using Edulytics.Services.Practice;

namespace Edulytics.Web.Extensions;

public static class PracticeRegistrationExtensions
{
    public static IServiceCollection AddPracticePhase30(this IServiceCollection services)
    {
        services.AddScoped<IPracticeRepository, PracticeRepository>();
        services.AddScoped<IPracticeService, PracticeService>();
        services.AddScoped<IStudentPrivatePracticeRepository, StudentPrivatePracticeRepository>();
        services.AddScoped<IStudentPrivatePracticeService, StudentPrivatePracticeService>();
        return services;
    }
}
