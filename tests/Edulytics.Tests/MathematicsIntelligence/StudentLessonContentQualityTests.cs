using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class StudentLessonContentQualityTests
{
    private const string ScaleBuildCode =
        "PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:BUILD";

    [Fact]
    public void ReadingScalesBuildLessonIsLearnerFacingAndTargetSpecific()
    {
        var lesson = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .SelectMany(document => document.Lessons)
            .Single(lesson => string.Equals(
                lesson.LessonCode,
                ScaleBuildCode,
                StringComparison.Ordinal));

        var english = Assert.Single(
            lesson.Translations.Where(translation =>
                translation.CultureCode.StartsWith(
                    "en",
                    StringComparison.OrdinalIgnoreCase)));

        Assert.Contains(
            "equal intervals",
            english.Explanation,
            StringComparison.OrdinalIgnoreCase);
        Assert.Contains(
            "60 − 20 = 40",
            english.WorkedExamples,
            StringComparison.Ordinal);
        Assert.Contains(
            "40 ÷ 4 = 10",
            english.WorkedExamples,
            StringComparison.Ordinal);
        Assert.Contains(
            "Count spaces",
            english.KeyConceptsAndRules,
            StringComparison.OrdinalIgnoreCase);

        var combined = string.Join(
            " ",
            english.Explanation,
            english.KeyConceptsAndRules,
            english.WorkedExamples,
            english.StepByStepSolutions,
            english.CommonMistakes,
            english.QuickSummary);

        Assert.DoesNotContain(
            "A digit's value depends on its position",
            combined,
            StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(
            "6,203,405",
            combined,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "Compose and decompose numbers with place-value units",
            combined,
            StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(
            "Always state what each number, unit, operation or geometric property represents before calculating",
            combined,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ReadingScalesBuildLessonAndPracticeResolveToTheSameExactTarget()
    {
        const string title =
            "Reading scales with 2, 4, 5 or 10 intervals: Build the Idea";

        Assert.True(
            SupportingPracticeTargetRuleRegistry.TryResolve(
                ScaleBuildCode,
                title,
                out var rule));
        Assert.NotNull(rule);
        Assert.Equal(
            "measurement.scale.read_equal_intervals",
            rule!.SkillId);
        Assert.Contains(
            "measurement.scale.equal_intervals.read_value",
            rule.Families);

        Assert.True(
            LessonPracticeCapabilityResolver.TryResolve(
                ScaleBuildCode,
                out var contract));
        Assert.NotNull(contract);
        Assert.Equal(rule.SkillId, contract!.SkillId);
        Assert.Contains(
            "measurement.scale.equal_intervals.read_value",
            contract.AllowedQuestionFamilies);
    }

    [Fact]
    public void CommonDenominationBuildLessonAndPracticeResolveToTheSameExactTarget()
    {
        const string lessonCode =
            "PED:CAMBRIDGE-INTL-MATH:S6:6F-2:BUILD";
        const string title =
            "Express fractions in a common denomination: Build the Idea";

        Assert.True(
            SupportingPracticeTargetRuleRegistry.TryResolve(
                lessonCode,
                title,
                out var rule));
        Assert.NotNull(rule);

        Assert.True(
            LessonPracticeCapabilityResolver.TryResolve(
                lessonCode,
                out var contract));
        Assert.NotNull(contract);

        Assert.Equal(
            "fractions.compare.unlike_denominators",
            rule!.SkillId);
        Assert.Equal(rule.SkillId, contract!.SkillId);
        Assert.Contains(
            "fractions.compare.unlike.common_denominator",
            contract.AllowedQuestionFamilies);
    }
}
