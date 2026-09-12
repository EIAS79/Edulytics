namespace Edulytics.Tests.Acceptance;

public sealed class StagingCurriculumBootstrapContractTests
{
    [Fact]
    public void CurriculumBootstrap_IsExplicitStagingOnlyAndOrdered()
    {
        var root = FindRoot();
        var source = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Bootstrap/EdulyticsDatabaseBootstrapper.cs"));

        const string flag = "Edulytics:Deployment:SeedCurriculum";
        Assert.Contains(flag, source, StringComparison.Ordinal);
        Assert.Contains("ASPNETCORE_ENVIRONMENT", source, StringComparison.Ordinal);
        Assert.Contains("DOTNET_ENVIRONMENT", source, StringComparison.Ordinal);
        Assert.Contains("\"Staging\"", source, StringComparison.Ordinal);

        var pack = source.IndexOf(
            "new MathematicsCurriculumPackSeeder(_db)",
            StringComparison.Ordinal);
        var lessons = source.IndexOf(
            "new MathematicsPedagogicalLessonSeeder(_db)",
            StringComparison.Ordinal);
        var content = source.IndexOf(
            "new MathematicsCanonicalLessonContentSeeder(_db)",
            StringComparison.Ordinal);

        Assert.True(pack >= 0, "Curriculum pack seeder must be present.");
        Assert.True(lessons > pack, "Pedagogical lessons must seed after curriculum packs.");
        Assert.True(content > lessons, "Canonical lesson content must seed after pedagogical lessons.");
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Edulytics solution root not found.");
    }
}
