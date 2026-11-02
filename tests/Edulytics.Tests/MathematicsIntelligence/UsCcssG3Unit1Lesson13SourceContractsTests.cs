using System;
using System.Linq;
using Edulytics.Core.Curriculum;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Stage A–C assertions are authored ONLY. Do not run tests until Stage D.</summary>
public sealed class UsCcssG3Unit1Lesson13SourceContractsTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U01:L13";
    [Fact]
    public void AllSixteenOriginalSubtasksIncludeRecoveredImages()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(16,lesson.SourceSubtasks.Count);
        Assert.All(lesson.SourceSubtasks,q=>Assert.Equal("multiplication-equation-either-order",q.AnswerType));
        Assert.Equal("8x2=16",lesson.SourceSubtasks.Single(x=>x.Id=="G3-U01-L13-A1-03").Expected);
        Assert.Equal("4x2=8",lesson.SourceSubtasks.Single(x=>x.Id=="G3-U01-L13-A1-08").Expected);
        Assert.True(lesson.ReadyForStageD);
    }
    [Theory]
    [InlineData("8x2=16",true)]
    [InlineData("2x8=16",true)]
    [InlineData("16=8x2",true)]
    [InlineData("16",false)]
    [InlineData("8x2",false)]
    [InlineData("8x2=18",false)]
    [InlineData("3x8=24",false)]
    public void RecoveredSourceDotImageStrictEquationWithCommutativeOrder(string answer,bool correct)
    {
        Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson);
        var q=lesson!.SourceSubtasks.Single(x=>x.Id=="G3-U01-L13-A1-03");
        var actual=Grade3SourceCompanionCatalogue.Grade(q,answer);
        Assert.Equal(correct,actual.NumericCorrect);
        Assert.True(actual.TeacherReview);
    }
    [Theory]
    [InlineData("4x2=8",true)]
    [InlineData("2x4=8",true)]
    [InlineData("8=4x2",true)]
    [InlineData("4x2=9",false)]
    [InlineData("8",false)]
    [InlineData("4+2=6",false)]
    public void RecoveredSourceTapeImageStrictEquation(string answer,bool correct)
    {
        Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson);
        var q=lesson!.SourceSubtasks.Single(x=>x.Id=="G3-U01-L13-A1-08");
        var actual=Grade3SourceCompanionCatalogue.Grade(q,answer);
        Assert.Equal(correct,actual.NumericCorrect);
    }
    [Fact]
    public void LegacyFactorOrderContractIsNotChanged()
    {
        var q=new Grade3OriginalQuestion {AnswerType="multiplication-equation",Expected="3x5=15"};
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q,"3x5=15").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(q,"5x3=15").NumericCorrect);
    }
    [Fact]
    public void DedicatedRichV2IncludesCompleteSourceSequenceWithoutCopyingGraphics()
    {
        var r=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(r);
        Assert.True(r.ExplanationParagraphs.Count>=6);
        Assert.Contains(r.ExplanationParagraphs,s=>s.Contains("Nine",StringComparison.OrdinalIgnoreCase));
        Assert.Contains(r.ExplanationParagraphs,s=>s.Contains("seven",StringComparison.OrdinalIgnoreCase));
        Assert.Contains(r.KeyConcepts,x=>x.Title=="Factor ordering");
        Assert.Contains(r.WorkedExamples,x=>x.Check.Contains("Sixteen colored dots"));
        Assert.Empty(r.Visuals);
        Assert.Empty(r.Videos);
    }
}
