using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit8Lesson11MixedAdditionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:08:11";
    [Fact]
    public void UsesPriorLessonExactMixedAdditionEngine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("fractions.add",c!.SkillId);
        Assert.Equal(2,c.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Fact]
    public void UnlikeMixedNumbersHaveExpectedImproperSum()
    {
        const string family="uae.g5.u08.l10.model_mixed_sum_fraction";
        var p=new Dictionary<string,int>{{"w1",4},{"n1",3},{"d1",8},
            {"w2",7},{"n2",1},{"d2",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"٩٣/٨"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"186/16"));
    }
}