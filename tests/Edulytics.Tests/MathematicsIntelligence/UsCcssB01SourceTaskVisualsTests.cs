using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssB01SourceTaskVisualsTests
{
    private readonly ExactSkillContractQuestionEngine _engine = new();

    private ExactSkillGeneratedQuestion One(string code,string family,int seed) =>
        Assert.Single(_engine.Generate("b01-visual-structured",code,[family],
            ExactSkillQuestionDifficulty.Standard,1,seed,[]));

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G1:U04:L03",
        "usccss.b01.g1.add_subtract_10_tens",true)]
    [InlineData("PED:US-CCSS-MATH:G1:U04:L04",
        "usccss.b01.g1.add_subtract_tens_multiples",false)]
    public void OriginalGradeOneCompleteTensLessonsNeverGenerateOnesOnlyQuestions(
        string code,string family,bool exactlyTen)
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        var operations=new HashSet<int>();
        for(var seed=71000;seed<71100;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            int left=p["left"],right=p["right"],mode=p["operation"];
            operations.Add(mode);
            Assert.Equal(0,left%10);
            Assert.Equal(0,right%10);
            if(exactlyTen)Assert.Equal(10,right);
            Assert.Equal(mode==0?left+right:left-right,
                int.Parse(q.CorrectAnswer));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var chart=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.PlaceValueChart,chart.Kind);
            Assert.Equal(3,chart.Rows.Count);
            var values=chart.Rows.Select(row=>
                int.Parse(row.Cells[0])*10+int.Parse(row.Cells[1])).ToArray();
            Assert.Equal(new[]{left,right,int.Parse(q.CorrectAnswer)},values);
            Assert.All(chart.Rows,row=>Assert.Equal("0",row.Cells[1]));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="9999"}]));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                "PED:US-CCSS-MATH:G1:U05:L03",[q]));
        }
        Assert.Equal(new[]{0,1},operations.OrderBy(x=>x).ToArray());
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G2:U02:L01",
        "usccss.b01.g2.add_subtract_without_regroup",false)]
    [InlineData("PED:US-CCSS-MATH:G2:U02:L07",
        "usccss.b01.g2.subtract_decompose_ten",true)]
    public void OriginalGradeTwoTensBorrowConditionsAreIndependentlyVerified(
        string code,string family,bool mustBorrow)
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        var operations=new HashSet<int>();
        for(var seed=72000;seed<72100;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            var a=p["left"];var b=p["right"];var op=p["operation"];
            operations.Add(op);
            Assert.Equal(op==0?a+b:a-b,int.Parse(q.CorrectAnswer));
            if(op==1)Assert.Equal(mustBorrow,a%10<b%10);
            else
            {
                Assert.False(mustBorrow);
                Assert.True(a%10+b%10<10);
            }
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var chart=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.PlaceValueChart,chart.Kind);
            Assert.Equal(mustBorrow?4:3,chart.Rows.Count);
            if(mustBorrow)
            {
                var decomposed=chart.Rows[2].Cells.Select(int.Parse).ToArray();
                Assert.Equal(a,decomposed[0]*10+decomposed[1]);
                Assert.Equal(a/10-1,decomposed[0]);
                Assert.Equal(a%10+10,decomposed[1]);
            }
            Assert.Equal(int.Parse(q.CorrectAnswer),
                int.Parse(chart.Rows[^1].Cells[0])*10+
                int.Parse(chart.Rows[^1].Cells[1]));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="99999"}]));
        }
        Assert.Contains(1,operations);
        if(!mustBorrow)Assert.Contains(0,operations);
    }

    [Fact]
    public void GradeOneTensAndOnesComparisonShowsCorrectOrderedNumbers()
    {
        const string code="PED:US-CCSS-MATH:G1:U04:L17";
        const string family="usccss.b01.g1.compare_order_tens_ones";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        var testedModes=new HashSet<int>();
        for(var seed=78000;seed<78090;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            var numbers=new[]{p["first"],p["second"],p["third"]};
            Assert.Equal(numbers.Length,numbers.Distinct().Count());
            Assert.Equal(p["first"]/10,p["second"]/10);
            var sorted=numbers.OrderBy(x=>x).ToArray();
            testedModes.Add(p["position"]);
            var expected=p["position"]==0?sorted[2]:
                p["position"]==1?sorted[0]:sorted[1];
            Assert.Equal(expected,int.Parse(q.CorrectAnswer));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var chart=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.PlaceValueChart,chart.Kind);
            Assert.Equal(numbers,chart.Rows.Select(r=>
                int.Parse(r.Cells[0])*10+int.Parse(r.Cells[1])).ToArray());
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="999"}]));
        }
        Assert.Equal(new[]{0,1,2},testedModes.OrderBy(x=>x).ToArray());
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G3:U06:L04")]
    [InlineData("PED:US-CCSS-MATH:G3:U06:L05")]
    public void GradeThreeFractionalInchLinePlotsUseExactQuarterUnitIntervals(
        string code)
    {
        const string family="usccss.b01.g3.quarter_inch_line_plot_frequency";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var lesson));
        Assert.Equal([family],lesson!.AllowedQuestionFamilies);
        for(var seed=910000;seed<910060;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            var tick=p["targetTick"];
            Assert.InRange(tick,0,4);
            Assert.Equal(p[$"count{tick+1}"],int.Parse(q.CorrectAnswer));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var plot=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.LinePlot,plot.Kind);
            Assert.Equal(new[]{"1 in","1 1/4 in","1 1/2 in",
                "1 3/4 in","2 in"},plot.Rows.Select(r=>r.Label));
            for(var i=0;i<5;i++)
            {
                Assert.Equal(p[$"count{i+1}"],plot.Rows[i].Cells.Count);
                Assert.All(plot.Rows[i].Cells,mark=>Assert.Equal("X",mark));
            }
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="99999"}]));
        }
    }

    [Fact]
    public void OriginalGradeOneAdditionToHundredNeverComposesTen()
    {
        const string code="PED:US-CCSS-MATH:G1:U05:L04";
        const string family="usccss.b01.g1.add_within100_no_compose_ten";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var lesson));
        Assert.Equal([family],lesson!.AllowedQuestionFamilies);
        for(var seed=111000;seed<111075;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            var a=p["left"];var b=p["right"];
            Assert.InRange(a+b,20,99);
            Assert.True(a%10+b%10<10);
            Assert.Equal(a+b,int.Parse(q.CorrectAnswer));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var chart=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.PlaceValueChart,chart.Kind);
            Assert.Equal(new[]{a,b,a+b},chart.Rows.Select(row=>
                int.Parse(row.Cells[0])*10+int.Parse(row.Cells[1])).ToArray());
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="10001"}]));
        }
    }

    [Fact]
    public void GradeTwoAddSubtractThousandExcludesAllCarryingAndBorrowing()
    {
        const string code="PED:US-CCSS-MATH:G2:U07:L04";
        const string family="usccss.b01.g2.add_subtract_1000_no_regroup";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var lesson));
        Assert.Equal([family],lesson!.AllowedQuestionFamilies);
        var operations=new HashSet<int>();
        for(var seed=975000;seed<975100;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            var a=p["left"];var b=p["right"];var mode=p["operation"];
            operations.Add(mode);
            Assert.Equal(mode==0?a+b:a-b,int.Parse(q.CorrectAnswer));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            foreach(var place in new[]{1,10,100})
            {
                var da=a/place%10;var db=b/place%10;
                if(mode==0)Assert.True(da+db<10);
                else Assert.True(da>=db);
            }
            var chart=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.PlaceValueChart,chart.Kind);
            Assert.Equal(new[]{a,b,int.Parse(q.CorrectAnswer)},
                chart.Rows.Select(row=>row.Cells.Select(int.Parse)
                    .Aggregate(0,(acc,digit)=>acc*10+digit)).ToArray());
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="99999"}]));
        }
        Assert.Equal(new[]{0,1},operations.OrderBy(x=>x).ToArray());
    }

    [Fact]
    public void GradeTwoThreeDigitComparisonsUseTrueNumberLinePositions()
    {
        const string code="PED:US-CCSS-MATH:G2:U05:L09";
        const string family="usccss.b01.g2.order_three_digits_number_line";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var lesson));
        Assert.Equal([family],lesson!.AllowedQuestionFamilies);
        for(var seed=951000;seed<951080;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            Assert.Equal(Math.Max(p["first"],p["second"]),
                int.Parse(q.CorrectAnswer));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var line=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.NumberLine,line.Kind);
            Assert.Equal(11,line.Items.Count);
            Assert.Equal(p["first"],int.Parse(line.Items[p["tickA"]]));
            Assert.Equal(p["second"],int.Parse(line.Items[p["tickB"]]));
            Assert.Equal(new[]{p["tickA"],p["tickB"]},
                line.SecondaryItems.Select(int.Parse).ToArray());
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="99999"}]));
        }
    }

    [Fact]
    public void OriginalG4WholeMinusFractionDoesNotGenerateIntegerSubtraction()
    {
        const string code="PED:US-CCSS-MATH:G4:U03:L10";
        const string family="usccss.b01.g4.whole_minus_proper_fraction";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(
            family,out var interaction));
        Assert.Equal(LessonPracticeInteractionKind.FractionEntry,
            interaction!.InteractionKind);
        for(var seed=640000;seed<640055;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            var parts=q.CorrectAnswer.Split('/').Select(int.Parse).ToArray();
            Assert.Equal(2,parts.Length);
            Assert.Equal((p["whole"]*p["denominator"]-p["numerator"])*parts[1],
                p["denominator"]*parts[0]);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var fraction=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.FractionBars,fraction.Kind);
            Assert.Equal(p["whole"],fraction.Rows.Count);
            Assert.Equal(p["whole"]*p["denominator"]-p["numerator"],
                fraction.Rows.Sum(row=>row.Numerator));
            Assert.All(fraction.Rows,
                row=>Assert.Equal(p["denominator"],row.Denominator));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="999/8"}]));
        }
    }

    [Fact]
    public void OriginalG4DecimalLessonShowsTwoExactHundredthsPositions()
    {
        const string code="PED:US-CCSS-MATH:G4:U04:L03";
        const string family="usccss.b01.g4.decimal_number_line_greater";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(
            family,out var input));
        Assert.Equal(LessonPracticeInteractionKind.NumericEntry,input!.InteractionKind);
        for(var seed=120300;seed<120395;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            Assert.Equal(p["start"]+p["tickA"],p["first"]);
            Assert.Equal(p["start"]+p["tickB"],p["second"]);
            Assert.NotEqual(p["first"],p["second"]);
            var answer=Math.Max(p["first"],p["second"])/100m;
            Assert.Equal(answer,decimal.Parse(q.CorrectAnswer,
                System.Globalization.CultureInfo.InvariantCulture));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,answer.ToString("0.##",
                    System.Globalization.CultureInfo.InvariantCulture)));
            var visual=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.NumberLine,visual.Kind);
            Assert.Equal(11,visual.Items.Count);
            Assert.Equal((p["first"]/100m).ToString("0.00",
                System.Globalization.CultureInfo.InvariantCulture),
                visual.Items[p["tickA"]]);
            Assert.Equal((p["second"]/100m).ToString("0.00",
                System.Globalization.CultureInfo.InvariantCulture),
                visual.Items[p["tickB"]]);
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="999.0"}]));
        }
    }

    [Fact]
    public void PublisherG3DivisionLessonHasBothInverseOperationsAndExactArray()
    {
        const string code="PED:US-CCSS-MATH:G3:U08:L10";
        const string family="usccss.b01.g3.multiplication_division_inverse_array";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        var modes=new HashSet<int>();
        for(var seed=881000;seed<881120;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            var rows=p["rows"];
            var cols=p["columns"];
            var total=p["total"];
            var op=p["operation"];
            modes.Add(op);
            Assert.InRange(total,4,100);
            Assert.Equal(rows*cols,total);
            Assert.Equal(op==0?total:cols,int.Parse(q.CorrectAnswer));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var diagram=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.ShapeDecomposition,diagram.Kind);
            Assert.Equal(rows,diagram.Rows.Count);
            Assert.All(diagram.Rows,row=>Assert.Equal(cols,row.Cells.Count));
            Assert.All(diagram.Rows.SelectMany(row=>row.Cells),
                cell=>Assert.Equal("■",cell));
            Assert.Contains($"{total} ÷ {rows} = {cols}",diagram.Description);
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="99999"}]));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                "PED:US-CCSS-MATH:G4:U08:L10",[q]));
        }
        Assert.Equal(new[]{0,1},modes.OrderBy(x=>x).ToArray());
    }

    [Fact]
    public void SourceG2EqualSquareTileCompositionsBuildTrueSquaresAndRectangles()
    {
        const string code="PED:US-CCSS-MATH:G2:U06:L06";
        const string family="usccss.b01.g2.compose_equal_square_shapes";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(
            family,out var input));
        Assert.Equal(LessonPracticeInteractionKind.StructuredEntry,
            input!.InteractionKind);
        var modes=new HashSet<int>();
        for(var seed=278100;seed<278170;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            modes.Add(p["mode"]);
            var square=p["rows"]==p["columns"];
            Assert.Equal(square?"square":"rectangle",q.CorrectAnswer);
            Assert.Equal(square?0:1,p["mode"]);
            Assert.Equal(p["rows"]*p["columns"],p["tiles"]);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var v=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.ShapeDecomposition,v.Kind);
            Assert.Equal("Equal-size-square-grid",v.SecondaryLabel);
            Assert.Equal(p["rows"],v.Rows.Count);
            Assert.All(v.Rows,row=>Assert.Equal(p["columns"],row.Cells.Count));
            Assert.Equal(p["tiles"],v.Rows.Sum(row=>row.Cells.Count));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="trapezoid"}]));
        }
        Assert.Equal(new[]{0,1},modes.OrderBy(x=>x).ToArray());
    }

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
    [InlineData("PED:US-CCSS-MATH:G1:U07:L13",
        "usccss.b01.clock.read_on_hour",false)]
    [InlineData("PED:US-CCSS-MATH:G2:U06:L13",
        "usccss.b01.clock.read_five_minutes_am_pm",true)]
    public void SourceSpecificClockLessonsUseVerifiedHourMinuteAndAmPm(
        string code,string family,bool hasDayPart)
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var lesson));
        Assert.Equal([family],lesson!.AllowedQuestionFamilies);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(
            family,out var presentation));
        Assert.True(presentation!.HasVisualRepresentation);
        Assert.Equal(LessonPracticeInteractionKind.StructuredEntry,
            presentation.InteractionKind);
        for(var seed=558600;seed<558617;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            Assert.InRange(p["hour"],1,12);
            Assert.InRange(p["minutes"],0,55);
            Assert.Equal(0,p["minutes"]%5);
            if(!hasDayPart)Assert.Equal(0,p["minutes"]);
            Assert.Equal(
                $"{p["hour"]}:{p["minutes"]:00}" +
                (hasDayPart ? (p["period"]==0?" a.m.":" p.m.") : ""),
                q.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var clock=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.ClockFace,clock.Kind);
            Assert.Equal(p["hour"],int.Parse(clock.Items[0]));
            Assert.Equal(p["minutes"],int.Parse(clock.Items[1]));
            Assert.Contains(q.CorrectAnswer,clock.Title);
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="99:99 p.m."}]));
        }
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

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G3:U05:L12")]
    [InlineData("PED:US-CCSS-MATH:G4:U02:L04")]
    [InlineData("PED:US-CCSS-MATH:G4:U02:L05")]
    [InlineData("PED:US-CCSS-MATH:G4:U02:L08")]
    public void EquivalentFractionSourceNumberLineTasksMatchRealFractionArithmetic(
        string code)
    {
        const string family="fractions.equivalent.number_line";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        for(var seed=209100;seed<209111;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            Assert.Equal(p["baseN"]*p["targetD"],
                int.Parse(q.CorrectAnswer)*p["baseD"]);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var line=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.NumberLine,line.Kind);
            Assert.Equal("0",line.Items[0]);
            Assert.Equal("1",line.Items[^1]);
            Assert.Equal(p["baseD"]+1,line.Items.Count);
            Assert.Equal(
                $"{p["baseN"]}/{p["baseD"]}",line.Items[p["baseN"]]);
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="999999"}]));
        }
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

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G2:U04:L03",false)]
    [InlineData("PED:US-CCSS-MATH:G2:U04:L06",true)]
    public void Grade2OriginalWholeNumberLineAndOptionalArithmeticKeepCorrectLearningTargets(
        string code,bool alsoPracticesArithmetic)
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.NotNull(contract);
        Assert.Contains("usccss.b01.number_line.locate_whole_to100",
            contract!.AllowedQuestionFamilies);
        if(alsoPracticesArithmetic)
        {
            Assert.Contains("number.whole.add_subtract.within_100.apply",
                contract.AllowedQuestionFamilies);
            Assert.Contains("number.whole.add_subtract.within_100.build",
                contract.AllowedQuestionFamilies);
        }
        else Assert.Single(contract.AllowedQuestionFamilies);

        for(var seed=660121;seed<660127;seed++)
        {
            var q=One(code,"usccss.b01.number_line.locate_whole_to100",seed);
            var line=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.NumberLine,line.Kind);
            Assert.Equal(11,line.Items.Count);
            var labels=line.Items.Select(int.Parse).ToArray();
            Assert.Equal(0,labels[0]);
            Assert.All(labels.Zip(labels.Skip(1),(a,b)=>b-a),
                delta=>Assert.Equal(q.Parameters["step"],delta));
            Assert.Equal(labels[q.Parameters["tick"]],int.Parse(q.CorrectAnswer));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="111111"}]));
        }
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G2:U04:L04",
        "usccss.b01.number_line.move_left_right_within100")]
    [InlineData("PED:US-CCSS-MATH:G2:U04:L05",
        "usccss.b01.number_line.estimate_location_within100")]
    public void OriginalGrade2DirectionAndEstimationTargetSpecificNumberLineQuestions(
        string code,string family)
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        for(var seed=660260;seed<660275;seed++)
        {
            var question=One(code,family,seed);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,question.Parameters,question.CorrectAnswer));
            var p=question.Parameters;
            if(family.EndsWith("move_left_right_within100",StringComparison.Ordinal))
                Assert.Equal(p["start"]+p["direction"]*p["distance"],
                    int.Parse(question.CorrectAnswer));
            else
            {
                Assert.InRange(p["target"],p["lower"]+1,p["upper"]-1);
                Assert.Equal(Math.Abs(p["target"]-p["lower"])<
                    Math.Abs(p["target"]-p["upper"])?p["lower"]:p["upper"],
                    int.Parse(question.CorrectAnswer));
            }
            var line=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[question]));
            Assert.Equal(RichLessonVisualKind.NumberLine,line.Kind);
            Assert.Equal(11,line.Items.Count);
            Assert.Equal(Enumerable.Range(0,11).Select(i=>i*10),
                line.Items.Select(int.Parse));
            Assert.Contains(question.CorrectAnswer,line.Description);
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [question with {CorrectAnswer="100500"}]));
        }
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G2:U03:L03",
        "usccss.b01.ruler.compare_centimeters")]
    [InlineData("PED:US-CCSS-MATH:G2:U03:L10",
        "usccss.b01.ruler.measure_without_zero")]
    public void SourceGrade2RulerMeasurementsHaveExactCmTicksAndCorrectAnswer(
        string code,string family)
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        for(var seed=90501;seed<90521;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            var expected=family.EndsWith("compare_centimeters",StringComparison.Ordinal)
                ?p["longLength"]-p["shortLength"]:p["end"]-p["start"];
            Assert.Equal(expected,int.Parse(q.CorrectAnswer));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var ruler=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.Scale,ruler.Kind);
            var ticks=ruler.Items.Select(int.Parse).ToArray();
            Assert.Equal(0,ticks[0]);
            Assert.All(ticks.Zip(ticks.Skip(1),(a,b)=>b-a),
                diff=>Assert.Equal(1,diff));
            Assert.Contains(q.CorrectAnswer,ruler.Description);
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="999"}]));
        }
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G3:U01:L03",
        "usccss.b01.g3.picture_graph.scaled_compare")]
    [InlineData("PED:US-CCSS-MATH:G3:U01:L04",
        "usccss.b01.g3.picture_graph.construct")]
    public void OriginalGrade3PictureGraphsRespectExactScaleAndSymbolCounts(
        string code,string family)
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        for(var seed=84000;seed<84024;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            Assert.Contains(p["scale"],new[]{2,5});
            var expected=family.EndsWith("scaled_compare",StringComparison.Ordinal)
                ?(p["aIcons"]-p["bIcons"])*p["scale"]:p["bIcons"];
            Assert.Equal(expected,int.Parse(q.CorrectAnswer));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var graph=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.PictureGraph,graph.Kind);
            Assert.Equal(2,graph.Rows.Count);
            Assert.Equal(p["aIcons"],graph.Rows[0].Cells.Count);
            Assert.Equal(p["bIcons"],graph.Rows[1].Cells.Count);
            Assert.All(graph.Rows.SelectMany(x=>x.Cells),x=>Assert.Equal("●",x));
            Assert.Equal(p["scale"],int.Parse(Assert.Single(graph.Items)));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="999999"}]));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                "PED:US-CCSS-MATH:G4:U01:L03",[q]));
        }
    }

    [Fact]
    public void Grade2Lesson15ConstructsAnExactMeasurementLinePlot()
    {
        const string code="PED:US-CCSS-MATH:G2:U03:L15";
        const string family="usccss.b01.g2.measurement.read_line_plot";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        for(var seed=94000;seed<94016;seed++)
        {
            var q=One(code,family,seed);
            var v=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.LinePlot,v.Kind);
            Assert.Contains("Construct",v.Title);
            for(var length=2;length<=5;length++)
                Assert.Equal(q.Parameters[$"count{length}"],v.Rows[length-2].Cells.Count);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,q.CorrectAnswer));
        }
    }

    [Fact]
    public void MixedGrade2SourceLesson17PractisesArithmeticRulerAndLinePlot()
    {
        const string code="PED:US-CCSS-MATH:G2:U03:L17";
        string[] families=[
            "usccss.b01.g2.add_subtract_without_regroup",
            "usccss.b01.ruler.measure_without_zero",
            "usccss.b01.g2.measurement.read_line_plot"
        ];
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal(families,contract!.AllowedQuestionFamilies);
        var kinds=new[]{
            RichLessonVisualKind.PlaceValueChart,
            RichLessonVisualKind.Scale,
            RichLessonVisualKind.LinePlot
        };
        for(var i=0;i<families.Length;i++)
        for(var seed=50000;seed<50018;seed++)
        {
            var q=One(code,families[i],seed);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                families[i],q.Parameters,q.CorrectAnswer));
            var visual=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(kinds[i],visual.Kind);
            if(i==0)
            {
                var p=q.Parameters;
                Assert.Equal(p["operation"]==0
                    ?p["left"]+p["right"]:p["left"]-p["right"],
                    int.Parse(q.CorrectAnswer));
            }
            else if(i==1)
            {
                Assert.Equal(q.Parameters["end"]-q.Parameters["start"],
                    int.Parse(q.CorrectAnswer));
            }
            else
            {
                var p=q.Parameters;
                Assert.Equal(p[$"count{p["targetLength"]}"],
                    int.Parse(q.CorrectAnswer));
                Assert.Equal(4,visual.Rows.Count);
                for(var length=2;length<=5;length++)
                    Assert.Equal(p[$"count{length}"],
                        visual.Rows[length-2].Cells.Count);
            }
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="999999"}]));
        }
    }

    [Fact]
    public void Grade2SourceLinePlotUsesExactlyVerifiedXMarkCounts()
    {
        const string code="PED:US-CCSS-MATH:G2:U03:L14";
        const string family="usccss.b01.g2.measurement.read_line_plot";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        for(var seed=99001;seed<99030;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            Assert.Equal(p[$"count{p["targetLength"]}"],int.Parse(q.CorrectAnswer));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var plot=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.LinePlot,plot.Kind);
            Assert.Equal(4,plot.Rows.Count);
            for(var length=2;length<=5;length++)
            {
                var row=plot.Rows[length-2];
                Assert.Equal($"{length} cm",row.Label);
                Assert.Equal(p[$"count{length}"],row.Cells.Count);
                Assert.All(row.Cells,c=>Assert.Equal("X",c));
            }
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="999"}]));
            // L15 now legitimately constructs the same genuine line plot
            // from source data; only non-line-plot lessons must reject it.
            Assert.Equal(RichLessonVisualKind.LinePlot,
                Assert.Single(UsCcssB01SourceTaskVisuals.Build(
                    "PED:US-CCSS-MATH:G2:U03:L15",[q])).Kind);
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                "PED:US-CCSS-MATH:G2:U03:L16",[q]));
        }
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G2:U01:L15")]
    [InlineData("PED:US-CCSS-MATH:G2:U01:L16")]
    public void OriginalCompareProblemsIncludeAllUnknownPositionsWithCorrectTape(
        string code)
    {
        const string family="usccss.b01.g2.compare_tape_unknown_positions";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        var positions=new HashSet<int>();
        for(var seed=81200;seed<81340;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            var m=p["unknownPosition"];
            positions.Add(m);
            Assert.InRange(m,0,2);
            var expected=m switch
            {
                0=>p["difference"],1=>p["longCount"],_=>p["shortCount"]
            };
            Assert.Equal(expected,int.Parse(q.CorrectAnswer));
            Assert.Equal(p["difference"],p["longCount"]-p["shortCount"]);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var diagram=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.TapeDiagram,diagram.Kind);
            Assert.Equal(new[]{p["longCount"],p["shortCount"],p["difference"],m},
                diagram.Items.Select(int.Parse).ToArray());
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="100000"}]));
        }
        Assert.Equal(new[]{0,1,2},positions.OrderBy(x=>x).ToArray());
    }

    [Fact]
    public void Grade2SourceCompareTapeLengthsAreExactlyProportional()
    {
        const string code="PED:US-CCSS-MATH:G2:U01:L14";
        const string family="usccss.b01.g2.compare_tape_difference";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        for(var seed=992000;seed<992025;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            var first=p["longCount"];var second=p["shortCount"];
            Assert.InRange(first,8,19);
            Assert.InRange(second,2,18);
            Assert.True(first>second);
            Assert.Equal(first-second,int.Parse(q.CorrectAnswer));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var tape=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.TapeDiagram,tape.Kind);
            Assert.Equal(new[]{first,second,first-second},
                tape.Items.Select(int.Parse).ToArray());
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="9999"}]));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                "PED:US-CCSS-MATH:G2:U01:L16",[q]));
        }
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

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G3:U02:L05",
        "usccss.b01.geometry.rectangle_area_grid60",true)]
    [InlineData("PED:US-CCSS-MATH:G3:U02:L09",
        "usccss.b01.geometry.rectangle_area_ungridded",false)]
    public void OriginalGrade3AreaMultiplicationGoalsProduceCheckedGeometricModels(
        string code,string family,bool gridded)
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        for(var seed=330010;seed<330030;seed++)
        {
            var q=One(code,family,seed);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,q.CorrectAnswer));
            var visual=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(gridded?RichLessonVisualKind.ShapeDecomposition:
                RichLessonVisualKind.RectangleDiagram,visual.Kind);
            var expected=gridded
                ?q.Parameters["rows"]*q.Parameters["columns"]
                :q.Parameters["length"]*q.Parameters["width"];
            Assert.Equal(expected,int.Parse(q.CorrectAnswer));
            if(gridded)
                Assert.Equal(expected,visual.Rows.Sum(r=>r.Cells.Count));
            else
                Assert.Equal(expected,int.Parse(visual.Items[0])*
                    int.Parse(visual.Items[1]));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="999999"}]));
        }
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G3:U07:L11",
        "usccss.b01.g3.same_perimeter_different_area",true)]
    [InlineData("PED:US-CCSS-MATH:G3:U07:L12",
        "usccss.b01.g3.same_area_different_perimeter",false)]
    public void OriginalRectanglesWithEqualPerimeterOrAreaHaveProportionalVisualProof(
        string code,string family,bool samePerimeter)
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        for(var seed=710011;seed<710045;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            int aw=p["widthA"],ah=p["heightA"],bw=p["widthB"],bh=p["heightB"];
            int areaA=aw*ah,areaB=bw*bh;
            int perimeterA=2*(aw+ah),perimeterB=2*(bw+bh);
            if(samePerimeter)
            {
                Assert.Equal(perimeterA,perimeterB);
                Assert.NotEqual(areaA,areaB);
                Assert.Equal(areaB-areaA,int.Parse(q.CorrectAnswer));
            }
            else
            {
                Assert.Equal(areaA,areaB);
                Assert.NotEqual(perimeterA,perimeterB);
                Assert.Equal(perimeterA-perimeterB,
                    int.Parse(q.CorrectAnswer));
            }
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var visuals=UsCcssB01SourceTaskVisuals.Build(code,[q]);
            Assert.Equal(2,visuals.Count);
            Assert.All(visuals,v=>
            {
                Assert.Equal(RichLessonVisualKind.RectangleDiagram,v.Kind);
                Assert.Equal("fixed-comparison-scale",v.SecondaryLabel);
                Assert.Contains(q.CorrectAnswer,v.Description);
            });
            Assert.Equal(new[]{aw,ah},visuals[0].Items.Select(int.Parse));
            Assert.Equal(new[]{bw,bh},visuals[1].Items.Select(int.Parse));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="99999"}]));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                "PED:US-CCSS-MATH:G4:U07:L11",[q]));
        }
    }

    [Fact]
    public void GradeFourFactorPairsComposeTwoDifferentSameAreaRectangles()
    {
        const string code="PED:US-CCSS-MATH:G4:U01:L02";
        const string family="usccss.b01.g4.factor_pairs_two_rectangles";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        var seen=new HashSet<string>();
        for(var seed=850010;seed<850100;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            int aw=p["widthA"],ah=p["heightA"];
            int bw=p["widthB"],bh=p["heightB"];
            int area=p["area"];
            Assert.Equal(area,aw*ah);
            Assert.Equal(area,bw*bh);
            Assert.True(aw<bw);
            Assert.Equal(bh,int.Parse(q.CorrectAnswer));
            seen.Add($"{aw}:{ah}:{bw}:{bh}");
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var visual=UsCcssB01SourceTaskVisuals.Build(code,[q]);
            Assert.Equal(2,visual.Count);
            Assert.All(visual,v=>
            {
                Assert.Equal(RichLessonVisualKind.RectangleDiagram,v.Kind);
                Assert.Equal("fixed-comparison-scale",v.SecondaryLabel);
            });
            Assert.Equal(new[]{aw,ah},visual[0].Items.Select(int.Parse));
            Assert.Equal(new[]{bw,bh},visual[1].Items.Select(int.Parse));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="9999"}]));
        }
        Assert.True(seen.Count>=10,
            $"Factor pair variety only {seen.Count}; 10 distinct examples required.");
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G4:U01:L01","usccss.b01.g4.rectangle_area_is_multiple")]
    [InlineData("PED:US-CCSS-MATH:G3:U04:L10","usccss.b01.g3.distributive_area_partition")]
    [InlineData("PED:US-CCSS-MATH:G3:U07:L10","usccss.b01.g3.rectangle_area_perimeter_story")]
    public void OriginalAreaMultiplesDistributiveAndPerimeterSourcesHaveExactVisuals(
        string code,string family)
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        var observedModes=new HashSet<int>();
        for(var seed=953000;seed<953044;seed++)
        {
            var q=One(code,family,seed);
            var p=q.Parameters;
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var visual=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            if(family=="usccss.b01.g4.rectangle_area_is_multiple")
            {
                var area=p["length"]*p["width"];
                Assert.Equal(area,int.Parse(q.CorrectAnswer));
                Assert.Equal(RichLessonVisualKind.RectangleDiagram,visual.Kind);
                Assert.Equal(0,area%p["length"]);
                Assert.Equal(0,area%p["width"]);
                Assert.Equal(new[]{p["length"],p["width"]},
                    visual.Items.Select(int.Parse));
            }
            else if(family=="usccss.b01.g3.distributive_area_partition")
            {
                var expected=p["height"]*(p["leftWidth"]+p["rightWidth"]);
                Assert.Equal(expected,int.Parse(q.CorrectAnswer));
                Assert.Equal(RichLessonVisualKind.ShapeDecomposition,visual.Kind);
                Assert.Equal(p["height"],visual.Rows.Count);
                Assert.Equal(expected,visual.Rows.Sum(r=>r.Cells.Count));
                Assert.Equal(p["height"]*p["leftWidth"],
                    visual.Rows.Sum(r=>r.Cells.Count(x=>x=="■")));
                Assert.Equal(p["height"]*p["rightWidth"],
                    visual.Rows.Sum(r=>r.Cells.Count(x=>x=="□")));
            }
            else
            {
                Assert.Equal(RichLessonVisualKind.RectangleDiagram,visual.Kind);
                observedModes.Add(p["mode"]);
                Assert.Equal(p["mode"]==0
                    ?p["length"]*p["width"]
                    :2*(p["length"]+p["width"]),
                    int.Parse(q.CorrectAnswer));
                Assert.Equal("m",visual.SecondaryLabel);
                Assert.Equal(new[]{p["length"],p["width"]},
                    visual.Items.Select(int.Parse));
            }
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="999999"}]));
        }
        if(family=="usccss.b01.g3.rectangle_area_perimeter_story")
            Assert.Equal(new[]{0,1},observedModes.OrderBy(x=>x).ToArray());
    }

    [Fact]
    public void GriddedGrade3RectangleHasTheExactUnitSquareCountAtMostSixty()
    {
        const string lesson="PED:US-CCSS-MATH:G3:U02:L04";
        for(var seed=570201;seed<570216;seed++)
        {
            var q=One(lesson,"usccss.b01.geometry.rectangle_area_grid60",seed);
            var visual=Assert.Single(UsCcssB01SourceTaskVisuals.Build(lesson,[q]));
            Assert.Equal(RichLessonVisualKind.ShapeDecomposition,visual.Kind);
            Assert.Equal(q.Parameters["rows"],visual.Rows.Count);
            var cells=visual.Rows.Sum(r=>r.Cells.Count(c=>c=="■"));
            Assert.Equal(q.Parameters["rows"]*q.Parameters["columns"],cells);
            Assert.Equal(int.Parse(q.CorrectAnswer),cells);
            Assert.InRange(cells,1,60);
        }
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G3:U02:L07")]
    [InlineData("PED:US-CCSS-MATH:G3:U02:L10")]
    public void SourceGrade3RealWorldAreaUsesMeasuredSquareFeetAndMeters(
        string lesson)
    {
        const string family="usccss.b01.geometry.rectangle_area_real_world_units";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(lesson,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        var units=new HashSet<int>();
        for(var seed=78931;seed<79002;seed++)
        {
            var q=One(lesson,family,seed);
            var p=q.Parameters;
            var unit=p["unitMode"];
            units.Add(unit);
            Assert.InRange(unit,0,1);
            Assert.Equal(p["length"]*p["width"],int.Parse(q.CorrectAnswer));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,q.CorrectAnswer));
            var visual=Assert.Single(UsCcssB01SourceTaskVisuals.Build(lesson,[q]));
            Assert.Equal(RichLessonVisualKind.RectangleDiagram,visual.Kind);
            Assert.Equal(unit==0?"m":"ft",visual.SecondaryLabel);
            Assert.Equal(p["length"],int.Parse(visual.Items[0]));
            Assert.Equal(p["width"],int.Parse(visual.Items[1]));
            Assert.Contains(unit==0?"square meters":"square feet",
                visual.Description);
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(lesson,
                [q with {CorrectAnswer="888888"}]));
        }
        Assert.Equal(new[]{0,1},units.OrderBy(x=>x).ToArray());
    }

    [Fact]
    public void UngriddedRectangleDiagramHasTheActualSideRatioAndGradedArea()
    {
        const string lesson="PED:US-CCSS-MATH:G3:U02:L08";
        for(var seed=570221;seed<570231;seed++)
        {
            var q=One(lesson,"usccss.b01.geometry.rectangle_area_ungridded",seed);
            var visual=Assert.Single(UsCcssB01SourceTaskVisuals.Build(lesson,[q]));
            Assert.Equal(RichLessonVisualKind.RectangleDiagram,visual.Kind);
            Assert.Equal(2,visual.Items.Count);
            Assert.Equal(q.Parameters["length"].ToString(),visual.Items[0]);
            Assert.Equal(q.Parameters["width"].ToString(),visual.Items[1]);
            Assert.Equal(int.Parse(q.CorrectAnswer),
                int.Parse(visual.Items[0])*int.Parse(visual.Items[1]));
        }
    }

    [Fact]
    public void PublisherPolygonSortingDiagramHasTheCorrectSideCount()
    {
        var expectedSides = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["triangle"]=3,
            ["quadrilateral"]=4,
            ["pentagon"]=5,
            ["hexagon"]=6
        };
        const string code = "PED:US-CCSS-MATH:G2:U06:L01";
        for(var seed=810000;seed<810045;seed++)
        {
            var q=One(code,"usccss.b01.shape.polygon_sides",seed);
            var visual=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.PolygonDiagram,visual.Kind);
            Assert.Equal(q.CorrectAnswer,Assert.Single(visual.Items));
            Assert.Equal(q.Parameters["sides"],expectedSides[q.CorrectAnswer]);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family,q.Parameters,q.CorrectAnswer));
        }
    }

    [Fact]
    public void PublisherShapeSubcategoryDiagramFollowsVerifiedSideAngleDefinition()
    {
        var names=new[]
        {
            "equilateral triangle","right triangle","rectangle","square"
        };
        const string code="PED:US-CCSS-MATH:G3:U07:L02";
        for(var seed=820000;seed<820050;seed++)
        {
            var q=One(code,"usccss.b01.shape.specific_subcategory",seed);
            Assert.Equal(names[q.Parameters["subcategory"]],q.CorrectAnswer);
            var visual=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.PolygonDiagram,visual.Kind);
            Assert.Equal(q.CorrectAnswer,Assert.Single(visual.Items));
        }
    }

    [Fact]
    public void OriginalUngridCompositeFigureRequiresAndReconstructsMissingSide()
    {
        const string code="PED:US-CCSS-MATH:G3:U02:L14";
        for(var seed=830000;seed<830040;seed++)
        {
            var q=One(code,"usccss.b01.area.missing_composite_side",seed);
            var p=q.Parameters;
            Assert.Equal(p["widthTotal"],p["widthKnown"]+p["widthMissing"]);
            Assert.NotEqual(p["heightLeft"],p["heightRight"]);
            var independent=p["widthKnown"]*p["heightLeft"]+
                p["widthMissing"]*p["heightRight"];
            Assert.Equal(independent,int.Parse(q.CorrectAnswer));
            var visual=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.ShapeDecomposition,visual.Kind);
            Assert.Equal(2,visual.Rows.Count);
            Assert.Contains(q.CorrectAnswer,visual.Description);
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                q.Family,p,(independent+1).ToString()));
        }
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G2:U04:L01","usccss.b01.number_line.locate_whole_to100")]
    [InlineData("PED:US-CCSS-MATH:G2:U05:L08","usccss.b01.number_line.locate_three_digits")]
    [InlineData("PED:US-CCSS-MATH:G4:U04:L11","usccss.b01.number_line.locate_large_numbers")]
    public void PublisherGradeNumberLinesUseTenEqualTicksAndExactAnswer(
        string code,string family)
    {
        for(var seed=870000;seed<870015;seed++)
        {
            var q=One(code,family,seed);
            var visual=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.NumberLine,visual.Kind);
            Assert.Equal(11,visual.Items.Count);
            Assert.Equal(q.Parameters["tick"].ToString(),
                Assert.Single(visual.SecondaryItems));
            var labels=visual.Items.Select(int.Parse).ToArray();
            Assert.Equal(0,labels[0]);
            for(var k=1;k<labels.Length;k++)
                Assert.Equal(q.Parameters["step"],labels[k]-labels[k-1]);
            Assert.Equal(q.CorrectAnswer,visual.Items[q.Parameters["tick"]]);
            Assert.Contains(q.CorrectAnswer,visual.Description);
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
