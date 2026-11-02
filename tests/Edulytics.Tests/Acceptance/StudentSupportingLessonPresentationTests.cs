using Edulytics.Web.Presentation;

namespace Edulytics.Tests.Acceptance;

public sealed class StudentSupportingLessonPresentationTests
{
    [Fact]
    public void StudentLessonView_DoesNotRenderSupportingGovernanceUi()
    {
        var studentView = Read(
            "src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml");

        Assert.DoesNotContain(
            "SupportingLessonHelp",
            studentView,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "lesson-supporting-message",
            studentView,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "lesson-content-status--supporting",
            studentView,
            StringComparison.Ordinal);
    }

    [Fact]
    public void StaffLessonView_RetainsSupportingGovernanceMetadata()
    {
        var staffView = Read(
            "src/Edulytics.Web/Views/LessonContent/Detail.cshtml");

        Assert.Contains(
            "SupportingLessonHelp",
            staffView,
            StringComparison.Ordinal);
        Assert.Contains(
            "lesson-supporting-message",
            staffView,
            StringComparison.Ordinal);
    }

    [Fact]
    public void StudentCopyFilter_RemovesSupportingAndProvenanceDisclaimers()
    {
        const string source =
            "This Cambridge Primary Stage 6 supporting lesson develops the open DfE Year 6 ready-to-progress focus “Quantify additive and multiplicative relationships”. " +
            "Quantifying a relationship means expressing how quantities are connected by exact operations. " +
            "This Supporting lesson remains pedagogical content and does not create or imply an official curriculum OutcomeCode. " +
            "The lesson is Edulytics-authored from OGL material; Cambridge remains the academic reference authority and no Cambridge objective wording is reproduced here.";

        var cleaned = StudentLessonPresentationFilter.Clean(source);

        Assert.Contains(
            "This lesson develops the open DfE Year 6 ready-to-progress focus",
            cleaned,
            StringComparison.Ordinal);
        Assert.Contains(
            "Quantifying a relationship means expressing how quantities are connected by exact operations.",
            cleaned,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "supporting lesson",
            cleaned,
            StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(
            "OutcomeCode",
            cleaned,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "no Cambridge objective wording",
            cleaned,
            StringComparison.OrdinalIgnoreCase);
    }

    private static string Read(string relativePath)
    {
        var root = FindRoot();
        return File.ReadAllText(Path.Combine(root, relativePath));
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
            ?? throw new DirectoryNotFoundException("Edulytics solution root not found.");
    }
}
