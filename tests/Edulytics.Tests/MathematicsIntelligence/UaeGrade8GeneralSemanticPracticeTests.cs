using System.Globalization;
using System.Text.Json;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeGrade8GeneralSemanticPracticeTests
{
    private const string Prefix = "PED:UAE-MOE-MATH:L8:GENERAL:";

    [Fact]
    public void Grade8ComposedPracticeReachesEveryAllowedFamilyAcrossSeededAttempts()
    {
        const string code = Prefix + "01:01:RATIONAL-NUMBERS";
        Assert.True(LessonPracticeContractRegistry.TryResolve(code, out var contract));
        Assert.NotNull(contract);
        var observed = new HashSet<string>(StringComparer.Ordinal);
        var engine = new Stage18SkillContractPracticeEngine();
        for (var seed = 1; seed <= contract!.AllowedQuestionFamilies.Count * 6; seed++)
        {
            var items = engine.GenerateComposed(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                contract.ToLegacyStage18Contract(),
                StudentPrivatePracticeDifficulty.MyLevel, 3, seed, [],
                Guid.NewGuid());
            Assert.Equal(3, items.Count);
            foreach (var item in items)
                observed.Add(Assert.IsType<string>(item.GenerationFamily));
        }
        Assert.All(contract.AllowedQuestionFamilies,
            family => Assert.Contains(family, observed));
        Assert.Contains("uae.g6.u05.l08.classify_fraction_decimal", observed);
        Assert.Contains("uae.g6.u05.l08.fraction_exact_decimal", observed);
    }

    [Theory]
    [InlineData("supporting.algebra.simplify")]
    [InlineData("supporting.algebra.expand")]
    [InlineData("supporting.algebra.equations.mixed")]
    [InlineData("algebra.linear.ax_plus_b_equals_c")]
    [InlineData("supporting.powers10.evaluate")]
    [InlineData("supporting.powers10.multiply")]
    [InlineData("supporting.powers10.divide")]
    [InlineData("supporting.powers10.missing_exponent")]
    [InlineData("geometry.coordinate.evaluate_linear_rule")]
    [InlineData("geometry.coordinate.y_intercept_from_rule")]
    [InlineData("geometry.similarity.scale_factor")]
    [InlineData("algebra.linear.standard_form.slope_intercept")]
    [InlineData("geometry.angles.supplementary")]
    [InlineData("geometry.polygons.missing_interior_angle")]
    [InlineData("statistics.center_spread.mean")]
    [InlineData("statistics.center_spread.median")]
    public void Grade8GeneralExactArithmeticPromptAndExplanationAreArabicOnly(
        string family)
    {
        var exact = new ExactSkillContractQuestionEngine().Generate(
            "g8-arabic-localization", Prefix + "01:01:RATIONAL-NUMBERS",
            [family], ExactSkillQuestionDifficulty.Standard, 1, 35819, [])[0];
        var lesson = Prefix + "01:01:RATIONAL-NUMBERS";
        var prompt = UaeGrade8SetPracticeLocalization.Prompt(lesson, exact);
        var solution = UaeGrade8SetPracticeLocalization.Solution(lesson, exact);
        Assert.Contains(prompt, c => c is >= 'ء' and <= 'ي');
        Assert.Contains(solution, c => c is >= 'ء' and <= 'ي');
        Assert.DoesNotContain("Find ", prompt, StringComparison.Ordinal);
        Assert.DoesNotContain("Solve ", prompt, StringComparison.Ordinal);
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            family, exact.Parameters, exact.CorrectAnswer));
        Assert.Equal(exact.Prompt, UaeGrade8SetPracticeLocalization.Prompt(
            "PED:US-CCSS:L8:GENERAL:01:01:RATIONAL-NUMBERS", exact));
    }

    [Theory]
    [InlineData("uae.g8.u01.l03.monomial_multiply")]
    [InlineData("uae.g8.u01.l03.monomial_divide")]
    [InlineData("uae.g8.u01.l04.monomial_power")]
    [InlineData("uae.g8.u01.l05.negative_integer_exponent")]
    [InlineData("uae.g8.u01.l08.square_root_exact")]
    [InlineData("uae.g8.u01.l08.cube_root_exact")]
    [InlineData("uae.g8.u01.l09.square_root_bounds")]
    [InlineData("uae.g8.u01.l09.cube_root_bounds")]
    [InlineData("uae.g8.u02.l03.write_two_step_equation")]
    [InlineData("uae.g8.u03.l03.direct_variation")]
    [InlineData("uae.g8.u10.l02.segment_addition")]
    [InlineData("uae.g8.u10.l03.midpoint_coordinate")]
    [InlineData("uae.g8.u10.l04.segment_reason")]
    [InlineData("uae.g8.u11.l03.parallelogram_test")]
    [InlineData("uae.g8.u11.l04.rectangle_test")]
    [InlineData("uae.g8.u11.l05.rhombus_diagonals")]
    [InlineData("uae.g8.u03.l07.system_solution_count")]
    [InlineData("uae.g8.u09.l01.scatter_direction")]
    [InlineData("uae.g8.u09.l06.distribution_shape")]
    public void Grade8ObjectiveSpecificFamiliesGenerateArabicAndGradeIndependently(
        string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        foreach(var seed in new[] { 9, 817, 1773, 9923, 13021 })
        {
            var questions=new ExactSkillContractQuestionEngine().Generate(
                "g8-batch-academic-closure", family, [family],
                ExactSkillQuestionDifficulty.Standard, 3, seed, []);
            Assert.Equal(3,questions.Count);
            foreach(var q in questions)
            {
                Assert.Equal(family,q.Family);
                Assert.Contains(q.Prompt,c=>c >= (char)0x0621 && c <= (char)0x064A);
                Assert.Contains(q.Solution,c=>c >= (char)0x0621 && c <= (char)0x064A);
                Assert.DoesNotMatch(@"[A-Za-z]{4,}",q.Prompt);
                Assert.DoesNotMatch(@"[A-Za-z]{4,}",q.Solution);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,"invalid-answer"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,new Dictionary<string,int>(),q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer+"9"));
            }
        }
    }

    [Fact]
    public void All76Grade8ComposedAttemptsHaveArabicQuestionAndArabicExplanation()
    {
        var index=System.IO.Path.GetFullPath(System.IO.Path.Combine(
            AppContext.BaseDirectory,"../../../../../",
            "artifacts/curriculum-delivery/UAE-MOE-MATH/G8/general-practice-semantic-audit.json"));
        Assert.True(System.IO.File.Exists(index),index);
        using var doc=JsonDocument.Parse(System.IO.File.ReadAllText(index));
        var lessons=doc.RootElement.GetProperty("lessons").EnumerateArray().ToArray();
        Assert.Equal(76,lessons.Length);
        var engine=new Stage18SkillContractPracticeEngine();
        foreach(var row in lessons)
        {
            var code=row.GetProperty("lessonCode").GetString()!;
            Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract),code);
            Assert.NotNull(contract);
            var observed=new HashSet<string>(StringComparer.Ordinal);
            for(int seed=1;seed<=64;seed++)
            {
                var questions=engine.GenerateComposed(
                    Guid.NewGuid(),Guid.NewGuid(),Guid.NewGuid(),
                    contract!.ToLegacyStage18Contract(),
                    StudentPrivatePracticeDifficulty.MyLevel,3,20000+seed,[],
                    Guid.NewGuid());
                Assert.Equal(3,questions.Count);
                foreach(var q in questions)
                {
                    var prompt=Assert.IsType<string>(q.Prompt);
                    var solution=Assert.IsType<string>(q.Solution);
                    Assert.Contains(prompt,c=>c >= (char)0x0621 && c <= (char)0x064A);
                    Assert.Contains(solution,c=>c >= (char)0x0621 && c <= (char)0x064A);
                    Assert.DoesNotMatch(@"[A-Za-z]{4,}",prompt);
                    Assert.DoesNotMatch(@"[A-Za-z]{4,}",solution);
                    observed.Add(Assert.IsType<string>(q.GenerationFamily));
                }
            }
            Assert.True(new HashSet<string>(contract!.AllowedQuestionFamilies).SetEquals(observed));
        }
    }

    [Theory]
    [InlineData("uae.g8.u05.l01.transversal_angle_measure")]
    [InlineData("uae.g8.u05.l03.triangle_angle_measure")]
    [InlineData("uae.g8.u05.l05.pythagorean_integer_triple")]
    [InlineData("uae.g8.u05.l06.pythagorean_context_diagonal")]
    [InlineData("uae.g8.u05.l07.coordinate_plane_distance")]
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
    [InlineData("05:01:LINES", "uae.g8.u05.l01.transversal_angle_measure")]
    [InlineData("05:03:ANGLES-OF-TRIANGLES", "uae.g8.u05.l03.triangle_angle_measure")]
    [InlineData("05:05:THE-PYTHAGOREAN-THEOREM", "uae.g8.u05.l05.pythagorean_integer_triple")]
    [InlineData("05:06:USING-THE-PYTHAGOREAN-THEOREM", "uae.g8.u05.l06.pythagorean_context_diagonal")]
    [InlineData("05:07:DISTANCE-ON-THE-COORDINATE-PLANE", "uae.g8.u05.l07.coordinate_plane_distance")]
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

    [Theory]
    [InlineData("uae.g8.u05.l01.transversal_angle_measure")]
    [InlineData("uae.g8.u05.l03.triangle_angle_measure")]
    public void AngleQuestionsMatchTheirGeometricSourceAndRejectTamperedModes(string family)
    {
        var engine = new ExactSkillContractQuestionEngine();
        var all = Enumerable.Range(1, 80).SelectMany(seed =>
            engine.Generate("g8-angle-semantic-regression", family, [family],
                ExactSkillQuestionDifficulty.Standard, 3, 25000 + seed, []))
            .ToArray();
        Assert.Equal([0, 1, 2], all.Select(x => x.Parameters["mode"])
            .Distinct().OrderBy(x => x).ToArray());
        foreach (var question in all)
        {
            var p = question.Parameters;
            var expected = family.EndsWith("transversal_angle_measure", StringComparison.Ordinal)
                ? (p["mode"] == 2 ? 180 - p["given"] : p["given"])
                : p["mode"] switch
                {
                    0 => 180 - p["a"] - p["b"],
                    1 => p["a"] + p["b"],
                    _ => p["b"]
                };
            Assert.Equal(expected.ToString(CultureInfo.InvariantCulture),
                question.CorrectAnswer);
            Assert.Contains(
                family.Contains("transversal", StringComparison.Ordinal)
                    ? "مستقيمان متوازيان" : "مثلث",
                question.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family, p, question.CorrectAnswer));
            var invalid = new Dictionary<string, int>(p) { ["mode"] = 4 };
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, invalid, question.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, p, (expected + 1).ToString(CultureInfo.InvariantCulture)));
        }
    }

    [Theory]
    [InlineData("uae.g8.u05.l05.pythagorean_integer_triple")]
    [InlineData("uae.g8.u05.l06.pythagorean_context_diagonal")]
    [InlineData("uae.g8.u05.l07.coordinate_plane_distance")]
    public void PythagoreanQuestionsComputeIndependentGeometryAndRejectTamperedCases(string family)
    {
        var triples = new (int A, int B, int C)[]
            { (3, 4, 5), (5, 12, 13), (8, 15, 17), (7, 24, 25), (9, 12, 15) };
        var space = new (int X, int Y, int Z, int D)[]
            { (1, 2, 2, 3), (2, 3, 6, 7), (3, 4, 12, 13), (6, 6, 7, 11) };
        var engine = new ExactSkillContractQuestionEngine();
        var all = Enumerable.Range(1, 80).SelectMany(seed =>
            engine.Generate("g8-original-pythagoras-regression", family, [family],
                ExactSkillQuestionDifficulty.Standard, 3, 61000 + seed, []))
            .ToArray();
        var expectedModes = family.EndsWith("pythagorean_integer_triple",
            StringComparison.Ordinal) ? new[] { 0, 1, 2 } : new[] { 0, 1, 2, 3 };
        Assert.Equal(expectedModes, all.Select(q => q.Parameters["mode"])
            .Distinct().OrderBy(x => x).ToArray());
        foreach (var q in all)
        {
            var p = q.Parameters;
            var triple = triples[p["i"]];
            var scale = p["scale"];
            int expected;
            if (family.EndsWith("pythagorean_integer_triple", StringComparison.Ordinal))
            {
                expected = p["mode"] switch
                {
                    0 => triple.C * scale,
                    1 => triple.B * scale,
                    _ => p["altered"] == 0 ? 1 : 0
                };
                if (p["mode"] == 2)
                {
                    // An altered purported hypotenuse breaks the exact converse.
                    var c = triple.C + p["altered"];
                    Assert.Equal(p["altered"] == 0,
                        triple.A * triple.A + triple.B * triple.B == c * c);
                }
            }
            else if (family.EndsWith("pythagorean_context_diagonal", StringComparison.Ordinal))
            {
                expected = p["mode"] switch
                {
                    0 or 3 => triple.C * scale,
                    1 => triple.B * scale,
                    _ => space[p["i"]].D * scale
                };
                if (p["mode"] == 2)
                {
                    var (x, y, z, d) = space[p["i"]];
                    Assert.Equal(d * d, x * x + y * y + z * z);
                }
            }
            else
            {
                var pairs = System.Text.RegularExpressions.Regex.Matches(
                    q.Prompt, @"\((-?\d+)، (-?\d+)\)");
                Assert.Equal(2, pairs.Count);
                int x1 = int.Parse(pairs[0].Groups[1].Value, CultureInfo.InvariantCulture);
                int y1 = int.Parse(pairs[0].Groups[2].Value, CultureInfo.InvariantCulture);
                int x2 = int.Parse(pairs[1].Groups[1].Value, CultureInfo.InvariantCulture);
                int y2 = int.Parse(pairs[1].Groups[2].Value, CultureInfo.InvariantCulture);
                var sum = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
                expected = (int)Math.Sqrt(sum);
                Assert.Equal(sum, expected * expected);
            }
            Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), q.CorrectAnswer);
            Assert.Contains(q.Prompt, c => c is >= 'ء' and <= 'ي');
            Assert.True(ExactSkillContractQuestionEngine.Verify(family, p, q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, p,
                (expected + 1).ToString(CultureInfo.InvariantCulture)));
            var tampered = new Dictionary<string, int>(p) { ["mode"] = 999 };
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, tampered, q.CorrectAnswer));
            var extra = new Dictionary<string, int>(p) { ["unapproved"] = 7 };
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, extra, q.CorrectAnswer));
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
                // Submit the correct numerical answer for the forged equal-base shape:
                // only the trapezoid-geometry validation may reject it.
                var forgedNumericalAnswer = a.ToString(CultureInfo.InvariantCulture);
                Assert.NotEqual(question.CorrectAnswer, forgedNumericalAnswer);
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family, impossible, forgedNumericalAnswer));
            }
        }
    }
}
