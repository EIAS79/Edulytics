using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit8Lesson10MixedModelsTests
{
    private const string L="PED:UAE-MOE-MATH:L5:GENERAL:08:10";
    [Fact]
    public void ModelMixedAdditionUsesExistingEngineAndExactFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal("fractions.add",c!.SkillId);
        Assert.Equal(2,c.AllowedQuestionFamilies.Count);
    }
    [Fact]
    public void UnequalDenominatorsAreNormalizedAndVerified()
    {
        var p=new Dictionary<string,int>{{"w1",2},{"n1",2},{"d1",3},
            {"w2",1},{"n2",1},{"d2",2}};
        const string f="uae.g5.u08.l10.model_mixed_sum_fraction";
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٢٥/٦"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"50/12"));
    }
}