using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade10 General U04L05 exact discrimination and quadratic formula.
/// Stage A-B-C: test source WRITTEN_NOT_RUN. Stage D must execute.
/// </summary>
public sealed class UaeG10QuadraticFormulaTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L05:C038";
    private const string Delta="uae.g10.general.u04.l05.discriminant";
    private const string Count="uae.g10.general.u04.l05.real_root_count";
    private const string Roots="uae.g10.general.u04.l05.integer_roots_formula";

    [Theory]
    [InlineData(Delta,LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData(Count,LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData(Roots,LessonPracticeInteractionKind.StructuredEntry)]
    public void ExactQuestionKindMatchesDeclaredResponse(string family,LessonPracticeInteractionKind kind)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.Equal(kind,ui!.InteractionKind);
    }

    [Theory]
    [InlineData(Delta)]
    [InlineData(Count)]
    [InlineData(Roots)]
    public void SourceScopedExactProblemsIndependentlyCheckDeltaAndTwoRoots(string family)
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
                Assert.Equal(3,p.Count);
                int a=p["a"],b=p["b"],c=p["c"],D=b*b-4*a*c;
                Assert.NotEqual(0,a);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خاطئ"));
                var localized=string.Concat(q.CorrectAnswer.Select(ch=>ch switch
                {
                    >= '0' and <= '9'=>(char)('٠'+ch-'0'),
                    ','=>'،','-'=>'−',_=>ch
                }));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,localized));
                var corrupted=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                corrupted["a"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,corrupted,q.CorrectAnswer));
                if(family==Delta)
                {
                    Assert.Equal(AssessmentItemType.Numeric,q.ItemType);
                    Assert.Equal(D,int.Parse(q.CorrectAnswer,CultureInfo.InvariantCulture));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,p,(D+1).ToString(CultureInfo.InvariantCulture)));
                }
                else if(family==Count)
                {
                    int expected=D>0?2:D==0?1:0;
                    Assert.Equal(expected,int.Parse(q.CorrectAnswer,CultureInfo.InvariantCulture));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,p,(expected+1).ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    Assert.True(D>0);
                    int sqrt=(int)Math.Sqrt(D);
                    Assert.Equal(D,sqrt*sqrt);
                    Assert.Equal(AssessmentItemType.ShortAnswer,q.ItemType);
                    var pair=q.CorrectAnswer.Split(',');
                    Assert.Equal(2,pair.Length);
                    foreach(string answer in pair)
                    {
                        int root=int.Parse(answer,CultureInfo.InvariantCulture);
                        Assert.Equal(0,a*root*root+b*root+c);
                    }
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,$"{pair[1]},{pair[0]}"));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,$"{pair[0]},{pair[0]}"));
                }
            }
        }
    }

    [Fact]
    public void MathematicalGoldenCasesDistinguishRealDoubleIrrationalAndComplexRoots()
    {
        Assert.Equal(144,(-10)*(-10)-4*1*(-11));
        foreach(int x in new[]{11,-1})Assert.Equal(0,x*x-10*x-11);
        Assert.Equal(0,8*8-4*1*16);      // repeated root -4
        Assert.Equal(92,6*6-4*2*(-7));   // irrational roots (-3 ± √23)/2
        Assert.Equal(-4,(-6)*(-6)-4*1*10);// complex 3 ± i
        Assert.Equal(-36,2*2-4*2*5);    // no real roots
        foreach(int t in new[]{-1,5})Assert.Equal(0,-5*t*t+20*t+25);
    }
}
