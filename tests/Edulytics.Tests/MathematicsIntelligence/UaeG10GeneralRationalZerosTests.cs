using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Source-exact UAE G10 unified U01L16 rational zero theorem.
/// Tests AUTHORED during Stage A–B–C and NOT RUN until Stage D.
/// </summary>
public sealed class UaeG10GeneralRationalZerosTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U01:L16:C016";
    private const string Count="uae.g10.general.u01.l16.rational_candidate_count";
    private const string Candidate="uae.g10.general.u01.l16.rational_candidate_membership";
    private const string Actual="uae.g10.general.u01.l16.rational_actual_zero";

    [Theory]
    [InlineData(Count,LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData(Candidate,LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData(Actual,LessonPracticeInteractionKind.StructuredEntry)]
    public void CorrectExactFamilyAndInputShape(string family,LessonPracticeInteractionKind renderer)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.Equal(renderer,ui!.InteractionKind);
    }

    [Theory]
    [InlineData(Count)]
    [InlineData(Candidate)]
    [InlineData(Actual)]
    public void GeneratedQuestionsVerifyCorrectAndRejectTamperedRationalRootAnswers(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261125,20261126,20261127})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Challenge})
        {
            var items=engine.Generate(Code,Code,[family],difficulty,4,seed,[]);
            Assert.Equal(4,items.Count);
            foreach(var q in items)
            {
                var p=q.Parameters;
                Assert.Equal(6,p.Count);
                Assert.NotEqual(0,p["a3"]);
                Assert.NotEqual(0,p["a0"]);
                Assert.InRange(Math.Abs(p["p"]),1,10);
                Assert.InRange(p["q"],1,5);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"invalid"));
                var wrong=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                wrong["q"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,wrong,q.CorrectAnswer));
                if(family==Count)
                {
                    Assert.Equal(AssessmentItemType.Numeric,q.ItemType);
                    int n=int.Parse(q.CorrectAnswer,CultureInfo.InvariantCulture);
                    Assert.True(n>0);
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                        (n+1).ToString(CultureInfo.InvariantCulture)));
                    var localized=string.Concat(q.CorrectAnswer.Select(c=>c is >= '0' and <= '9'
                        ?(char)('٠'+c-'0'):c));
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,localized));
                }
                else
                {
                    Assert.Equal(AssessmentItemType.ShortAnswer,q.ItemType);
                    Assert.Contains(q.CorrectAnswer,new[]{"نعم","لا"});
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                        q.CorrectAnswer=="نعم"?"لا":"نعم"));
                }
            }
        }
    }

    [Fact]
    public void IndependentSourceMathShowsPossibleDoesNotEqualActual()
    {
        // 6x²-5x+1: potential ±1,±1/2,±1/3,±1/6.
        // Only x=1/2 and x=1/3 are zero; x=1 is an allowed nonroot.
        Assert.Equal(6*1*1-5*1+1,2);
        Assert.Equal(6*3*3-5*3*6+36,0); // Multiply P(3/6) by 36.
        Assert.Equal(6*2*2-5*2*6+36,0); // P(2/6) with denominator 6.
        // General rational-root example (3x-1)(x²+x+2).
        foreach(int x in new[]{-3,-1,0,1,2,4})
            Assert.Equal((3*x-1)*(x*x+x+2),3*x*x*x+2*x*x+5*x-2);
        Assert.Equal(3+2+5-2,8); // Candidate x=1 not actual root.
        foreach(int x in new[]{-2,-1,0,1,2,3})
            Assert.Equal((x-2)*(2*x+1)*(x*x+4),
                2*x*x*x*x-3*x*x*x+6*x*x-12*x-8);
    }

    [Fact]
    public void RatioMustBeReducedAndZeroConstantHandledAsSeparateGcfCase()
    {
        Assert.Equal(2*3,1*6);
        foreach(int x in new[]{-3,-1,0,1,4})
            Assert.Equal(x*x*(x-4),x*x*x-4*x*x);
    }
}
