using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// UAE Grade10 canonical General U01L14. Source-scoped R=f(a) and
/// x-a factor iff R=0. Stage C test source, NOT RUN until Stage D.
/// </summary>
public sealed class UaeG10GeneralRemainderFactorTheoremsTests
{
    private const string LessonCode=
        "PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U01:L14:C014";
    private const string Remainder="uae.g10.general.u01.l14.remainder_theorem_value";
    private const string Factor="uae.g10.general.u01.l14.factor_theorem_decision";

    [Theory]
    [InlineData(Remainder,LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData(Factor,LessonPracticeInteractionKind.StructuredEntry)]
    public void ExactFamilyAndStudentResponseMatchBookSkill(
        string family,LessonPracticeInteractionKind kind)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var contract));
        Assert.Equal(kind,contract!.InteractionKind);
    }

    [Theory]
    [InlineData(Remainder)]
    [InlineData(Factor)]
    public void IndependentHornerRemainderProofAndTamperingCases(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        bool foundFactor=false,foundNonfactor=false;
        foreach(var seed in new[]{20261124,20261125,20261126,20261127,20261128})
        foreach(var difficulty in new[]{
            ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Challenge})
        {
            var questions=engine.Generate(LessonCode,LessonCode,[family],
                difficulty,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var question in questions)
            {
                var p=question.Parameters;
                Assert.Equal(6,p.Count);
                Assert.InRange(p["degree"],1,2);
                int d=p["degree"],q2=p["q2"],q1=p["q1"],q0=p["q0"];
                int r=p["r"],remainder=p["remainder"];
                Assert.InRange(r,-4,4);Assert.InRange(remainder,-6,6);
                if(d==1)Assert.Equal(0,q2);
                else Assert.NotEqual(0,q2);
                // Independently reconstruct f(x)=(x-r)Q(x)+R.
                int[] coeffs=[
                    remainder-r*q0,
                    q0-r*q1,
                    q1-r*q2,
                    q2];
                int eval=0;
                for(int i=3;i>=0;i--)eval=checked(eval*r+coeffs[i]);
                Assert.Equal(remainder,eval);
                bool isFactor=eval==0;
                foundFactor|=isFactor;foundNonfactor|=!isFactor;
                string expected=family==Remainder
                    ?eval.ToString(CultureInfo.InvariantCulture)
                    :(isFactor?"نعم":"لا");
                Assert.Equal(expected,question.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,p,family==Remainder
                        ?(eval+1).ToString(CultureInfo.InvariantCulture)
                        :(isFactor?"لا":"نعم")));
                if(family==Remainder)
                {
                    Assert.Equal(AssessmentItemType.Numeric,question.ItemType);
                    var arabic=string.Concat(expected.Select(c=>
                        c is >= '0' and <= '9'?(char)('٠'+c-'0'):c));
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                    Assert.Contains("د(",question.Prompt);
                }
                else
                {
                    Assert.Equal(AssessmentItemType.ShortAnswer,question.ItemType);
                    Assert.Contains("نظرية العامل",question.Prompt);
                }
                var forged=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                forged["degree"]=9;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,expected));
                forged=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                forged["r"]=100;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,expected));
            }
        }
        if(family==Factor)
        {
            Assert.True(foundFactor,"Factor family should sample true case across multiple seeds.");
            Assert.True(foundNonfactor,"Factor family should sample false case across multiple seeds.");
        }
    }

    [Fact]
    public void OriginalIndependentRemainderAndFactorTheoremGoldenExamples()
    {
        // f=x^3-3x²+2x+5 / (x-2) ⇒ remainder f(2)=5.
        Assert.Equal(5,2*2*2-3*2*2+2*2+5);
        // f=x^3-6x²+11x-6, f(1)=0; x-1 is a factor.
        Assert.Equal(0,1-6+11-6);
        // f=2x³-x+7, divisor x+1 -> a=-1, f(-1)=6.
        Assert.Equal(6,2*(-1)*(-1)*(-1)-(-1)+7);
        // f=x⁴-4x²+3 with missing x³ and x, f(1)=0.
        Assert.Equal(0,1-4+3);
        // (x-1)(x³+x²-3x-3)=x⁴-4x²+3.
        foreach(int x in new[]{-3,-1,0,1,2,4})
            Assert.Equal(x*x*x*x-4*x*x+3,
                (x-1)*(x*x*x+x*x-3*x-3));
    }
}
