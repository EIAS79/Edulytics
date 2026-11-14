using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit8Lesson05UnlikeSumsTests
{
    private const string Code="PED:UAE-MOE-MATH:L5:GENERAL:08:05";
    [Fact]
    public void ReusesExistingExactFamiliesWithoutCreatingAnotherEngine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Code,out var contract));
        Assert.Equal("fractions.add",contract!.SkillId);
        Assert.Equal(2,contract.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Fact]
    public void UnlikeSumChecksReducedResult()
    {
        var p=new Dictionary<string,int>{{"n1",2},{"d1",3},{"n2",3},{"d2",4}};
        const string family="uae.g5.u08.l04.model_unlike_sum_simplified";
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"17/12"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"34/24"));
    }
}