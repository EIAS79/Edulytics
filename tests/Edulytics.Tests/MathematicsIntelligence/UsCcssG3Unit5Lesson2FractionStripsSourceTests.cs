using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Exact original 2021 fractional warmup and six-strip task. Tests authored, NOT RUN.</summary>
public sealed class UsCcssG3Unit5Lesson2FractionStripsSourceTests
{
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U05:L02",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
[Theory]
    [InlineData("G3-U05-L02-WU-PARTS-A","2",true)]
    [InlineData("G3-U05-L02-WU-PARTS-A","3",false)]
    [InlineData("G3-U05-L02-WU-PARTS-B","3",true)]
    [InlineData("G3-U05-L02-WU-PARTS-B","4",false)]
    [InlineData("G3-U05-L02-WU-PARTS-C","2",true)]
    [InlineData("G3-U05-L02-WU-PARTS-C","3",false)]
    [InlineData("G3-U05-L02-WU-PARTS-D","2",true)]
    [InlineData("G3-U05-L02-WU-PARTS-D","3",false)]
    [InlineData("G3-U05-L02-A1-DENOM-2","2",true)]
    [InlineData("G3-U05-L02-A1-DENOM-2","3",false)]
    [InlineData("G3-U05-L02-A1-DENOM-3","3",true)]
    [InlineData("G3-U05-L02-A1-DENOM-3","4",false)]
    [InlineData("G3-U05-L02-A1-DENOM-4","4",true)]
    [InlineData("G3-U05-L02-A1-DENOM-4","5",false)]
    [InlineData("G3-U05-L02-A1-DENOM-6","6",true)]
    [InlineData("G3-U05-L02-A1-DENOM-6","7",false)]
    [InlineData("G3-U05-L02-A1-DENOM-8","8",true)]
    [InlineData("G3-U05-L02-A1-DENOM-8","9",false)]
    [InlineData("G3-U05-L02-SYNTHESIS-1OF6","1",true)]
    [InlineData("G3-U05-L02-SYNTHESIS-1OF6","2",false)]
    [InlineData("G3-U05-L02-SYNTHESIS-DENOM6","6",true)]
    [InlineData("G3-U05-L02-SYNTHESIS-DENOM6","7",false)]
public void UnitFractionCountsFromOriginalSource(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(z=>z.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
[Fact]
public void PartnerTradeIsIntentionallyNonUniqueAndHumanReviewed(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(11,l.SourceSubtasks.Count(z=>z.AnswerType=="integer"));Assert.Equal(23,l.SourceSubtasks.Count(z=>z.AnswerType=="teacher"));foreach(var id in new[]{"G3-U05-L02-A2-SECRET","G3-U05-L02-A2-TRADE","G3-U05-L02-WU-A"}){var q=l.SourceSubtasks.Single(z=>z.Id==id);Assert.Null(Grade3SourceCompanionCatalogue.Grade(q,"1/6").NumericCorrect);}}
}
