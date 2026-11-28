using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Stage C-only authored checks for unified Grade10 General U01L10.
/// Source 01متقدم_.pdf Advanced 2025–2026 pp204–212.
/// DO NOT RUN until Stage D; no CI/Practice result is asserted here.
/// </summary>
public sealed class UaeG10GeneralPolynomialDivisionReuseTests
{
    private const string LessonCode=
        "PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U01:L10:C010";
    private const string Monomial=
        "uae.g10.general.u01.l10.monomial_division_expression";
    private const string Quotient=
        "uae.g10.general.u01.l10.full_linear_divisor_quotient";
    private const string Remainder=
        "uae.g10.general.u01.l10.linear_divisor_remainder";

    [Theory]
    [InlineData(Monomial,LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData(Quotient,LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData(Remainder,LessonPracticeInteractionKind.IntegerKeypad)]
    public void SourceSpecificDivisionFamiliesHaveCorrectInput(string family,LessonPracticeInteractionKind kind)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.Equal(kind,interaction!.InteractionKind);
    }

    [Theory]
    [InlineData(Monomial)]
    [InlineData(Quotient)]
    [InlineData(Remainder)]
    public void ExactDivisionGeneratorIsIndependentlyVerifiable(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var seed in new[]{20261124,20261125,20261126})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Challenge})
        {
            var items=engine.Generate(LessonCode,LessonCode,[family],difficulty,4,seed,[]);
            Assert.Equal(4,items.Count);
            foreach(var question in items)
            {
                var p=question.Parameters;
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,p,question.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,p,question.CorrectAnswer+"+1"));
                if(family==Monomial)
                {
                    Assert.Equal(5,p.Count);
                    Assert.NotEqual(0,p["m"]);
                    int k=p["k"],m=p["m"];
                    Assert.InRange(k,0,2);
                    int[] pcoeff=new int[5];
                    for(int degree=0;degree<=2;degree++)
                        pcoeff[degree+k]=m*p["q"+degree];
                    // Distribution/monomial division have to be inverse.
                    for(int x=-3;x<=3;x++)
                    {
                        int polynomial=0;
                        for(int degree=0;degree<5;degree++)
                            polynomial+=pcoeff[degree]*(int)Math.Pow(x,degree);
                        int quotient=p["q0"]+p["q1"]*x+p["q2"]*x*x;
                        Assert.Equal(polynomial,m*(int)Math.Pow(x,k)*quotient);
                    }
                    var forged=p.ToDictionary(z=>z.Key,z=>z.Value,StringComparer.Ordinal);
                    forged["m"]=0;
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,forged,question.CorrectAnswer));
                }
                else
                {
                    Assert.Equal(6,p.Count);
                    int root=p["r"],q0=p["q0"],q1=p["q1"],q2=p["q2"],rem=p["remainder"];
                    for(int x=-3;x<=3;x++)
                    {
                        int q=q0+q1*x+q2*x*x;
                        int dividend=(x-root)*q+rem;
                        Assert.Equal(dividend,
                            q2*x*x*x+(q1-root*q2)*x*x+
                            (q0-root*q1)*x+rem-root*q0);
                    }
                    if(family==Remainder)
                    {
                        Assert.Equal(rem,int.Parse(question.CorrectAnswer,CultureInfo.InvariantCulture));
                        Assert.Equal(AssessmentItemType.Numeric,question.ItemType);
                    }
                    else Assert.Equal(AssessmentItemType.ShortAnswer,question.ItemType);
                }
                string arabicDigits=string.Concat(question.CorrectAnswer.Select(ch=>
                    ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabicDigits));
            }
        }
    }

    [Fact]
    public void SourceGoldenLongDivisionAndRemainderCasesAreMathematicallyExact()
    {
        for(int x=-5;x<=5;x++)
        {
            Assert.Equal(x*x+3*x-40,(x-5)*(x+8));
            Assert.Equal(2*x*x*x-3*x*x+4*x+1,
                (x-2)*(2*x*x+x+6)+13);
            Assert.Equal(x*x*x-5*x+6,
                (x-2)*(x*x+2*x-1)+4);
            Assert.Equal(x*x+7*x-11,(3-x)*(-x-10)+19);
        }
    }
}
