using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit9Lesson04WholeMultiplyTests
{
    private const string L="PED:UAE-MOE-MATH:L5:GENERAL:09:04";
    [Fact]
    public void ReusesExactG4Engine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal("fractions.multiply_whole",c!.SkillId);
        Assert.Equal(3,c.AllowedQuestionFamilies.Count);
    }
    [Fact]
    public void ModelFamilyMatchesSourceSkill()
    {
        const string f="uae.g4.u09.l09.product_whole";
        var p=new Dictionary<string,int>{{"denominator",5},{"numerator",3},{"repeatCount",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٣"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"-100"));
    }
}