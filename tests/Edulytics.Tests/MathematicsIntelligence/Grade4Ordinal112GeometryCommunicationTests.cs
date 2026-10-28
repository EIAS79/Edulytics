using System;
using System.Linq;
using Edulytics.Core.Curriculum;
using Edulytics.Services.Mathematics;
using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class Grade4Ordinal112GeometryCommunicationTests
{
    private const string Code="PED:US-CCSS-MATH:G4:U07:L01";
    [Fact]
    public void LessonPreservesInformalSourceGoalAndFiveLineDrawingConstraint()
    {
        var lesson=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(lesson);
        Assert.Contains(lesson!.ExplanationParagraphs,s=>s.Contains("not yet to demand formal definitions"));
        Assert.Contains(lesson.WorkedExamples,x=>x.Question.Contains("five long straight lines"));
        Assert.Contains(lesson.WorkedExamples,x=>x.Question.Contains("back-to-back") || x.Title.Contains("Back-to-back"));
        Assert.Contains(lesson.WorkedExamples,x=>x.Title.Contains("Cool-down") && x.Answer.Contains("Teacher-reviewed"));
        Assert.All(lesson.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
        Assert.Equal(3,lesson.Visuals.Count);
        Assert.DoesNotContain(lesson.WorkedExamples,e=>e.Check.Contains("0+1 = 1"));
    }
    [Fact]
    public void DoNotFakeNumericScoringForOpenDrawingActivities()
    {
        // A first-party digital precision-of-words exercise is legitimate support.
        // It does NOT machine-score the original hidden picture cards, dot drawing,
        // or teacher review of the source figures.
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Code,out _));
        var engine = new ExactSkillContractQuestionEngine();
        const string family = "usccss.g4.u07.l01.geometric_description_precision";
        var examples = engine.Generate("g4-2021-u07-l01-open-drawings-separate", family,
            [family], ExactSkillQuestionDifficulty.Standard, 4, 20210701, []);
        Assert.All(examples, item => {
            Assert.Contains("Which instruction is clearer", item.Prompt);
            Assert.DoesNotContain("original blackline", item.Prompt);
        });
    }
}
