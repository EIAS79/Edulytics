using System.Text.Json;
using Edulytics.Core.Assessments;
using Edulytics.Core.Entities;
using Edulytics.Core.Mathematics.Assessment;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Assessments;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Stage19AssessmentBuilderMigrationTests
{
    [Fact]
    public void ManifestMatchesRuntimeAssessmentSkillContracts()
    {
        var root = FindRoot();
        using var manifest = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/stage19-assessment-builder-migration-manifest.v1.json")));

        var manifestEntries = manifest.RootElement
            .GetProperty("entries")
            .EnumerateArray()
            .ToDictionary(
                x => x.GetProperty("outcomeCode").GetString()!,
                x => x,
                StringComparer.OrdinalIgnoreCase);

        Assert.Equal(4, manifestEntries.Count);
        Assert.Equal(manifestEntries.Count, Stage19AssessmentSkillContracts.All.Count);

        foreach (var contract in Stage19AssessmentSkillContracts.All)
        {
            Assert.True(manifestEntries.TryGetValue(contract.OutcomeCode, out var entry));
            Assert.Equal(contract.SkillId, entry.GetProperty("skillId").GetString());

            var families = entry.GetProperty("allowedQuestionFamilies")
                .EnumerateArray()
                .Select(x => x.GetString()!)
                .ToArray();
            var lessons = entry.GetProperty("evidenceLessonCodes")
                .EnumerateArray()
                .Select(x => x.GetString()!)
                .ToArray();

            Assert.Equal(
                contract.AllowedQuestionFamilies.OrderBy(x => x),
                families.OrderBy(x => x));
            Assert.Equal(
                contract.EvidenceLessonCodes.OrderBy(x => x),
                lessons.OrderBy(x => x));
        }
    }

    [Fact]
    public void AmbiguousOperationsOutcomeIsNotPromotedToFractionSkillContract()
    {
        Assert.False(Stage19AssessmentSkillContracts.TryResolve(
            "CCSS:3.OA.B.5",
            out _));
    }

    [Fact]
    public void EveryStage19AllowedFamilyGeneratesAndIndependentlyVerifies()
    {
        var generatedFamilies = new HashSet<string>(StringComparer.Ordinal);
        var expectedFamilies = Stage19AssessmentSkillContracts.All
            .SelectMany(x => x.AllowedQuestionFamilies)
            .ToHashSet(StringComparer.Ordinal);

        var seed = 1900;
        foreach (var contract in Stage19AssessmentSkillContracts.All)
        {
            var outcome = new LearningOutcome
            {
                Id = Guid.NewGuid(),
                SchoolId = Guid.NewGuid(),
                AcademicProgramId = Guid.NewGuid(),
                FrameworkVersionId = Guid.NewGuid(),
                SubjectId = Guid.NewGuid(),
                GradeLevelId = Guid.NewGuid(),
                CurriculumAdoptionId = Guid.NewGuid(),
                TopicId = Guid.NewGuid(),
                Code = contract.OutcomeCode,
                Description = "Stage 19 exact assessment outcome",
                Weight = 1m,
                Order = 1
            };

            var engine = new Stage19AssessmentSkillContractEngine();
            var batch = engine.Generate(
                outcome.SchoolId,
                outcome.CurriculumAdoptionId!.Value,
                "CCSS",
                outcome,
                contract,
                AssessmentBuilderDifficulty.Stretch,
                contract.AllowedQuestionFamilies.Count,
                seed++,
                [],
                Guid.NewGuid());

            Assert.Equal(contract.AllowedQuestionFamilies.Count, batch.Items.Count);
            Assert.Equal(Stage19AssessmentSkillContracts.GenerationMethod, batch.GeneratorVersion);

            foreach (var generated in batch.Items)
            {
                Assert.Equal(outcome.Id, generated.OutcomeLink.LearningOutcomeId);
                Assert.Equal(generated.Item.Id, generated.OutcomeLink.AssessmentItemId);
                Assert.Equal(Stage19AssessmentSkillContracts.GenerationMethod, generated.Item.GenerationMethod);
                Assert.Contains(generated.Item.GenerationFamily!, contract.AllowedQuestionFamilies);
                Assert.True(Stage19AssessmentSkillContractEngine.VerifyPersistedItem(
                    contract,
                    generated.Item));
                Assert.Contains(
                    "\"solverVerified\":true",
                    generated.Item.ValidationMetadataJson,
                    StringComparison.Ordinal);
                Assert.Contains(
                    "\"broadFallbackUsed\":false",
                    generated.Item.ValidationMetadataJson,
                    StringComparison.Ordinal);
                Assert.Contains(
                    "\"teacherReviewRequired\":true",
                    generated.Item.ValidationMetadataJson,
                    StringComparison.Ordinal);
                generatedFamilies.Add(generated.Item.GenerationFamily!);
            }
        }

        Assert.Equal(
            expectedFamilies.OrderBy(x => x),
            generatedFamilies.OrderBy(x => x));
    }

    [Fact]
    public void PracticeAndAssessmentUseTheSameExactMathematicsKernel()
    {
        var root = FindRoot();
        var practiceEngine = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Practice/Stage18SkillContractPracticeEngine.cs"));
        var assessmentEngine = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Assessments/Stage19AssessmentSkillContractEngine.cs"));
        var sharedEngine = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs"));

        Assert.Contains("ExactSkillContractQuestionEngine().Generate", practiceEngine, StringComparison.Ordinal);
        Assert.Contains("ExactSkillContractQuestionEngine().Generate", assessmentEngine, StringComparison.Ordinal);
        Assert.Contains("private static string Solve", sharedEngine, StringComparison.Ordinal);
        Assert.Contains("public static bool Verify", sharedEngine, StringComparison.Ordinal);
        Assert.DoesNotContain("MathematicsQuestionGenerationEngine", assessmentEngine, StringComparison.Ordinal);
    }

    [Fact]
    public void AssessmentBuilderRoutesExactOutcomesBeforeLegacyAndFailsMixedRequestsClosed()
    {
        var root = FindRoot();
        var service = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Assessments/AssessmentBuilderService.cs"));

        var start = service.IndexOf(
            "private MathematicsGenerationBatch? TryGenerate(",
            StringComparison.Ordinal);
        var end = service.IndexOf(
            "private static MathematicsGenerationBatch? TryGenerateExact(",
            start,
            StringComparison.Ordinal);

        Assert.True(start >= 0 && end > start);
        var block = service[start..end];

        var exact = block.IndexOf(
            "Stage19AssessmentSkillContracts.TryResolve",
            StringComparison.Ordinal);
        var legacy = block.IndexOf(
            "NativeMathematicsOutcomeProfileResolver.Resolve",
            StringComparison.Ordinal);

        Assert.True(exact >= 0);
        Assert.True(legacy > exact);
        Assert.Contains(
            "if (exactContracts.Any(contract => contract is null))",
            block,
            StringComparison.Ordinal);
        Assert.Contains(
            "return TryGenerateExact(",
            block,
            StringComparison.Ordinal);
    }

    [Fact]
    public void TeacherWorkflowRemainsSelectGenerateReviewApprovePublish()
    {
        var root = FindRoot();
        var service = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Assessments/AssessmentBuilderService.cs"));
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/AssessmentBuilder/Index.cshtml"));

        Assert.Contains("name=\"outcomeIds\"", view, StringComparison.Ordinal);
        Assert.Contains("GenerateQuestionsWithAI", view, StringComparison.Ordinal);
        Assert.Contains("BuilderStatus", view, StringComparison.Ordinal);
        Assert.Contains("ApproveQuestionAsync", service, StringComparison.Ordinal);
        Assert.Contains("PublishAsync", service, StringComparison.Ordinal);
    }

    [Fact]
    public void Stage19FamiliesAreBackedByStage18VerifiedLessonFamilies()
    {
        var root = FindRoot();
        using var stage18 = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/stage18-practice-migration-manifest.v1.json")));

        var byLesson = stage18.RootElement
            .GetProperty("entries")
            .EnumerateArray()
            .ToDictionary(
                x => x.GetProperty("lessonCode").GetString()!,
                x => x,
                StringComparer.Ordinal);

        foreach (var contract in Stage19AssessmentSkillContracts.All)
        {
            var verifiedFamilies = contract.EvidenceLessonCodes
                .SelectMany(code => byLesson[code]
                    .GetProperty("allowedQuestionFamilies")
                    .EnumerateArray()
                    .Select(x => x.GetString()!))
                .ToHashSet(StringComparer.Ordinal);

            Assert.All(
                contract.AllowedQuestionFamilies,
                family => Assert.Contains(family, verifiedFamilies));
        }
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
