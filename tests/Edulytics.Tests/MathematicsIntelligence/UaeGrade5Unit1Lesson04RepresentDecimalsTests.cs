using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade 5 Arabic source printed pp29–34, PDF pp48–53.
// Stage C test artifacts written; NOT_RUN until Stage D.
public sealed class UaeGrade5Unit1Lesson04RepresentDecimalsTests
{
    private const string Lesson = "PED:UAE-MOE-MATH:L5:GENERAL:01:04";
    private const string Decimal = "uae.g5.u01.l04.thousandths_decimal";
    private const string Numerator = "uae.g5.u01.l04.thousandths_numerator";

    [Fact]
    public void Grade5UsesExistingDecimalFamiliesAndTwoThousandthExtensions()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("decimals.place_value",contract!.SkillId);
        Assert.Equal(4,contract.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
        }
    }

    [Fact]
    public void NinetyFiveThousandthsRejectsMisplacedDecimalAndSupportsArabicDigits()
    {
        var p=new Dictionary<string,int>
        { ["numerator"]=95,["denominator"]=1000 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Decimal,p,"0.095"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Decimal,p,"٠٫٠٩٥"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Decimal,p,"0.950"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Decimal,p,"0.95"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Numerator,p,"٩٥"));
    }

    [Fact]
    public void ThousandthZeroOneAndInvalidDenominatorAreHandledSafely()
    {
        var p=new Dictionary<string,int>
        { ["numerator"]=1,["denominator"]=1000 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Decimal,p,"0.001"));
        p["numerator"]=1000;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Decimal,p,"1.000"));
        p["numerator"]=0;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Decimal,p,"0.000"));
        p["numerator"]=1001;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Decimal,p,"1.001"));
        p["numerator"]=95;
        p["denominator"]=100;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Decimal,p,"0.950"));
    }

    [Fact]
    public void GeneratedThousandthQuestionsHaveNoAnswerLeakage()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in new[]{Decimal,Numerator})
        for(int seed=1;seed<=8;seed++)
        foreach(var item in engine.Generate(Lesson,family,[family],
            ExactSkillQuestionDifficulty.Standard,2,seed,[]))
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,item.CorrectAnswer));
            Assert.DoesNotContain("result",item.Parameters.Keys);
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"-1"));
        }
    }
}
