using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Enums;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.Mathematics;
using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class SupportingLessonPracticeR11R12Tests
{
    [Theory]
    [InlineData("Trigonometric ratios using sine cosine and tangent", "trigonometry.right_triangle.sin_cos_tan")]
    [InlineData("Pythagoras theorem", "geometry.right_triangle.pythagorean")]
    [InlineData("Congruence and similarity", "geometry.similarity")]
    [InlineData("Surface area and volume", "geometry.surface_area_volume")]
    [InlineData("Coordinates and straight-line graphs", "geometry.coordinate.straight_line")]
    [InlineData("Angle relationships in parallel lines", "geometry.angles.relationships")]
    [InlineData("Magnitude of a vector", "vectors.magnitude")]
    [InlineData("Vector subtraction", "vectors.subtract")]
    [InlineData("Scalar multiplication of vectors", "vectors.scalar_multiply")]
    [InlineData("Vector between two points", "vectors.between_points")]
    public void ExactCapabilityResolverSeparatesHighSchoolGeometryTargets(
        string context,
        string expectedSkill)
    {
        var resolved = ExactMathematicsCapabilityResolver.Resolve(context);

        Assert.NotNull(resolved);
        Assert.Equal(expectedSkill, resolved!.SkillId);
        Assert.NotEmpty(resolved.QuestionFamilies);
    }

    [Fact]
    public void TrigonometryDoesNotCollapseToPythagoras()
    {
        var trig = ExactMathematicsCapabilityResolver.Resolve(
            "Grade 10 trigonometric ratios: sine cosine and tangent");

        Assert.NotNull(trig);
        Assert.Equal("trigonometry.right_triangle.sin_cos_tan", trig!.SkillId);
        Assert.DoesNotContain(
            "geometry.right_triangle.pythagorean.exact",
            trig.QuestionFamilies);
        Assert.Contains(
            "trigonometry.right_triangle.ratio_exact",
            trig.QuestionFamilies);
    }

    [Theory]
    [InlineData("geometry.coordinate.gradient_between_points")]
    [InlineData("geometry.angles.parallel_lines")]
    [InlineData("geometry.angles.supplementary")]
    [InlineData("geometry.angles.algebraic_supplementary")]
    [InlineData("geometry.similarity.find_missing_length")]
    [InlineData("geometry.similarity.scale_factor")]
    [InlineData("geometry.congruence.identify_criterion")]
    [InlineData("geometry.surface_area.rectangular_prism")]
    [InlineData("geometry.volume.rectangular_prism")]
    [InlineData("geometry.rectangle.area.exact")]
    [InlineData("geometry.rectangle.perimeter.exact")]
    [InlineData("geometry.right_triangle.pythagorean.exact")]
    [InlineData("geometry.right_triangle.pythagorean.find_leg_exact")]
    [InlineData("trigonometry.right_triangle.ratio_exact")]
    [InlineData("trigonometry.right_triangle.find_side_exact")]
    [InlineData("trigonometry.right_triangle.find_angle_exact")]
    [InlineData("trigonometry.modelling.contextual")]
    public void EveryNewGeometryFamilyGeneratesAndIndependentlyVerifies(string family)
    {
        var engine = new ExactSkillContractQuestionEngine();
        var questions = engine.Generate(
            "r12-test",
            family,
            [family],
            ExactSkillQuestionDifficulty.Challenge,
            8,
            22001,
            []);

        Assert.Equal(8, questions.Count);
        Assert.All(questions, q =>
        {
            Assert.Equal(family, q.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family,
                q.Parameters,
                q.CorrectAnswer));
            Assert.False(string.IsNullOrWhiteSpace(q.Solution));
        });
    }

    [Theory]
    [InlineData("vectors.add.exact_rational")]
    [InlineData("vectors.subtract.exact_rational")]
    [InlineData("vectors.scalar_multiply.exact_rational")]
    [InlineData("vectors.dot.exact_rational")]
    [InlineData("vectors.magnitude.exact")]
    [InlineData("vectors.between_points.exact")]
    [InlineData("algebra.linear.variables_both_sides")]
    [InlineData("algebra.linear.inequality.variables_both_sides")]
    public void VectorFamiliesGenerateAndIndependentlyVerify(string family)
    {
        var engine = new ExactSkillContractQuestionEngine();
        var questions = engine.Generate(
            "vector-test",
            family,
            [family],
            ExactSkillQuestionDifficulty.Challenge,
            10,
            78231,
            []);

        Assert.Equal(10, questions.Count);
        Assert.All(questions, question =>
        {
            Assert.Equal(family, question.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                question.Family,
                question.Parameters,
                question.CorrectAnswer));
            Assert.False(string.IsNullOrWhiteSpace(question.Solution));
        });
    }

    [Theory]
    [InlineData("number.whole.add.direct")]
    [InlineData("number.whole.subtract.direct")]
    [InlineData("number.lcm.two_numbers")]
    [InlineData("percentages.of_quantity.direct")]
    [InlineData("fractions.represent.interpret.fraction_bar")]
    [InlineData("geometry.surface_area_volume.rectangular_prism_surface_area")]
    [InlineData("geometry.surface_area_volume.rectangular_prism_volume")]
    public void NewlyClosedCoreFamiliesGenerateAndVerify(string family)
    {
        var engine = new ExactSkillContractQuestionEngine();
        var questions = engine.Generate(
            "core-gap-test",
            family,
            [family],
            ExactSkillQuestionDifficulty.Stretch,
            6,
            94413,
            []);

        Assert.Equal(6, questions.Count);
        Assert.All(questions, question =>
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(question.Family));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                question.Family,
                question.Parameters,
                question.CorrectAnswer));
        });
    }

    [Theory]
    [InlineData("percentages.core.of_quantity")]
    [InlineData("percentages.core.increase_decrease")]
    [InlineData("sequences.core.arithmetic_nth_term")]
    [InlineData("sequences.core.geometric_nth_term")]
    [InlineData("sequences.core.find_position_arithmetic")]
    [InlineData("geometry.polygons.interior_angle_sum")]
    [InlineData("geometry.polygons.missing_interior_angle")]
    [InlineData("geometry.polygons.regular_interior_angle")]
    [InlineData("geometry.perimeter_area.triangle_area")]
    [InlineData("geometry.perimeter_area.rectangle_missing_side")]
    public void R6CoreNumberGeometryFamiliesGenerateAndVerify(string family)
    {
        var engine = new ExactSkillContractQuestionEngine();
        var questions = engine.Generate(
            "r6-core-test",
            family,
            [family],
            ExactSkillQuestionDifficulty.Challenge,
            10,
            66031,
            []);

        Assert.Equal(10, questions.Count);
        Assert.All(questions, question =>
        {
            Assert.Equal(family, question.Family);
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(question.Family));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                question.Family,
                question.Parameters,
                question.CorrectAnswer));
            Assert.False(string.IsNullOrWhiteSpace(question.Solution));
        });
    }

    [Theory]
    [InlineData("PED:CAMBRIDGE-INTL-MATH:L10:CORE:01:03:PERCENTAGES", "percentages.core", "percentages.core.increase_decrease")]
    [InlineData("PED:CAMBRIDGE-INTL-MATH:L10:EXTENDED:02:05:SEQUENCES", "sequences.core", "sequences.core.geometric_nth_term")]
    [InlineData("PED:UAE-MOE-MATH:L12:ADVANCED:03:02:POLYGONS", "geometry.polygons.angle_sum", "geometry.polygons.regular_interior_angle")]
    [InlineData("PED:UAE-MOE-MATH:L11:GENERAL:03:05:PERIMETER-AND-AREA", "geometry.perimeter_area", "geometry.perimeter_area.triangle_area")]
    public void R6PromotedMappingsProjectIntoExactRuntimeContracts(
        string lessonCode,
        string expectedSkill,
        string expectedFamily)
    {
        Assert.True(
            Edulytics.Core.Mathematics.Practice.LessonPracticeContractRegistry.TryResolve(
                lessonCode,
                out var contract));

        Assert.NotNull(contract);
        Assert.Equal(expectedSkill, contract!.SkillId);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Contains(expectedFamily, contract.AllowedQuestionFamilies);
        Assert.All(
            contract.AllowedQuestionFamilies,
            family => Assert.True(
                ExactSkillContractQuestionEngine.SupportsFamily(family),
                $"R6 contract {lessonCode} routes unsupported family {family}."));
    }

    [Fact]
    public void R6PromotionRaisesRuntimeExactContractCountTo232()
    {
        Assert.Equal(
            232,
            Edulytics.Core.Mathematics.Practice.LessonPracticeContractRegistry.All.Count);
    }

    [Theory]
    [InlineData("PED:CAMBRIDGE-INTL-MATH:L10:CORE:02:02:LINEAR-EQUATIONS", "algebra.linear.solve", "algebra.linear.variables_both_sides")]
    [InlineData("PED:UAE-MOE-MATH:L12:ADVANCED:02:06:INEQUALITIES", "algebra.linear.inequality.solve", "algebra.linear.inequality.variables_both_sides")]
    [InlineData("PED:UAE-MOE-MATH:L10:ADVANCED:03:07:PYTHAGORAS-THEOREM", "geometry.right_triangle.pythagorean", "geometry.right_triangle.pythagorean.find_leg_exact")]
    [InlineData("PED:UAE-MOE-MATH:L9:ADVANCED:03:01:ANGLE-RELATIONSHIPS", "geometry.angles.relationships", "geometry.angles.algebraic_supplementary")]
    public void R5PromotedSupportingMappingsProjectIntoExactRuntimeContracts(
        string lessonCode,
        string expectedSkill,
        string expectedDeepFamily)
    {
        Assert.True(
            Edulytics.Core.Mathematics.Practice.LessonPracticeContractRegistry.TryResolve(
                lessonCode,
                out var contract));

        Assert.NotNull(contract);
        Assert.Equal(expectedSkill, contract!.SkillId);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Contains(expectedDeepFamily, contract.AllowedQuestionFamilies);
        Assert.All(
            contract.AllowedQuestionFamilies,
            family => Assert.True(
                ExactSkillContractQuestionEngine.SupportsFamily(family),
                $"R5 contract {lessonCode} routes unsupported family {family}."));
    }

    [Fact]
    public void R5PromotionKeepsAtLeast162RuntimeExactContracts()
    {
        Assert.True(
            Edulytics.Core.Mathematics.Practice.LessonPracticeContractRegistry.All.Count >= 162);
    }

    [Fact]
    public void ApprovedOfficialMappingProjectsIntoRuntimePracticeRegistry()
    {
        Assert.True(
            Edulytics.Core.Mathematics.Practice.LessonPracticeContractRegistry.TryResolve(
                "PED:US-CCSS-MATH:G3:U05:L10",
                out var contract));

        Assert.NotNull(contract);
        Assert.Equal("fractions.equivalent", contract!.SkillId);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Equal(
            "lesson-practice-projection-v1",
            contract.ContractVersion);
        Assert.All(
            contract.AllowedQuestionFamilies,
            family => Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family)));
    }

    [Fact]
    public void FractionRepresentationMappingProjectsWithRequiredExactFamily()
    {
        Assert.True(
            Edulytics.Core.Mathematics.Practice.LessonPracticeContractRegistry.TryResolve(
                "PED:US-CCSS-MATH:G4:U02:L02",
                out var contract));

        Assert.NotNull(contract);
        Assert.Contains(
            "fractions.represent.interpret.fraction_bar",
            contract!.AllowedQuestionFamilies);
        Assert.All(
            contract.AllowedQuestionFamilies,
            family => Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family)));
    }

    [Fact]
    public void EveryRuntimePracticeFamilyIsSupportedByExactEngine()
    {
        Assert.All(
            Edulytics.Core.Mathematics.Practice.LessonPracticeContractRegistry.All,
            contract => Assert.All(
                contract.AllowedQuestionFamilies,
                family => Assert.True(
                    ExactSkillContractQuestionEngine.SupportsFamily(family),
                    $"Runtime contract {contract.LessonCode} routes unsupported family {family}.")));
    }

    [Fact]
    public void UniversalEngineUsesExactSkillContractWithoutContextualFallback()
    {
        var schoolId = Guid.NewGuid();
        var adoptionId = Guid.NewGuid();
        var outcomeId = Guid.NewGuid();

        var blueprint = new AssessmentBlueprint(
            schoolId,
            adoptionId,
            "Grade 10",
            null,
            null,
            AssessmentPurpose.TeacherAssessment,
            1,
            [new OutcomeBlueprintAllocation(outcomeId, 1, 1m, "ExactGeometryTest")],
            [new DifficultyBlueprintAllocation(AssessmentItemDifficulty.Challenging, 1)],
            [new QuestionFamilyBlueprintAllocation(AssessmentQuestionFamily.MathematicalReasoning, 1)],
            [new ItemTypeBlueprintAllocation(AssessmentItemType.Numeric, 1)],
            [new OutcomeEvidenceRequirement(outcomeId, 1, true, true)],
            [],
            "r11-r12-test");

        var profile = new MathematicsOutcomeGenerationProfile(
            outcomeId,
            "TEST:G10:TRIG",
            [MathematicsGeneratorFamily.CurriculumContextCheck])
        {
            GenerationContext = "Trigonometric ratios using sine cosine and tangent",
            IsContextualAssisted = false,
            ExactSkillId = "trigonometry.right_triangle.sin_cos_tan",
            ExactQuestionFamilies = ["trigonometry.right_triangle.find_side_exact"]
        };

        var batch = new UniversalMathematicsQuestionGenerationEngine().Generate(
            new MathematicsGenerationRequest(blueprint, [profile], 31415));

        var item = Assert.Single(batch.Items).Item;
        Assert.Equal("exact-skill-contract-v1", item.GenerationMethod);
        Assert.Equal(
            "trigonometry.right_triangle.find_side_exact",
            item.GenerationFamily);
        Assert.DoesNotContain(
            "CurriculumContextCheck",
            item.GenerationFamily ?? string.Empty,
            StringComparison.Ordinal);
        Assert.Contains(
            @"""broadFallbackUsed"":false",
            item.ValidationMetadataJson,
            StringComparison.Ordinal);
        Assert.Contains(
            @"""capabilityLevel"":""READY_VERIFIED""",
            item.ValidationMetadataJson,
            StringComparison.Ordinal);
    }
}
