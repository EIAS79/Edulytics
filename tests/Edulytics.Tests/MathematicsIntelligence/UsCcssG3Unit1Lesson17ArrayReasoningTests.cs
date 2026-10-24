using Edulytics.Core.Curriculum;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Authored Stage C contract only; tests intentionally NOT RUN.</summary>
public sealed class UsCcssG3Unit1Lesson17ArrayReasoningTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U01:L17";

    [Fact]
    public void SourceCardSortWodbAndDrawingsNeverReceiveNumericAutoApproval()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.False(lesson.ReadyForStageD);
        Assert.Equal(6,lesson.SourceSubtasks.Count);
        Assert.All(lesson.SourceSubtasks,task=>
        {
            Assert.Equal("teacher",task.AnswerType);
            var feedback=Grade3SourceCompanionCatalogue.Grade(task,"16");
            Assert.Null(feedback.NumericCorrect);
            Assert.True(feedback.TeacherReview);
        });
        Assert.Equal("teacher",lesson.OriginalEdulyticsCoolDown!.AnswerType);
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(
            lesson.OriginalEdulyticsCoolDown,"16").NumericCorrect);
    }

    [Fact]
    public void SourceSpecificRichV2CoversHumanGeometryRatherThanOnlyProducts()
    {
        var rich=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(rich);
        var body=string.Join(" ",rich.ExplanationParagraphs);
        Assert.Contains("15 dots",body);
        Assert.Contains("16",body);
        Assert.Contains("MLR1 Stronger and Clearer Each Time",body);
        Assert.Contains("four groups of three",body);
        Assert.Contains("incomplete column",body);
        Assert.Contains("multiple arrays",body);
        Assert.Contains(rich.CommonMistakes,q=>q.Mistake.Contains("automatic pass"));
        Assert.Contains(rich.SummaryPoints,q=>q.Contains("teacher-assessed"));
    }
}
