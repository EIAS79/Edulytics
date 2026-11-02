using Edulytics.Core.Curriculum;
using Edulytics.Web.Presentation;
using Xunit;

namespace Edulytics.Tests.Phase29;

public sealed class Phase29FinalAcceptanceTests
{
    [Fact]
    public void CambridgeCoreSequence_IsNotMislabelledSupporting()
    {
        const string code =
            "PED:CAMBRIDGE-INTL-MATH:L7:SHARED:01:01:INTEGERS-AND-DIRECTED-NUMBER";

        Assert.True(
            CanonicalLessonRoleRegistry.TryGetIsSupporting(
                code,
                out var supporting));
        Assert.False(supporting);
    }

    [Fact]
    public void PolishNumberLesson_GetsDeterministicInstructionalVisual()
    {
        var items = LessonPresentationParser.Parse(
            "Lekcja dotyczy posługiwania się liczbami. Uczeń rozpoznaje relacje i sprawdza wynik działaniem odwrotnym.",
            sectionKind: "explanation");

        Assert.Contains(
            items,
            x => x.VisualType == LessonVisualType.NumberLine);
    }

    [Fact]
    public void GenericAlgebraLesson_GetsConceptFlowInsteadOfNoVisual()
    {
        var items = LessonPresentationParser.Parse(
            "Solve an algebraic expression by preserving equality and checking the result by substitution.",
            sectionKind: "explanation");

        Assert.Contains(
            items,
            x => x.VisualType == LessonVisualType.ConceptFlow);
    }

    [Fact]
    public void LessonLibraryView_PreservesContextAndShowsOnlyRequestedKpis()
    {
        var root = FindRepositoryRoot();
        var source = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Views/LessonContent/Index.cshtml"));

        Assert.Contains("asp-route-academicYearId", source, StringComparison.Ordinal);
        Assert.Contains("asp-route-academicProgramId", source, StringComparison.Ordinal);
        Assert.Contains("asp-route-curriculumAdoptionId", source, StringComparison.Ordinal);
        Assert.Contains("@L[\"TotalLessons\"]", source, StringComparison.Ordinal);
        Assert.Contains("@L[\"ProductionReady\"]", source, StringComparison.Ordinal);
        Assert.Contains("@L[\"SupportingLessons\"]", source, StringComparison.Ordinal);
        Assert.DoesNotContain("@L[\"Coverage\"]", source, StringComparison.Ordinal);
        Assert.DoesNotContain("@L[\"OfficiallyAligned\"]", source, StringComparison.Ordinal);
        Assert.Contains("var isSupporting = lesson.IsSupporting;", source, StringComparison.Ordinal);
    }

    [Fact]
    public void LessonDetailBackLink_PreservesExactLibraryContext()
    {
        var root = FindRepositoryRoot();
        var source = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Views/LessonContent/Detail.cshtml"));

        Assert.Contains("asp-route-academicYearId", source, StringComparison.Ordinal);
        Assert.Contains("asp-route-academicProgramId", source, StringComparison.Ordinal);
        Assert.Contains("asp-route-curriculumAdoptionId", source, StringComparison.Ordinal);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;
            directory = directory.Parent;
        }

        throw new InvalidOperationException("Repository root was not found.");
    }
}
