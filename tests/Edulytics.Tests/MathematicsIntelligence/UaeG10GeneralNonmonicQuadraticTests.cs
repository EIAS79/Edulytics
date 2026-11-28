using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// UAE Grade10 General nonmonic a*x²+b*x+c, a>1. Full factors, rational roots
/// and integer-irreducibility; source-specific original tests are WRITTEN_NOT_RUN
/// during stages A/B/C, for Stage D execution after academic source closure.
/// </summary>
public sealed class UaeG10GeneralNonmonicQuadraticTests
{
    private const string LessonCode=
        "PED:UAE-MOE-MATH:L10:GENERAL:01:07:SOLVING-AX-BX-C-0";
    private const string Factor="uae.g10.general.u01.l07.nonmonic_full_factor";
    private const string Roots="uae.g10.general.u01.l07.nonmonic_rational_roots";
    private const string Prime="uae.g10.general.u01.l07.nonmonic_prime_over_integers";

    [Theory]
    [InlineData(Factor)]
    [InlineData(Roots)]
    [InlineData(Prime)]
    public void AllNonmonicFamilyQuestionsUseStructuredMathResponse(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.Equal(LessonPracticeInteractionKind.StructuredEntry,interaction!.InteractionKind);
    }

    [Theory]
    [InlineData(Factor)]
    [InlineData(Roots)]
    [InlineData(Prime)]
    public void GeneratorAndIndependentExactVerifierKeepNonmonicScope(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var seed in new[]{20261124,20261125,20261126})
        foreach(var level in new[]{ExactSkillQuestionDifficulty.Standard,
                                     ExactSkillQuestionDifficulty.Challenge})
        {
            var questions=engine.Generate(LessonCode,LessonCode,[family],level,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var question in questions)
            {
                Assert.Equal(AssessmentItemType.ShortAnswer,question.ItemType);
                Assert.Contains(question.Prompt,ch=>ch is >= '\u0600' and <= '\u06FF');
                var p=question.Parameters;
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,p,question.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,p,question.CorrectAnswer+"+1"));
                string localized=string.Concat(question.CorrectAnswer.Select(ch=>
                    ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,localized));

                if(family==Prime)
                {
                    Assert.Equal(3,p.Count);
                    int a=p["a"],b=p["b"],c=p["c"];
                    Assert.InRange(a,2,8);Assert.NotEqual(0,b);Assert.NotEqual(0,c);
                    bool expected=false;
                    for(int u=1;u<=a;u++)
                    {
                        if(a%u!=0)continue;
                        int w=a/u;
                        for(int v=-20;v<=20;v++)
                        {
                            if(v==0||c%v!=0)continue;
                            int z=c/v;
                            if(u*z+w*v==b)expected=true;
                        }
                    }
                    Assert.Equal(expected?"نعم":"لا",question.CorrectAnswer);
                    var forged=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                    forged["a"]=1;
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,forged,question.CorrectAnswer));
                }
                else
                {
                    Assert.Equal(4,p.Count);
                    int u=p["u"],w=p["w"],v=p["v"],z=p["z"];
                    Assert.InRange(u,1,4);Assert.InRange(w,1,4);
                    Assert.True(u*w>1);Assert.NotEqual(0,v);Assert.NotEqual(0,z);
                    int a=u*w,b=u*z+w*v,c=v*z;
                    foreach(int x in new[]{-3,-1,0,1,2,7})
                        Assert.Equal((u*x+v)*(w*x+z),a*x*x+b*x+c);
                    if(family==Factor)
                    {
                        string A=u==1?"س":u.ToString(CultureInfo.InvariantCulture)+"س";
                        string B=w==1?"س":w.ToString(CultureInfo.InvariantCulture)+"س";
                        string term1="("+A+(v<0?"−":"+")+Math.Abs(v)+")";
                        string term2="("+B+(z<0?"−":"+")+Math.Abs(z)+")";
                        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,term2+term1));
                    }
                    else
                    {
                        Assert.NotEqual(0,u*z-w*v);
                        // Deliberate unreduced fractions with the same exact values
                        // must still be accepted by cross-product rational comparison.
                        string root1=(-2*v).ToString(CultureInfo.InvariantCulture)+"/"+(2*u);
                        string root2=(-2*z).ToString(CultureInfo.InvariantCulture)+"/"+(2*w);
                        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,root2+","+root1));
                        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"1/0,2"));
                    }
                    var malformed=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                    malformed["u"]=0;
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,malformed,question.CorrectAnswer));
                }
            }
        }
    }

    [Fact]
    public void IndependentOriginalExamplesAndPhysicalContextHaveExpectedCoefficients()
    {
        foreach(var x in new[]{-3,-1,0,1,2,6})
        {
            Assert.Equal(6*x*x+11*x+3,(3*x+1)*(2*x+3));
            Assert.Equal(3*x*x-5*x-2,(3*x+1)*(x-2));
            Assert.Equal(4*x*x-4*x-3,(2*x+1)*(2*x-3));
            Assert.Equal(6*x*x-x-2,(3*x-2)*(2*x+1));
            Assert.Equal(2*x*x+11*x-30,(2*x+15)*(x-2));
            Assert.Equal(-6*x*x+7*x+3,-(3*x+1)*(2*x-3));
        }
        Assert.Equal(42,(2*2+3)*(2+4));
        Assert.Equal(0,-10*4*4+30*4+40);
        Assert.Equal(17,1-4*2*(-2)); // positive but nonsquare integer discriminant
    }
}
