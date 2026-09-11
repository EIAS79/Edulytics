using Edulytics.Web.Localization;

namespace Edulytics.Tests.Localization;

public sealed class CultureCookieTests
{
    [Theory]
    [InlineData("en")]
    [InlineData("pl")]
    [InlineData(" EN ")]
    [InlineData(" PL ")]
    public void TryParseValue_AcceptsLegacyRawValues(
        string cookieValue)
    {
        var parsed =
            CultureCookie.TryParseValue(
                cookieValue,
                out var culture);

        Assert.True(parsed);
        Assert.Equal(
            cookieValue.Trim().ToLowerInvariant(),
            culture);
    }

    [Theory]
    [InlineData("en")]
    [InlineData("pl")]
    public void TryParseValue_AcceptsCanonicalValues(
        string culture)
    {
        var cookieValue =
            CultureCookie.CreateValue(culture);

        var parsed =
            CultureCookie.TryParseValue(
                cookieValue,
                out var parsedCulture);

        Assert.True(parsed);
        Assert.Equal(culture, parsedCulture);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("de")]
    [InlineData("c=de|uic=de")]
    [InlineData("not-a-culture-cookie")]
    public void TryParseValue_RejectsUnsupportedValues(
        string? cookieValue)
    {
        var parsed =
            CultureCookie.TryParseValue(
                cookieValue,
                out var culture);

        Assert.False(parsed);
        Assert.Equal(string.Empty, culture);
    }
}
