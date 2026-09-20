namespace Edulytics.Tests.Acceptance;

public sealed class AccountLoginCultureContractTests
{
    [Fact]
    public void Login_AllowsFreshBrowserWithoutCultureCookie()
    {
        var source = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "Controllers",
            "AccountController.cs");

        Assert.Contains(
            "EnsureLoginCulture();",
            source,
            StringComparison.Ordinal);

        Assert.Contains(
            "const string defaultCulture = \"pl\";",
            source,
            StringComparison.Ordinal);

        Assert.Contains(
            "AppendCultureCookie(",
            source,
            StringComparison.Ordinal);

        Assert.DoesNotContain(
            "if (!CultureCookie.TryRead(Request, out _))\n        {\n            return RedirectToAction",
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
