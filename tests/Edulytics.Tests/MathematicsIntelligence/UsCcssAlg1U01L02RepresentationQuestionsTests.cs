using System;
using System.Collections.Generic;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// ALG1 U01:L02 course-only numeric adapter fixtures, AUTHORED_NOT_RUN.
/// This does NOT test or assert shared selected dispatch, saved teacher grades,
/// actual rendered student graphics, or Stage D/CI execution.
/// </summary>
public sealed class UsCcssAlg1U01L02RepresentationQuestionsTests
{
    private static IReadOnlyDictionary<string, int> Params(int dataSet, int selection) =>
        new Dictionary<string, int> { ["dataset"] = dataSet, ["selection"] = selection };

    [Theory]
    [InlineData(0, 0, "2")]
    [InlineData(0, 1, "3.5")]
    [InlineData(0, 2, "5.5")]
    [InlineData(0, 3, "7.5")]
    [InlineData(0, 4, "10")]
    [InlineData(1, 0, "3")]
    [InlineData(1, 1, "4")]
    [InlineData(1, 2, "6")]
    [InlineData(1, 3, "8.5")]
    [InlineData(1, 4, "10")]
    public void FiveNumberSummaryUsesExplicitHalvesConvention(
        int dataset, int component, string expected)
    {
        var parameters = Params(dataset, component);
        Assert.Equal(expected,
            UsCcssAlg1U01L02RepresentationQuestions.Solve(
                UsCcssAlg1U01L02RepresentationQuestions.FiveNumberFamily, parameters));
        Assert.True(UsCcssAlg1U01L02RepresentationQuestions.Verify(
            UsCcssAlg1U01L02RepresentationQuestions.FiveNumberFamily, parameters, expected));
    }

    [Theory]
    [InlineData(0, 0, "3")]
    [InlineData(0, 1, "3")]
    [InlineData(0, 2, "3")]
    [InlineData(0, 3, "2")]
    [InlineData(0, 4, "1")]
    [InlineData(1, 0, "3")]
    [InlineData(1, 1, "1")]
    [InlineData(1, 2, "2")]
    [InlineData(1, 3, "2")]
    public void HistogramUsesNonOverlappingHalfOpenWidthTwoBins(
        int dataset, int bin, string expected)
    {
        Assert.Equal(expected,
            UsCcssAlg1U01L02RepresentationQuestions.Solve(
                UsCcssAlg1U01L02RepresentationQuestions.HistogramFamily, Params(dataset, bin)));
    }

    [Theory]
    [InlineData(0, 1, "2")]
    [InlineData(0, 3, "2")]
    [InlineData(1, 1, "2")]
    public void DotCountsFrequencyAtTheNamedDistinctValue(
        int dataset, int index, string expected)
    {
        Assert.Equal(expected,
            UsCcssAlg1U01L02RepresentationQuestions.Solve(
                UsCcssAlg1U01L02RepresentationQuestions.DotFamily, Params(dataset, index)));
    }

    [Theory]
    [InlineData(" 7/2 ", true)]
    [InlineData("3.50", true)]
    [InlineData("14/4", true)]
    [InlineData("7/0", false)]
    [InlineData("NaN", false)]
    [InlineData("3.5 minutes", false)]
    [InlineData("7/2/1", false)]
    [InlineData("", false)]
    public void ExactQuartileAcceptsNumericEquivalenceButRejectsMalformed(
        string response, bool shouldAccept)
    {
        Assert.Equal(shouldAccept,
            UsCcssAlg1U01L02RepresentationQuestions.Verify(
                UsCcssAlg1U01L02RepresentationQuestions.FiveNumberFamily, Params(0, 1), response));
    }

    [Fact]
    public void WrongFamilyOrForgedParametersFailClosed()
    {
        Assert.False(UsCcssAlg1U01L02RepresentationQuestions.Verify(
            "usccss.alg1.u01.l01.statistical_questions_p3", Params(0, 1), "3.5"));
        Assert.False(UsCcssAlg1U01L02RepresentationQuestions.Verify(
            UsCcssAlg1U01L02RepresentationQuestions.FiveNumberFamily, Params(3, 1), "3.5"));
        Assert.False(UsCcssAlg1U01L02RepresentationQuestions.Verify(
            UsCcssAlg1U01L02RepresentationQuestions.HistogramFamily, Params(0, 9), "1"));
    }

    [Fact]
    public void LearnerPromptShowsDatasetAndTeacherBoundaryWithoutLeakingTheAnswer()
    {
        var question = UsCcssAlg1U01L02RepresentationQuestions.Build(
            UsCcssAlg1U01L02RepresentationQuestions.HistogramFamily, new Random(20261031));
        Assert.True(question.Prompt.Contains("teacher review", StringComparison.OrdinalIgnoreCase));
        Assert.True(question.Prompt.Contains("right endpoint", StringComparison.OrdinalIgnoreCase));
        Assert.True(question.Solution.Contains("teacher", StringComparison.OrdinalIgnoreCase));
        Assert.NotEmpty(question.Parameters);
    }
}
