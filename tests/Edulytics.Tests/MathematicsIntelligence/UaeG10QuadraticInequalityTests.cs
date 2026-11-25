using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>UAE G10 canonical39: tests authored for Stage D, NOT RUN in A–B–C.</summary>
public sealed class UaeG10QuadraticInequalityTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L06:C039";
    private const string Probe="uae.g10.general.u04.l06.probe_truth";
    private const string Middle="uae.g10.general.u04.l06.between_roots_truth";
    private const string Boundary="uae.g10.general.u04.l06.roots_included";

    [Theory]
    [InlineData(Probe)]
    [InlineData(Middle)]
    [InlineData(Boundary)]
    public void ExactSourceFamiliesAreBoundedIntegerKeypad(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var contract));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,contract!.InteractionKind);
    }

    [Theory]
    [InlineData(Probe)]
    [InlineData(Middle)]
    [InlineData(Boundary)]
    public void GeneratedQuadraticSignChartAnswersRecomputeFromSourceParameters(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261125,20261126,20261127})
        foreach(var diff in new[]{ExactSkillQuestionDifficulty.Standard,ExactSkillQuestionDifficulty.Challenge})
        {
            var items=engine.Generate(Code,Code,[family],diff,4,seed,[]);
            Assert.Equal(4,items.Count);
            foreach(var item in items)
            {
                var p=item.Parameters;
                Assert.Equal(5,p.Count);
                int a=p["a"],r1=p["r1"],r2=p["r2"],q=p["q"],x=p["x"];
                Assert.InRange(a,-4,4);Assert.NotEqual(0,a);
                Assert.True(r1<r2);Assert.InRange(q,0,3);Assert.InRange(x,-9,9);
                long value=(long)a*(x-r1)*(x-r2);
                int truth=family switch
                {
                    Probe=>q switch{0=>value>0?1:0,1=>value>=0?1:0,2=>value<0?1:0,_=>value<=0?1:0},
                    Middle=>q switch{0=>a<0?1:0,1=>a<0?1:0,2=>a>0?1:0,_=>a>0?1:0},
                    _=>q is 1 or 3?1:0
                };
                Assert.Equal(truth,int.Parse(item.CorrectAnswer,CultureInfo.InvariantCulture));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,item.CorrectAnswer));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,truth==1?"١":"٠"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,(1-truth).ToString(CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"نعم"));
                var forged=p.ToDictionary(y=>y.Key,y=>y.Value,StringComparer.Ordinal);
                forged["r2"]=forged["r1"];
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,item.CorrectAnswer));
            }
        }
    }

    [Fact]
    public void OriginalBookSignExamplesHaveCorrectSolutionsAndBoundaryConditions()
    {
        int f(int x)=>x*x-2*x-8;
        Assert.True(f(0)<0);
        Assert.Equal(0,f(-2));Assert.Equal(0,f(4));
        Assert.True(f(-3)>0&&f(5)>0);
        Assert.True((0-3)*(0-3)>0);
        Assert.Equal(0,(3-3)*(3-3));
        int h(int t)=>-5*t*t+20*t+25;
        Assert.Equal(25,h(0));Assert.True(h(2)>25);Assert.Equal(25,h(4));
    }
}
