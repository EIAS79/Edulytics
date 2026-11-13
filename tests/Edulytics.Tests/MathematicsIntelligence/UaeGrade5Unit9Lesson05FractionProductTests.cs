using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit9Lesson05FractionProductTests
{
    private const string L="PED:UAE-MOE-MATH:L5:GENERAL:09:05";
    [Fact]
    public void FractionMultiplicationUsesExistingExactFamily()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal("supporting.fractions.multiply_divide",c!.SkillId);
        Assert.Single(c.AllowedQuestionFamilies);
    }
    [Fact]
    public void FractionOfFractionIsSimplified()
    {
        const string f="supporting.fractions.multiply";
        var p=new Dictionary<string,int>{{"n1",2},{"d1",3},{"n2",3},{"d2",5}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"2/5"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"6/15"));
    }
}