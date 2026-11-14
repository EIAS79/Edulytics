using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit8Lesson07UnlikeSubtractionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:08:07";
    [Fact]
    public void ReusesSourceAlignedExactSubtractionModel()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("fractions.subtract",c!.SkillId);
        Assert.Equal(2,c.AllowedQuestionFamilies.Count);
    }
    [Fact]
    public void ProperFractionsHaveIndependentVerification()
    {
        const string f="uae.g5.u08.l06.model_subtract_difference";
        var p=new Dictionary<string,int>{{"n1",4},{"d1",5},{"n2",1},{"d2",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٣/١٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"6/20"));
    }
}