using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Lesson 9 source contract authored, never run as Stage D here.</summary>
public sealed class UsCcssG3Unit1Lesson9OriginalSourceCoverageTests
{
    [Fact]
    public void OriginalWarmupSignAndSituationMathematicsAreIndividualTeacherReviewedSubtasks()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U01:L09",out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(12,lesson.SourceSubtasks.Count);
        var expected=new[]{75,80,81,92,6,2,10,8,20,15};
        for(var i=0;i<expected.Length;i++)
        {
            var q=lesson.SourceSubtasks[i];
            Assert.Equal("integer",q.AnswerType);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q,expected[i].ToString()).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(q,(expected[i]+1).ToString()).NumericCorrect);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q,expected[i].ToString()).TeacherReview);
        }
        Assert.All(lesson.SourceSubtasks.Skip(10),q=>{
            Assert.Equal("teacher",q.AnswerType);
            Assert.Null(Grade3SourceCompanionCatalogue.Grade(q,"15").NumericCorrect);
        });
    }
    [Fact]
    public void PublisherCoolDownIsJadasThreeBagsFiveBraceletsNotStickers()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U01:L09",out var l));
        Assert.Contains("Jada",l!.SourceSubtasks.Last().Prompt);
        Assert.Contains("THREE DISTINCT EQUAL GROUPS",l.SourceSubtasks.Last().Prompt);
        Assert.Contains("NOT symbolic multiplication expressions",l.IndependentTeachingGuidance!.Caution);
        Assert.Equal(2,Grade3SourceCompanionCatalogue.SourceGraphSignsPerSymbol);
    }
}
