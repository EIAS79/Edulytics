using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit8Lesson12MixedSubtractionTests
{
    private const string L="PED:UAE-MOE-MATH:L5:GENERAL:08:12";
    [Fact]
    public void ReusesG4MixedSubtractionEngineWithTwoGrade5Families()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal("fractions.subtract",c!.SkillId);
        Assert.Equal(2,c.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Fact]
    public void ExactImproperDifferenceCanIncludeRegrouping()
    {
        const string f="uae.g5.u08.l12.mixed_difference_fraction";
        var p=new Dictionary<string,int>{{"w1",4},{"n1",1},{"d1",4},
            {"w2",2},{"n2",5},{"d2",8}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"١٣/٨"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"26/16"));
    }
}