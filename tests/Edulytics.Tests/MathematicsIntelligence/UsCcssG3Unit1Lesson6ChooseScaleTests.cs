using Edulytics.Core.Mathematics.Curriculum;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Stage C authoring-only, intentionally not executed.</summary>
public sealed class UsCcssG3Unit1Lesson6ChooseScaleTests
{
    [Fact]
    public void Archived2021SourceCategoryCountsMatch()
    {
        Assert.Equal(new[]{16,13,6,10,30},
            UsCcssG3Unit1Lesson6ChooseScale.SourcePets.Select(x=>x.Count));
        Assert.Equal(new[]{24,13,40,22},
            UsCcssG3Unit1Lesson6ChooseScale.SourceSeasons.Select(x=>x.Count));
        Assert.Equal(40,UsCcssG3Unit1Lesson6ChooseScale.GraphMaximum(40,5));
        Assert.Equal(40,UsCcssG3Unit1Lesson6ChooseScale.GraphMaximum(40,10));
        Assert.Equal(42,UsCcssG3Unit1Lesson6ChooseScale.GraphMaximum(41,2));
    }
    [Fact]
    public void AllThreeOriginalVerticalPetGraphsKeepThirtyOneTicksAndUnchangedHeights()
    {
        // Authoring-only: no tests or Razor render are executed before Stage D.
        var pets = UsCcssG3Unit1Lesson6ChooseScale.SourcePets;
        Assert.Equal(new[] { 16, 13, 6, 10, 30 }, pets.Select(p => p.Count));
        foreach (var (maximum, step) in new[] { (30, 1), (60, 2), (300, 10) })
        {
            Assert.Equal(31, maximum / step + 1);
            Assert.Equal(pets.Count, pets.Count(p => p.Count <= maximum));
        }
        DirectoryInfo? dir = new(AppContext.BaseDirectory);
        string? path = null;
        while (dir is not null)
        {
            var p = Path.Combine(dir.FullName, "src", "Edulytics.Web",
                "Views", "Grade3Lesson6Activities", "Index.cshtml");
            if (File.Exists(p)) { path = p; break; }
            dir = dir.Parent;
        }
        Assert.NotNull(path);
        var view = File.ReadAllText(path!);
        Assert.Contains("pet-vertical-bar", view);
        Assert.Contains("height:@(row.Count*372/max)px", view);
        Assert.Contains("tick=max;tick>=0;tick-=setting", view);
        Assert.Contains("Type of pets", view);
        Assert.Contains("Lifespan of Pets vertical bar graph", view);
    }

    [Theory]
    [InlineData("L06-SOURCE-PET-CAT",16)]
    [InlineData("L06-SOURCE-PET-PYTHON",30)]
    [InlineData("L06-SOURCE-PET-TOTAL",75)]
    [InlineData("L06-SOURCE-SEASONS-SPRING",13)]
    [InlineData("L06-SOURCE-SEASONS-TOTAL",99)]
    [InlineData("L06-SOURCE-SEASONS-SUMMER-MINUS-SPRING",27)]
    [InlineData("L06-EDU-CD-MUSIC",19)]
    [InlineData("L06-EDU-CD-SCIENCE",26)]
    [InlineData("L06-EDU-CD-DIFFERENCE",7)]
    public void ExactSourceAndOriginalCooldownMathDoNotAcceptWrongValues(string id,int correct)
    {
        Assert.True(UsCcssG3Unit1Lesson6ChooseScale.TryGradeNumeric(id,correct,out var yes));
        Assert.True(yes);
        Assert.True(UsCcssG3Unit1Lesson6ChooseScale.TryGradeNumeric(id,correct+1,out var wrong));
        Assert.False(wrong);
        Assert.False(UsCcssG3Unit1Lesson6ChooseScale.TryGradeNumeric(id+"-DRAW",correct,out _));
    }
    [Fact]
    public void Original2021ImageGeometriesAndIndependentSymbolCountsStayExact()
    {
        // Original SVG 7053kvgtyqg9pibtx7gto33ajaea: one exactly duplicated
        // green path = 27 green path occurrences but 26 visible triangles.
        Assert.Equal(new[]{26,5,15,10},
            UsCcssG3Unit1Lesson6ChooseScale.SourcePatternBlocks.Select(x=>x.Count));
        var symbols=UsCcssG3Unit1Lesson6ChooseScale.FirstPartyPatternShapeOrder;
        Assert.Equal(56,symbols.Count);
        foreach(var row in UsCcssG3Unit1Lesson6ChooseScale.SourcePatternBlocks)
        {
            Assert.Equal(row.Count,symbols.Count(x=>x==row.Category));
            Assert.True(UsCcssG3Unit1Lesson6ChooseScale.TryGradePatternCount(row.Category,row.Count,out var pass));
            Assert.True(pass);
            Assert.True(UsCcssG3Unit1Lesson6ChooseScale.TryGradePatternCount(row.Category,row.Count+1,out var wrong));
            Assert.False(wrong);
        }
    }

