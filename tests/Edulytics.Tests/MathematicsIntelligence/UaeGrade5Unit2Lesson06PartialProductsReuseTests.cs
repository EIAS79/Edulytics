using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit2Lesson06PartialProductsReuseTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:02:06";
    [Fact] public void Grade5RoutesExistingExactThreeFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("number.multiply_two_digit_place_value_models",c!.SkillId);
        Assert.Equal(3,c.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
        }
    }
    [Fact] public void StudentPartialProductsEqualSevenTimesFiftySix()
    {
        var p=new Dictionary<string,int>{{"multiplier",7},{"tens",5},{"ones",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l03.model_two_digit_product",p,"392"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l03.model_tens_product",p,"350"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l03.model_ones_product",p,"42"));
    }
}
