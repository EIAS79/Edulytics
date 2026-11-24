using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Owner-source UAE Grade10 canonical General U01L13 cubic/quartic equations.
/// WRITTEN_NOT_RUN in Stage A–B–C. Stage D executes and proves runtime.
/// </summary>
public sealed class UaeG10GeneralPolynomialEquationsTests
{
    private const string LessonCode =
        "PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U01:L13:C013";
    private const string Cubic="uae.g10.general.u01.l13.cubic_integer_roots";
    private const string Biquadratic="uae.g10.general.u01.l13.biquadratic_integer_roots";

    [Theory]
    [InlineData(Cubic)]
    [InlineData(Biquadratic)]
    public void FamilyHasSourceCorrectStructuredLearnerResponse(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var x));
        Assert.Equal(LessonPracticeInteractionKind.StructuredEntry,x!.InteractionKind);
    }

    [Theory]
    [InlineData(Cubic)]
    [InlineData(Biquadratic)]
    public void GeneratedPolynomialEquationsHaveIndependentFullRootVerification(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261124,20261125,20261126})
        foreach(var difficulty in new[]{
            ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Challenge})
        {
            var problems=engine.Generate(LessonCode,LessonCode,[family],difficulty,4,seed,[]);
            Assert.Equal(4,problems.Count);
            foreach(var q in problems)
            {
                var p=q.Parameters;
                Assert.Equal(7,p.Count);
                Assert.Contains("حل المعادلة",q.Prompt);
                Assert.Equal(AssessmentItemType.ShortAnswer,q.ItemType);
                Assert.NotEqual(0,p["a"]);
                int[] coeffs=new int[5];
                if(family==Cubic)
                {
                    Assert.Equal(1,p["mode"]);
                    int a=p["a"],r1=p["r1"],r2=p["r2"],r3=p["r3"];
                    Assert.True(r1<r2&&r2<r3);
                    coeffs[0]=-a*r1*r2*r3;
                    coeffs[1]=a*(r1*r2+r1*r3+r2*r3);
                    coeffs[2]=-a*(r1+r2+r3);
                    coeffs[3]=a;
                }
                else
                {
                    Assert.Equal(2,p["mode"]);
                    int a=p["a"],u=p["u"],v=p["v"];
                    Assert.True(u>=1&&u<v&&v<=4);
                    coeffs[0]=a*u*u*v*v;
                    coeffs[2]=-a*(u*u+v*v);
                    coeffs[4]=a;
                }
                int Eval(int x)
                {
                    int sum=0;
                    for(int d=4;d>=0;d--)sum=sum*x+coeffs[d];
                    return sum;
                }
                var roots=Enumerable.Range(-4,9).Where(x=>Eval(x)==0).ToArray();
                Assert.Equal(family==Cubic?3:4,roots.Length);
                string expected=string.Join(",",roots);
                Assert.Equal(expected,q.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,p,expected.Replace(',', '،')));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,p,expected+",100"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,p,string.Join(",",roots.Reverse())));

                var localDigits=string.Concat(expected.Select(ch=>ch is >= '0' and <= '9'
                    ?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,localDigits));
                var forged=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                forged["a"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,expected));
                forged=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                if(family==Cubic)forged["r2"]=forged["r1"];
                else forged["u"]=forged["v"];
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,expected));
            }
        }
    }

    [Fact]
    public void GoldenCubicBiquadraticAndPhysicalBoxVolumeExamplesAreCorrect()
    {
        foreach(var x in new[]{-4,-3,-2,-1,0,1,2,3,4})
        {
            Assert.Equal(x*x*x+x*x-6*x,x*(x+3)*(x-2));
            Assert.Equal(x*x*x-6*x*x+11*x-6,(x-1)*(x-2)*(x-3));
            Assert.Equal(x*x*x+2*x*x-9*x-18,(x+2)*(x-3)*(x+3));
            Assert.Equal(x*x*x*x-5*x*x+4,(x*x-1)*(x*x-4));
            Assert.Equal(x*x*x*x-13*x*x+36,(x*x-4)*(x*x-9));
        }
        Assert.Equal(60,3*(3+1)*(3+2));
        Assert.True(Math.Pow(1.76,3)-2*1.76-2<0);
        Assert.True(Math.Pow(1.77,3)-2*1.77-2>0);
    }
}
