using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit9Lesson07MixedMultiplyTests
{
    private const string L="PED:UAE-MOE-MATH:L5:GENERAL:09:07";
    [Fact]
    public void G5MixedMultiplicationFamiliesRegistered()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Fact]
    public void MultiplyTwoMixedNumbersExactly()
    {
        const string f="uae.g5.u09.l07.multiply_two_mixed";
        var p=new Dictionary<string,int>{{"w1",2},{"n1",2},{"d1",5},
            {"w2",2},{"n2",3},{"d2",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٣٣/٥"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"132/20"));
    }
}