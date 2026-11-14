using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit9Lesson02EstimateFractionProductTests
{
    private const string L="PED:UAE-MOE-MATH:L5:GENERAL:09:02";
    [Fact]
    public void EstimationUsesReviewedFractionFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal("supporting.fractions.multiply_divide",c!.SkillId);
        Assert.Equal(2,c.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Fact]
    public void CompatibleFractionOfSeventeenEstimatesSix()
    {
        const string f="uae.g5.u09.l02.estimate_whole_fraction_compatible";
        var p=new Dictionary<string,int>{{"whole",17},{"denominator",3},{"numerator",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٦"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"17"));
    }
}