using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original Grade10 U02L02 quotient, zero and negative exponent
/// math. Written during A–B–C; DO NOT RUN before Stage D.</summary>
public sealed class UaeG10GeneralExponentQuotientTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:02:02:DIVISION-PROPERTIES-OF-EXPONENTS";
    private const string Quotient="uae.g10.general.u02.l02.quotient_exponent_difference";
    private const string Negative="uae.g10.general.u02.l02.negative_power_fraction";
    private const string Zero="uae.g10.general.u02.l02.zero_exponent_value";

    [Theory]
    [InlineData(Quotient,LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData(Negative,LessonPracticeInteractionKind.FractionEntry)]
    [InlineData(Zero,LessonPracticeInteractionKind.IntegerKeypad)]
    public void ExactFamilyHasSourceCorrectLearnerInput(string family,LessonPracticeInteractionKind renderer)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.Equal(renderer,ui!.InteractionKind);
    }

    [Theory]
    [InlineData(Quotient)]
    [InlineData(Negative)]
    [InlineData(Zero)]
    public void GeneratorAndIndependentVerifierAcceptEquivalentFractionsAndRejectWrongAnswers(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261125,20261126,20261127})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,
                                         ExactSkillQuestionDifficulty.Challenge})
        {
            var items=engine.Generate(Code,Code,[family],difficulty,4,seed,[]);
            Assert.Equal(4,items.Count);
            foreach(var item in items)
            {
                var p=item.Parameters;
                Assert.Equal(4,p.Count);
                Assert.InRange(p["base"],2,8);
                Assert.InRange(p["negative"],1,4);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                var forged=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                forged["base"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,item.CorrectAnswer));
                if(family==Negative)
                {
                    Assert.Equal(AssessmentItemType.ShortAnswer,item.ItemType);
                    int denominator=(int)Math.Pow(p["base"],p["negative"]);
                    Assert.Equal("1/"+denominator.ToString(CultureInfo.InvariantCulture),item.CorrectAnswer);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,
                        "2/"+(denominator*2).ToString(CultureInfo.InvariantCulture)));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                        "1/"+(denominator+1).ToString(CultureInfo.InvariantCulture)));
                    var arabic=string.Concat(item.CorrectAnswer.Select(c=>c is >= '0' and <= '9'
                        ? (char)('٠'+c-'0'):c));
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                }
                else
                {
                    Assert.Equal(AssessmentItemType.Numeric,item.ItemType);
                    int actual=int.Parse(item.CorrectAnswer,CultureInfo.InvariantCulture);
                    Assert.Equal(family==Zero?1:p["top"]-p["bottom"],actual);
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                        (actual+1).ToString(CultureInfo.InvariantCulture)));
                }
            }
        }
    }

    [Fact]
    public void OriginalStudentRulesAndDomainAreMathematicallyCorrect()
    {
        foreach(int n in new[]{2,3,4})
        {
            Assert.Equal((int)Math.Pow(n,5),
                (int)Math.Pow(n,9)/(int)Math.Pow(n,4));
            Assert.Equal(1, (int)Math.Pow(n,0));
        }
        Assert.Equal(1d/16d,Math.Pow(2,-4));
        Assert.Equal(1d/32d,Math.Pow(2,3)/Math.Pow(2,8));
        Assert.NotEqual(0d,Math.Pow(3,0));
    }
}
