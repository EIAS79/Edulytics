using System;
using System.Globalization;
using System.Linq;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class Grade4Ordinal105PartialQuotientsRemainderTests
{
    private const string Code="PED:US-CCSS-MATH:G4:U06:L19";
    private const string Family="usccss.g4.u06.l19.partial_quotients_remainders";
    [Fact] public void OriginalMathAndIndependentLessonModels()
    {
        Assert.Equal(55,389/7);Assert.Equal(4,389%7);
        Assert.Equal(234,702/3);Assert.Equal(463,2316/5);
        Assert.Equal(1,2316%5);Assert.Equal(1098,3294/3);
        Assert.Equal(236,947/4);Assert.Equal(3,947%4);
        var l=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(l);Assert.Equal(3,l!.Visuals.Count);
        Assert.Contains(l.WorkedExamples,e=>e.Question.Contains("3294"));
    }
    [Fact] public void ExactLessonRouteUsesStructuredRemainderEntry()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Code,out var c));
        Assert.NotNull(c);Assert.Equal(new[]{Family},c!.AllowedQuestionFamilies);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(Family,out var ui));
        Assert.Equal(LessonPracticeInteractionKind.StructuredEntry,ui!.InteractionKind);
        Assert.True(ui.HasVisualRepresentation);
    }
    [Fact] public void GeneratorVerifierRejectsIncorrectAndMalformedRemainders()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        var zero=false;var positive=false;
        foreach(var seed in new[]{20211019,20261027,20211020})
        foreach(var q in engine.Generate("im2021-g4-u06-l19",Family,[Family],
            ExactSkillQuestionDifficulty.Standard,8,seed,[]))
        {
            var p=q.Parameters;var dividend=p["dividend"];var divisor=p["divisor"];
            var result=p["partialTens"]+p["partialOnes"];var r=p["remainder"];
            zero|=r==0;positive|=r>0;
            Assert.Equal(dividend,divisor*result+r);Assert.InRange(r,0,divisor-1);
            Assert.Equal(result.ToString(CultureInfo.InvariantCulture)+" r "+r,q.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,(result+1)+" r "+r));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,result+" r "+(r+1)));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"bad response"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,""));
            var tampered=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
            tampered["partialTens"]+=10;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,tampered,q.CorrectAnswer));
        }
        Assert.True(zero);Assert.True(positive);
    }
}
