using System;
using System.Linq;
using Edulytics.Core.Curriculum;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Stage A-C code-contract tests authored but deliberately NOT EXECUTED.</summary>
public sealed class UsCcssG3Unit1Lesson14UnknownEquationSourceContractsTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U01:L14";
    [Fact]
    public void SourceHasFourWarmupsSixCardPairsAndTenTwoStepEquationResponses()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(20,lesson.SourceSubtasks.Count);
        Assert.Equal(4,lesson.SourceSubtasks.Count(q=>q.Id.Contains("-WU-")));
        Assert.Equal(6,lesson.SourceSubtasks.Count(q=>q.Id.Contains("-CARD-")));
        Assert.Equal(5,lesson.SourceSubtasks.Count(q=>q.Id.Contains("-SYMBOL")));
        Assert.Equal(5,lesson.SourceSubtasks.Count(q=>q.Id.StartsWith("G3-U01-L14-A2-")&&!q.Id.Contains("-SYMBOL")));
        Assert.True(lesson.ReadyForStageD);
    }
    [Theory]
    [InlineData("?x5=35",true)]
    [InlineData("5x?=35",true)]
    [InlineData("35 = □ × 5",true)]
    [InlineData("_ * 5 = 35",true)]
    [InlineData("5x7=35",false)]
    [InlineData("5x?=30",false)]
    [InlineData("5x?=42",false)]
    [InlineData("35",false)]
    public void OriginalFirstDiagramRequiresUnknownFactorSymbolWithCorrectKnownTotal(string answer,bool expected)
    {
        Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson);
        var q=lesson!.SourceSubtasks.Single(t=>t.Id=="G3-U01-L14-A2-01-SYMBOL");
        var actual=Grade3SourceCompanionCatalogue.Grade(q,answer);
        Assert.Equal(expected,actual.NumericCorrect);
        Assert.True(actual.TeacherReview);
    }
    [Theory]
    [InlineData("5x2=?",true)]
    [InlineData("□ = 2×5",true)]
    [InlineData("2*5=_",true)]
    [InlineData("5x?=10",false)]
    [InlineData("5x2=10",false)]
    [InlineData("10",false)]
    [InlineData("5x2=11",false)]
    public void OriginalThirdDiagramRequiresUnknownProductPosition(string answer,bool expected)
    {
        Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson);
        var q=lesson!.SourceSubtasks.Single(t=>t.Id=="G3-U01-L14-A2-03-SYMBOL");
        Assert.Equal(expected,Grade3SourceCompanionCatalogue.Grade(q,answer).NumericCorrect);
    }
    [Fact]
    public void SpecificRichV2AndTeacherCardSortPairsAreRepresented()
    {
        var rich=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(rich);
        Assert.Contains(rich.ExplanationParagraphs,x=>x.Contains("A–L, B–H, C–E, D–F, G–K, I–J"));
        Assert.Contains(rich.KeyConcepts,x=>x.Title=="Unknown group count");
        Assert.Contains(rich.WorkedExamples,x=>x.Answer.Contains("7×5=35"));
        Assert.Empty(rich.Visuals);
    }
}
