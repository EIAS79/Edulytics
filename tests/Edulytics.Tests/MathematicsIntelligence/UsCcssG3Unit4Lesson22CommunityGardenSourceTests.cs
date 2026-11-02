using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>2021 OPTIONAL garden source and first-party samples; written but never run.</summary>
public sealed class UsCcssG3Unit4Lesson22CommunityGardenSourceTests
{
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L22",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
[Theory]
    [InlineData("G3-U04-L22-A1-PLANTS","56",true)]
    [InlineData("G3-U04-L22-A1-PLANTS","57",false)]
    [InlineData("G3-U04-L22-A1-STRAWBERRIES-ROW","96",true)]
    [InlineData("G3-U04-L22-A1-STRAWBERRIES-ROW","97",false)]
    [InlineData("G3-U04-L22-FIRSTPARTY-ALL-STRAWBERRIES","672",true)]
    [InlineData("G3-U04-L22-FIRSTPARTY-ALL-STRAWBERRIES","673",false)]
    [InlineData("G3-U04-L22-FIRSTPARTY-TOMATOES","80",true)]
    [InlineData("G3-U04-L22-FIRSTPARTY-TOMATOES","81",false)]
    [InlineData("G3-U04-L22-FIRSTPARTY-CANTALOUPES","64",true)]
    [InlineData("G3-U04-L22-FIRSTPARTY-CANTALOUPES","65",false)]
    [InlineData("G3-U04-L22-FIRSTPARTY-ZUCCHINI","80",true)]
    [InlineData("G3-U04-L22-FIRSTPARTY-ZUCCHINI","81",false)]
public void GardenExactAndLabelledIndependentNumericChecks(string id,string response,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,response);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
[Fact]
public void TwoCropsDesignHasNoUniversalAnswer(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(6,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(13,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));var q=l.SourceSubtasks.Single(x=>x.Id=="G3-U04-L22-A2-VALID-PLAN");Assert.Null(Grade3SourceCompanionCatalogue.Grade(q,"72,80").NumericCorrect);}
}
