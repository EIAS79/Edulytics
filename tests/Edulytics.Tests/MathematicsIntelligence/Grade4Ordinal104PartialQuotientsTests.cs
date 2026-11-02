using System;
using System.Globalization;
using System.Linq;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class Grade4Ordinal104PartialQuotientsTests
{
    private const string Code="PED:US-CCSS-MATH:G4:U06:L18";
    private const string Family="usccss.g4.u06.l18.partial_quotients_recording";
    [Fact] public void SourceEquationsAndIndependentVisualsAreMathematicallyValid()
    {
        Assert.Equal(30,90/3);Assert.Equal(32,96/3);Assert.Equal(320,960/3);
        Assert.Equal(318,954/3);Assert.Equal(93,465/5);Assert.Equal(107,428/4);
        Assert.Equal(82,738/9);Assert.Equal(114,684/6);
        var l=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(l);Assert.Equal("Divide with Partial Quotients",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);Assert.Equal(3,l.Visuals.Count);
        Assert.Contains(l.WorkedExamples,e=>e.Question.Contains("428"));
        Assert.Contains(l.WorkedExamples,e=>e.Question.Contains("465"));
    }
    [Fact] public void OnlyExactLessonFamilyRoutesToNumericPractice()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Code,out var c));
        Assert.NotNull(c);Assert.Equal(new[]{Family},c!.AllowedQuestionFamilies);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(Family,out var ui));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
        Assert.True(ui.HasVisualRepresentation);
    }
    [Fact] public void ReusesDivisionEngineAndRejectsWrongMalformedAndTamperedDecompositions()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        foreach(var seed in new[]{20211018,20261027})
        foreach(var q in engine.Generate("g4-2021-u06-l18",Family,[Family],
            ExactSkillQuestionDifficulty.Standard,6,seed,[]))
        {
            var p=q.Parameters;
            var d=p["divisor"];var n=p["dividend"];
            var partial=p["tensQuotient"]+p["onesQuotient"];
            Assert.Equal(n/d,partial);
            Assert.Equal(n,d*partial);
            Assert.Equal(partial.ToString(CultureInfo.InvariantCulture),q.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,(partial+1).ToString()));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"garbage"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,""));
            var tampered=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
            tampered["tensQuotient"]+=10;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,tampered,q.CorrectAnswer));
        }
    }
}
