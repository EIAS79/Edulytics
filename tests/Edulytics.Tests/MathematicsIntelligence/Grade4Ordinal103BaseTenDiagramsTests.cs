using System;
using System.Globalization;
using System.Linq;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class Grade4Ordinal103BaseTenDiagramsTests
{
    private const string Code="PED:US-CCSS-MATH:G4:U06:L17";
    private const string Family="usccss.g4.u06.l16.base_ten_equal_groups";
    [Fact] public void SourcePriyaAndNoahExamplesLoadWithTheCorrectDiagramModels()
    {
        Assert.Equal(16,64/4);Assert.Equal(39,117/3);
        Assert.Equal(47,235/5);Assert.Equal(72,432/6);Assert.Equal(39,156/4);
        var l=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(l);Assert.Equal("Base-ten Diagrams to Represent Division",l!.Title);
        Assert.Equal(3,l.WorkedExamples.Count);Assert.Equal(3,l.Visuals.Count);
        Assert.Equal(4,l.Visuals[0].Rows.Count);
        Assert.Contains(l.WorkedExamples,e=>e.Question.Contains("235"));
        Assert.Contains(l.WorkedExamples,e=>e.Question.Contains("432"));
    }
    [Fact] public void ReusesOnlyPreviousSourceAlignedPlaceValueDivisionFamily()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Code,out var c));
        Assert.NotNull(c);Assert.Equal(new[]{Family},c!.AllowedQuestionFamilies);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(Family,out var ui));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
        Assert.True(ui.HasVisualRepresentation);
    }
    [Fact] public void GeneratedDiagramCompatibleDivisionRejectsWrongAndMalformedAnswers()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var q in engine.Generate("g4-2021-u06-l17",Family,[Family],ExactSkillQuestionDifficulty.Standard,6,20211017,[]))
        {
            var p=q.Parameters;
            Assert.Equal(p["dividend"]/p["divisor"],int.Parse(q.CorrectAnswer,CultureInfo.InvariantCulture));
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,(int.Parse(q.CorrectAnswer)+1).ToString()));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"garbage"));
            var bad=p.ToDictionary(k=>k.Key,v=>v.Value,StringComparer.Ordinal);
            bad["onesAfterTrade"]++;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,bad,q.CorrectAnswer));
        }
    }
}
