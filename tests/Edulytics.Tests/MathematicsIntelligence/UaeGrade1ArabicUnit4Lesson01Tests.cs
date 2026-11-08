using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Stage A–B–C: WRITTEN_NOT_RUN.
public sealed class UaeGrade1ArabicUnit4Lesson01Tests
{
    [Fact]
    public void ScopedExactFamily_Generates_AndIndependentlyVerifies()
    {
        const string code = "PED:UAE-MOE-MATH:L1:COMMON:05:01";
        const string family = "uae.g1.u05.l01.count_back_1_2_3";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code, out var route));
        Assert.NotNull(route);
        Assert.Equal(family, Assert.Single(route!.AllowedQuestionFamilies));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var qs = new ExactSkillContractQuestionEngine().Generate(
            "stage18", code, new[] { family },
            ExactSkillQuestionDifficulty.Standard, 12, 43001, Array.Empty<string>());
        Assert.NotEmpty(qs);
        Assert.All(qs, q =>
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99"));
        });
    }
}
