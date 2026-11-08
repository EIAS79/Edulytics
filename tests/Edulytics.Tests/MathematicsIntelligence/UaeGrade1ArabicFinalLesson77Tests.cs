using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// First-party UAE G1 Stage A–B–C: WRITTEN_NOT_RUN. Stage D owns execution.
public sealed class UaeGrade1ArabicFinalLesson77Tests
{
    [Fact]
    public void ArabicSourceBoundedFamily_GeneratesCorrectGradedQuestions()
    {
        const string code="PED:UAE-MOE-MATH:L1:COMMON:08:04";
        const string family="uae.g1.u08.l04.read_picture_graph";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var route));
        Assert.Equal(family,Assert.Single(route!.AllowedQuestionFamilies));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var questions=new ExactSkillContractQuestionEngine().Generate(
            "stage18",code,new[]{family},ExactSkillQuestionDifficulty.Standard,
            12,70077,Array.Empty<string>());
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
