using System.Text.RegularExpressions;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// G5 U03:L03 AUTHORING ONLY, TESTS AUTHORED_NOT_RUN.
/// The course-local candidate is NOT wired into the shared Practice engine.
/// </summary>
public sealed class UsCcssG5U03L03UnitFractionCandidateTests
{
    private static Dictionary<string, int> P(int scenario, int slot = 0, int reversed = 0) =>
        new(StringComparer.Ordinal)
        {
            ["case"] = scenario, ["slot"] = slot, ["reversed"] = reversed
        };

    [Theory]
    [InlineData(0, 0, "1")] // 4 columns × 2 rows = 1/8
    [InlineData(1, 1, "2")] // 4 × 3 = 1/12
    [InlineData(2, 2, "3")] // 4 × 4 = 1/16
    [InlineData(3, 0, "1")] // 4 × 5 = 1/20
    [InlineData(4, 1, "2")] // predicted next 4 × 6 = 1/24
    public void OriginalFourColumnPattern_UsesTheOriginalWhole(
        int sourceCase, int slot, string expected)
    {
        var p = P(sourceCase, slot);
        Assert.Equal(expected,
            UsCcssG5U03L03UnitFractionQuestions.Solve(
                UsCcssG5U03L03UnitFractionQuestions.PatternFamily, p));
        Assert.True(UsCcssG5U03L03UnitFractionQuestions.Verify(
            UsCcssG5U03L03UnitFractionQuestions.PatternFamily, p, expected));
        Assert.False(UsCcssG5U03L03UnitFractionQuestions.Verify(
            UsCcssG5U03L03UnitFractionQuestions.PatternFamily, p, "0"));
    }

    [Theory]
    [InlineData(0, 0, "1")] // source 1/5 × 1/3 = 1/15
    [InlineData(1, 1, "2")] // source 1/2 × 1/6 = 1/12
    [InlineData(2, 2, "3")] // source 1/4 × 1/6 = 1/24
    public void SourceActivityTwo_ExactProductsHaveBoundedChoices(
        int sourceCase, int slot, string expected)
    {
        var family = UsCcssG5U03L03UnitFractionQuestions.EquationFamily;
        var p = P(sourceCase, slot);
        Assert.Equal(expected, UsCcssG5U03L03UnitFractionQuestions.Solve(family, p));
        Assert.True(UsCcssG5U03L03UnitFractionQuestions.Verify(family, p, expected));
        Assert.False(UsCcssG5U03L03UnitFractionQuestions.Verify(family, p,
            expected == "1" ? "2" : "1"));
        var reversed = P(sourceCase, slot, 1);
        Assert.Equal(expected, UsCcssG5U03L03UnitFractionQuestions.Solve(family, reversed));
    }

    [Fact]
    public void MalformedForeignAndOutOfDomainParametersFailClosed()
    {
        foreach (var family in new[]
        {
            UsCcssG5U03L03UnitFractionQuestions.PatternFamily,
            UsCcssG5U03L03UnitFractionQuestions.EquationFamily
        })
        {
            foreach (var p in new[]
            {
                P(-1), P(5), P(0, 3), P(0, -1), P(0, 0, 2), P(0, 0, -1)
            })
                Assert.False(UsCcssG5U03L03UnitFractionQuestions.Verify(family, p, "1"));
            var extra = P(0);
            extra["otherLesson"] = 1;
            Assert.False(UsCcssG5U03L03UnitFractionQuestions.Verify(family, extra, "1"));
            Assert.False(UsCcssG5U03L03UnitFractionQuestions.Verify(family, P(0), "1/8"));
            Assert.False(UsCcssG5U03L03UnitFractionQuestions.Verify(family, P(0), " 1"));
            Assert.False(UsCcssG5U03L03UnitFractionQuestions.Verify(family, null, "1"));
        }
        Assert.False(UsCcssG5U03L03UnitFractionQuestions.Supports(
            UsCcssG5U03L03UnitFractionQuestions.PatternFamily + ".foreign"));
    }

    private const string Lesson = "PED:US-CCSS-MATH:G5:U03:L03";

    [Fact]
    public void SourceSpecificFamiliesAreSelectedThroughActualLessonPracticeContract()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Equal("supporting.fractions.multiply_divide", contract.SkillId);
        Assert.Equal(new[]
        {
            UsCcssG5U03L03UnitFractionQuestions.PatternFamily,
            UsCcssG5U03L03UnitFractionQuestions.EquationFamily
        }, contract.AllowedQuestionFamilies);

        var engine = new ExactSkillContractQuestionEngine();
        var actual = engine.Generate("g5-u03-l03-stage-d-exact",
            Lesson, contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard, 4, 50303, Array.Empty<string>());
        Assert.Equal(4, actual.Count);
        foreach (var question in actual)
        {
            Assert.Contains(question.Family, contract.AllowedQuestionFamilies);
            Assert.Equal(3, question.Parameters.Count);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                question.Family, question.Parameters, question.CorrectAnswer));
            Assert.Contains("ORIGINAL", question.Prompt, StringComparison.Ordinal);
            Assert.Contains("TEACHER_REVIEW_REQUIRED", question.Prompt, StringComparison.Ordinal);
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                question.Family, question.Parameters, "not-a-selected-choice"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                question.Family, question.Parameters, " 1"));
        }

        Assert.DoesNotContain(contract.AllowedQuestionFamilies,
            family => family.StartsWith("usccss.g5.u03.l01.", StringComparison.Ordinal));
    }

    [Fact]
    public void EverySourcePatternPresentsThreeDistinctOptionsWithExactlyOneCorrectFraction()
    {
        var seenFourByFour = false;
        for (var seed = 0; seed < 180; seed++)
        {
            var item = UsCcssG5U03L03UnitFractionQuestions.Build(
                UsCcssG5U03L03UnitFractionQuestions.PatternFamily, new Random(seed));
            var match = Regex.Match(item.Prompt,
                @"Select 1: (1/\d+); 2: (1/\d+); 3: (1/\d+)\.");
            Assert.True(match.Success, item.Prompt);
            var options = new[]
            {
                match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value
            };
            Assert.Equal(3, options.Distinct(StringComparer.Ordinal).Count());
            var rowCount = item.Parameters["case"] + 2;
            var expected = $"1/{4 * rowCount}";
            Assert.Equal(expected, options[int.Parse(
                UsCcssG5U03L03UnitFractionQuestions.Solve(item.Family, item.Parameters)) - 1]);
            if (rowCount == 4)
                seenFourByFour = true;
        }
        Assert.True(seenFourByFour, "The original 4 × 4 source diagram must be exercised");
    }

    [Fact]
    public void WrittenSourceReasoningRemainsTeacherReviewed()
    {
        foreach (var family in new[]
        {
            UsCcssG5U03L03UnitFractionQuestions.PatternFamily,
            UsCcssG5U03L03UnitFractionQuestions.EquationFamily
        })
        {
            var prompt = UsCcssG5U03L03UnitFractionQuestions.Build(family,new Random(7));
            Assert.Contains("ORIGINAL", prompt.Prompt);
            Assert.Contains("TEACHER_REVIEW_REQUIRED", prompt.Prompt);
            Assert.Contains("not an assessment of the open source drawing", prompt.Solution);
        }
    }
}
