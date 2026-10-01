namespace Edulytics.Tests.Bootstrap;

public sealed class ProductionRehearsalDatasetContractTests
{
    private static readonly string Root = FindRoot();

    [Fact]
    public void Rehearsal_dataset_defines_twelve_stage_specific_schools()
    {
        var source = Read(
            "src/Edulytics.Web/Bootstrap/MeetingDemoProvisioner.cs");

        Assert.Contains(
            "production-rehearsal-2026-10-01-v1",
            source,
            StringComparison.Ordinal);
        Assert.Contains(
            "ProductionRehearsalSeed",
            source,
            StringComparison.Ordinal);

        foreach (var code in new[]
        {
            "REHEARSAL-GB-PRIMARY",
            "REHEARSAL-GB-MIDDLE",
            "REHEARSAL-GB-SECONDARY",
            "REHEARSAL-US-PRIMARY",
            "REHEARSAL-US-MIDDLE",
            "REHEARSAL-US-SECONDARY",
            "REHEARSAL-AE-PRIMARY",
            "REHEARSAL-AE-MIDDLE",
            "REHEARSAL-AE-SECONDARY",
            "REHEARSAL-PL-PRIMARY",
            "REHEARSAL-PL-MIDDLE",
            "REHEARSAL-PL-SECONDARY"
        })
        {
            Assert.Contains(code, source, StringComparison.Ordinal);
        }

        Assert.Contains(
            "x.LogicalLevel >= definition.MinimumLogicalLevel",
            source,
            StringComparison.Ordinal);
        Assert.Contains(
            "x.LogicalLevel <= definition.MaximumLogicalLevel",
            source,
            StringComparison.Ordinal);
        Assert.Contains(
            "if (schoolCount != Schools.Length)",
            source,
            StringComparison.Ordinal);
    }

    [Fact]
    public void Rehearsal_reset_deletes_new_child_tables_before_parent_items()
    {
        var source = Read(
            "src/Edulytics.Web/Bootstrap/MeetingDemoProvisioner.cs");

        var adaptiveTurns = source.IndexOf(
            "DELETE FROM \"AdaptivePracticeTurns\";",
            StringComparison.Ordinal);
        var assessmentItems = source.IndexOf(
            "DELETE FROM \"AssessmentItems\";",
            StringComparison.Ordinal);
        var taskResponses = source.IndexOf(
            "DELETE FROM \"AssessmentTaskResponses\";",
            StringComparison.Ordinal);
        var assessments = source.IndexOf(
            "DELETE FROM \"Assessments\";",
            StringComparison.Ordinal);

        Assert.True(adaptiveTurns >= 0 && adaptiveTurns < assessmentItems);
        Assert.True(taskResponses >= 0 && taskResponses < assessments);

        foreach (var table in new[]
        {
            "AdaptiveDecisionSnapshots",
            "AdaptivePracticeShadowObservations",
            "StudentMisconceptionStates",
            "StudentRepresentationFluencyStates",
            "AdaptivePracticeSessions",
            "AssessmentAttempts"
        })
        {
            Assert.Contains(
                $"DELETE FROM \"{table}\";",
                source,
                StringComparison.Ordinal);
        }
    }

    [Fact]
    public void Rehearsal_provisioner_is_wired_into_database_bootstrap()
    {
        var source = Read(
            "src/Edulytics.Web/Bootstrap/EdulyticsDatabaseBootstrapper.cs");

        Assert.Contains(
            "MeetingDemoProvisioner.RunAsync",
            source,
            StringComparison.Ordinal);
    }

    [Fact]
    public void Rehearsal_reset_preserves_global_curriculum_and_roles()
    {
        var source = Read(
            "src/Edulytics.Web/Bootstrap/MeetingDemoProvisioner.cs");

        Assert.Contains(
            "DELETE FROM \"AspNetUsers\" WHERE \"SchoolId\" IS NOT NULL",
            source,
            StringComparison.Ordinal);
        Assert.Contains(
            "DELETE FROM \"CurriculumFrameworks\" WHERE \"OwnerSchoolId\" IS NOT NULL",
            source,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "DELETE FROM \"CurriculumFrameworks\";",
            source,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "DELETE FROM \"AspNetRoles\"",
            source,
            StringComparison.Ordinal);
    }

    private static string Read(string relative) =>
        File.ReadAllText(Path.Combine(Root, relative));

    private static string FindRoot()
    {
        for (
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            directory is not null;
            directory = directory.Parent)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;
        }

        throw new DirectoryNotFoundException("Repository root not found.");
    }
}
