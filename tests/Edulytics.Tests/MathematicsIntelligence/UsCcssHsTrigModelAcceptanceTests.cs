using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Direct exact-kernel acceptance for five Coder 9 L03 source cases.
/// This is not a claim of authenticated HTTP/DB/renderer acceptance.
/// </summary>
public sealed class UsCcssHsTrigModelAcceptanceTests
{
    private const string Family = UsCcssHsFourthCourseQuestions.TrigModel;

    private static Dictionary<string, int> P(int period, int amplitude, int offset,
        int target, int mode, int inclusive = 0) =>
        new()
        {
            ["period"] = period,
            ["amplitude"] = amplitude,
            ["offset"] = offset,
            ["target"] = target,
            ["targetMode"] = mode,
            ["endInclusive"] = inclusive
        };

    [Fact]
    public void Coder9TwoRootsTwelveRejectsMissingAndExtraRoots()
    {
        var p = P(12, 4, 10, 12, 1);
        Assert.Equal("1,5", UsCcssHsFourthCourseQuestions.Solve(Family, p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, "1,5"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "1,5,13"));
    }

    [Fact]
    public void Coder9TwoRootsEightAcceptsExactFractionsAndDecimalSeconds()
    {
        var p = P(8, 6, 14, 17, 1);
        Assert.Equal("2/3,10/3", UsCcssHsFourthCourseQuestions.Solve(Family, p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, "2/3,10/3"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p,
            "0.6666666666666666,3.3333333333333335"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p,
            "0.6666666666666666,3.3333333333333335,8.666666666666666"));
    }

    [Fact]
    public void Coder9OpenRightEndpointExcludesRepeatedZeroRoot()
    {
        var p = P(10, 3, 8, 8, 0);
        Assert.Equal("0,5", UsCcssHsFourthCourseQuestions.Solve(Family, p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, "0,5"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "0,5,10"));
    }

    [Theory]
    [InlineData(9, "0,9/2")]
    [InlineData(10, "0,5")]
    [InlineData(11, "0,11/2")]
    [InlineData(13, "0,13/2")]
    public void ZeroCrossingsAreCorrectForNonQuarterMultiplePeriodsAndOpenEndpoint(
        int period, string expected)
    {
        var p = P(period, 3, 8, 8, 0);
        Assert.Equal(expected, UsCcssHsFourthCourseQuestions.Solve(Family, p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Family, p, expected + "," + period));
    }

    [Theory]
    [InlineData(9, "0,9/2,9")]
    [InlineData(10, "0,5,10")]
    [InlineData(11, "0,11/2,11")]
    public void ClosedRightEndpointIncludesTheDistinctPeriodEndRoot(
        int period, string expected)
    {
        var p = P(period, 3, 8, 8, 0, inclusive: 1);
        Assert.Equal(expected, UsCcssHsFourthCourseQuestions.Solve(Family, p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Family, p, expected[..expected.LastIndexOf(',')]));
    }

    [Fact]
    public void NonQuarterMultiplePeriodsPreserveAllNegativeHalfAmplitudeSolutions()
    {
        var p = P(10, 4, 8, 6, 2);
        Assert.Equal("35/6,55/6", UsCcssHsFourthCourseQuestions.Solve(Family, p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Family, p, "35/6,55/6"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Family, p, "35/6,55/6,95/6"));
    }

    [Fact]
    public void InvalidPeriodStillFailsClosedRatherThanProducingARoot()
    {
        var p = P(0, 3, 8, 8, 0);
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "0,0"));
        Assert.Throws<ArgumentException>(() =>
            UsCcssHsFourthCourseQuestions.Solve(Family, p));
        p["period"] = 101;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "0,101/2"));
    }

    [Fact]
    public void Coder9NegativeSineExtremumHasExactlyOneRoot()
    {
        var p = P(12, 4, 10, 6, 4);
        Assert.Equal("9", UsCcssHsFourthCourseQuestions.Solve(Family, p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, "9.0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "3,9"));
    }

    [Fact]
    public void Coder9OutOfRangeTargetHasNoRealTimeSolutions()
    {
        var p = P(12, 4, 10, 15, 5);
        Assert.Equal("none", UsCcssHsFourthCourseQuestions.Solve(Family, p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, "none"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "1"));
    }

    [Theory]
    [InlineData(1, "4/3")]
    [InlineData(3, "36")]
    public void Coder9L04IntegralSphereRadiusHasCorrectExactPiCoefficient(
        int radius, string coefficient)
    {
        var p = new Dictionary<string, int> { ["radius"] = radius };
        Assert.Equal(coefficient, UsCcssHsFourthCourseQuestions.Solve(
            UsCcssHsFourthCourseQuestions.CavalieriVolume, p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsFourthCourseQuestions.CavalieriVolume, p, coefficient));
    }

    [Fact]
    public void Coder9L04FractionalRadiusTwoPointFiveHasExactOneHundredTwentyFiveSixths()
    {
        var p = new Dictionary<string, int>
        {
            ["radiusNumerator"] = 5, ["radiusDenominator"] = 2
        };
        Assert.Equal("125/6", UsCcssHsFourthCourseQuestions.Solve(
            UsCcssHsFourthCourseQuestions.CavalieriVolume, p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsFourthCourseQuestions.CavalieriVolume, p, "250/12"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsFourthCourseQuestions.CavalieriVolume, p, "125/3"));
    }

    [Fact]
    public void CandidateSphereGeneratorIncludesValidFractionalRadiusTasks()
    {
        var count = 0;
        for (var seed = 0; seed < 80; seed++)
        {
            var q = UsCcssHsFourthCourseQuestions.Build(
                UsCcssHsFourthCourseQuestions.CavalieriVolume, new Random(seed));
            var solution = UsCcssHsFourthCourseQuestions.Solve(q.Family, q.Parameters);
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family, q.Parameters, solution));
            if (q.Parameters.ContainsKey("radiusNumerator"))
            {
                count++;
                Assert.Equal(2, q.Parameters["radiusDenominator"]);
                Assert.Contains("/2 cm", q.Prompt, StringComparison.Ordinal);
            }
        }
        Assert.InRange(count, 1, 79);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2)]
    public void Coder9L04NonpositiveSphereRadiusIsNotGradableAsRealSphereVolume(int radius)
    {
        var p = new Dictionary<string, int> { ["radius"] = radius };
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsFourthCourseQuestions.CavalieriVolume, p, "0"));
        Assert.Throws<ArgumentException>(() => UsCcssHsFourthCourseQuestions.Solve(
            UsCcssHsFourthCourseQuestions.CavalieriVolume, p));
    }

    [Fact]
    public void Coder9L04MalformedFractionalRadiusCannotBypassDomainGate()
    {
        var p = new Dictionary<string, int>
        {
            ["radiusNumerator"] = 5, ["radiusDenominator"] = 0
        };
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsFourthCourseQuestions.CavalieriVolume, p, "0"));
        p["radiusDenominator"] = 2;
        p["radius"] = 5;
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsFourthCourseQuestions.CavalieriVolume, p, "125/6"));
    }

    [Fact]
    public void FabricatedTargetAndMalformedNumericExpressionsAreRejected()
    {
        var p = P(12, 4, 10, 12, 1);
        p["target"] = 15;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "1,5"));
        p["target"] = 12;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "1+0,5"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "1,5<script>"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "Infinity,5"));
    }
}
