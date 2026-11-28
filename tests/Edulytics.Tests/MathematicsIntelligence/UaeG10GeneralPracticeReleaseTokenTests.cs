using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

// Execute in independent processes with absent, incorrect and exact tokens.
public sealed class UaeG10GeneralPracticeReleaseTokenTests
{
    [Fact]
    public void Only_exact_release_token_exposes_all_148_general_digital_contracts()
    {
        const string prefix = "PED:UAE-MOE-MATH:L10:";
        const string general = prefix + "GENERAL:";
        const string token = "CONFIRM_RELEASE_UAE_GRADE10_GENERAL_148_DIGITAL_PRACTICE";
        Assert.True(string.IsNullOrEmpty(Environment.GetEnvironmentVariable(
            "EDULYTICS_G10_CI_PRACTICE_REHEARSAL")));
        var enabled = string.Equals(Environment.GetEnvironmentVariable(
            "EDULYTICS_G10_DIGITAL_PRACTICE_RELEASE"), token, StringComparison.Ordinal);
        var visible = LessonPracticeContractRegistry.All.Where(x =>
            x.LessonCode.StartsWith(prefix, StringComparison.Ordinal)).ToArray();
        Assert.Equal(enabled ? 148 : 0, visible.Length);
        Assert.All(visible, x =>
        {
            Assert.StartsWith(general, x.LessonCode);
            Assert.Equal("READY_VERIFIED", x.Readiness);
            Assert.NotEmpty(x.AllowedQuestionFamilies);
        });
        var blueprint = Assert.Single(PedagogicalLessonBlueprintRegistry.LoadEmbeddedDocuments(),
            x => x.PackCode == "UAE-MOE-MATH" && x.LogicalLevel == 10 && x.Pathway == "General");
        foreach (var lesson in blueprint.Lessons)
            Assert.Equal(enabled, LessonPracticeContractRegistry.TryResolve(lesson.LessonCode, out _));
        if (enabled)
        {
            Assert.Equal(148, visible.Select(x => x.LessonCode).Distinct(StringComparer.Ordinal).Count());
            Assert.Equal(691, visible.Sum(x => x.AllowedQuestionFamilies.Count));
            Assert.Equal(678, visible.SelectMany(x => x.AllowedQuestionFamilies)
                .Distinct(StringComparer.Ordinal).Count());
        }
        Assert.False(LessonPracticeContractRegistry.TryResolve(
            prefix + "ADVANCED:01:01:GRAPHING-QUADRATIC-FUNCTIONS", out _));
        Assert.False(LessonPracticeContractRegistry.TryResolve(
            prefix + "APPLIED:01:01:UNAPPROVED", out _));
    }
}
