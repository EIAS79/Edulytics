using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssHsTrigonometryExactAnglesTests
{
    private static Dictionary<string, int> P(int twelfths, int function, int transform) =>
        new() { ["twelfths"] = twelfths, ["function"] = function, ["transform"] = transform };

    [Fact]
    public void Coder9L01ReflectionCosinePreservesCorrectQuadrantAndRadical()
    {
        var p = P(2, 1, 0);
        Assert.Equal("-sqrt(3)/2", UsCcssHsTrigonometryExactAngles.Solve(
            UsCcssHsTrigonometryExactAngles.Family, p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsTrigonometryExactAngles.Family, p, "-0.8660254037844386"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsTrigonometryExactAngles.Family, p, "sqrt(3)/2"));
    }

    [Fact]
    public void Coder9L01RotationSinePreservesNegativeSign()
    {
        var p = P(4, 0, 1);
        Assert.Equal("-sqrt(3)/2", UsCcssHsTrigonometryExactAngles.Solve(
            UsCcssHsTrigonometryExactAngles.Family, p));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsTrigonometryExactAngles.Family, p, "sqrt(3)/2"));
    }

    [Fact]
    public void Coder9L01FullRotationAcceptsExactEquivalentAndRejectsOppositeSign()
    {
        var p = P(3, 1, 2);
        Assert.Equal("sqrt(2)/2", UsCcssHsTrigonometryExactAngles.Solve(
            UsCcssHsTrigonometryExactAngles.Family, p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsTrigonometryExactAngles.Family, p, "1/sqrt(2)"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsTrigonometryExactAngles.Family, p, "-sqrt(2)/2"));
    }

    [Fact]
    public void TangentPoleIsOutOfDomainAndCannotCreateAnAutoGradedResponse()
    {
        var p = P(6, 2, 2);
        var result = UsCcssHsTrigonometryExactAngles.Evaluate(p);
        Assert.Equal("OUT_OF_DOMAIN", result.Status);
        Assert.Null(result.Value);
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsTrigonometryExactAngles.Family, p, "0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsTrigonometryExactAngles.Family, p, "undefined"));
        Assert.Throws<ArgumentException>(() =>
            UsCcssHsTrigonometryExactAngles.Solve(UsCcssHsTrigonometryExactAngles.Family, p));
    }

    [Theory]
    [InlineData(2, 1, 0, "-sqrt(3)/2", "-0.8660254037844386", "sqrt(3)/2")] // l01-reflection-cos
    [InlineData(4, 0, 1, "-sqrt(3)/2", "-0.8660254037844386", "sqrt(3)/2")] // l01-rotation-sin
    [InlineData(3, 1, 2, "sqrt(2)/2", "1/sqrt(2)", "-sqrt(2)/2")] // l01-fullturn-cos
    public void ThreeExactSourceCasesCheckEquivalentAndIncorrectResponsesWithoutDisclosure(
        int twelfths, int function, int transform, string canonical,
        string equivalent, string opposite)
    {
        var p = P(twelfths, function, transform);
        Assert.Equal(canonical, UsCcssHsTrigonometryExactAngles.Solve(
            UsCcssHsTrigonometryExactAngles.Family, p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsTrigonometryExactAngles.Family, p, canonical));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsTrigonometryExactAngles.Family, p, equivalent));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsTrigonometryExactAngles.Family, p, opposite));
        foreach (var malformed in new[] { "sqrt(", "sin(theta)", "NaN", "<script>", "1e309" })
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                UsCcssHsTrigonometryExactAngles.Family, p, malformed));
    }

    [Fact]
    public void TangentPoleAndWrongAngleDomainMustStayNonGradable()
    {
        var pole = P(6, 2, 2); // tan(3*pi/2)
        Assert.Equal("OUT_OF_DOMAIN", UsCcssHsTrigonometryExactAngles.Evaluate(pole).Status);
        foreach (var submission in new[] { "undefined", "0", "infinity", "NaN", "1e309" })
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                UsCcssHsTrigonometryExactAngles.Family, pole, submission));
        var unsupportedAngle = P(5, 2, 2);
        Assert.Equal("MALFORMED", UsCcssHsTrigonometryExactAngles.Evaluate(unsupportedAngle).Status);
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsTrigonometryExactAngles.Family, unsupportedAngle, "1"));
    }

    [Fact]
    public void GeneratedExactAnglesAreBoundedAndNonduplicatedAcrossSampledSeeds()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(
            UsCcssHsTrigonometryExactAngles.Family));
        var examples = new HashSet<string>(StringComparer.Ordinal);
        for (var seed = 0; seed < 30; seed++)
        {
            var q = UsCcssHsTrigonometryExactAngles.Build(
                UsCcssHsTrigonometryExactAngles.Family, new Random(seed));
            var solution = UsCcssHsTrigonometryExactAngles.Solve(q.Family, q.Parameters);
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family, q.Parameters, solution));
            examples.Add(q.Prompt);
        }
        Assert.True(examples.Count >= 3);
    }
}
