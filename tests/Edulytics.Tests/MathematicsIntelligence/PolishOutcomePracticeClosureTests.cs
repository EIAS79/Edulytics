using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class PolishOutcomePracticeClosureTests
{
    [Fact]
    public void Polish_map_resolves_all_306_official_outcomes()
    {
        Assert.Equal(306, PolishOutcomePracticeMapRegistry.All.Count);
        Assert.Equal(
            Enumerable.Range(1, 306),
            PolishOutcomePracticeMapRegistry.All.Values
                .Select(x => x.Serial)
                .OrderBy(x => x));
        Assert.All(
            PolishOutcomePracticeMapRegistry.All.Values,
            row =>
            {
                Assert.NotEmpty(row.TargetRules);
                Assert.NotEmpty(row.SourceLocator);
                Assert.NotEmpty(row.SourceContentHash);
            });
    }

    [Fact]
    public void All_1569_Polish_learner_lessons_resolve_exact_Practice_contracts()
    {
        var polishLessons = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .Where(x => x.PackCode == MathematicsCurriculumPackRegistry.PolandCode)
            .SelectMany(x => x.Lessons)
            .ToArray();

        Assert.Equal(1569, polishLessons.Length);
        Assert.All(
            polishLessons,
            lesson =>
            {
                Assert.Single(lesson.OutcomeCodes);
                Assert.True(
                    PolishOutcomePracticeMapRegistry.TryResolve(
                        lesson.OutcomeCodes[0],
                        out var mapping),
                    $"Missing Polish exact outcome mapping: {lesson.OutcomeCodes[0]}");
                Assert.NotNull(mapping);

                Assert.True(
                    LessonPracticeContractRegistry.TryResolve(
                        lesson.LessonCode,
                        out var contract),
                    $"Missing Polish Practice contract: {lesson.LessonCode}");
                Assert.NotNull(contract);
                Assert.Equal("READY_VERIFIED", contract!.Readiness);
                Assert.Equal(
                    "PolishOfficialOutcomeMap",
                    contract.SourceType);
                Assert.NotEmpty(contract.SkillIds);
                Assert.NotEmpty(contract.AllowedQuestionFamilies);
                Assert.All(
                    contract.AllowedQuestionFamilies,
                    family => Assert.True(
                        ExactSkillContractQuestionEngine.SupportsFamily(family),
                        $"Unsupported Polish family {family} for {lesson.LessonCode}"));
            });
    }

    [Fact]
    public void Polish_mapping_is_deterministic_and_title_independent()
    {
        var first = PolishOutcomePracticeMapRegistry.All.Values
            .OrderBy(x => x.Serial)
            .First();
        Assert.True(
            PolishOutcomePracticeMapRegistry.TryResolve(
                first.OutcomeCode,
                out var again));
        Assert.NotNull(again);
        Assert.Equal(first.Serial, again!.Serial);
        Assert.Equal(first.TargetRuleIds, again.TargetRuleIds);
    }
}
