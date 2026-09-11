using Microsoft.AspNetCore.Localization;

namespace Edulytics.Web.Localization;

public static class CultureCookie
{
    public const string Name = "Edulytics.Culture";

    public static readonly IReadOnlyList<string> SupportedCultures =
        new[] { "en", "pl" };

    public static bool IsSupported(string? culture)
    {
        return culture is not null &&
               SupportedCultures.Contains(culture, StringComparer.Ordinal);
    }

    public static string CreateValue(string culture)
    {
        if (!IsSupported(culture))
        {
            throw new ArgumentOutOfRangeException(
                nameof(culture),
                culture,
                "Unsupported Edulytics culture.");
        }

        return CookieRequestCultureProvider.MakeCookieValue(
            new RequestCulture(culture));
    }

    public static bool TryParseValue(
        string? cookieValue,
        out string culture)
    {
        culture = string.Empty;

        if (string.IsNullOrWhiteSpace(cookieValue))
        {
            return false;
        }

        var trimmed = cookieValue.Trim();
        var legacyCulture = trimmed.ToLowerInvariant();

        if (IsSupported(legacyCulture))
        {
            culture = legacyCulture;
            return true;
        }

        foreach (var supportedCulture in SupportedCultures)
        {
            if (string.Equals(
                trimmed,
                CreateValue(supportedCulture),
                StringComparison.Ordinal))
            {
                culture = supportedCulture;
                return true;
            }
        }

        return false;
    }

    public static bool TryRead(HttpRequest request, out string culture)
    {
        culture = string.Empty;

        return request.Cookies.TryGetValue(Name, out var cookieValue) &&
               TryParseValue(cookieValue, out culture);
    }
}
