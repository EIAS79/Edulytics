using System;
using System.Globalization;
using System.Linq;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class Grade4Ordinal102BaseTenSourceTests
{
    private const string Lesson="PED:US-CCSS-MATH:G4:U06:L16";
    private const string Family="usccss.g4.u06.l16.base_ten_equal_groups";
    [Fact] public void ExactTeacherSourceMathAndFirstPartyBlockModelsLoad()
    {
        Assert.Equal(122,488/4);
        Assert.Equal(13,104/8);
        Assert.Equal(24,96/4);
        Assert.Equal(43,86/2);
        Assert.Equal(12,108/9);
        Assert.Equal(22,132/6);
        var l=RichLessonContentV2Registry.Find(Lesson,"en");
        Assert.NotNull(l);
        Assert.Equal("Base-ten Blocks to Divide",l!.Title);
        Assert.Equal(3,l.WorkedExamples.Count);
        Assert.Equal(3,l.Visuals.Count);
        Assert.Equal(4,l.Visuals[0].Rows.Count);
    }
    [Fact] public void RuntimeRoutesOnlyTheSourceExactBaseTenFamily()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var routed));
        Assert.NotNull(routed);
        Assert.Equal(new[]{Family},routed!.AllowedQuestionFamilies);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(Family,out var ui));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
        Assert.True(ui.HasVisualRepresentation);
    }
    [Fact] public void GeneratedExchangeTraceRejectsWrongAndMalformedAnswers()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var seed in new[]{20211016,20261027})
        {
            var questions=engine.Generate("g4-2021-u06-l16",Family,[Family],ExactSkillQuestionDifficulty.Standard,8,seed,[]);
            Assert.Equal(8,questions.Count);
            foreach(var q in questions)
            {
                var p=q.Parameters;
                Assert.Equal(p["dividend"]/p["divisor"],int.Parse(q.CorrectAnswer,CultureInfo.InvariantCulture));
                Assert.Equal(p["dividend"]/10%10+10*(p["dividend"]/100%p["divisor"]),p["tensAfterTrade"]);
                Assert.Equal(p["dividend"]%10+10*(p["tensAfterTrade"]%p["divisor"]),p["onesAfterTrade"]);
                Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,(int.Parse(q.CorrectAnswer)+1).ToString()));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"wrong"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,""));
                var mutated=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                mutated["tensAfterTrade"]++;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family,mutated,q.CorrectAnswer));
            }
        }
    }
}
