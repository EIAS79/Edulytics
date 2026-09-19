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
    [InlineData("geometry.similarity.find_missing_length")]
    [InlineData("geometry.similarity.scale_factor")]
    [InlineData("geometry.congruence.identify_criterion")]
    [InlineData("geometry.surface_area.rectangular_prism")]
    [InlineData("geometry.volume.rectangular_prism")]
    [InlineData("geometry.rectangle.area.exact")]
    [InlineData("geometry.rectangle.perimeter.exact")]
    [InlineData("geometry.right_triangle.pythagorean.exact")]
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
