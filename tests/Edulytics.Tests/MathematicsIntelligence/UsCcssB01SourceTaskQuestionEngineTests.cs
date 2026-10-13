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
        ("G4:U04:L10", "place_value.ten_times")
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
        Assert.Equal(25, lessonIds.Count);
        Assert.Equal(19, familyIds.Count);
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
        Assert.Equal(75, generated);
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
