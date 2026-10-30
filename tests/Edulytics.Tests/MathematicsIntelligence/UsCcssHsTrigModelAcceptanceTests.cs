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
        var p = P(12, 4, 10, 18, 5);
        Assert.Equal("none", UsCcssHsFourthCourseQuestions.Solve(Family, p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, "none"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "1"));
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
