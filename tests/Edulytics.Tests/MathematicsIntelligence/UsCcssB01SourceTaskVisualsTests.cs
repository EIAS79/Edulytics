using Edulytics.Core.Curriculum;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssB01SourceTaskVisualsTests
{
    private readonly ExactSkillContractQuestionEngine _engine = new();

    private ExactSkillGeneratedQuestion One(string code,string family,int seed) =>
        Assert.Single(_engine.Generate("b01-visual-structured",code,[family],
            ExactSkillQuestionDifficulty.Standard,1,seed,[]));

    [Fact]
    public void ArrayVisualContainsExactlyTheUnitSquaresInTheVerifiedQuestion()
    {
        const string code="PED:US-CCSS-MATH:G3:U01:L16";
        var q=One(code,"usccss.b01.array.equal_groups",92911);
        var visual=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
        Assert.Equal(RichLessonVisualKind.ShapeDecomposition,visual.Kind);
        Assert.Equal(q.Parameters["rows"],visual.Rows.Count);
        Assert.Equal(q.Parameters["rows"]*q.Parameters["columns"],
            visual.Rows.Sum(r=>r.Cells.Count(x=>x=="■")));
        Assert.Contains(q.CorrectAnswer, visual.Description);
    }

    [Fact]
    public void MarketInventoryVisualTotals100AndSubtractsOnlyTheSoldStock()
    {
        const string code="PED:US-CCSS-MATH:G2:U02:L16";
        var q=One(code,"usccss.b01.market.inventory_100",22849);
        var visual=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
        Assert.Equal(RichLessonVisualKind.RatioTable,visual.Kind);
        Assert.Equal(3,visual.Rows.Count);
        Assert.Equal(100,visual.Rows.Sum(r=>int.Parse(r.Cells[0])));
        Assert.Equal(q.Parameters["second"]-q.Parameters["sold"],
            int.Parse(visual.Rows[1].Cells[1]));
        Assert.Equal(int.Parse(visual.Rows[0].Cells[0]),int.Parse(visual.Rows[0].Cells[1]));
        Assert.Equal(int.Parse(visual.Rows[2].Cells[0]),int.Parse(visual.Rows[2].Cells[1]));
    }

    [Fact]
    public void FractionDecompositionVisualPreservesSameDenominatorAndExactSum()
    {
        const string code="PED:US-CCSS-MATH:G4:U03:L07";
        var q=One(code,"usccss.b01.fraction.decompose_same_denominator",122934);
        var visual=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
        Assert.Equal(RichLessonVisualKind.FractionBars,visual.Kind);
        Assert.Equal(3,visual.Rows.Count);
        Assert.All(visual.Rows,r=>
            Assert.Equal(q.Parameters["denominator"],r.Denominator));
        Assert.Equal(q.Parameters["wholeN"],
            visual.Rows[0].Numerator+visual.Rows[1].Numerator);
        Assert.Equal(q.Parameters["wholeN"], visual.Rows[2].Numerator);
    }

    [Fact]
    public void SourceClockVisualUsesVerifiedQuarterHourAndHourHands()
    {
        const string code = "PED:US-CCSS-MATH:G2:U06:L11";
        var q = One(code, "usccss.b01.clock.half_quarters", 41897);
        var visual = Assert.Single(UsCcssB01SourceTaskVisuals.Build(code, [q]));
        Assert.Equal(RichLessonVisualKind.ClockFace, visual.Kind);
        Assert.Equal(q.Parameters["hour"], int.Parse(visual.Items[0]));
        Assert.Equal(15 * (q.Parameters["position"] + 1), int.Parse(visual.Items[1]));
        Assert.Contains(q.CorrectAnswer, visual.Description);
        Assert.InRange(int.Parse(visual.Items[0]), 1, 12);
        Assert.Contains(int.Parse(visual.Items[1]), new[] { 15, 30, 45 });
    }

    [Fact]
    public void DoNotDisplayB01SpecificDiagramsOnLaterBatchOrOtherCurricula()
    {
        var q=One("PED:US-CCSS-MATH:G3:U01:L16",
            "usccss.b01.array.equal_groups",92911);
        Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
            "PED:US-CCSS-MATH:G4:U04:L21",[q]));
        Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
            "PED:US-CCSS-MATH:G4:U05:L01",[q]));
        Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
            "PED:CAMBRIDGE-INTL-MATH:L7:SHARED:01:03:DECIMALS-AND-ROUNDING",[q]));
    }
}
