using System.Text.RegularExpressions;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class PolishRichLessonContentV2ClosureTests
{
    private static readonly Regex ObviousEnglishProse = new(
        @"\b(the|find|calculate|evaluate|solve|use|then|divide|multiply|add|subtract|write|round|compare|order|which|what|how|where|when|number|value|digit|place|power|whole|quantity|fraction|denominator|numerator|angle|triangle|side|base|height|radius|area|volume|probability|mean|median|range|sequence|term|function|equation|expression|total|result|answer|from|between|with|without|into|by|of|and|or|is|are|has|have|each|every)\b",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

    [Fact]
    public void All_1569_Polish_lessons_compile_to_verified_localized_Rich_V2()
    {
        var lessons = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .Where(x => x.PackCode == MathematicsCurriculumPackRegistry.PolandCode)
            .SelectMany(x => x.Lessons)
            .OrderBy(x => x.LessonCode, StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(1569, lessons.Length);

        foreach (var lesson in lessons)
        {
            var translation = Assert.Single(
                lesson.Translations,
                x => x.CultureCode.StartsWith(
                    "pl",
                    StringComparison.OrdinalIgnoreCase));

            Assert.True(
                LessonPracticeContractRegistry.TryResolve(
                    lesson.LessonCode,
                    out var contract),
                $"Missing Polish Practice contract: {lesson.LessonCode}");
            Assert.NotNull(contract);
            Assert.Equal("READY_VERIFIED", contract!.Readiness);
            Assert.Equal("PolishOfficialOutcomeMap", contract.SourceType);

            var body = new CanonicalLessonTranslationRecord(
                translation.CultureCode,
                translation.Title,
                translation.Explanation,
                translation.KeyConceptsAndRules,
                translation.WorkedExamples,
                translation.StepByStepSolutions,
                translation.CommonMistakes,
                translation.QuickSummary);

            var rich = RichLessonContentV2RuntimeComposer.TryCompose(
                lesson.LessonCode,
                body);

            Assert.NotNull(rich);
            Assert.StartsWith(
                "pl",
                rich!.CultureCode,
                StringComparison.OrdinalIgnoreCase);
            Assert.True(rich.ExplanationParagraphs.Count >= 4);
            Assert.True(rich.KeyConcepts.Count >= 3);
            Assert.InRange(rich.WorkedExamples.Count, 4, 12);
            Assert.True(rich.CommonMistakes.Count >= 3);
            Assert.True(rich.SummaryPoints.Count >= 4);
            Assert.NotEmpty(rich.Visuals);

            Assert.All(
                rich.WorkedExamples,
                example =>
                {
                    Assert.True(example.Steps.Count >= 5);
                    Assert.Contains(
                        "Zweryfikowano",
                        example.Check,
                        StringComparison.Ordinal);
                });
        }
    }

    [Fact]
    public void Every_Polish_Practice_family_has_a_deterministic_Polish_math_presentation()
    {
        var families = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .Where(x => x.PackCode == MathematicsCurriculumPackRegistry.PolandCode)
            .SelectMany(x => x.Lessons)
            .Select(lesson =>
            {
                Assert.True(
                    LessonPracticeContractRegistry.TryResolve(
                        lesson.LessonCode,
                        out var contract));
                return contract!;
            })
            .SelectMany(x => x.AllowedQuestionFamilies)
            .Distinct(StringComparer.Ordinal)
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(138, families.Length);

        var engine = new ExactSkillContractQuestionEngine();

        for (var index = 0; index < families.Length; index++)
        {
            var family = families[index];
            var question = Assert.Single(
                engine.Generate(
                    "polish-rich-v2-localization-gate",
                    family,
                    [family],
                    ExactSkillQuestionDifficulty.Standard,
                    1,
                    20260923 + index,
                    []));

            var prompt = PolishMathematicsTextLocalizer.Localize(question.Prompt);
            var solution = PolishMathematicsTextLocalizer.Localize(question.Solution);
            var answer = PolishMathematicsTextLocalizer.LocalizeAnswer(question.CorrectAnswer);
            var label = PolishMathematicsTextLocalizer.FamilyLabel(family);

            Assert.False(string.IsNullOrWhiteSpace(prompt));
            Assert.False(string.IsNullOrWhiteSpace(solution));
            Assert.False(string.IsNullOrWhiteSpace(answer));
            Assert.False(string.IsNullOrWhiteSpace(label));

            var promptLeak = ObviousEnglishProse.Match(prompt);
            Assert.False(
                promptLeak.Success,
                $"English prose leaked for {family} prompt: {prompt}");

            var solutionLeak = ObviousEnglishProse.Match(solution);
            Assert.False(
                solutionLeak.Success,
                $"English prose leaked for {family} solution: {solution}");

            var labelLeak = ObviousEnglishProse.Match(label);
            Assert.False(
                labelLeak.Success,
                $"English prose leaked for {family} label: {label}");
        }
    }
}
