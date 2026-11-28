using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original source Grade10 General U02L01 exponent products.
/// Authored during A–B–C; deliberately NOT RUN before Stage D.
/// </summary>
public sealed class UaeG10GeneralExponentProductsTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:02:01:MULTIPLICATION-PROPERTIES-OF-EXPONENTS";
    private const string SameBase="uae.g10.general.u02.l01.same_base_exponent";
    private const string PowerOfPower="uae.g10.general.u02.l01.power_of_power_exponent";
    private const string ProductCoefficient="uae.g10.general.u02.l01.power_of_product_coefficient";

    [Theory]
    [InlineData(SameBase)]
    [InlineData(PowerOfPower)]
    [InlineData(ProductCoefficient)]
    public void SourceExactFamiliesUseIntegerKeypadAndRegisteredEngine(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var presentation));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,presentation!.InteractionKind);
    }

    [Theory]
    [InlineData(SameBase)]
    [InlineData(PowerOfPower)]
    [InlineData(ProductCoefficient)]
    public void GeneratedItemsMatchIndependentProductLawsAndRejectForgedAnswers(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261125,20261126,20261127})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,
                                        ExactSkillQuestionDifficulty.Challenge})
        {
            var questions=engine.Generate(Code,Code,[family],difficulty,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var item in questions)
            {
                var p=item.Parameters;
                Assert.Equal(5,p.Count);
                Assert.InRange(p["base"],2,9);
                Assert.InRange(p["a"],1,7);
                Assert.InRange(p["b"],1,7);
                Assert.InRange(Math.Abs(p["coefficient"]),1,6);
                Assert.InRange(p["power"],2,3);

                int expected=family switch
                {
                    SameBase=>p["a"]+p["b"],
                    PowerOfPower=>p["a"]*p["b"],
                    _=>(int)Math.Pow(p["coefficient"],p["power"])
                };
                Assert.Equal(AssessmentItemType.Numeric,item.ItemType);
                Assert.Equal(expected.ToString(CultureInfo.InvariantCulture),item.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                    (expected+1).ToString(CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                var localized=string.Concat(item.CorrectAnswer.Select(c=>
                    c is >= '0' and <= '9'?(char)('٠'+c-'0'):c));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,localized));
                var broken=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                broken["power"]=10;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,broken,item.CorrectAnswer));
            }
        }
    }

    [Fact]
    public void OriginalSourceExponentIdentitiesMatchIndependentArithmetic()
    {
        foreach(int x in new[]{2,3,4})
        {
            Assert.Equal((int)Math.Pow(x,7),(int)Math.Pow(x,3)*(int)Math.Pow(x,4));
            Assert.Equal((int)Math.Pow(x,6),(int)Math.Pow(Math.Pow(x,2),3));
        }
        Assert.Equal(-27,(int)Math.Pow(-3,3));
        Assert.Equal(9,(int)Math.Pow(-3,2));
        Assert.Equal(-72,9*(-8));
        Assert.Equal(15,5*3*1);
    }
}
