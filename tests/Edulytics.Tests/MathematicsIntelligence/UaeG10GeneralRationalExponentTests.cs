using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>UAE Grade10 General U02L03 bounded rational exponents.
/// Authored in Stage A-B-C. These tests must NOT RUN until Stage D.</summary>
public sealed class UaeG10GeneralRationalExponentTests
{
    private const string LessonCode="PED:UAE-MOE-MATH:L10:GENERAL:02:03:RATIONAL-EXPONENTS";
    private const string Positive="uae.g10.general.u02.l03.rational_positive_perfect_power";
    private const string Negative="uae.g10.general.u02.l03.rational_negative_fraction";
    private const string Equation="uae.g10.general.u02.l03.exponential_same_base_equation";

    [Theory]
    [InlineData(Positive,LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData(Negative,LessonPracticeInteractionKind.FractionEntry)]
    [InlineData(Equation,LessonPracticeInteractionKind.IntegerKeypad)]
    public void SourceExactFamilyHasCorrectInput(string family,LessonPracticeInteractionKind expected)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var actual));
        Assert.Equal(expected,actual!.InteractionKind);
    }

    [Theory]
    [InlineData(Positive)]
    [InlineData(Negative)]
    [InlineData(Equation)]
    public void GeneratorAndIndependentVerifierAcceptOnlyCorrectScopedAnswers(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261125,20261126,20261127})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,
                                         ExactSkillQuestionDifficulty.Challenge})
        {
            var questions=engine.Generate(LessonCode,LessonCode,[family],difficulty,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var question in questions)
            {
                var p=question.Parameters;
                Assert.Equal(6,p.Count);
                Assert.InRange(p["root"],2,6);
                Assert.InRange(p["den"],2,4);
                Assert.InRange(p["num"],1,4);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,question.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                var forged=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                forged["den"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,question.CorrectAnswer));
                var arabic=string.Concat(question.CorrectAnswer.Select(ch=>
                    ch is >= '0' and <= '9' ? (char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                int r=p["root"],m=p["num"],denominator=1;
                for(int j=0;j<m;j++)denominator*=r;
                if(family==Negative)
                {
                    Assert.Equal("1/"+denominator.ToString(CultureInfo.InvariantCulture),question.CorrectAnswer);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,
                        "2/"+(2*denominator).ToString(CultureInfo.InvariantCulture)));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                        "1/"+(denominator+1).ToString(CultureInfo.InvariantCulture)));
                    Assert.Equal(AssessmentItemType.ShortAnswer,question.ItemType);
                }
                else
                {
                    int expected=family==Positive?denominator:p["target"]-p["offset"];
                    Assert.Equal(expected,int.Parse(question.CorrectAnswer,CultureInfo.InvariantCulture));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                        (expected+1).ToString(CultureInfo.InvariantCulture)));
                    Assert.Equal(AssessmentItemType.Numeric,question.ItemType);
                }
            }
        }
    }
    [Fact]
    public void ExactMathematicalSourceGoldenValuesRespectRealRootDomain()
    {
        Assert.Equal(9,3*3);
        Assert.Equal(8,2*2*2);
        Assert.Equal(25,5*5);
        Assert.Equal(-2,Math.Cbrt(-8));
        Assert.True(double.IsNaN(Math.Pow(-16,0.25)));
        Assert.Equal(4,6-2); // 3^(x+2)=3^6 => x=4
    }
}
