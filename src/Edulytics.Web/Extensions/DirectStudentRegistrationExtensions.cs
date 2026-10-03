using Edulytics.Core.Interfaces;
using Edulytics.Data.Repositories;
using Edulytics.Services.Entitlements;
using Edulytics.Services.DirectStudents;

namespace Edulytics.Web.Extensions;

public static class DirectStudentRegistrationExtensions
{
    public static IServiceCollection AddDirectStudentCommerce(
        this IServiceCollection services)
    {
        services.AddScoped<
            IDirectStudentAccountRepository,
            IdentityDirectStudentAccountRepository>();

        services.AddScoped<
            IDirectStudentRegistrationService,
            DirectStudentRegistrationService>();

        services.AddScoped<
            IPersonalEntitlementRepository,
            PersonalEntitlementRepository>();

        services.AddScoped<
            IStudentEntitlementService,
            StudentEntitlementService>();

        return services;
    }
}
