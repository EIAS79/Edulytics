using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG5RichLessonSourceNoteVisibilityTests
{
    [Fact]
    public void All148Grade5LessonsKeepSourceNotesInternalAndPreserveMathematicalSummaries()
    {
        var grade5 = RichLessonContentV2Registry.AllDocuments
            .SelectMany(document => document.Lessons)
            .Where(lesson => lesson.LessonCode.StartsWith(
                "PED:US-CCSS-MATH:G5:", StringComparison.Ordinal))
            .ToArray();

        Assert.Equal(148, grade5.Length);
        var hidden = 0;
        foreach (var lesson in grade5)
        {
            var authored = lesson.SummaryPoints.ToArray();
            var rendered = RichLessonSummaryVisibility.ForReader(
                lesson.LessonCode, lesson.SummaryPoints);

            Assert.True(rendered.Count >= 2,
                $"{lesson.LessonCode}: keep actual mathematical Quick Summary items");
            Assert.DoesNotContain(rendered,
                RichLessonSummaryVisibility.IsGrade5InternalSourceNote);
            hidden += authored.Length - rendered.Count;
            Assert.Equal(authored, lesson.SummaryPoints);
        }

        Assert.True(hidden >= 95,
            $"Expected at least the 95 documented private source links, actual {hidden}");
    }

    [Fact]
    public void NotesWithPublisherLinksStayInMemoryAndAreNotOutputForAnyGrade5Role()
    {
        var sourceNote = "Publisher reference: IM 2021; https://im.kendallhunt.com/k5/teacher";
        var mathNote = "3 × 4 = 12 cubic units.";
        var input = new[] { mathNote, sourceNote };
        var g5 = RichLessonSummaryVisibility.ForReader(
            "PED:US-CCSS-MATH:G5:U01:L01", input);

        Assert.Equal(new[] { mathNote }, g5);
        Assert.Equal(sourceNote, input[1]);
        Assert.Equal(input, RichLessonSummaryVisibility.ForReader(
            "PED:US-CCSS-MATH:G4:U01:L01", input));
    }

    [Fact]
    public void MathematicalDescriptionsThatUseSourceWordAloneAreNotAccidentallyRemoved()
    {
        var mathNote = "Compare the source of two equivalent numerical expressions.";
        var visible = RichLessonSummaryVisibility.ForReader(
            "PED:US-CCSS-MATH:G5:U08:L04", new[] { mathNote });

        Assert.Equal(new[] { mathNote }, visible);
    }
}
