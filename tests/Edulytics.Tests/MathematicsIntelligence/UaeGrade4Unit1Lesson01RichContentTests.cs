using Edulytics.Core.Curriculum;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit1Lesson01RichContentTests
{
    [Fact]
    public void ArabicLessonHasOriginalSourceAlignedPlaceValueChart()
    {
        var lesson=RichLessonContentV2Registry.Find(
            "PED:UAE-MOE-MATH:L4:COMMON:01:01","ar");
        Assert.NotNull(lesson);
        Assert.Equal("ar",lesson!.CultureCode);
        Assert.Equal("القيمة المكانية",lesson.Title);
        Assert.True(lesson.WorkedExamples.Count>=3);
        Assert.True(lesson.KeyConcepts.Count>=3);
        var chart=Assert.Single(lesson.Visuals);
        Assert.Equal(RichLessonVisualKind.PlaceValueChart,chart.Kind);
        Assert.Equal(7,chart.Rows[0].Cells.Count);
        Assert.Equal("٦٠٠٬٠٠٠",chart.Rows[2].Cells[1]);
        Assert.Contains(lesson.CommonMistakes,x=>x.Mistake.Contains("الأصفار"));
    }
}
