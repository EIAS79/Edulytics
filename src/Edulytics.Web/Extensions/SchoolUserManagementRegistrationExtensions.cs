using Edulytics.Core.Interfaces;
using Edulytics.Data.Repositories;
using Edulytics.Services.Auditing;
using Edulytics.Services.Users;
using Edulytics.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Extensions;

public static class SchoolUserManagementRegistrationExtensions
{
    public static IServiceCollection
        AddSchoolUserManagementPhase05(
            this IServiceCollection services)
    {
        services.AddScoped<
            ISchoolUserRepository,
            IdentitySchoolUserRepository>();

        services.AddScoped<ISchoolUserManagementService>(
            provider =>
                new SchoolUserManagementService(
                    provider.GetRequiredService<
                        ISchoolUserRepository>(),
                    provider.GetRequiredService<
                        ISchoolRepository>(),
                    provider.GetRequiredService<
                        IAuditService>(),
                    provider.GetRequiredService<
                        IApplicationTransactionManager>(),
                    provider.GetRequiredService<
                        ICustomerOnboardingRepository>(),
                    provider.GetRequiredService<
                        ISchoolSubscriptionRepository>()));

        services.AddScoped<
            DirectStudentCreationFilter>();

        services.Configure<MvcOptions>(
            options =>
                options.Filters.AddService<
                    DirectStudentCreationFilter>());

        return services;
    }
}
