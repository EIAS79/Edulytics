using Edulytics.Core.Curriculum;
using Edulytics.Services.LessonContent;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG10ArabicRuntimePolicyTests
{
    [Fact]
    public void EveryCanonicalArabicG10LessonIsExposedWithoutInventedOfficialAlignment()
    {
        var doc = PedagogicalLessonBlueprintRegistry.LoadEmbeddedDocuments()
            .Single(x => x.PackCode == "UAE-MOE-MATH" && x.LogicalLevel == 10 &&
                x.Pathway == "General");
        Assert.Equal(148, doc.Lessons.Count);
        Assert.Equal(18, doc.Units.Count);
        foreach (var lesson in doc.Lessons)
        {
            Assert.True(LessonContentPolicy.IsUaeG10ArabicLesson("UAE-MOE-MATH", lesson.LessonCode));
            Assert.True(LessonContentPolicy.CanExposeStudentLesson("UAE-MOE-MATH", true, lesson.LessonCode));
            var rich = RichLessonContentV2Registry.Find(lesson.LessonCode, "ar");
            Assert.NotNull(rich);
            Assert.Equal("ar", rich!.CultureCode);
            Assert.Equal(lesson.Title, rich.Title);
        }
    }

    [Theory]
    [InlineData("PED:UAE-MOE-MATH:L10:ADVANCED:01:01:GRAPHING-QUADRATIC-FUNCTIONS")]
    [InlineData("PED:UAE-MOE-MATH:L10:APPLIED:01:01:NOT-APPROVED")]
    [InlineData("PED:UAE-MOE-MATH:L10:GENERAL:99:99:NOT-APPROVED")]
    public void RetiredOrUnknownG10IdentitiesAreNotExposedEvenIfAligned(string code)
    {
        Assert.False(LessonContentPolicy.IsUaeG10ArabicLesson("UAE-MOE-MATH", code));
        Assert.False(LessonContentPolicy.CanExposeStudentLesson("UAE-MOE-MATH", false, code));
        Assert.False(LessonContentPolicy.IsUaeG10ArabicLesson("US-CCSS-MATH", code));
    }
}
