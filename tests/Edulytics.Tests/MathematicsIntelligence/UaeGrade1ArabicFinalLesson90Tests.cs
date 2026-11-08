using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// First-party UAE G1 Stage A–B–C: WRITTEN_NOT_RUN. Stage D owns execution.
public sealed class UaeGrade1ArabicFinalLesson90Tests
{
    [Fact]
    public void ArabicSourceBoundedFamily_GeneratesCorrectGradedQuestions()
    {
        const string code="PED:UAE-MOE-MATH:L1:COMMON:10:02";
        const string family="uae.g1.u10.l02.triangle_trapezoid";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var route));
        Assert.Equal(family,Assert.Single(route!.AllowedQuestionFamilies));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var questions=new ExactSkillContractQuestionEngine().Generate(
            "stage18",code,new[]{family},ExactSkillQuestionDifficulty.Standard,
            12,70090,Array.Empty<string>());
        Assert.NotEmpty(questions);
        Assert.All(questions,q=>
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,"999"));
        });
    }
}
