using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original 2021 price anchors and individually labeled FIRST-PARTY sample totals, unrun.</summary>
public sealed class UsCcssG3Unit3Lesson21BudgetSourceTests{
 private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U03:L21",out var x));return Assert.IsType<Grade3CompanionLesson>(x);}
 [Theory]
 [InlineData("G3-U03-L21-SOURCE-MARKERS","5",true)]
 [InlineData("G3-U03-L21-SOURCE-MARKERS","6",false)]
 [InlineData("G3-U03-L21-SOURCE-NATURE","400",true)]
 [InlineData("G3-U03-L21-SOURCE-NATURE","401",false)]
 [InlineData("G3-U03-L21-SOURCE-AQUARIUM","150",true)]
 [InlineData("G3-U03-L21-SOURCE-AQUARIUM","151",false)]
 [InlineData("G3-U03-L21-SOURCE-ZOO","350",true)]
 [InlineData("G3-U03-L21-SOURCE-ZOO","351",false)]
 [InlineData("G3-U03-L21-EXAMPLE-SUPPLIES","68",true)]
 [InlineData("G3-U03-L21-EXAMPLE-SUPPLIES","69",false)]
 [InlineData("G3-U03-L21-EXAMPLE-BOOKS","658",true)]
 [InlineData("G3-U03-L21-EXAMPLE-BOOKS","659",false)]
 [InlineData("G3-U03-L21-EXAMPLE-SPECIAL","230",true)]
 [InlineData("G3-U03-L21-EXAMPLE-SPECIAL","231",false)]
 [InlineData("G3-U03-L21-EXAMPLE-GAMES","15",true)]
 [InlineData("G3-U03-L21-EXAMPLE-GAMES","16",false)]
 [InlineData("G3-U03-L21-EXAMPLE-TOTAL","971",true)]
 [InlineData("G3-U03-L21-EXAMPLE-TOTAL","972",false)]
 [InlineData("G3-U03-L21-EXAMPLE-CHANGE","29",true)]
 [InlineData("G3-U03-L21-EXAMPLE-CHANGE","30",false)]
 public void SourcePricesAndExampleTotals(string id,string answer,bool expected){var t=Lesson().SourceSubtasks.Single(x=>x.Id==id);var g=Grade3SourceCompanionCatalogue.Grade(t,answer);Assert.Equal(expected,g.NumericCorrect);Assert.True(g.TeacherReview);}
 [Fact]public void ArbitraryShoppingListsAreHumanReviewed(){Assert.True(Lesson().ReadyForStageD);Assert.Equal(10,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(7,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var t in Lesson().SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var g=Grade3SourceCompanionCatalogue.Grade(t,"971");Assert.Null(g.NumericCorrect);Assert.True(g.TeacherReview);}}
}
