using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade 9 Stage D: all 132 actual lesson-scoped Practice families must
/// generate and grade through the registered exact engine, without fallback.
/// This is deterministic technical evidence, not human teacher approval or HTTPS E2E.
/// </summary>
public sealed class UsCcssAlgebra1All132ExactPracticeStageDTests
{
    private const string Prefix = "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:";

    public static IEnumerable<object[]> AllGrade9Lessons()
    {
        var counts = new[] { 16, 26, 10, 18, 21, 17, 24 };
        var ordinal = 0;
        for (var unit = 1; unit <= counts.Length; unit++)
            for (var lesson = 1; lesson <= counts[unit - 1]; lesson++)
                yield return new object[]
                {
                    ++ordinal,
                    Prefix + $"U{unit:D2}:L{lesson:D2}"
                };
    }

    [Theory]
    [MemberData(nameof(AllGrade9Lessons))]
    public void EachLessonRoutesToVerifiedExactGeneratorWithCorrectAndMalformedAnswers(
        int ordinal, string lessonCode)
    {
        var rich = RichLessonContentV2Registry.Find(lessonCode, "en");
        Assert.NotNull(rich);
        Assert.Equal(132, AllGrade9Lessons().Count());

        Assert.True(LessonPracticeContractRegistry.TryResolve(lessonCode, out var contract),
            $"Missing exact lesson-scoped Practice contract #{ordinal}: {lessonCode}");
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.NotEmpty(contract.AllowedQuestionFamilies);

        var engine = new ExactSkillContractQuestionEngine();
        foreach (var (family, index) in contract.AllowedQuestionFamilies
                     .Select((value, index) => (value, index)))
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family),
                $"No exact generator for {lessonCode} / {family}");
            var generated = engine.Generate(
                "stage18", lessonCode, [family],
                ExactSkillQuestionDifficulty.Standard, 1, 900000 + ordinal * 101 + index, []);
            var question = Assert.Single(generated);
            Assert.Equal(family, question.Family);
            Assert.False(string.IsNullOrWhiteSpace(question.Prompt));
            Assert.False(string.IsNullOrWhiteSpace(question.CorrectAnswer));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family, question.Parameters, question.CorrectAnswer),
                $"Generated right answer does not grade correctly: {lessonCode}, {family}");
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, question.Parameters, "NOT_VALID_ANSWER___!"),
                $"Invalid/malformed answer accepted: {lessonCode}, {family}");
        }
    }
}
