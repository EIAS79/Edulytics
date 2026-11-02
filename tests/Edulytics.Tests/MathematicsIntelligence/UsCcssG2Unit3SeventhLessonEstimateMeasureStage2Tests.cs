using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3SeventhLessonEstimateMeasureStage2Tests
{
    [Fact]
    public void TeacherTalkAndNotebookSourceFactsAreCorrect()
    {
        var talk=UsCcssG2Unit3SeventhLessonEstimateMeasureStage2.Talk("17","16","20","21");
        Assert.True(talk.Correct);Assert.True(talk.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3SeventhLessonEstimateMeasureStage2.Talk("17","17","20","21").Correct);
        var notebook=UsCcssG2Unit3SeventhLessonEstimateMeasureStage2.SourceNotebook("20","31","11");
        Assert.True(notebook.Correct);Assert.True(notebook.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3SeventhLessonEstimateMeasureStage2.SourceNotebook("20","31","-11").Correct);
    }
    [Fact]
    public void FirstPartyIllustrationsAreNotClaimedRealMeasurement()
    {
        var firstParty=UsCcssG2Unit3SeventhLessonEstimateMeasureStage2.Independent("7","5","2");
        Assert.True(firstParty.Correct);
        Assert.False(firstParty.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3SeventhLessonEstimateMeasureStage2.Independent("7","-5","2").Correct);
        Assert.True(UsCcssG2Unit3SeventhLessonEstimateMeasureStage2.TeacherRubric.Count>=6);
    }
}
