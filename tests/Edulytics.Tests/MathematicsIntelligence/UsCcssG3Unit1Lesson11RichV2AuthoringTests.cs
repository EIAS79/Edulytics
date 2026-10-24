using Edulytics.Core.Curriculum;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage A–C evidence tests AUTHORED but deliberately not run.</summary>
public sealed class UsCcssG3Unit1Lesson11RichV2AuthoringTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U01:L11";

    [Fact]
    public void Original2021RichV2KeepsFiveTrueCardSetsAndLessonSequence()
    {
        var rich = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(rich);
        Assert.Equal("Multiplication Expressions",rich.Title);
        Assert.Equal(6,rich.ExplanationParagraphs.Count);
        Assert.Equal(5,rich.WorkedExamples.Count);
        var intro=string.Join(" ",rich.ExplanationParagraphs);
        Assert.Contains("A–F–L=3×5",intro);
        Assert.Contains("B–H–K=4×3",intro);
        Assert.Contains("C–I=3×2",intro);
        Assert.Contains("D–G=2×10",intro);
        Assert.Contains("E–J=3×10",intro);
        Assert.Contains("SIX envelopes",intro);
        Assert.Contains("first-edition",intro);
        Assert.Empty(rich.Visuals);
        Assert.Empty(rich.Videos);
    }

    [Theory]
    [InlineData("G3-U01-L11-A3-01","3x3","9")]
    [InlineData("G3-U01-L11-A3-02","2x4","8")]
    [InlineData("G3-U01-L11-A3-03","2x6","12")]
    [InlineData("G3-U01-L11-CD-01","6x2","12")]
    public void StudentSourceExpressionMustShowOrderedGroupFactors(
        string id,string correct,string productOnly)
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        var q=lesson.SourceSubtasks.Single(x=>x.Id==id);
        Assert.Equal("multiplication-expression",q.AnswerType);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q,correct).NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q,correct+"="+(int.Parse(productOnly))).NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(q,productOnly).NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q,correct).TeacherReview);
    }

    [Fact]
    public void AllElevenOriginalSubtasksAreSeparateFromOptionalEdulyticsAssessment()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(11,lesson.SourceSubtasks.Count);
        Assert.Equal(7,lesson.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));
        Assert.Equal(4,lesson.SourceSubtasks.Count(x=>x.AnswerType=="multiplication-expression"));
        Assert.True(lesson.OriginalStudentTaskPdfRead);
        Assert.Equal("6x2",lesson.SourceSubtasks.Last().Expected);
        Assert.Equal("4x6",lesson.OriginalEdulyticsCoolDown!.Expected);
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(lesson.SourceSubtasks[2],"10").NumericCorrect);
        Assert.True(lesson.ReadyForStageD);
    }
}
