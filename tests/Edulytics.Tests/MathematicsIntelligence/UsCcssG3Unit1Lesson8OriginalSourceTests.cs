using Edulytics.Core.Mathematics.Curriculum;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Author-only contract for first-edition original, not executed.</summary>
public sealed class UsCcssG3Unit1Lesson8OriginalSourceTests
{
    [Fact]
    public void CooldownUsesOriginalReadingTimeNotFictionalBookInterest()
    {
        var rows=UsCcssG3Unit1Lesson8TwoStepDataProblems.OriginalSourceReadingTime;
        Assert.Equal(new[]{"Morning","Lunchtime","Afternoon","Evenings"},rows.Select(x=>x.Category));
        Assert.Equal(new[]{12,25,23,44},rows.Select(x=>x.Value));
        Assert.True(UsCcssG3Unit1Lesson8TwoStepDataProblems.TryCheckSourceReadingTimeEstimate(
            "L08-SOURCE-CD-MORNING-VS-AFTERNOON",12,out var plausible));
        Assert.True(plausible);
        Assert.True(UsCcssG3Unit1Lesson8TwoStepDataProblems.TryCheckSourceReadingTimeEstimate(
            "L08-SOURCE-CD-EVENING-VS-MORNING-LUNCH",7,out var plausible2));
        Assert.True(plausible2);
        Assert.True(UsCcssG3Unit1Lesson8TwoStepDataProblems.RequiresHumanReview(
            "original-source-reading-time-estimate"));
    }
    [Fact]
    public void OriginalSynthesisAndReadingTimePrecedeOptionalArithmetic()
    {
        DirectoryInfo? dir=new(AppContext.BaseDirectory);string? path=null;
        while(dir is not null)
        {
            var p=Path.Combine(dir.FullName,"src","Edulytics.Web","Views","Grade3Lesson8Activities","Index.cshtml");
            if(File.Exists(p)){path=p;break;}dir=dir.Parent;
        }
        Assert.NotNull(path);
        var v=File.ReadAllText(path!);
        Assert.Contains("CoolMorningAfternoon",v);
        Assert.Contains("CoolEveningMinusMorningLunch",v);
        Assert.Contains("CoolSourceEquations",v);
        Assert.Contains("0–50 by-five",v);
        Assert.True(v.IndexOf("4. Lesson synthesis",StringComparison.Ordinal)<
            v.IndexOf("5. ORIGINAL 2021 source Cool-down",StringComparison.Ordinal));
        Assert.True(v.IndexOf("5. ORIGINAL 2021 source Cool-down",StringComparison.Ordinal)<
            v.IndexOf("7. Optional original Edulytics",StringComparison.Ordinal));
    }
}
