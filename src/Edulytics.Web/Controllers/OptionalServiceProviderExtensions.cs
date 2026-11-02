namespace Edulytics.Web.Controllers;

internal static class OptionalServiceProviderExtensions
{
    public static T? GetService<T>(this IServiceProvider? provider)
        where T : class =>
        provider?.GetService(typeof(T)) as T;
}
