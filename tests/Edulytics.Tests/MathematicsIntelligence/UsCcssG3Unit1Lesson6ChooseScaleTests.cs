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
