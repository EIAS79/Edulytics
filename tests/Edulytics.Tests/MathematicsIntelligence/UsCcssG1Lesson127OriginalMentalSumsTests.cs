using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source U07 L08 warm-up arithmetic and human-led Match Mine.</summary>
public sealed class UsCcssG1Lesson127OriginalMentalSumsTests
{
    private const string Family="usccss.g1.u07.l08.center_shapes_add_subtract";

    [Theory]
    [InlineData(6,4,8,5,18)]
    [InlineData(6,8,4,6,18)]
    [InlineData(6,8,5,7,19)]
    [InlineData(6,5,9,8,20)]
    public void OriginalThreeAddendSums(int a,int b,int c,int mode,int result)
    {
        var p=new Dictionary<string,int>
        {
            ["a"]=a,["b"]=b,["c"]=c,["mode"]=mode
        };
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(Family,p));
        Assert.Equal($"RESULT={result}",
            UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(
            Family,p,$"RESULT={result}"));
        Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(
            Family,p,$"RESULT={result+1}"));
    }
}
