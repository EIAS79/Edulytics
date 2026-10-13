using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Independent structured-parameter oracle for 25 B01 lesson target corrections.
/// An exact mathematical answer alone does not prove the source lesson is fully
/// taught; the original publisher-purpose matching remains a separate job.
/// </summary>
public sealed class UsCcssB01SourceTaskQuestionEngineTests
{
    private static readonly (string Suffix, string Family)[] ExactMappings =
    [
        ("G1:U07:L05", "triangle.attributes"),
        ("G1:U07:L09", "partition.halves_fourths"),
        ("G1:U07:L10", "partition.halves_fourths"),
        ("G2:U04:L02", "number_line.equal_intervals"),
        ("G2:U06:L11", "clock.half_quarters"),
        ("G2:U08:L12", "rectangle.unit_squares"),
        ("G2:U09:L03", "measure.map_lengths"),
        ("G3:U01:L06", "graph.choose_scale"),
        ("G3:U01:L16", "array.equal_groups"),
        ("G3:U01:L18", "array.equal_groups"),
        ("G3:U01:L21", "array.equal_groups"),
        ("G3:U02:L03", "rectangle.unit_squares"),
        ("G3:U02:L11", "rectangle.unit_squares"),
        ("G3:U02:L15", "geometry.composite_rectangle_area"),
        ("G3:U04:L06", "division.unknown_factor"),
        ("G3:U05:L01", "fraction.name_parts"),
        ("G3:U06:L02", "measure.quarter_inch"),
        ("G4:U01:L02", "factors.factor_pairs"),
        ("G4:U01:L03", "factors.prime_composite"),
        ("G4:U01:L06", "factors.locker_square"),
        ("G4:U01:L07", "factors.factors_multiples"),
        ("G4:U01:L08", "factors.factor_pairs"),
        ("G4:U03:L04", "fraction.whole_multiply"),
        ("G4:U03:L07", "fraction.decompose_same_denominator"),
        ("G4:U04:L10", "place_value.ten_times"),
        ("G2:U02:L16", "market.inventory_100"),
        ("G1:U07:L14", "clock.read_half_hour"),
        ("G1:U07:L15", "clock.read_half_hour"),
        ("G2:U06:L12", "clock.read_five_minutes"),
        ("G2:U08:L07", "array.equal_groups"),
        ("G2:U08:L08", "array.columns"),
        ("G2:U08:L09", "array.equal_addends"),
        ("G2:U08:L10", "array.equal_equation"),
        ("G2:U08:L11", "rectangle.unit_squares"),
        ("G3:U01:L17", "array.equal_groups"),
        ("G3:U01:L19", "array.unknown_factor"),
        ("G3:U01:L20", "array.commutativity"),
        ("G3:U02:L02", "rectangle.unit_squares"),
        ("G3:U05:L02", "fraction.unit_parts"),
        ("G2:U06:L01", "shape.polygon_sides"),
        ("G3:U07:L02", "shape.specific_subcategory"),
        ("G3:U02:L14", "area.missing_composite_side"),
        ("G1:U04:L08", "place_value.tens_ones"),
        ("G1:U04:L09", "place_value.tens_ones"),
        ("G1:U04:L10", "place_value.tens_ones"),
        ("G1:U08:L07", "place_value.count_to120"),
        ("G1:U08:L08", "place_value.tens_ones"),
        ("G2:U05:L04", "place_value.three_digits"),
        ("G3:U01:L07", "graph.scaled_compare"),
        ("G3:U02:L04", "geometry.rectangle_area_grid60"),
        ("G3:U02:L08", "geometry.rectangle_area_ungridded"),
        ("G3:U02:L12", "geometry.composite_rectangle_area"),
        ("G3:U02:L13", "geometry.composite_rectangle_area")
    ];

