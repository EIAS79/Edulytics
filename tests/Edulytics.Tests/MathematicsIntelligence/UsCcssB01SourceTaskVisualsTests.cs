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
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                "PED:US-CCSS-MATH:G2:U03:L15",[q]));
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
