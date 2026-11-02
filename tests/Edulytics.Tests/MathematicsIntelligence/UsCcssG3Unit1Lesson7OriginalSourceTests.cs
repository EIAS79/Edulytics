using Edulytics.Core.Mathematics.Curriculum;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Source exactness tests authored for Stage D; never executed here.</summary>
public sealed class UsCcssG3Unit1Lesson7OriginalSourceTests
{
    [Fact]
    public void Source2021SportsCountsAndTwoSeparateExactDifferences()
    {
        var r=UsCcssG3Unit1Lesson7GraphDifferences.OriginalSourceCoolDown;
        Assert.Equal(new[]{"Football","Soccer","Basketball","Hockey"},r.Select(x=>x.Category));
        Assert.Equal(new[]{28,61,35,22},r.Select(x=>x.Count));
        foreach(var (id,n) in new[]{("L07-SOURCE-CD-SOCCER-MINUS-FOOTBALL",33),
            ("L07-SOURCE-CD-BASKETBALL-MINUS-HOCKEY",13)})
        {
            Assert.True(UsCcssG3Unit1Lesson7GraphDifferences.TryGradeExact(id,n,out var yes));
            Assert.True(yes);
            Assert.True(UsCcssG3Unit1Lesson7GraphDifferences.TryGradeExact(id,n+1,out var no));
            Assert.False(no);
        }
    }
    [Fact]
    public void HumanEstimatesMustNotBeAutoPassedDespiteNearValue()
    {
        Assert.True(UsCcssG3Unit1Lesson7GraphDifferences.TryCheckApproximateBugAnswer(
            "L07-BUGS-ANTS-MINUS-SPIDERS",53,out var plausible));
        Assert.True(plausible);
        Assert.True(UsCcssG3Unit1Lesson7GraphDifferences.RequiresTeacherReview("scaled-chart-estimate"));
    }
    [Fact]
    public void OriginalAndOptionalCooldownAreSeparateAndWarmupWasCorrected()
    {
        DirectoryInfo? d=new(AppContext.BaseDirectory);
        string? path=null;
        while(d is not null)
        {
            var p=Path.Combine(d.FullName,"src","Edulytics.Web","Views","Grade3Lesson7Activities","Index.cshtml");
            if(File.Exists(p)){path=p;break;}d=d.Parent;
        }
        Assert.NotNull(path);
        var v=File.ReadAllText(path!);
        Assert.Contains("SEVEN and TWENTY-TWO dots",v);
        Assert.Contains("SourceSportEquations",v);
        Assert.True(v.IndexOf("4. Lesson synthesis",StringComparison.Ordinal)<
            v.IndexOf("5. ORIGINAL 2021 source Cool-down",StringComparison.Ordinal));
        Assert.True(v.IndexOf("5. ORIGINAL 2021 source Cool-down",StringComparison.Ordinal)<
            v.IndexOf("6. Optional independent Edulytics",StringComparison.Ordinal));
    }
}
