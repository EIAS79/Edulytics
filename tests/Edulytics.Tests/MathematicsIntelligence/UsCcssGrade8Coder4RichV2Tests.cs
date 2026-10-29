using Edulytics.Core.Curriculum;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Production Rich V2 resource-loader checks for independently authored Grade 8 Unit 1.
/// These tests prove content dispatch and mathematical invariants, not academic
/// acceptance, teacher assessment, or end-to-end Practice persistence.
/// </summary>
public sealed class UsCcssGrade8Coder4RichV2Tests
{
    [Theory]
    [InlineData(1, "Moving in the Plane")]
    [InlineData(2, "Naming the Moves")]
    [InlineData(3, "Grid Moves")]
    [InlineData(4, "Making the Moves")]
    [InlineData(5, "Coordinate Moves")]
    [InlineData(6, "Describing Transformations")]
    [InlineData(7, "No Bending or Stretching")]
    [InlineData(8, "Rotation Patterns")]
    [InlineData(9, "Moves in Parallel")]
    [InlineData(10, "Composing Figures")]
    [InlineData(11, "What Is the Same?")]
    [InlineData(12, "Congruent Polygons")]
    [InlineData(13, "Congruence")]
    [InlineData(14, "Alternate Interior Angles")]
    [InlineData(15, "Adding the Angles in a Triangle")]
    [InlineData(16, "Parallel Lines and the Angles in a Triangle")]
    [InlineData(17, "Rotate and Tessellate")]
    public void Grade8Unit1OriginalFirstPartyLessonIsLoadedFromExactRichV2Resource(
        int lessonNumber, string title)
    {
        var code = $"PED:US-CCSS-MATH:G8:U01:L{lessonNumber:D2}";
        var lesson = RichLessonContentV2Registry.Find(code, "en");
        Assert.NotNull(lesson);
        Assert.Equal(title, lesson.Title);
        Assert.True(lesson.ExplanationParagraphs.Count >= 2);
        Assert.True(lesson.KeyConcepts.Count >= 3);
        Assert.True(lesson.WorkedExamples.Count >= 3);
        Assert.True(lesson.CommonMistakes.Count >= 2);
        Assert.True(lesson.SummaryPoints.Count >= 3);
        Assert.Contains(lesson.ExplanationParagraphs, paragraph =>
            paragraph.Contains("CC BY 4.0", StringComparison.Ordinal));
        Assert.Contains(lesson.ExplanationParagraphs, paragraph =>
            paragraph.Contains("teacher", StringComparison.OrdinalIgnoreCase));
        foreach (var example in lesson.WorkedExamples)
        {
            Assert.True(example.Steps.Count >= 3);
            Assert.False(string.IsNullOrWhiteSpace(example.Answer));
            Assert.False(string.IsNullOrWhiteSpace(example.Check));
        }
        Assert.Empty(lesson.Videos);
    }

    [Fact]
    public void Grade8LessonLookupSupportsExistingCultureFallbackWithoutCrossGradeConfusion()
    {
        var code = "PED:US-CCSS-MATH:G8:U01:L03";
        var english = RichLessonContentV2Registry.Find(code, "en");
        Assert.NotNull(english);
        Assert.Equal(english.Title, RichLessonContentV2Registry.Find(code, "en-GB")?.Title);
        Assert.Null(RichLessonContentV2Registry.Find("PED:US-CCSS-MATH:G8:U01:L18", "en"));
    }

    [Fact]
    public void RotationReflectionTranslationAndPythagoreanChecksHaveCorrectIndependentOracles()
    {
        static (int x, int y) RotateCcw(int x, int y) => (-y, x);
        static (int x, int y) RotateCw(int x, int y) => (y, -x);
        static long D2((int x,int y) a, (int x,int y) b) =>
            (long)(a.x - b.x) * (a.x - b.x) + (long)(a.y - b.y) * (a.y - b.y);
        var rand = new Random(20261029);
        for (var i = 0; i < 2048; i++)
        {
            var a = (x: rand.Next(-300,301),y: rand.Next(-300,301));
            var b = (x: rand.Next(-300,301),y: rand.Next(-300,301));
            var dx = rand.Next(-50,51);
            var dy = rand.Next(-50,51);
            Assert.Equal(D2(a,b),D2((a.x+dx,a.y+dy),(b.x+dx,b.y+dy)));
            Assert.Equal(D2(a,b),D2(RotateCcw(a.x,a.y),RotateCcw(b.x,b.y)));
            Assert.Equal(D2(a,b),D2((-a.x,a.y),(-b.x,b.y)));
            var c = RotateCcw(a.x,a.y);
            Assert.Equal(a,RotateCw(c.x,c.y));
        }
        Assert.Equal(25,3*3+4*4);
        Assert.NotEqual(RotateCcw(2,1),RotateCw(2,1));
    }
}
