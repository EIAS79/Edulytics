namespace Edulytics.Tests.Acceptance;

public sealed class RenderHttpsProxyContractTests
{
    [Fact]
    public void Program_SkipsAppHttpsRedirectBehindTrustedEdgeProxy()
    {
        var source = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "Program.cs");

        Assert.Contains(
            "var trustEdgeHttps =",
            source,
            StringComparison.Ordinal);

        Assert.Contains(
            "\"Edulytics:Hosting:TrustForwardedHeaders\"",
            source,
            StringComparison.Ordinal);

        Assert.Contains(
            "if (!trustEdgeHttps)",
            source,
            StringComparison.Ordinal);

        Assert.Contains(
            "app.UseHttpsRedirection();",
            source,
            StringComparison.Ordinal);
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
