using System;
using System.Linq;
using Edulytics.Core.Mathematics.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

// Designed ONLY: NOT_RUN during Unit-First A-B-C. No authenticated grading claim.
public sealed class UsCcssHsAdvTrigGeoU01L01PracticeCandidateTests
{
    private const string L01 = UsCcssHsAdvTrigGeoU01L01PracticeCandidate.LessonCode;

    [Fact]
    public void CandidateRemainsFailClosedWithSeparateHumanProof()
    {
        Assert.False(UsCcssHsAdvTrigGeoU01L01PracticeCandidate.StudentPracticeRoutingAuthorized);
        Assert.False(UsCcssHsAdvTrigGeoU01L01PracticeCandidate.AcademicApprovalRecorded);
        Assert.False(UsCcssHsAdvTrigGeoU01L01PracticeCandidate.ContentPredecessorApproved);
        Assert.False(UsCcssHsAdvTrigGeoU01L01PracticeCandidate.SharedVerifierExactPolicyVerified);
        Assert.False(UsCcssHsAdvTrigGeoU01L01PracticeCandidate.MayAutomaticallyScoreHumanProof);
        Assert.Equal("INTEGRATION_PENDING", UsCcssHsAdvTrigGeoU01L01PracticeCandidate.Readiness);
        Assert.Equal("EXACT_SYMBOLIC_ONLY_ALLOWLIST", UsCcssHsAdvTrigGeoU01L01PracticeCandidate.AnswerPolicy);
        Assert.Equal(6, UsCcssHsAdvTrigGeoU01L01PracticeCandidate.TeacherProofRubricMaxPoints);
    }

    [Fact]
    public void ExactlyFourCasesContainThreeScorableAndOnePole()
    {
        var rows = UsCcssHsAdvTrigGeoU01L01PracticeCandidate.ReviewCases;
        Assert.Equal(4, rows.Count);
        Assert.Equal(3, rows.Count(x => x.IsScorable));
        Assert.Equal(4, rows.Select(x => x.Id).Distinct(StringComparer.Ordinal).Count());
        Assert.Equal("OUT_OF_DOMAIN",
            UsCcssHsAdvTrigGeoU01L01PracticeCandidate.ClassifyReviewerSubmission(
                L01, "l01-tangent-pole", "0"));
        Assert.Equal("LESSON_OR_CASE_MISMATCH",
            UsCcssHsAdvTrigGeoU01L01PracticeCandidate.ClassifyReviewerSubmission(
                "PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L02",
                "l01-reflection-cos", "-sqrt(3)/2"));
    }

    [Theory]
    [InlineData("l01-reflection-cos", 2, 1, 0, "-sqrt(3)/2", "-sqrt(12)/4")]
    [InlineData("l01-rotation-sin", 4, 0, 1, "-sqrt(3)/2", "-sqrt(12)/4")]
    [InlineData("l01-fullturn-cos", 3, 1, 2, "sqrt(2)/2", "1/sqrt(2)")]
    [InlineData("l01-fullturn-cos", 3, 1, 2, "sqrt(2)/2", "sqrt(8)/4")]
    public void ExactRadicalFormsAcceptedAndParametersMapToExistingKernel(
        string caseId, int twelfths, int fn, int transform, string expected, string alias)
    {
        Assert.Equal("EXACT_ACCEPTED",
            UsCcssHsAdvTrigGeoU01L01PracticeCandidate.ClassifyReviewerSubmission(
                L01, caseId, expected));
        Assert.Equal("EXACT_ACCEPTED",
            UsCcssHsAdvTrigGeoU01L01PracticeCandidate.ClassifyReviewerSubmission(
                L01, caseId, alias));
        Assert.True(UsCcssHsAdvTrigGeoU01L01PracticeCandidate
            .TryGetReviewerKernelParameters(L01, caseId, out var parameters));
        Assert.NotNull(parameters);
        Assert.Equal(twelfths, parameters["twelfths"]);
        Assert.Equal(fn, parameters["function"]);
        Assert.Equal(transform, parameters["transform"]);
    }

    [Theory]
    [InlineData("l01-reflection-cos", "-0.8660254037844386")]
    [InlineData("l01-reflection-cos", "-8.660254037844386e-1")]
    [InlineData("l01-rotation-sin", "-0.866")]
    [InlineData("l01-fullturn-cos", "0.7071067811865476")]
    [InlineData("l01-fullturn-cos", "0.707")]
    public void DecimalAndToleranceBasedResponsesRejected(string caseId, string answer)
    {
        Assert.Equal("DECIMAL_REJECTED_EXACT_REQUIRED",
            UsCcssHsAdvTrigGeoU01L01PracticeCandidate.ClassifyReviewerSubmission(
                L01, caseId, answer));
    }

    [Theory]
    [InlineData("l01-reflection-cos", "sqrt(3)/2", "INCORRECT")]
    [InlineData("l01-rotation-sin", "sqrt(3)/2", "INCORRECT")]
    [InlineData("l01-fullturn-cos", "-sqrt(2)/2", "INCORRECT")]
    [InlineData("l01-reflection-cos", "cos(", "MALFORMED_OR_UNSUPPORTED")]
    [InlineData("l01-reflection-cos", "NaN", "MALFORMED_OR_UNSUPPORTED")]
    [InlineData("l01-reflection-cos", "<script>", "MALFORMED_OR_UNSUPPORTED")]
    [InlineData("l01-reflection-cos", "sqrt(3)/2 + evil()", "MALFORMED_OR_UNSUPPORTED")]
    public void WrongAndMalformedSubmissionsFailClosed(
        string caseId, string submission, string status)
    {
        Assert.Equal(status, UsCcssHsAdvTrigGeoU01L01PracticeCandidate
            .ClassifyReviewerSubmission(L01, caseId, submission));
    }
}
