using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// A–B–C authored, WRITTEN_NOT_RUN; Stage D execution required.
public sealed class UaeGrade1ArabicUnit6Lesson05Tests
{
    [Fact]
    public void Unit6SourceExactFamily_RejectsWrongAnswers()
    {
        const string code="PED:UAE-MOE-MATH:L1:COMMON:07:05";
        const string family="uae.g1.u07.l05.regroup_ones_for_tens";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var route));
        Assert.Equal(family,Assert.Single(route!.AllowedQuestionFamilies));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var questions=new ExactSkillContractQuestionEngine().Generate(
            "stage18",code,new[] {family},ExactSkillQuestionDifficulty.Standard,12,
            66005,Array.Empty<string>());
        Assert.NotEmpty(questions);
        Assert.All(questions,q=>
        {
            Assert.InRange(q.Parameters["result"],0,100);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"999"));
        });
    }
}
