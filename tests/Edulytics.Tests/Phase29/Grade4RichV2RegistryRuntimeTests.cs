using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.Phase29;

public sealed class Grade4RichV2RegistryRuntimeTests
{
    [Fact]
    public void Grade4Lessons101Through149MaterializeThroughTheRealEmbeddedRegistry()
    {
        for (var unit = 6; unit <= 9; unit++)
        {
            var first = unit switch { 6 => 15, 7 => 1, 8 => 1, 9 => 1, _ => throw new InvalidOperationException() };
            var last = unit switch { 6 => 25, 7 => 16, 8 => 10, 9 => 12, _ => throw new InvalidOperationException() };
            for (var lessonNumber = first; lessonNumber <= last; lessonNumber++)
            {
                var code = $"PED:US-CCSS-MATH:G4:U{unit:00}:L{lessonNumber:00}";
                var lesson = RichLessonContentV2Registry.Find(code, "en");
                Assert.NotNull(lesson);
                Assert.True(lesson!.ExplanationParagraphs.Count >= 2);
                Assert.True(lesson.KeyConcepts.Count >= 3);
                Assert.True(lesson.WorkedExamples.Count >= 3);
                Assert.True(lesson.CommonMistakes.Count >= 2);
                Assert.True(lesson.SummaryPoints.Count >= 3);
            }
        }
    }
}
