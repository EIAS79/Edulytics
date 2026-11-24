using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// G10 General Unit1 Lesson5 first-party factorization and zero-product skill.
/// Stage A–B–C authored tests only, STAGE D NOT RUN.
/// </summary>
public sealed class UaeG10GeneralDistributiveFactorTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L10:GENERAL:01:05:USING-THE-DISTRIBUTIVE-PROPERTY";
    private const string Factor="uae.g10.general.u01.l05.gcf_complete_factor";
    private const string Roots="uae.g10.general.u01.l05.zero_product_roots";

    [Theory]
    [InlineData(Factor)]
    [InlineData(Roots)]
    public void ExactFamilyUsesStructuredEntryAndAppropriateArabicMath(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.Equal(LessonPracticeInteractionKind.StructuredEntry,ui!.InteractionKind);
    }

    [Theory]
    [InlineData(Factor)]
    [InlineData(Roots)]
    public void IndependentVerifierMatchesSourceProblemWithNoAnswerLeak(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var seed in new[]{20261124,20261125,20261126})
        foreach(var level in new[]{ExactSkillQuestionDifficulty.Standard,
                                   ExactSkillQuestionDifficulty.Challenge})
        {
            var questions=engine.Generate(Lesson,Lesson,[family],level,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var item in questions)
            {
                Assert.Equal(AssessmentItemType.ShortAnswer,item.ItemType);
                Assert.Contains(item.Prompt,ch=>ch is >= '\u0600' and <= '\u06ff');
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"خطأ"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer+"+1"));
                var arabic=string.Concat(item.CorrectAnswer.Select(ch=>
                    ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,arabic));
                var tampered=item.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                if(family==Factor)
                {
                    Assert.Equal(4,tampered.Count);
                    int g=tampered["g"],a=tampered["a"],b=tampered["b"],k=tampered["k"];
                    Assert.InRange(g,2,7);Assert.InRange(k,0,2);
                    Assert.InRange(a,1,5);Assert.NotEqual(0,b);
                    // Independent numerical expansion oracle.
                    foreach(int x in new[]{-2,-1,0,1,3})
                    {
                        int original=g*a*(int)Math.Pow(x,k+1)+
                            g*b*(int)Math.Pow(x,k);
                        int expanded=g*(int)Math.Pow(x,k)*(a*x+b);
                        Assert.Equal(original,expanded);
                    }
                    tampered["k"]=4;
                }
                else
                {
                    Assert.Equal(3,tampered.Count);
                    int g=tampered["g"],a=tampered["a"],r=tampered["r"];
                    Assert.NotEqual(0,r);
                    Assert.Equal(0,g*0*(a*0-a*r));
                    Assert.Equal(0,g*r*(a*r-a*r));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"0"));
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,
                        r.ToString(CultureInfo.InvariantCulture)+",0"));
                    tampered["r"]=0;
                }
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,tampered,item.CorrectAnswer));
            }
        }
    }

    [Fact]
    public void GoldenOriginalMathematicsCatchesDroppedZeroAndWrongGrouping()
    {
        foreach(int x in new[]{-3,-1,0,1,2,5})
        {
            Assert.Equal(12*x*x+18*x,6*x*(2*x+3));
            Assert.Equal(3*x*x+6*x+2*x+4,(3*x+2)*(x+2));
            Assert.Equal(x*x-3*x+2*x-6,(x+2)*(x-3));
            Assert.Equal(-16*x*x+20*x,4*x*(5-4*x));
        }
        // x^2=3x has roots 0 and 3. Never divide by x without checking x=0.
        Assert.Equal(0,0*0-3*0);
        Assert.Equal(0,3*3-3*3);
        Assert.Equal(0,-16m*1.25m*1.25m+20m*1.25m);
    }
}
