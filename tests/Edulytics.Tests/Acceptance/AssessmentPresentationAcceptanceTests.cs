namespace Edulytics.Tests.Acceptance;

public sealed class AssessmentPresentationAcceptanceTests
{
    [Fact]
    public void AssessmentDetails_RendersCompactOutcomeCodeWithoutVisibleRawIdentity()
    {
        var details = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "Assessments", "Details.cshtml");

        Assert.Contains(
            "LearningOutcomePresentation.DisplayCode(outcome.Code)",
            details,
            StringComparison.Ordinal);
        Assert.Contains(
            "data-code=\"@outcome.Code\"",
            details,
            StringComparison.Ordinal);
        Assert.Contains(
            "data-display-code=\"@displayCode\"",
            details,
            StringComparison.Ordinal);
        Assert.Contains(
            "<strong>@displayCode</strong>",
            details,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "<strong>@outcome.Code</strong>",
            details,
            StringComparison.Ordinal);
    }

    [Fact]
    public void TeacherAssessmentListAndDetails_ShowPersistedDeliveryMode()
    {
        var index = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "Assessments", "Index.cshtml");
        var details = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "Assessments", "Details.cshtml");

        Assert.Contains(
            "item.DeliveryMode == AssessmentDeliveryMode.Online",
            index,
            StringComparison.Ordinal);
        Assert.Contains(
            "assessment.DeliveryMode == AssessmentDeliveryMode.Online",
            details,
            StringComparison.Ordinal);
        Assert.Contains("DeliveryOnlineBadge", index, StringComparison.Ordinal);
        Assert.Contains("DeliveryOfflineBadge", index, StringComparison.Ordinal);
        Assert.Contains("DeliveryOnlineBadge", details, StringComparison.Ordinal);
        Assert.Contains("DeliveryOfflineBadge", details, StringComparison.Ordinal);
        Assert.Contains("CurrentDelivery", index, StringComparison.Ordinal);
        Assert.Contains("CurrentDelivery", details, StringComparison.Ordinal);
        Assert.Contains("ed-delivery-badge", index, StringComparison.Ordinal);
        Assert.Contains("ed-delivery-badge", details, StringComparison.Ordinal);
    }

    [Fact]
    public void PersistedDeliveryBadge_HasDistinctOnlineAndOfflinePresentation()
    {
        var css = ReadRepositoryFile(
            "src", "Edulytics.Web", "wwwroot", "css", "acceptance-corrective.css");

        Assert.Contains(".ed-delivery-badge", css, StringComparison.Ordinal);
        Assert.Contains(".ed-delivery-badge.is-online", css, StringComparison.Ordinal);
        Assert.Contains(".ed-delivery-badge.is-offline", css, StringComparison.Ordinal);
    }

    private static string ReadRepositoryFile(params string[] relativeSegments)
    {
        var root = FindRoot();
        return File.ReadAllText(Path.Combine([root, .. relativeSegments]));
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null &&
               !File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
            ?? throw new InvalidOperationException("Repository root not found.");
    }
}
