using System.Globalization;
using System.Text.Json;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeGrade8GeneralSemanticPracticeTests
{
    private const string Prefix = "PED:UAE-MOE-MATH:L8:GENERAL:";

    [Theory]
    [InlineData("uae.g8.u05.l02.proof_reason_classify")]
    [InlineData("uae.g8.u06.l03.rotation_origin_coordinate")]
    [InlineData("uae.g8.u11.l06.trapezoid_midsegment")]
    [InlineData("uae.g8.u11.l06.kite_diagonal_area")]
    [InlineData("uae.g8.u12.l01.membership_finite_roster")]
    [InlineData("uae.g8.u12.l01.empty_finite_infinite_classify")]
    [InlineData("uae.g8.u12.l02.subset_classify")]
    [InlineData("uae.g8.u12.l02.union_cardinality")]
    [InlineData("uae.g8.u12.l02.intersection_cardinality")]
    [InlineData("uae.g8.u12.l02.difference_cardinality")]
    [InlineData("uae.g8.u12.l03.three_set_venn_region_count")]
    public void EverySourceScopedFamilyGeneratesArabicAndIndependentlyGrades(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var seed in new[] { 817, 1259, 16103 })
        {
            var questions = engine.Generate("g8-semantic-audit", family, [family],
                ExactSkillQuestionDifficulty.Standard, 6, seed, []);
            Assert.Equal(6, questions.Count);
            Assert.Equal(6, questions.Select(q => q.ExposureFingerprint).Distinct().Count());
            foreach (var q in questions)
            {
                Assert.Equal(family, q.Family);
                Assert.Contains(q.Prompt, c => c is >= 'ء' and <= 'ي');
                Assert.Contains(q.Solution, c => c is >= 'ء' and <= 'ي');
                Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family, q.Parameters, q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,
                    q.Parameters, (int.Parse(q.CorrectAnswer, CultureInfo.InvariantCulture) + 1)
                        .ToString(CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family, q.Parameters, "not-a-number"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,
                    new Dictionary<string, int>(), q.CorrectAnswer));
            }
        }
    }

    [Theory]
    [InlineData("05:02:GEOMETRIC-PROOF", "uae.g8.u05.l02.proof_reason_classify")]
    [InlineData("06:03:ROTATIONS", "uae.g8.u06.l03.rotation_origin_coordinate")]
    [InlineData("11:06:TRAPEZOIDS-AND-KITES", "uae.g8.u11.l06.trapezoid_midsegment")]
    [InlineData("12:01:INTRODUCTION-TO-SET-THEORY", "uae.g8.u12.l01.membership_finite_roster")]
    [InlineData("12:02:SUBSETS-AND-OPERATIONS-ON-SETS", "uae.g8.u12.l02.subset_classify")]
    [InlineData("12:03:USING-VENN-DIAGRAMS-TO-STUDY-SET-OPERATIONS", "uae.g8.u12.l03.three_set_venn_region_count")]
    public void AuthenticatedPracticeAdapterUsesLessonObjectiveNotFormerWrongFamily(
        string suffix, string requiredFamily)
    {
        var code = Prefix + suffix;
        Assert.True(LessonPracticeContractRegistry.TryResolve(code, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED", contract!.Readiness);
        Assert.Contains(requiredFamily, contract.AllowedQuestionFamilies);
        Assert.All(contract.AllowedQuestionFamilies,
            f => Assert.StartsWith("uae.g8.", f, StringComparison.Ordinal));
        var items = new Stage18SkillContractPracticeEngine().GenerateComposed(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            contract.ToLegacyStage18Contract(), StudentPrivatePracticeDifficulty.MyLevel,
            3, 98631, [], Guid.NewGuid());

        Assert.Equal(3, items.Count);
        foreach (var item in items)
        {
            Assert.Contains(Assert.IsType<string>(item.GenerationFamily),
                contract.AllowedQuestionFamilies);
            Assert.Contains(item.Prompt, c => c is >= 'ء' and <= 'ي');
            using var doc = JsonDocument.Parse(Assert.IsType<string>(item.GenerationParametersJson));
            var parameters = doc.RootElement.GetProperty("parameters")
                .EnumerateObject().ToDictionary(x => x.Name, x => x.Value.GetInt32());
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                Assert.IsType<string>(item.GenerationFamily), parameters,
                Assert.IsType<string>(item.CorrectAnswer)));
        }
    }

    [Fact]
    public void RotationCoversQuarterAndHalfTurnsAndNeverAccidentallyGradesReflectionOnly()
    {
        const string f = "uae.g8.u06.l03.rotation_origin_coordinate";
        var all = Enumerable.Range(1, 40).SelectMany(seed =>
            new ExactSkillContractQuestionEngine().Generate("rotation-regression", f, [f],
                ExactSkillQuestionDifficulty.Standard, 4, seed * 109, [])).ToArray();
        var quarters = all.Select(q => q.Parameters["quarter"]).Distinct().OrderBy(x => x).ToArray();
        Assert.Equal([1, 2, 3], quarters);
        foreach (var q in all)
        {
            var x = q.Parameters["x"];
            var y = q.Parameters["y"];
            var quarter = q.Parameters["quarter"];
            var value = (quarter, q.Parameters["component"]) switch
            {
                (1, 0) => -y, (1, 1) => x,
                (2, 0) => -x, (2, 1) => -y,
                (3, 0) => y, _ => -x
            };
            Assert.Equal(value.ToString(CultureInfo.InvariantCulture), q.CorrectAnswer);
        }
    }

    [Fact]
    public void VennThreeSetUnionCountsOverlapsExactlyOnce()
    {
        const string f = "uae.g8.u12.l03.three_set_venn_region_count";
        var all = Enumerable.Range(1, 60).SelectMany(seed =>
            new ExactSkillContractQuestionEngine().Generate("venn-regression", f, [f],
                ExactSkillQuestionDifficulty.Standard, 3, 2000 + seed, [])).ToArray();
        Assert.Contains(all, q => q.Parameters["mode"] == 0);
        Assert.Contains(all, q => q.Parameters["mode"] == 2);
        foreach (var q in all)
        {
            var p = q.Parameters;
            var expected = p["mode"] switch
            {
                0 => p["r7"], 1 => p["r4"] + p["r7"],
                2 => p["r1"] + p["r2"] + p["r4"] + p["r5"] + p["r6"] + p["r7"],
                3 => p["outside"], 4 => p["r1"] + p["r5"], _ => p["r1"]
            };
            Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), q.CorrectAnswer);
        }
    }

    [Fact]
    public void ExclusiveTrapezoidNeverUsesEqualParallelBasesAndRejectsTampering()
    {
        const string family = "uae.g8.u11.l06.trapezoid_midsegment";
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var seed in Enumerable.Range(1, 120))
        {
            var questions = engine.Generate("g8-exclusive-trapezoid",
                family, [family], ExactSkillQuestionDifficulty.Standard,
                3, 19000 + seed, []);
            foreach (var question in questions)
            {
                var a = question.Parameters["base1"];
                var b = question.Parameters["base2"];
                Assert.NotEqual(a, b);
                Assert.Equal(((a + b) / 2).ToString(CultureInfo.InvariantCulture),
                    question.CorrectAnswer);
                var impossible = new Dictionary<string, int>(question.Parameters)
                {
                    ["base2"] = a
                };
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family, impossible, question.CorrectAnswer));
            }
        }
    }
}
