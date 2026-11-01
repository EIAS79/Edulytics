using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Executable Stage D selected Practice mathematics for G5 U03:L02.
/// This is NOT proof of authenticated browser rendering, persisted freehand
/// drawings, signed educator scoring, or use of the publisher's gated artwork.
/// </summary>
public sealed class UsCcssG5U03L02SelectedPracticeRuntimeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G5:U03:L02";
    private const string Product = UsCcssG5U03L02UnitFractionQuestions.ProductFamily;
    private const string Comparison = UsCcssG5U03L02UnitFractionQuestions.CompareFamily;
    private static readonly (int First, int Second)[] Cases =
        [(3, 4), (4, 3), (2, 5), (5, 2)];

    private static Dictionary<string, int> Params(
        int scenario, int layout = 0, int slot = 0,
        int comparison = 0, int differentWhole = 0) =>
        new(StringComparer.Ordinal)
        {
            ["case"] = scenario,
            ["layout"] = layout,
            ["slot"] = slot,
            ["comparison"] = comparison,
            ["differentWhole"] = differentWhole
        };

    [Fact]
    public void RegisteredLessonCanGenerateSolveAndVerifyTenDistinctSelectedItems()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Equal("supporting.fractions.multiply_divide", contract.SkillId);
        Assert.Equal(new[] { Product, Comparison }, contract.AllowedQuestionFamilies);
        Assert.All(contract.AllowedQuestionFamilies,
            family => Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family)));

        var questions = new ExactSkillContractQuestionEngine().Generate(
            "g5-u03-l02-stage-d-mathematical-scope",
            Lesson, contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard, 10, 50302, []);
        Assert.Equal(10, questions.Count);
        Assert.Equal(10, questions.Select(q => q.ExposureFingerprint).Distinct().Count());
        foreach (var question in questions)
        {
            Assert.Contains(question.Family, contract.AllowedQuestionFamilies);
            Assert.Equal(5, question.Parameters.Count);
            var independentlyExpected = IndependentAnswer(question.Family, question.Parameters);
            Assert.Equal(independentlyExpected, question.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                question.Family, question.Parameters, independentlyExpected));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                question.Family, question.Parameters, "not-an-option"));
            Assert.Contains("Teacher separately", question.Prompt);
        }
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(0, 2)]
    [InlineData(1, 3)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    public void AllSourceProductsUseTheOriginalWholeNotSelectedSubpart(int scenario, int slot)
    {
        var p = Params(scenario, slot: slot);
        var correct = (slot + 1).ToString(System.Globalization.CultureInfo.InvariantCulture);
        Assert.Equal(correct, UsCcssG5U03L02UnitFractionQuestions.Solve(Product, p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Product, p, correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Product, p, "1/12"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Product, p, " 1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Product, p, ""));
    }

    [Fact]
    public void EveryComparedPairAndLayoutMustRetainSameOriginalWhole()
    {
        for (var first = 0; first < Cases.Length; first++)
        for (var second = 0; second < Cases.Length; second++)
        for (var layout = 0; layout < 4; layout++)
        for (var differentWhole = 0; differentWhole < 2; differentWhole++)
        {
            var p = Params(first, layout: layout,
                comparison: second, differentWhole: differentWhole);
            var expected = (differentWhole == 0 &&
                Cases[first].First * Cases[first].Second ==
                Cases[second].First * Cases[second].Second) ? "1" : "0";
            Assert.Equal(expected, IndependentAnswer(Comparison, p));
            Assert.Equal(expected, UsCcssG5U03L02UnitFractionQuestions.Solve(Comparison, p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(Comparison, p, expected));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                Comparison, p, expected == "1" ? "0" : "1"));
        }
    }

    [Fact]
    public void InvalidParametersUnknownFamiliesAndOtherLessonsFailClosed()
    {
        foreach (var family in new[] { Product, Comparison })
        {
            foreach (var bad in new[]
            {
                Params(-1), Params(4), Params(0, layout: -1),
                Params(0, layout: 4), Params(0, slot: 3),
                Params(0, comparison: 4), Params(0, differentWhole: 2)
            })
                Assert.False(ExactSkillContractQuestionEngine.Verify(family, bad, "1"));

            var injected = Params(0);
            injected["crossLesson"] = 1;
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, injected, "1"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, Params(0), " 1"));
        }
        Assert.False(ExactSkillContractQuestionEngine.SupportsFamily(
            "usccss.g5.u03.l02.unregistered"));
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G5:U03:L01", out _));
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G5:U03:L03", out _));
    }

    private static string IndependentAnswer(string family,
        IReadOnlyDictionary<string, int> p)
    {
        if (family == Product)
            return (p["slot"] + 1).ToString(
                System.Globalization.CultureInfo.InvariantCulture);
        if (family == Comparison)
            return (p["differentWhole"] == 0 &&
                Cases[p["case"]].First * Cases[p["case"]].Second ==
                Cases[p["comparison"]].First * Cases[p["comparison"]].Second)
                ? "1" : "0";
        throw new InvalidOperationException("Unrecognized lesson family");
    }
}
