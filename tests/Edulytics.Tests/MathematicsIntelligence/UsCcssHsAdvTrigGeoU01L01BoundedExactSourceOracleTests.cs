using System;
using System.Linq;
using Edulytics.Core.Mathematics.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

// AUTHORED_NOT_RUN. Isolated reviewer math, NOT learner-selected Practice grading.
public sealed class UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracleTests
{
    private const string L01 = UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.LessonCode;

    [Fact]
    public void OracleIsCourseOnlyAndProofIsHumanScored()
    {
        Assert.False(UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.StudentPracticeRoutingAuthorized);
        Assert.False(UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.CanAutomaticallyScoreProof);
        Assert.Equal("REVIEW_ORACLE_ONLY_NOT_ROUTED",
            UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.Readiness);
        Assert.Equal("radians", UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.AngleMode);
        Assert.Equal(6, UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.TeacherProofRubricMaxPoints);
        Assert.Equal(UsCcssHsAdvTrigGeoU01L01PracticeCandidate.SourceFamily,
            UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.SourceFamily);
    }

    [Theory]
    [InlineData(2, 1, 0, 10, "-sqrt(3)/2")]
    [InlineData(4, 0, 1, 16, "-sqrt(3)/2")]
    [InlineData(3, 1, 2, 21, "sqrt(2)/2")]
    [InlineData(-2, 0, 0, 14, "-1/2")]
    [InlineData(-3, 1, 2, 3, "sqrt(2)/2")]
    [InlineData(3, 2, 0, 9, "-1")]
    [InlineData(2, 2, 0, 10, "-sqrt(3)/3")]
    [InlineData(4, 2, 1, 16, "sqrt(3)")]
    [InlineData(24, 0, 1, 12, "0")]
    [InlineData(-24, 1, 2, 0, "1")]
    [InlineData(6, 1, 0, 6, "0")]
    [InlineData(12, 1, 0, 0, "1")]
    [InlineData(0, 0, 0, 12, "0")]
    [InlineData(3, 0, 0, 9, "sqrt(2)/2")]
    public void SignedExactSourceCasesHaveCanonicalResults(
        int thetaTwelfths, int functionCode, int transformCode, int resultTwelfths,
        string exact)
    {
        Assert.True(UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.TryBuild(
            L01, "radians", thetaTwelfths, functionCode, transformCode, out var c));
        Assert.NotNull(c);
        Assert.Equal("OK", c!.ExpectedStatus);
        Assert.True(c.IsScorable);
        Assert.Equal(resultTwelfths, c.ResultTwelfths);
        Assert.Equal(exact, c.CanonicalExact);
        Assert.Contains("exactly (radians)", c.Prompt);
    }

