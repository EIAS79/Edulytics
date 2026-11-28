using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade 10 General U05L01, Applied source PDF111–117 (printed294–300).
/// Tests WRITTEN in Stage A–B–C; execution belongs to Stage D only.
/// </summary>
public sealed class UaeG10ExponentialGraphTests
{
    private const string Code =
        "PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U05:L01:C042";
    private const string Value = "uae.g10.general.u05.l01.value_at_x";
    private const string Growth = "uae.g10.general.u05.l01.is_growth";
    private const string Asymptote = "uae.g10.general.u05.l01.horizontal_asymptote";
    private const string Anchor = "uae.g10.general.u05.l01.value_at_shift";

    [Theory]
    [InlineData(Value)]
    [InlineData(Growth)]
    [InlineData(Asymptote)]
    [InlineData(Anchor)]
    public void ExactlyBoundedExponentialGraphFamilyUsesIntegerKeypad(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
    }

    [Theory]
    [InlineData(Value)]
    [InlineData(Growth)]
    [InlineData(Asymptote)]
    [InlineData(Anchor)]
    public void GeneratedProblemsMatchIndependentGrowthDecayOrAsymptoteOracle(string family)
    {
        var engine = new ExactSkillContractQuestionEngine();
        foreach(var seed in new[] {20261125,20261126,20261127})
        foreach(var difficulty in new[] {ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Challenge})
        {
            var tasks=engine.Generate(Code,Code,[family],difficulty,5,seed,[]);
            Assert.Equal(5,tasks.Count);
            foreach(var task in tasks)
            {
                var p=task.Parameters;
                Assert.Equal(6,p.Count);
                Assert.Equal(AssessmentItemType.Numeric,task.ItemType);
                Assert.Contains("للدالة الأسية",task.Prompt,StringComparison.Ordinal);
                int b=p["base"],c=p["coeff"],d=p["direction"],h=p["h"],k=p["k"],step=p["step"];
                Assert.InRange(b,2,5);
                Assert.InRange(c,1,4);
                Assert.InRange(d,0,1);
                Assert.InRange(h,-2,2);
                Assert.InRange(k,-4,4);
                Assert.InRange(step,0,2);
                int amplitude=d==1?c:c*b*b;
                int value=d==1?amplitude*Pow(b,step)+k:amplitude/Pow(b,step)+k;
                int expected=family switch
                {
                    Value=>value,Growth=>d,Asymptote=>k,Anchor=>amplitude+k,
                    _=>throw new InvalidOperationException()
                };
                var submitted=int.Parse(task.CorrectAnswer,CultureInfo.InvariantCulture);
                Assert.Equal(expected,submitted);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,task.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,(expected+1).ToString(CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"wrong"));
                var arabic=string.Concat(task.CorrectAnswer.Select(ch=>
                    ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                var invalid=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                invalid["base"]=1;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,task.CorrectAnswer));
                // Anchor is f(h), not f(0); this catches shifted-coordinate confusion.
                if(family==Anchor)Assert.Equal(amplitude+k,submitted);
            }
        }
    }
    private static int Pow(int b,int p)
    {
        int value=1;for(int i=0;i<p;i++)value*=b;return value;
    }

    [Fact]
    public void OriginalGraphingLearningGoalsHaveCorrectTables()
    {
        int[] x=[-2,-1,0,1,2];
        double[] growth=[1d/9,1d/3,1,3,9];
        double[] decay=[9,3,1,1d/3,1d/9];
        for(int i=0;i<x.Length;i++)
        {
            Assert.Equal(growth[i],Math.Pow(3,x[i]),10);
            Assert.Equal(decay[i],Math.Pow(1d/3,x[i]),10);
            Assert.True(growth[i]>0);
            Assert.True(decay[i]>0);
            Assert.Equal(1+Math.Pow(2,x[i]),Math.Pow(2,x[i])+1,10);
        }
        Assert.Equal(1,Math.Pow(3,0));
        Assert.Equal(2,Math.Pow(2,0)+1);
    }
}
