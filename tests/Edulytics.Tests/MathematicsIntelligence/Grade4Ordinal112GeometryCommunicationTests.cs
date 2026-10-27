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
        Assert.False(LessonPracticeCapabilityResolver.TryResolve(Code,out _));
    }
}
