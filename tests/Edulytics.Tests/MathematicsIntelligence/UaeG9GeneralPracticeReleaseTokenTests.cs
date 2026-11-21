using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Execute once without token and once in a separate process with exactly
/// EDULYTICS_G9_DIGITAL_PRACTICE_RELEASE=CONFIRM_RELEASE_UAE_GRADE9_GENERAL_80_DIGITAL_PRACTICE.
/// No database writes; checks that a Published Arabic pack alone never makes
/// these proposed digital contracts public until explicit gated release.
/// </summary>
public sealed class UaeG9GeneralPracticeReleaseTokenTests
{
    [Fact]
    public void Grade9_public_practice_is_exactly_80_on_explicit_token_otherwise_zero()
    {
        const string prefix = "PED:UAE-MOE-MATH:L9:GENERAL:";
        const string token = "CONFIRM_RELEASE_UAE_GRADE9_GENERAL_80_DIGITAL_PRACTICE";
        var enabled = string.Equals(Environment.GetEnvironmentVariable(
            "EDULYTICS_G9_DIGITAL_PRACTICE_RELEASE"), token, StringComparison.Ordinal);
        var visible = LessonPracticeContractRegistry.All
            .Where(x => x.LessonCode.StartsWith(prefix, StringComparison.Ordinal))
            .ToArray();

        Assert.Equal(enabled ? 80 : 0, visible.Length);
        Assert.All(LessonPracticeContractRegistry.All, c =>
            Assert.Equal("READY_VERIFIED", c.Readiness));
        Assert.DoesNotContain(visible, c =>
            c.LessonCode.Contains(":13:07:CONGRUENCE-TRANSFORMATIONS", StringComparison.Ordinal) ||
            c.LessonCode.Contains(":13:08:TRIANGLES-AND-COORDINATE-PROOF", StringComparison.Ordinal));
        Assert.Equal(enabled, LessonPracticeContractRegistry.TryResolve(
            "PED:UAE-MOE-MATH:L9:GENERAL:01:01:VARIABLES-AND-EXPRESSIONS", out _));
        Assert.Equal(enabled, LessonPracticeContractRegistry.TryResolve(
            "PED:UAE-MOE-MATH:L9:GENERAL:13:09:PARALLELOGRAM-TRIANGLE-AREA", out _));
        if (enabled)
        {
            Assert.Equal(80, visible.Select(x => x.LessonCode)
                .Distinct(StringComparer.Ordinal).Count());
            Assert.All(visible, c => Assert.NotEmpty(c.AllowedQuestionFamilies));
        }
    }
}
