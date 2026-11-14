using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit8Lesson08ReasonableEstimatesTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:08:08";
    [Fact]
    public void UsesReviewedEstimationFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("fractions.compare",c!.SkillId);
        Assert.Equal(2,c.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Fact]
    public void EstimationOfMixedSumIsNotExactFractionArithmetic()
    {
        const string f="uae.g5.u08.l08.estimate_mixed_sum";
        var p=new Dictionary<string,int>{{"w1",3},{"n1",2},{"d1",3},
            {"w2",2},{"n2",1},{"d2",7},{"proposed",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٦"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"5"));
    }
}