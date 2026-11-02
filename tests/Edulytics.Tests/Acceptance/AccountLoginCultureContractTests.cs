namespace Edulytics.Tests.Acceptance;

public sealed class AccountLoginCultureContractTests
{
    [Fact]
    public void RequestLocalization_ResolvesArabicLoginCultureBeforeMvc()
    {
        var source = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "Program.cs");

        var loginProviderIndex = source.IndexOf(
            "\"/account/login\"",
            StringComparison.Ordinal);
        var legacyArabicCookieIndex = source.IndexOf(
            "\"Edulytics.PublicLanguage\"",
            StringComparison.Ordinal);

        Assert.True(loginProviderIndex >= 0);
        Assert.True(legacyArabicCookieIndex >= 0);
        Assert.True(
            loginProviderIndex < legacyArabicCookieIndex,
            "The explicit login query culture must be evaluated before legacy/public cookies.");

        Assert.Contains(
            "context.Request.Query.TryGetValue(\n                                    \"culture\"",
            source,
            StringComparison.Ordinal);

        Assert.Contains(
            "culture is not (\"en\" or \"pl\" or \"ar\")",
            source,
            StringComparison.Ordinal);
    }

    [Fact]
    public void Login_HonorsExplicitEnglishPolishOrArabicCulture()
    {
        var source = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "Controllers",
            "AccountController.cs");

        Assert.Contains(
            "ApplyLoginCulture(culture);",
            source,
            StringComparison.Ordinal);

        Assert.Contains(
            "culture is not (\"en\" or \"pl\" or \"ar\")",
            source,
            StringComparison.Ordinal);

        Assert.Contains(
            "AppendCultureCookie(culture);",
            source,
            StringComparison.Ordinal);
    }

    [Fact]
    public void Login_PreservesArabicWebsiteCulture()
    {
        var source = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "Controllers",
            "AccountController.cs");

        Assert.Contains(
            "culture is not (\"en\" or \"pl\" or \"ar\")",
            source,
            StringComparison.Ordinal);

        Assert.DoesNotContain(
            "culture = \"en\";",
            source,
            StringComparison.Ordinal);

        Assert.Contains(
            "? cookieCulture\n                    : \"pl\";",
            source,
            StringComparison.Ordinal);
    }

    [Fact]
    public void Login_PreservesResolvedCultureOnPost()
    {
        var source = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "Views",
            "Account",
            "Login.cshtml");

        Assert.Contains(
            "var loginCulture = (ViewData[\"LoginCulture\"] as string)",
            source,
            StringComparison.Ordinal);

        Assert.Contains(
            "asp-route-culture=\"@loginCulture\"",
            source,
            StringComparison.Ordinal);

        Assert.Contains(
            "var isArabic = string.Equals(loginCulture, \"ar\"",
            source,
            StringComparison.Ordinal);

        Assert.Contains(
            "string L(string en, string pl, string ar)",
            source,
            StringComparison.Ordinal);
    }

    [Fact]
    public void Login_DoesNotExposePlatformAdministratorBypassCopy()
    {
        var source = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "Views",
            "Account",
            "Login.cshtml");

        var forbidden = new[]
        {
            "The platform administrator can sign in directly.",
            "The platform administrator can continue without a selection.",
            "The platform administrator does not need to select a public account type.",
            "Administrator platformy może zalogować się bezpośrednio.",
            "Administrator platformy może kontynuować bez wyboru.",
            "Administrator platformy nie musi wybierać publicznego typu konta."
        };

        Assert.All(
            forbidden,
            phrase => Assert.DoesNotContain(
                phrase,
                source,
                StringComparison.Ordinal));
    }

    private static string ReadRepositoryFile(
        params string[] relativeSegments)
    {
        var root = FindRoot();

        return File.ReadAllText(
            Path.Combine(
                [root, .. relativeSegments]));
    }

    private static string FindRoot()
    {
        var directory =
            new DirectoryInfo(
                AppContext.BaseDirectory);

        while (
            directory is not null &&
            !File.Exists(
                Path.Combine(
                    directory.FullName,
                    "Edulytics.sln")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
            ?? throw new InvalidOperationException(
                "Repository root not found.");
    }
}