using System;
using System.Linq;
using Edulytics.Core.Mathematics.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

// NOT_RUN: Unit-First V3 prohibits automated test runs in A-B-C authoring.
public sealed class UsCcssHsAdvTrigGeoU01L01PracticeCandidateTests
{
    [Fact]
    public void CandidateDoesNotAuthorizeStudentPracticeOrHumanProofAutoGrading()
    {
        Assert.False(UsCcssHsAdvTrigGeoU01L01PracticeCandidate.StudentPracticeRoutingAuthorized);
        Assert.False(UsCcssHsAdvTrigGeoU01L01PracticeCandidate.AcademicApprovalRecorded);
        Assert.False(UsCcssHsAdvTrigGeoU01L01PracticeCandidate.ContentPredecessorApproved);
        Assert.False(UsCcssHsAdvTrigGeoU01L01PracticeCandidate.MayAutomaticallyScoreHumanProof);
        Assert.Equal("INTEGRATION_PENDING", UsCcssHsAdvTrigGeoU01L01PracticeCandidate.Readiness);
    }

    [Fact]
    public void FourCasesHaveThreeScorableAndOneExcludedPole()
    {
        var rows = UsCcssHsAdvTrigGeoU01L01PracticeCandidate.ReviewCases;
        Assert.Equal(4, rows.Count);
        Assert.Equal(3, rows.Count(x => x.IsScorable));
        Assert.Equal(4, rows.Select(x => x.Id).Distinct(StringComparer.Ordinal).Count());
        Assert.False(UsCcssHsAdvTrigGeoU01L01PracticeCandidate.TryGetReviewCase(
            "PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L02",
            "l01-reflection-cos", out _));
        var pole = rows.Single(x => x.Id == "l01-tangent-pole");
        Assert.Equal("OUT_OF_DOMAIN", pole.ExpectedStatus);
        Assert.Null(pole.Exact);
        Assert.False(pole.IsScorable);
    }

    [Theory]
    [InlineData("l01-reflection-cos", 2, 1, 0, "-sqrt(3)/2")]
    [InlineData("l01-rotation-sin", 4, 0, 1, "-sqrt(3)/2")]
    [InlineData("l01-fullturn-cos", 3, 1, 2, "sqrt(2)/2")]
    public void KernelParameterMapIsExactAndDecimalModeNotImplicit(
        string caseId, int twelfths, int fn, int transform, string expected)
    {
        var code = UsCcssHsAdvTrigGeoU01L01PracticeCandidate.LessonCode;
        Assert.True(UsCcssHsAdvTrigGeoU01L01PracticeCandidate.TryGetReviewCase(
            code, caseId, out var fixture));
        Assert.NotNull(fixture);
        Assert.Equal(expected, fixture.Exact);
        Assert.False(fixture.ApproximationAllowedByDefault);
        Assert.True(UsCcssHsAdvTrigGeoU01L01PracticeCandidate
            .TryGetReviewerKernelParameters(code, caseId, out var parameters));
        Assert.NotNull(parameters);
        Assert.Equal(twelfths, parameters["twelfths"]);
        Assert.Equal(fn, parameters["function"]);
        Assert.Equal(transform, parameters["transform"]);
    }
}
