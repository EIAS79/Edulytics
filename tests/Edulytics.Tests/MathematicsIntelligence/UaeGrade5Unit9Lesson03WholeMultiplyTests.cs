using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit9Lesson03WholeMultiplyTests
{
    private const string L="PED:UAE-MOE-MATH:L5:GENERAL:09:03";
    [Fact]
    public void ReusesExactG4Engine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal("fractions.multiply_whole",c!.SkillId);
        Assert.Equal(2,c.AllowedQuestionFamilies.Count);
    }
    [Fact]
    public void ModelFamilyMatchesSourceSkill()
    {
        const string f="uae.g4.u09.l08.model_repeated_sum";
        var p=new Dictionary<string,int>{{"denominator",5},{"numerator",2},{"repeatCount",5}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"١٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"-100"));
    }
}