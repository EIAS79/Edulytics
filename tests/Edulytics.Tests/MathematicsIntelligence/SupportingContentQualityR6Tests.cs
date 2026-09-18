using System.Text.Json;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class SupportingContentQualityR6Tests
{
    private const string LessonCode =
        "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:BUILD";

    [Fact]
    public void TwoUnknownBuildLessonHasTargetSpecificWorkedEvidence()
    {
        var root = FindRoot();
        var path = Path.Combine(
            root,
            "src/Edulytics.Core/Curriculum/LessonContent/Packs/cambridge-primary-stage6-dfe-ogl-v1.lesson-content-pack.json");
        using var document = JsonDocument.Parse(File.ReadAllText(path));

        var lesson = document.RootElement
            .GetProperty("Lessons")
            .EnumerateArray()
            .Single(x => x.GetProperty("LessonCode").GetString() == LessonCode);
        var translation = lesson.GetProperty("Translations")[0];

        var worked = translation.GetProperty("WorkedExamples").GetString() ?? string.Empty;
        var solution = translation.GetProperty("StepByStepSolutions").GetString() ?? string.Empty;
        var mistakes = translation.GetProperty("CommonMistakes").GetString() ?? string.Empty;

        Assert.Contains("x + y = 34", worked, StringComparison.Ordinal);
        Assert.Contains("y − x = 6", worked, StringComparison.Ordinal);
        Assert.Contains("x + y = 36", worked, StringComparison.Ordinal);
        Assert.Contains("two relationships", solution, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("only one relationship", mistakes, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(
            "2adf8826715181fdba4a06917bda0ab48eda6afe22d315f0c360cf9958b51a2c",
            lesson.GetProperty("CanonicalBodySha256").GetString());
    }

    [Fact]
    public void ApprovedRuntimeCorrectionIncludesTwoUnknownBuildLesson()
    {
        Assert.Equal(
            LessonCode,
            CambridgePrimaryStage6LessonContentCorrections.TwoUnknownsBuildLessonCode);
    }

    [Fact]
    public void RepairedLessonNowHasExactPracticeContract()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(LessonCode, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("algebra.relationships.two_unknowns", contract!.SkillId);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Equal(
            new[] { "algebra.relationships.two_unknowns.total_difference" },
            contract.AllowedQuestionFamilies);
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
