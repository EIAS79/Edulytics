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

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G1:U07:L14", "usccss.b01.clock.read_half_hour")]
    [InlineData("PED:US-CCSS-MATH:G1:U07:L15", "usccss.b01.clock.read_half_hour")]
    [InlineData("PED:US-CCSS-MATH:G2:U06:L12", "usccss.b01.clock.read_five_minutes")]
    public void ClockHandsAndDigitalTimeUseExactlyTheSameVerifiedParameters(
        string code, string family)
    {
        var question = One(code, family, 380019);
        var visual = Assert.Single(UsCcssB01SourceTaskVisuals.Build(code, [question]));
        Assert.Equal(RichLessonVisualKind.ClockFace, visual.Kind);
        Assert.Equal(question.Parameters["hour"].ToString(), visual.Items[0]);
        Assert.Equal(question.Parameters["minutes"].ToString(), visual.Items[1]);
        Assert.Equal($"{question.Parameters["hour"]}:{question.Parameters["minutes"]:00}",
            question.CorrectAnswer);
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            family, question.Parameters, question.CorrectAnswer));
    }

    [Fact]
    public void FractionNumberLineLabelsUseExactUnitFractionSpacing()
    {
        const string code = "PED:US-CCSS-MATH:G3:U05:L05";
        var q = One(code, "fractions.equivalent.number_line", 390012);
        var visual = Assert.Single(UsCcssB01SourceTaskVisuals.Build(code, [q]));
        Assert.Equal(RichLessonVisualKind.NumberLine, visual.Kind);
        Assert.Equal(q.Parameters["baseD"] + 1, visual.Items.Count);
        Assert.Equal("0", visual.Items[0]);
        Assert.Equal("1", visual.Items[^1]);
        Assert.Equal(
            $"{q.Parameters["baseN"]}/{q.Parameters["baseD"]}",
            visual.Items[q.Parameters["baseN"]]);
        Assert.Equal(q.Parameters["baseN"] * q.Parameters["targetD"],
            int.Parse(q.CorrectAnswer) * q.Parameters["baseD"]);
    }

    [Fact]
    public void ReadingScaleDiagramIsEquallySpacedAndHasTheVerifiedAnswer()
    {
        const string code = "PED:US-CCSS-MATH:G2:U04:L02";
        var q = One(code, "measurement.scale.equal_intervals.read_value", 390021);
        var visual = Assert.Single(UsCcssB01SourceTaskVisuals.Build(code, [q]));
        Assert.Equal(RichLessonVisualKind.Scale, visual.Kind);
        Assert.Equal(q.Parameters["intervals"] + 1, visual.Items.Count);
        var labels = visual.Items.Select(int.Parse).ToArray();
        var step = labels[1] - labels[0];
        Assert.True(step > 0);
        for(var i=1;i<labels.Length;i++)
            Assert.Equal(step, labels[i] - labels[i-1]);
        Assert.Equal(q.CorrectAnswer, visual.Items[q.Parameters["pointer"]]);
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G2:U08:L08", "usccss.b01.array.columns")]
    [InlineData("PED:US-CCSS-MATH:G2:U08:L09", "usccss.b01.array.equal_addends")]
    [InlineData("PED:US-CCSS-MATH:G2:U08:L10", "usccss.b01.array.equal_equation")]
    [InlineData("PED:US-CCSS-MATH:G3:U01:L19", "usccss.b01.array.unknown_factor")]
    public void SourceMatchedArrayFamiliesProduceExactGridAndRepeatedAddition(
        string code, string family)
    {
        var question = One(code, family, 449101);
        var visual = Assert.Single(UsCcssB01SourceTaskVisuals.Build(code, [question]));
        Assert.Equal(RichLessonVisualKind.ShapeDecomposition, visual.Kind);
        Assert.Equal(question.Parameters["rows"],visual.Rows.Count);
        Assert.All(visual.Rows, r =>
            Assert.Equal(question.Parameters["columns"],r.Cells.Count));
        Assert.Contains(
            question.Parameters["rows"]*question.Parameters["columns"]+"",
            visual.Description);
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            family, question.Parameters, question.CorrectAnswer));
    }

    [Fact]
    public void CommutativityShowsBothRotatedArrangementsWithoutChangingObjectCount()
    {
        const string code = "PED:US-CCSS-MATH:G3:U01:L20";
        var q = One(code, "usccss.b01.array.commutativity", 449115);
        var vis = Assert.Single(UsCcssB01SourceTaskVisuals.Build(code, [q]));
        var a = q.Parameters["rows"];
        var b = q.Parameters["columns"];
        Assert.Equal(a+b, vis.Rows.Count);
        Assert.Equal(a*b, vis.Rows.Take(a).Sum(r=>r.Cells.Count));
        Assert.Equal(a*b, vis.Rows.Skip(a).Sum(r=>r.Cells.Count));
        Assert.Equal(b.ToString(), q.CorrectAnswer);
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G1:U04:L08", "usccss.b01.place_value.tens_ones", 2)]
    [InlineData("PED:US-CCSS-MATH:G1:U08:L07", "usccss.b01.place_value.count_to120", 3)]
    [InlineData("PED:US-CCSS-MATH:G2:U05:L04", "usccss.b01.place_value.three_digits", 3)]
    public void GradeBoundedBaseTenChartsReconstructTheExactGradedNumber(
        string code,string family,int expectedRows)
    {
        for(var seed=570091;seed<570101;seed++)
        {
            var q=One(code,family,seed);
            var model=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.PlaceValueChart,model.Kind);
            Assert.Equal(expectedRows,model.Rows.Count);
            var values=model.Rows.Select(r=>int.Parse(r.Cells[1])).ToArray();
            Assert.Equal(int.Parse(q.CorrectAnswer),values.Sum());
            Assert.Equal(q.Parameters["number"],values.Sum());
            if (code.Contains(":G1:U04:",StringComparison.Ordinal))
                Assert.InRange(values.Sum(),10,99);
            if (family=="usccss.b01.place_value.count_to120")
                Assert.InRange(values.Sum(),40,120);
            if (family=="usccss.b01.place_value.three_digits")
                Assert.InRange(values.Sum(),100,999);
        }
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
