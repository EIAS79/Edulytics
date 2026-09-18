using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class SupportingLessonPracticeR1Tests
{
    [Fact]
    public void RegistryContainsAllSevenBaselineReadyVerifiedSupportingLessons()
    {
        var expected = new[]
        {
            "PED:CAMBRIDGE-INTL-MATH:S5:5F-2:APPLY",
            "PED:CAMBRIDGE-INTL-MATH:S5:5F-2:BUILD",
            "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:APPLY",
            "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:APPLY",
            "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:BUILD",
            "PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:APPLY",
            "PED:US-CCSS-MATH:G7:U06:L15"
        };

        Assert.Equal(7, LessonPracticeContractRegistry.All.Count);
        Assert.Equal(
            expected.OrderBy(x => x, StringComparer.Ordinal),
            LessonPracticeContractRegistry.All
                .Select(x => x.LessonCode)
                .OrderBy(x => x, StringComparer.Ordinal));

        Assert.All(LessonPracticeContractRegistry.All, contract =>
        {
            Assert.Equal("SupportingLesson", contract.SourceType);
            Assert.Equal("READY_VERIFIED", contract.Readiness);
            Assert.Equal(LessonPracticeContractRegistry.Version, contract.ContractVersion);
            Assert.False(string.IsNullOrWhiteSpace(contract.SkillId));
            Assert.NotEmpty(contract.AllowedQuestionFamilies);
        });
    }

    [Fact]
    public void EveryR1ContractGeneratesAndIndependentlyVerifiesExactPractice()
    {
        var engine = new Stage18SkillContractPracticeEngine();
        var seed = 2100;

        foreach (var practiceContract in LessonPracticeContractRegistry.All)
        {
            var legacyContract = practiceContract.ToLegacyStage18Contract();
            var items = engine.Generate(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                legacyContract,
                StudentPrivatePracticeDifficulty.Stretch,
                Math.Max(2, legacyContract.AllowedQuestionFamilies.Count),
                seed++,
                [],
                Guid.NewGuid());

            Assert.NotEmpty(items);
            Assert.All(items, item =>
            {
                Assert.Contains(
                    practiceContract.AllowedQuestionFamilies,
                    family => string.Equals(
                        family,
                        item.GenerationFamily,
                        StringComparison.Ordinal));
                Assert.True(
                    Stage18SkillContractPracticeEngine.VerifyPersistedItem(
                        legacyContract,
                        item));
                Assert.Contains(
                    "\"solverVerified\":true",
                    item.ValidationMetadataJson,
                    StringComparison.Ordinal);
                Assert.Contains(
                    "\"broadFallbackUsed\":false",
                    item.ValidationMetadataJson,
                    StringComparison.Ordinal);
            });
        }
    }

    [Fact]
    public void EfficientlySolvingInequalitiesUsesExactInequalityFamily()
    {
        Assert.True(
            LessonPracticeContractRegistry.TryResolve(
                "PED:US-CCSS-MATH:G7:U06:L15",
                out var practiceContract));
        Assert.NotNull(practiceContract);

        var legacyContract = practiceContract!.ToLegacyStage18Contract();
        var item = new Stage18SkillContractPracticeEngine()
            .Generate(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                legacyContract,
                StudentPrivatePracticeDifficulty.Challenge,
                1,
                7401,
                [],
                Guid.NewGuid())
            .Single();

        Assert.Equal(
            "algebra.linear.inequality.ax_plus_b_relation_c",
            item.GenerationFamily);
        Assert.True(item.CorrectAnswer.StartsWith("x", StringComparison.Ordinal));
        Assert.True(
            item.CorrectAnswer.Contains('<') ||
            item.CorrectAnswer.Contains('>') ||
            item.CorrectAnswer.Contains('≤') ||
            item.CorrectAnswer.Contains('≥'));
        Assert.True(
            Stage18SkillContractPracticeEngine.VerifyPersistedItem(
                legacyContract,
                item));
    }

    [Fact]
    public void StudentLessonUiUsesExactPracticeReadinessIndependentlyOfGameRouting()
    {
        var root = FindRoot();
        var portal = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/StudentPortalController.cs"));
        var practice = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/StudentPracticeController.cs"));
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"));

        Assert.Contains(
            "LessonPracticeContractRegistry.TryResolve",
            portal,
            StringComparison.Ordinal);
        Assert.Contains(
            "ExactPracticeAdoptionId",
            portal,
            StringComparison.Ordinal);
        Assert.Contains(
            "StartLessonPractice",
            practice,
            StringComparison.Ordinal);
        Assert.Contains(
            "ExactPracticeAdoptionId",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "asp-action=\"StartLessonPractice\"",
            view,
            StringComparison.Ordinal);
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