    [Fact]
    public void TwentyFiveExplicitPublisherTargetsOnlyOverrideTheirOwnLessons()
    {
        var familyIds = new HashSet<string>(StringComparer.Ordinal);
        var lessonIds = new HashSet<string>(StringComparer.Ordinal);
        foreach (var (suffix, shortFamily) in ExactMappings)
        {
            var lessonCode = "PED:US-CCSS-MATH:" + suffix;
            var family = "usccss.b01." + shortFamily;
            Assert.True(lessonIds.Add(lessonCode));
            familyIds.Add(family);
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(lessonCode, out var contract),
                "No exact contract for source-bound lesson " + lessonCode);
            Assert.NotNull(contract);
            Assert.Equal(new[] { family }, contract!.AllowedQuestionFamilies);
            Assert.StartsWith("IM2021_", contract.SourceType, StringComparison.Ordinal);
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        }
        Assert.Equal(53, lessonIds.Count);
        Assert.Equal(37, familyIds.Count);
        // The exact prior approved PR #488 families must remain intact.
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G1:U01:L01", out var prior));
        Assert.Contains("usccss.g1.u01.l01.count_collection_to20",
            prior!.AllowedQuestionFamilies);
        if (LessonPracticeCapabilityResolver.TryResolve(
                "PED:US-CCSS-MATH:G4:U04:L21", out var future))
            Assert.DoesNotContain(future!.AllowedQuestionFamilies,
                family => family.StartsWith("usccss.b01.", StringComparison.Ordinal));
    }

    [Fact]
    public void SourceTaskFamiliesGenerateAndVerifyThroughTheRealRuntime()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var seed = 880001;
        var generated = 0;
        foreach (var (suffix, shortFamily) in ExactMappings)
            foreach (var difficulty in new[]
            {
                ExactSkillQuestionDifficulty.Standard,
                ExactSkillQuestionDifficulty.Stretch,
                ExactSkillQuestionDifficulty.Challenge
            })
            {
                var family = "usccss.b01." + shortFamily;
                var lesson = "PED:US-CCSS-MATH:" + suffix;
                var q = Assert.Single(engine.Generate(
                    "usccss-b01-source-task-independent-oracle", lesson, [family],
                    difficulty, 1, seed++, []));
                Assert.Equal(family, q.Family);
                Assert.False(string.IsNullOrWhiteSpace(q.Prompt));
                Assert.False(string.IsNullOrWhiteSpace(q.Solution));
                Assert.Equal(IndependentOracle(family, q.Parameters), q.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family, q.Parameters, q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family, q.Parameters, "guaranteed-invalid-answer"));
                generated++;
            }
        Assert.Equal(159, generated);
    }

    [Fact]
    public void PolygonFamilySupportsFullTenQuestionSessionWithoutDuplicatedPrompts()
    {
        const string family="usccss.b01.shape.polygon_sides";
        const string code="PED:US-CCSS-MATH:G2:U06:L01";
        var questions=new ExactSkillContractQuestionEngine().Generate(
            "usccss-b01-polygon-ten-distinct",
            code,[family],ExactSkillQuestionDifficulty.Standard,
            10,810901,[]);
        Assert.Equal(10,questions.Count);
        Assert.Equal(10,questions.Select(x=>x.Prompt)
            .Distinct(StringComparer.Ordinal).Count());
        Assert.All(questions,q=>
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family,q.Parameters,q.CorrectAnswer)));
    }

    [Fact]
    public void AllNewPracticeFamiliesRejectMissingOrTamperedData()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var i = 8001;
        foreach (var family in ExactMappings.Select(x => "usccss.b01." + x.Family)
                     .Distinct(StringComparer.Ordinal))
        {
            var q = Assert.Single(engine.Generate(
                "usccss-b01-tamper-resistance", family, [family],
                ExactSkillQuestionDifficulty.Standard, 1, i++, []));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, new Dictionary<string, int>(), q.CorrectAnswer));
        }
    }

    private static string IndependentOracle(
        string family, IReadOnlyDictionary<string, int> p)
    {
        int N(string k) => p[k];
        string Num(int n) => n.ToString(CultureInfo.InvariantCulture);
        switch (family)
        {
            case "usccss.b01.place_value.tens_ones":
                Assert.InRange(N("number"),10,99);
                Assert.Equal(N("number"),10*N("tens")+N("ones"));
                return Num(N("number"));
            case "usccss.b01.place_value.count_to120":
                Assert.InRange(N("number"),40,120);
                Assert.Equal(N("number"),
                    100*N("hundreds")+10*N("tens")+N("ones"));
                return Num(N("number"));
            case "usccss.b01.place_value.three_digits":
                Assert.InRange(N("number"),100,999);
                Assert.Equal(N("number"),
                    100*N("hundreds")+10*N("tens")+N("ones"));
                return Num(N("number"));
            case "usccss.b01.geometry.rectangle_area_grid60":
                Assert.InRange(N("rows")*N("columns"),1,60);
                return Num(N("rows")*N("columns"));
            case "usccss.b01.geometry.rectangle_area_ungridded":
                return Num(N("length")*N("width"));
            case "usccss.b01.graph.scaled_compare":
                Assert.Contains(N("unit"),new[]{2,5,10});
                Assert.Equal(N("countA"),N("unit")*N("ticksA"));
                Assert.Equal(N("countB"),N("unit")*N("ticksB"));
                Assert.InRange(N("countA"),1,100);
                Assert.InRange(N("countB"),1,100);
                return Num(Math.Abs(N("countA")-N("countB")));
            case "usccss.b01.clock.read_half_hour":
                Assert.True(N("minutes") is 0 or 30);
                return $"{N("hour")}:{N("minutes"):00}";
            case "usccss.b01.clock.read_five_minutes":
                Assert.InRange(N("minutes"), 0, 55);
                Assert.Equal(0, N("minutes")%5);
                return $"{N("hour")}:{N("minutes"):00}";
            case "usccss.b01.array.columns":
            case "usccss.b01.array.unknown_factor":
            case "usccss.b01.array.commutativity":
                return Num(N("columns"));
            case "usccss.b01.array.equal_addends":
                return string.Join("+", Enumerable.Repeat(Num(N("columns")), N("rows")));
            case "usccss.b01.array.equal_equation":
                return string.Join("+", Enumerable.Repeat(Num(N("columns")), N("rows"))) +
                    "=" + Num(N("rows")*N("columns"));
            case "usccss.b01.market.inventory_100":
                Assert.Equal(100, N("small") + N("second") + N("third"));
                Assert.InRange(N("small"), 1, 9);
                Assert.True(N("second") > 10 && N("third") > 10);
                return Num(N("mode") == 0 ? N("second") - N("sold") : N("third"));
            case "usccss.b01.shape.polygon_sides":
                return N("sides") switch
                {
                    3=>"triangle",4=>"quadrilateral",5=>"pentagon",6=>"hexagon",
                    _=>throw new InvalidOperationException()
                };
            case "usccss.b01.shape.specific_subcategory":
                return N("subcategory") switch
                {
                    0=>"equilateral triangle",1=>"right triangle",
                    2=>"rectangle",3=>"square",
                    _=>throw new InvalidOperationException()
                };
            case "usccss.b01.area.missing_composite_side":
                Assert.Equal(N("widthTotal"),
                    N("widthKnown")+N("widthMissing"));
                Assert.NotEqual(N("heightLeft"),N("heightRight"));
                return Num(N("widthKnown")*N("heightLeft")+
                    N("widthMissing")*N("heightRight"));
            case "usccss.b01.triangle.attributes":
                return Num(N("position"));
            case "usccss.b01.partition.halves_fourths":
                return N("denominator") == 2 ? "half" : "fourth";
            case "usccss.b01.number_line.equal_intervals":
                return Num(N("step") + N("step"));
            case "usccss.b01.clock.half_quarters":
                return Num(new[] { 15, 30, 45 }[N("position")]);
            case "usccss.b01.rectangle.unit_squares":
            case "usccss.b01.array.equal_groups":
                return Num(N("rows") * N("columns"));
            case "usccss.b01.measure.map_lengths":
                return Num(N("mode") == 0 ? N("a") + N("b") : N("b"));
            case "usccss.b01.graph.choose_scale":
                return Num(N("step"));
            case "usccss.b01.geometry.composite_rectangle_area":
                return Num(N("height") * N("width1") + N("height") * N("width2"));
            case "usccss.b01.division.unknown_factor":
                return Num(N("groups"));
            case "usccss.b01.fraction.unit_parts":
                Assert.Contains(N("denominator"),new[]{2,3,4,6,8});
                return "1/" + Num(N("denominator"));
            case "usccss.b01.fraction.name_parts":
                return Num(N("numerator")) + "/" + Num(N("denominator"));
            case "usccss.b01.measure.quarter_inch":
                return Num(N("wholeInches") * 4 + N("quarters")) + "/4";
            case "usccss.b01.factors.factor_pairs":
                return Num(N("secondFactor"));
            case "usccss.b01.factors.prime_composite":
                var n = N("number");
                var divisorCount = Enumerable.Range(1, n).Count(x => n % x == 0);
                return divisorCount == 2 ? "prime" : "composite";
            case "usccss.b01.factors.locker_square":
                n = N("locker");
                divisorCount = Enumerable.Range(1, n).Count(x => n % x == 0);
                return divisorCount % 2 == 1 ? "open" : "closed";
            case "usccss.b01.factors.factors_multiples":
                return (N("a") * N("b") + N("offset")) % N("a") == 0
                    ? "yes" : "no";
            case "usccss.b01.fraction.whole_multiply":
                var num = N("numerator") * N("whole");
                var den = N("denominator");
                var gcd = Enumerable.Range(1, Math.Min(num, den))
                    .Where(x => num % x == 0 && den % x == 0).Max();
                return Num(num / gcd) + "/" + Num(den / gcd);
            case "usccss.b01.fraction.decompose_same_denominator":
                return Num(N("wholeN") - N("firstN")) + "/" + Num(N("denominator"));
            case "usccss.b01.place_value.ten_times":
                return Num(N("digit") * N("place") * 10 / (N("digit") * N("place")));
            default:
                throw new InvalidOperationException("No independent test oracle: " + family);
        }
    }
}
