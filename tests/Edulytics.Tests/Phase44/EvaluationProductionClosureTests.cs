namespace Edulytics.Tests.Phase44;

public sealed class EvaluationProductionClosureTests
{
    private static readonly string Root = FindRoot();

    [Fact]
    public void ProductionReadinessAssetsExist()
    {
        Assert.True(File.Exists(Path.Combine(
            Root,
            "docs/evaluation/PRODUCTION_READINESS.md")));

        Assert.True(File.Exists(Path.Combine(
            Root,
            "tools/evaluation/evaluation_staging_smoke.py")));

        Assert.True(File.Exists(Path.Combine(
            Root,
            ".github/workflows/evaluation-production.yml")));
    }

    [Fact]
    public void ProductionReadinessPreservesPrivacyAndNoEvidenceSemantics()
    {
        var doc = File.ReadAllText(Path.Combine(
            Root,
            "docs/evaluation/PRODUCTION_READINESS.md"));

        Assert.Contains(
            "Missing evidence is never converted to 0% mastery",
            doc);
        Assert.Contains(
            "Private student Practice never enters staff analytics",
            doc);
        Assert.Contains(
            "browser cannot select another student profile",
            doc);
        Assert.Contains(
            "verified exact Assessment SkillContract",
            doc);
        Assert.Contains(
            "Generated intervention questions stay Draft",
            doc);
    }

    [Fact]
    public void EvaluationProductionWorkflowIncludesDatabaseSecurityAndRegressionGates()
    {
        var workflow = File.ReadAllText(Path.Combine(
            Root,
            ".github/workflows/evaluation-production.yml"));

        Assert.Contains(
            "dotnet ef database update",
            workflow);
        Assert.Contains(
            "has-pending-model-changes",
            workflow);
        Assert.Contains(
            "dotnet test tests/Edulytics.Tests/Edulytics.Tests.csproj",
            workflow);
        Assert.Contains(
            "ci-tenant-idor-gate.sh",
            workflow);
        Assert.Contains(
            "ci-dependency-gate.sh",
            workflow);
        Assert.Contains(
            "ci-localization-parity.py",
            workflow);
        Assert.Contains(
            "ci-architecture-gate.py",
            workflow);
    }

    [Fact]
    public void StagingSmokeIsHardLockedAndChecksEvaluationAccessBoundaries()
    {
        var smoke = File.ReadAllText(Path.Combine(
            Root,
            "tools/evaluation/evaluation_staging_smoke.py"));

        Assert.Contains(
            "LOCKED_HOST = \"staging.edulytiks.com\"",
            smoke);
        Assert.Contains(
            "\"/school/analytics\"",
            smoke);
        Assert.Contains(
            "\"/student/progress\"",
            smoke);
        Assert.Contains(
            "intervention-check",
            smoke);
        Assert.Contains(
            "EVALUATION_STAGING_SMOKE_PASS",
            smoke);
    }

    [Fact]
    public void CohortEvaluationReusesOneNormalizedEvidenceStream()
    {
        var service = File.ReadAllText(Path.Combine(
            Root,
            "src/Edulytics.Services/Analytics/AnalyticsService.cs"));

        Assert.Contains(
            "NormalizeOfficialEvidence(projection)",
            service);

        Assert.Contains(
            "BuildStudentSubject(",
            service);
        Assert.Contains(
            "normalized,",
            service);
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(
                    directory.FullName,
                    "Edulytics.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException();
    }
}
