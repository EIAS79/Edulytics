using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>First-party oracle tests from Grade 3 U03 L14 archived 2021 HTML; not executed. Original PDF and figure provenance pending.</summary>
public sealed class UsCcssG3Unit3Lesson14NearestTensHundredsSourceTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U03:L14";
    private static Grade3CompanionLesson Lesson()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        return Assert.IsType<Grade3CompanionLesson>(lesson);
    }

    [Theory]
    [InlineData("G3-U03-L14-A1-01-HUNDRED","100",true)]
    [InlineData("G3-U03-L14-A1-01-HUNDRED","110",false)]
    [InlineData("G3-U03-L14-A1-01-TEN","130",true)]
    [InlineData("G3-U03-L14-A1-01-TEN","140",false)]
    [InlineData("G3-U03-L14-A1-02-HUNDRED","300",true)]
    [InlineData("G3-U03-L14-A1-02-HUNDRED","310",false)]
    [InlineData("G3-U03-L14-A1-02-TEN","270",true)]
    [InlineData("G3-U03-L14-A1-02-TEN","280",false)]
    [InlineData("G3-U03-L14-A1-03-HUNDRED","400",true)]
    [InlineData("G3-U03-L14-A1-03-HUNDRED","410",false)]
    [InlineData("G3-U03-L14-A1-03-TEN","440",true)]
    [InlineData("G3-U03-L14-A1-03-TEN","450",false)]
    [InlineData("G3-U03-L14-A1-04-HUNDRED","100",true)]
    [InlineData("G3-U03-L14-A1-04-HUNDRED","110",false)]
    [InlineData("G3-U03-L14-A1-04-TEN","90",true)]
    [InlineData("G3-U03-L14-A1-04-TEN","100",false)]
    [InlineData("G3-U03-L14-A1-05-HUNDRED","400",true)]
    [InlineData("G3-U03-L14-A1-05-HUNDRED","410",false)]
    [InlineData("G3-U03-L14-A1-05-TEN","350",true)]
    [InlineData("G3-U03-L14-A1-05-TEN","360",false)]
    [InlineData("G3-U03-L14-A2-349-HUNDRED","300",true)]
    [InlineData("G3-U03-L14-A2-349-HUNDRED","310",false)]
    [InlineData("G3-U03-L14-A2-349-TEN","350",true)]
    [InlineData("G3-U03-L14-A2-349-TEN","360",false)]
    [InlineData("G3-U03-L14-A2-712-HUNDRED","700",true)]
    [InlineData("G3-U03-L14-A2-712-HUNDRED","710",false)]
    [InlineData("G3-U03-L14-A2-712-TEN","710",true)]
    [InlineData("G3-U03-L14-A2-712-TEN","720",false)]
    [InlineData("G3-U03-L14-A2-568-HUNDRED","600",true)]
    [InlineData("G3-U03-L14-A2-568-HUNDRED","610",false)]
    [InlineData("G3-U03-L14-A2-568-TEN","570",true)]
    [InlineData("G3-U03-L14-A2-568-TEN","580",false)]
    [InlineData("G3-U03-L14-A2-324-HUNDRED","300",true)]
    [InlineData("G3-U03-L14-A2-324-HUNDRED","310",false)]
    [InlineData("G3-U03-L14-A2-324-TEN","320",true)]
    [InlineData("G3-U03-L14-A2-324-TEN","330",false)]
    [InlineData("G3-U03-L14-A2-89-HUNDRED","100",true)]
    [InlineData("G3-U03-L14-A2-89-HUNDRED","110",false)]
    [InlineData("G3-U03-L14-A2-89-TEN","90",true)]
    [InlineData("G3-U03-L14-A2-89-TEN","100",false)]
    public void SourceTextNumericAnchorsGradeWithoutGrantingDrawingEvidence(string id, string answer, bool expected)
    {
        var source = Lesson().SourceSubtasks.Single(x => x.Id == id);
        var grade = Grade3SourceCompanionCatalogue.Grade(source, answer);
        Assert.Equal(expected, grade.NumericCorrect);
        Assert.True(grade.TeacherReview);
    }

    [Fact]
    public void EstimateDrawingReasoningAndGatedCoolDownRemainTeacherOnly()
    {
        Assert.Equal("Nearest Multiples of 10 and 100",Lesson().Title);
        Assert.False(Lesson().OriginalStudentTaskPdfRead);
        Assert.True(Lesson().ReadyForStageD);
        Assert.Equal(20,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="integer"));
        Assert.Equal(9,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="teacher"));
        foreach(var q in Lesson().SourceSubtasks.Where(x=>x.AnswerType=="teacher"))
        {
            var result=Grade3SourceCompanionCatalogue.Grade(q,"300");
            Assert.Null(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
    }
}