    [Theory]
    [InlineData("L06-SOURCE-PATTERN-TRIANGLE",26)]
    [InlineData("L06-SOURCE-PATTERN-SQUARE",5)]
    [InlineData("L06-SOURCE-PATTERN-TRAPEZOID",15)]
    [InlineData("L06-SOURCE-PATTERN-HEXAGON",10)]
    [InlineData("L06-SOURCE-SEASON-BAR-WINTER",24)]
    [InlineData("L06-SOURCE-SEASON-BAR-SPRING",13)]
    [InlineData("L06-SOURCE-SEASON-BAR-SUMMER",40)]
    [InlineData("L06-SOURCE-SEASON-BAR-FALL",22)]
    public void StudentAuthoredGraphCategoryTalliesAreCheckedWithoutRounding(string id,int answer)
    {
        Assert.True(UsCcssG3Unit1Lesson6ChooseScale.TryGradeNumeric(id,answer,out var right));
        Assert.True(right);
        Assert.True(UsCcssG3Unit1Lesson6ChooseScale.TryGradeNumeric(id,answer+1,out var wrong));
        Assert.False(wrong);
    }

    [Fact]
    public void SourceCooldownChronologyAndTwoDistinctOpenQuestionsAreWrittenNotRun()
    {
        DirectoryInfo? dir=new(AppContext.BaseDirectory);
        string? path=null;
        while(dir is not null)
        {
            var check=Path.Combine(dir.FullName,"src","Edulytics.Web","Views",
                "Grade3Lesson6Activities","Index.cshtml");
            if(File.Exists(check)){path=check;break;}
            dir=dir.Parent;
        }
        Assert.NotNull(path);
        var view=File.ReadAllText(path!);
        Assert.Contains("SourceCoolDownScale",view);
        Assert.Contains("SourceCoolDownLearning",view);
        Assert.Contains("MathCommunityNorm",view);
        Assert.Contains("FirstPartyPatternShapeOrder",view);
        Assert.Contains("SeasonWinterBar",view);
        Assert.True(view.IndexOf("4. Lesson synthesis",StringComparison.Ordinal)
            <view.IndexOf("5. Source-aligned Cool-down",StringComparison.Ordinal));
        Assert.True(view.IndexOf("5. Source-aligned Cool-down",StringComparison.Ordinal)
            <view.IndexOf("6. Math Community",StringComparison.Ordinal));
    }
    [Fact]
    public void ImageCountsRemainTeacherVerifiedAndScaleJudgmentIsNotMachinePassed()
    {
        UsCcssG3Unit1Lesson6ChooseScale.CategoryCount[] input=[
            new("Triangle",10),new("Square",15),new("Trapezoid",20),new("Hexagon",5)];
        Assert.True(UsCcssG3Unit1Lesson6ChooseScale.TryValidatePatternCounts(input,out _));
        Assert.False(UsCcssG3Unit1Lesson6ChooseScale.TryValidatePatternCounts(
            [new("Triangle",10),new("Triangle",15),new("Trapezoid",20),new("Hexagon",5)],out _));
        Assert.True(UsCcssG3Unit1Lesson6ChooseScale.RequiresTeacherReview("scale-justification"));
        Assert.True(UsCcssG3Unit1Lesson6ChooseScale.RequiresTeacherReview("pattern-figure-provenance"));
    }
}
