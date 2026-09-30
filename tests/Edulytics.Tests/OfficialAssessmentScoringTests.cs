using Edulytics.Core.Enums;
using Edulytics.Services.Assessments;
using Edulytics.Services.StudentPortal;

namespace Edulytics.Tests;

public sealed class OfficialAssessmentScoringTests
{
    [Theory]
    [InlineData("1/2", "0.5")]
    [InlineData("1/2", "0,5")]
    [InlineData("2/4", "0.5")]
    [InlineData("50%", "0.5")]
    [InlineData("x = 2", "2")]
    [InlineData("-1 1/2", "-1.5")]
    [InlineData("-0 1/2", "-0.5")]
    [InlineData("(3/4)", "0.75")]
    [InlineData("0,125", "1/8")]
    [InlineData("1,000", "1000")]
    [InlineData("1.000", "1000")]
    public void EquivalentScalarRepresentationsAreAccepted(string actual, string expected)
    {
        Assert.True(MathematicsAnswerEquivalence.AreEquivalent(actual, expected));
    }

    [Theory]
    [InlineData("1/3", "0.333")]
    [InlineData("2+2", "4")]
    [InlineData("1/0", "0")]
    [InlineData("x = 3", "2")]
    [InlineData("1,000", "1")]
    [InlineData("1.000", "1")]
    public void NonEquivalentOrUnsupportedExpressionsAreRejected(string actual, string expected)
    {
        Assert.False(MathematicsAnswerEquivalence.AreEquivalent(actual, expected));
    }

    [Fact]
    public void TextFallbackRemainsCaseInsensitiveAndWhitespaceTolerant()
    {
        Assert.True(MathematicsAnswerEquivalence.AreEquivalent("  TRUE   answer ", "true answer"));
    }

    [Theory]
    [InlineData(AssessmentStatus.Draft, AssessmentResultReleaseStatus.Withheld, false)]
    [InlineData(AssessmentStatus.Open, AssessmentResultReleaseStatus.Withheld, false)]
    [InlineData(AssessmentStatus.Closed, AssessmentResultReleaseStatus.Withheld, false)]
    [InlineData(AssessmentStatus.Closed, AssessmentResultReleaseStatus.Published, true)]
    public void StudentSeesOfficialResultOnlyAfterTeacherPublishesClosedAssessment(
        AssessmentStatus status,
        AssessmentResultReleaseStatus releaseStatus,
        bool expected)
    {
        Assert.Equal(
            expected,
            OfficialAssessmentResultReleasePolicy.CanStudentView(
                status,
                releaseStatus));
    }
}
