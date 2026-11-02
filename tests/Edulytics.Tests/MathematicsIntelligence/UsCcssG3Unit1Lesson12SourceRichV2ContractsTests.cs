using Edulytics.Core.Curriculum;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source alignment assertions only; NOT RUN during Stage A/B/C.</summary>
public sealed class UsCcssG3Unit1Lesson12SourceRichV2ContractsTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U01:L12";
    [Fact]
    public void RichV2Matches2021FirstEditionGroupCountsAndRevealOrder()
    {
        var l=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(l);
        var body=string.Join(" ",l.ExplanationParagraphs);
        Assert.Contains("FOUR 2-row-by-5-dot arrays",body);
        Assert.Contains("THIRTY dots in six rows of five",body);
        Assert.Contains("FIRST show only",body);
        Assert.Contains("THEN reveal",body);
        Assert.Contains("3 × 5 = 15",body);
        Assert.Contains("Ducks in a Pond",body);
        Assert.Equal(4,l.WorkedExamples.Count);
        Assert.Empty(l.Visuals);
    }
    [Fact]
    public void SixSourceNumbersAndFiveTeacherTasksRemainSeparate()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var l));
        Assert.NotNull(l);
        Assert.Equal(11,l.SourceSubtasks.Count);
        var expected=new[]{"15","8","14","50","20","30"};
        for(var i=0;i<6;i++){
            var q=l.SourceSubtasks[i];
            Assert.Equal("integer",q.AnswerType);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q,expected[i]).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(q,"999").NumericCorrect);
        }
        foreach(var q in l.SourceSubtasks.Skip(6)){
            Assert.Equal("teacher",q.AnswerType);
            var result=Grade3SourceCompanionCatalogue.Grade(q,"40");
            Assert.Null(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
        Assert.True(l.OriginalStudentTaskPdfRead);
        Assert.True(l.ReadyForStageD);
    }
}
