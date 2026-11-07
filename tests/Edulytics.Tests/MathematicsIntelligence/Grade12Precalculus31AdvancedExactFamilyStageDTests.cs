using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Independent Grade12 Stage D runtime acceptance for 31 source-skills.
/// These certify numerical subsets, NOT ungraded drawings or source parity.
/// </summary>
public sealed class Grade12Precalculus31AdvancedExactFamilyStageDTests
{
    private const string LessonPrefix =
        "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:";

    [Fact]
    public void All31NumericalFamiliesMatchIndependentlyComputedKnownAnswerVectors()
    {
        var baseParams = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["a"] = 3, ["b"] = 4, ["c"] = 5, ["d"] = 6,
            ["x"] = 2, ["n"] = 4, ["k"] = 3
        };
        var cases = new (string Code, int Answer, int? N, int? K, int? A, int? B)[]
        {
            ("u03.l04.polynomial_evaluate", -6, null,null,null,null),
            ("u03.l07.rational_asymptote",4,null,null,null,null),
            ("u03.l09.direct_variation",12,null,null,null,null),
            ("u05.l01.radians_to_degrees",90,null,null,null,null),
            ("u05.l02.unit_circle_coordinate",0,0,null,null,null),
            ("u05.l03.trigonometric_ratio",9,0,null,null,null),
            ("u06.l01.sine_wave_maximum",7,null,null,null,null),
            ("u06.l02.tangent_period",4,null,null,null,null),
            ("u06.l03.inverse_sine_principal",30,null,30,null,null),
            ("u07.l01.pythagorean_identity",5,2,null,null,null),
            ("u07.l02.angle_sum_sine",-1,null,null,null,null),
            ("u07.l05.trig_equation_solution_count",3,null,null,null,null),
            ("u07.l06.trig_model_amplitude",3,null,null,null,null),
            ("u08.l01.law_of_sines",3,null,null,null,null),
            ("u08.l02.law_of_cosines",13,null,null,null,null),
            ("u08.l03.polar_cartesian",3,0,null,null,null),
            ("u08.l04.polar_curve_evaluate",7,0,null,null,null),
            ("u08.l06.parametric_point",16,null,null,null,null),
            ("u08.l07.parametric_slope",5,null,null,null,null),
            ("u09.l04.partial_fraction_coefficient",4,null,null,null,null),
            ("u09.l07.matrix_determinant",-2,null,null,null,null),
            ("u10.l01.ellipse_major_axis",10,null,null,5,3),
            ("u10.l02.hyperbola_transverse_axis",6,null,null,null,null),
            ("u10.l03.parabola_focus",3,null,null,null,null),
            ("u10.l05.polar_conic_radius",3,null,null,null,null),
            ("u11.l04.arithmetic_series_sum",36,null,null,null,null),
            ("u11.l05.permutations",24,null,null,null,null),
            ("u11.l06.binomial_coefficient",4,null,null,null,null),
            ("u12.l01.polynomial_limit",25,null,null,null,null),
            ("u12.l02.removable_limit",6,null,null,null,null),
            ("u12.l03.continuity_matching",10,null,null,null,null)
        };
        Assert.Equal(31, cases.Length);
        foreach(var v in cases)
        {
            var p = new Dictionary<string, int>(baseParams, StringComparer.Ordinal);
            if (v.N is int n) p["n"] = n;
            if (v.K is int k) p["k"] = k;
            if (v.A is int a) p["a"] = a;
            if (v.B is int b) p["b"] = b;
            var family = Grade12PrecalculusAdvancedNumericQuestions.Prefix + v.Code;
            var expected = v.Answer.ToString(System.Globalization.CultureInfo.InvariantCulture);
            Assert.Equal(expected, Grade12PrecalculusAdvancedNumericQuestions.Solve(family,p));
            Assert.True(Grade12PrecalculusAdvancedNumericQuestions.Verify(family,p,expected));
            Assert.False(Grade12PrecalculusAdvancedNumericQuestions.Verify(family,p,
                (v.Answer + 1).ToString(System.Globalization.CultureInfo.InvariantCulture)));
        }
    }

    [Fact]
    public void EveryAdvancedOriginalFormulaHasAValidMathematicalOracleAcrossSeeds()
    {
        var failures = new List<string>();
        foreach (var (suffix, section) in Grade12PrecalculusAdvancedNumericQuestions.All)
        {
            var family = Grade12PrecalculusAdvancedNumericQuestions.Prefix + suffix;
            for (var seed = 1; seed <= 15; seed++)
            {
                try
                {
                    var item = Grade12PrecalculusAdvancedNumericQuestions.Build(
                        family, new Random(91000 + seed), 1);
                    var correct = Grade12PrecalculusAdvancedNumericQuestions.Solve(
                        item.Family, item.Parameters);
                    if (!Grade12PrecalculusAdvancedNumericQuestions.Verify(
                        family, item.Parameters, correct))
                        failures.Add(section + ": valid answer rejected");
                }
                catch (Exception ex)
                {
                    failures.Add(section + "/" + family + "/seed=" + seed +
                        ": " + ex.GetType().Name + " - " + ex.Message);
                }
            }
        }
        Assert.True(failures.Count == 0,
            "Exact Grade12 mathematical failures: " + string.Join(" | ", failures.Take(60)));
    }

    [Fact]
    public void All31AdvancedSourceSectionsHaveExplicitIndependentExactPractice()
    {
        var families=Grade12PrecalculusAdvancedNumericQuestions.All;
        Assert.Equal(31,families.Count);
        Assert.Equal(31,families.Values.Distinct(StringComparer.Ordinal).Count());

        var engine=new ExactSkillContractQuestionEngine();
        var index=0;
        foreach(var (suffix,section) in families)
        {
            var family=Grade12PrecalculusAdvancedNumericQuestions.Prefix+suffix;
            var lesson=LessonPrefix+section;
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family),family);
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(lesson,out var capability),
                $"Missing scoped runtime Practice capability: {lesson}");
            Assert.NotNull(capability);
            Assert.Equal("READY_VERIFIED",capability.Readiness);
            Assert.Single(capability.AllowedQuestionFamilies);
            Assert.Equal(family,capability.AllowedQuestionFamilies[0]);

            foreach(var difficulty in new[]
            {
                ExactSkillQuestionDifficulty.Standard,
                ExactSkillQuestionDifficulty.Stretch,
                ExactSkillQuestionDifficulty.Challenge
            })
            {
                var question=Assert.Single(engine.Generate(
                    "g12-31-original-numerical-source-skill",
                    lesson,[family],difficulty,1,33000+index,[]));
                Assert.Equal(family,question.Family);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,question.Parameters,question.CorrectAnswer),
                    $"Answer rejected: {section} {difficulty}");
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,question.Parameters,"not-an-integer"),
                    $"Malformed answer accepted: {section}");
                Assert.False(string.IsNullOrWhiteSpace(question.Prompt));
                Assert.False(string.IsNullOrWhiteSpace(question.Solution));
            }
            index++;
        }
    }

    [Fact]
    public void EveryAdvancedNumericalFamilySupportsTenDistinctVerifiedPracticeItems()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var index=0;
        foreach(var (suffix,section) in Grade12PrecalculusAdvancedNumericQuestions.All)
        {
            var family=Grade12PrecalculusAdvancedNumericQuestions.Prefix+suffix;
            var questions=engine.Generate("g12-advanced-10-item-capacity",
                LessonPrefix+section,
                [family],
                ExactSkillQuestionDifficulty.Standard,
                10,600000+index,[]);
            Assert.Equal(10,questions.Count);
            Assert.Equal(10,questions.Select(q=>q.ExposureFingerprint)
                .Distinct(StringComparer.Ordinal).Count());
            foreach(var q in questions)
            {
                Assert.Equal(family,q.Family);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer),
                    $"Invalid exact mathematics item in {section}");
            }
            index++;
        }
    }
}
