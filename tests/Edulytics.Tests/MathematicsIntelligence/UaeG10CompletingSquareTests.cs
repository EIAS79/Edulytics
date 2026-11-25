using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade 10 U04L04: completing the square; authored in Stage C,
/// strictly NOT EXECUTED until Stage D.</summary>
public sealed class UaeG10CompletingSquareTests
{
    private const string LessonCode="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L04:C037";
    private const string Term="uae.g10.general.u04.l04.square_constant";
    private const string Roots="uae.g10.general.u04.l04.completed_square_roots";

    [Theory]
    [InlineData(Term,LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData(Roots,LessonPracticeInteractionKind.StructuredEntry)]
    public void FamilyHasCorrectBoundedMathAndInput(string family,LessonPracticeInteractionKind interactionKind)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var contract));
        Assert.Equal(interactionKind,contract!.InteractionKind);
    }

    [Theory]
    [InlineData(Term)]
    [InlineData(Roots)]
    public void GeneratedCompletingSquareHasIndependentlyVerifiedAnswersAndTamperRejection(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var seed in new[]{20261125,20261126,20261127})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,ExactSkillQuestionDifficulty.Challenge})
        {
            var questions=engine.Generate(LessonCode,LessonCode,[family],difficulty,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var q in questions)
            {
                Assert.Equal(3,q.Parameters.Count);
                int a=q.Parameters["a"],p=q.Parameters["p"],r=q.Parameters["r"];
                Assert.InRange(a,1,4);
                Assert.InRange(p,-7,7);
                Assert.NotEqual(0,p);
                Assert.Contains("المربع",q.Prompt);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"خاطئ"));
                var forged=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                forged["p"]=99;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,q.CorrectAnswer));
                string localized=string.Concat(q.CorrectAnswer.Select(ch=>ch switch
                {
                    >= '0' and <= '9'=>(char)('٠'+ch-'0'),
                    ','=>'،',
                    '-'=>'−',
                    _=>ch
                }));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,localized));
                if(family==Term)
                {
                    Assert.Equal(1,a);Assert.Equal(0,r);
                    Assert.Equal(AssessmentItemType.Numeric,q.ItemType);
                    Assert.Equal(p*p,int.Parse(q.CorrectAnswer,CultureInfo.InvariantCulture));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,q.Parameters,(p*p+1).ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    Assert.InRange(r,1,6);
                    Assert.Equal(AssessmentItemType.ShortAnswer,q.ItemType);
                    int first=-p-r,second=-p+r;
                    foreach(int root in new[]{first,second})
                        Assert.Equal(0,a*root*root+2*a*p*root+a*(p*p-r*r));
                    string reverse=$"{second},{first}";
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,reverse));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,$"{first},{first}"));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer+",0"));
                }
            }
        }
    }

    [Fact]
    public void OriginalSourceMathematicsHasTheExpectedTwoBranches()
    {
        foreach(int x in new[]{1,-11})
            Assert.Equal(0,x*x+10*x-11);
        foreach(int x in new[]{1,5})
        {
            if(x==5) continue; // the second nonmonic root is fractional, checked exactly below
            Assert.Equal(0,2*x*x-7*x+5);
        }
        Assert.Equal(0,2*25-7*10+5*4); // x=5/2, multiply by denominator 4
        Assert.Equal(36,(1+5)*(1+5));
        Assert.Equal(36,(-11+5)*(-11+5));
    }
}