    [Theory]
    [InlineData(6, 2, 0, 6)]
    [InlineData(6, 2, 2, 18)]
    [InlineData(-6, 2, 1, 6)]
    public void TangentAtZeroCosineIsOutOfDomainNotScorable(
        int theta, int functionCode, int transformCode, int resultAngle)
    {
        Assert.True(UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.TryBuild(
            L01, "radians", theta, functionCode, transformCode, out var c));
        Assert.NotNull(c);
        Assert.Equal("OUT_OF_DOMAIN", c!.ExpectedStatus);
        Assert.Null(c.CanonicalExact);
        Assert.False(c.IsScorable);
        Assert.Equal(resultAngle, c.ResultTwelfths);
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L02", "radians", 2, 1, 0)]
    [InlineData("PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L01", "degrees", 2, 1, 0)]
    [InlineData("PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L01", "rad", 2, 1, 0)]
    [InlineData("PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L01", "", 2, 1, 0)]
    [InlineData("PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L01", "radians", -25, 1, 0)]
    [InlineData("PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L01", "radians", 25, 1, 0)]
    [InlineData("PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L01", "radians", 1, 1, 0)]
    [InlineData("PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L01", "radians", 5, 1, 0)]
    [InlineData("PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L01", "radians", 2, -1, 0)]
    [InlineData("PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L01", "radians", 2, 3, 0)]
    [InlineData("PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L01", "radians", 2, 1, -1)]
    [InlineData("PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L01", "radians", 2, 1, 3)]
    public void UnsupportedLessonModeAngleOrCodesRefuseToGenerate(
        string lesson, string mode, int theta, int function, int transform)
    {
        Assert.False(UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.TryBuild(
            lesson, mode, theta, function, transform, out var generated));
        Assert.Null(generated);
    }

    [Fact]
    public void EveryBoundedSourceParameterHasExactResultOrExplicitPole()
    {
        int count = 0, poles = 0;
        for (var theta = -24; theta <= 24; ++theta)
        {
            if (theta % 2 != 0 && theta % 3 != 0) continue;
            for (var function = 0; function <= 2; ++function)
            for (var transform = 0; transform <= 2; ++transform)
            {
                Assert.True(UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.TryBuild(
                    L01, "radians", theta, function, transform, out var c));
                Assert.NotNull(c);
                Assert.InRange(c!.ResultTwelfths, 0, 23);
                if (c.ExpectedStatus == "OUT_OF_DOMAIN")
                {
                    Assert.Equal(2, function);
                    Assert.True(c.ResultTwelfths is 6 or 18);
                    Assert.Null(c.CanonicalExact);
                    poles++;
                }
                else
                {
                    Assert.True(c.IsScorable);
                    Assert.NotNull(c.CanonicalExact);
                    Assert.DoesNotContain(".", c.CanonicalExact!);
                    Assert.False(c.CanonicalExact!.Contains("e-", StringComparison.OrdinalIgnoreCase));
                }
                count++;
            }
        }
        Assert.Equal(297, count); // 33 supported theta values x 3 functions x 3 transforms
        Assert.True(poles > 0);
    }

    [Theory]
    [InlineData(2, 1, 0, "-sqrt(3)/2", "EXACT_ACCEPTED")]
    [InlineData(2, 1, 0, "-sqrt(12)/4", "EXACT_ACCEPTED")]
    [InlineData(2, 1, 0, "-sqrt(27)/6", "EXACT_ACCEPTED")]
    [InlineData(4, 0, 1, "-3/sqrt(12)", "EXACT_ACCEPTED")]
    [InlineData(3, 1, 2, "sqrt(2)/2", "EXACT_ACCEPTED")]
    [InlineData(3, 1, 2, "1/sqrt(2)", "EXACT_ACCEPTED")]
    [InlineData(3, 1, 2, "sqrt(8)/4", "EXACT_ACCEPTED")]
    [InlineData(3, 1, 2, "sqrt(18)/6", "EXACT_ACCEPTED")]
    [InlineData(3, 1, 2, "2/sqrt(8)", "EXACT_ACCEPTED")]
    [InlineData(2, 1, 0, "sqrt(3)/2", "INCORRECT")]
    [InlineData(2, 1, 0, "-1/2", "INCORRECT")]
    [InlineData(2, 1, 0, "sqrt(12)/4", "INCORRECT")]
    [InlineData(3, 1, 2, "-sqrt(2)/2", "INCORRECT")]
    [InlineData(3, 1, 2, "-sqrt(12)/4", "INCORRECT")]
    [InlineData(2, 1, 0, "-0.8660254037844386", "DECIMAL_REJECTED_EXACT_REQUIRED")]
    [InlineData(2, 1, 0, "-8.660254037844386e-1", "DECIMAL_REJECTED_EXACT_REQUIRED")]
    [InlineData(3, 1, 2, "0.7071067811865476", "DECIMAL_REJECTED_EXACT_REQUIRED")]
    [InlineData(3, 1, 2, "7.071067811865476e-1", "DECIMAL_REJECTED_EXACT_REQUIRED")]
    [InlineData(2, 1, 0, "-sqrt(1 2)/4", "MALFORMED_OR_UNSUPPORTED")]
    [InlineData(2, 1, 0, "-sqrt(1\\t2)/4", "MALFORMED_OR_UNSUPPORTED")]
    [InlineData(2, 1, 0, "NaN", "MALFORMED_OR_UNSUPPORTED")]
    [InlineData(2, 1, 0, "Infinity", "MALFORMED_OR_UNSUPPORTED")]
    [InlineData(2, 1, 0, "<script>", "MALFORMED_OR_UNSUPPORTED")]
    [InlineData(6, 2, 2, "0", "OUT_OF_DOMAIN")]
    [InlineData(6, 2, 2, "-999.99", "OUT_OF_DOMAIN")]
    public void ReviewerClassificationMatchesExactPolicyAndNeverUsesDecimalTolerance(
        int theta, int function, int transform, string response, string status)
    {
        Assert.Equal(status,
            UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.ClassifyReviewerResponse(
                L01, "radians", theta, function, transform, response));
    }

    [Fact]
    public void AngleModeAndOtherLessonNeverAuthorizeReviewClassification()
    {
        Assert.Equal("ANGLE_MODE_MISMATCH",
            UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.ClassifyReviewerResponse(
                L01, "degrees", 2, 1, 0, "-sqrt(3)/2"));
        Assert.Equal("LESSON_OR_CASE_MISMATCH",
            UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.ClassifyReviewerResponse(
                "PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L02",
                "radians", 2, 1, 0, "-sqrt(3)/2"));
        Assert.Equal("LESSON_OR_CASE_MISMATCH",
            UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.ClassifyReviewerResponse(
                L01, "radians", 1, 1, 0, "-sqrt(3)/2"));
    }

    [Fact]
    public void CanonicalAnswersForEveryParameterAreReviewerAcceptedOnly()
    {
        for (var theta = -24; theta <= 24; ++theta)
        {
            if (theta % 2 != 0 && theta % 3 != 0) continue;
            for (var function = 0; function <= 2; ++function)
            for (var transform = 0; transform <= 2; ++transform)
            {
                Assert.True(UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.TryBuild(
                    L01, "radians", theta, function, transform, out var c));
                Assert.NotNull(c);
                Assert.Equal(c!.IsScorable ? "EXACT_ACCEPTED" : "OUT_OF_DOMAIN",
                    UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.ClassifyReviewerResponse(
                        L01, "radians", theta, function, transform,
                        c.CanonicalExact ?? "0"));
                Assert.Equal(c.IsScorable
                        ? "DECIMAL_REJECTED_EXACT_REQUIRED" : "OUT_OF_DOMAIN",
                    UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.ClassifyReviewerResponse(
                        L01, "radians", theta, function, transform, "0.707106781"));
            }
        }
    }

    [Fact]
    public void SourceFourExistingReviewerCasesAgreeWithOracle()
    {
        foreach (var original in UsCcssHsAdvTrigGeoU01L01PracticeCandidate.ReviewCases)
        {
            Assert.True(UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle.TryBuild(
                L01, "radians", original.Twelfths, original.FunctionCode,
                original.TransformCode, out var computed));
            Assert.NotNull(computed);
            Assert.Equal(original.ExpectedStatus, computed!.ExpectedStatus);
            Assert.Equal(original.Exact, computed.CanonicalExact);
        }
    }
}
