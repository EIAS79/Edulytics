using System.Numerics;
using System.Text.Json;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Curriculum;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Generation;
using Edulytics.Core.Mathematics.Identifiers;
using Edulytics.Core.Mathematics.Skills;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class MathematicsFoundationContractTests
{
    [Theory]
    [InlineData("fractions.compare.unlike_denominators")]
    [InlineData("algebra.relationships.two_unknowns")]
    [InlineData("measurement.scale.read_equal_intervals")]
    public void SkillId_NormalizesStableCanonicalIdentifiers(string value)
    {
        var id = new SkillId($"  {value.ToUpperInvariant()}  ");

        Assert.Equal(value, id.Value);
        Assert.Equal(value, id.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData("fractions compare")]
    [InlineData("fractions//compare")]
    [InlineData("fractions.$compare")]
    public void SkillId_RejectsAmbiguousOrUnsafeIdentifiers(string value)
    {
        Assert.ThrowsAny<ArgumentException>(() => new SkillId(value));
    }

    [Fact]
    public void ExactRational_NormalizesSignAndGreatestCommonDivisor()
    {
        var value = new ExactRational(new BigInteger(-6), new BigInteger(-8));

        Assert.Equal(new BigInteger(3), value.Numerator);
        Assert.Equal(new BigInteger(4), value.Denominator);
        Assert.Equal("3/4", value.ToString());
    }

    [Fact]
    public void ExactRational_ArithmeticRemainsExact()
    {
        var oneThird = new ExactRational(1, 3);
        var oneSixth = new ExactRational(1, 6);

        Assert.Equal(new ExactRational(1, 2), oneThird + oneSixth);
        Assert.Equal(new ExactRational(1, 18), oneThird * oneSixth);
    }

    [Fact]
    public void MathAst_RoundTripsExactIntegerAndRationalValues()
    {
        MathNode problem = new EquationNode(
            new IntegerNode(25),
            new RationalNode(new ExactRational(1, 2)));

        var json = JsonSerializer.Serialize(problem);
        var roundTrip = JsonSerializer.Deserialize<MathNode>(json);

        Assert.Equal(problem, roundTrip);
    }

    [Fact]
    public void LessonSkillProfile_RejectsAllowedForbiddenOverlap()
    {
        var skill = new SkillId("fractions.compare.unlike_denominators");

        Assert.Throws<ArgumentException>(() => new LessonSkillProfile(
            "PED:TEST",
            MathematicsLessonSourceType.SupportingLesson,
            [skill],
            allowedQuestionFamilies: ["fractions.compare.numeric_pair"],
            forbiddenQuestionFamilies: ["fractions.compare.numeric_pair"]));
    }

    [Fact]
    public void LessonSkillResolution_OrdersCandidatesByScoreWithoutPromotingThem()
    {
        var lower = new LessonSkillCandidate(
            new SkillId("fractions.equivalent"),
            6,
            [new LessonSkillEvidence(LessonSkillEvidenceType.Explanation, "Equivalent fractions are used.", 3)]);
        var higher = new LessonSkillCandidate(
            new SkillId("fractions.compare.unlike_denominators"),
            14,
            [new LessonSkillEvidence(LessonSkillEvidenceType.LessonTitle, "Compare fractions with different denominators", 8)]);

        var resolution = new LessonSkillResolution(
            "PED:TEST:FRACTIONS",
            MathematicsLessonSourceType.SupportingLesson,
            LessonSkillResolutionStatus.HighConfidenceCandidate,
            [lower, higher]);

        Assert.Equal("fractions.compare.unlike_denominators", resolution.Candidates[0].SkillId.Value);
        Assert.Equal(LessonSkillResolutionStatus.HighConfidenceCandidate, resolution.Status);
    }

    [Fact]
    public void LessonSkillResolution_HighConfidenceRequiresCandidateEvidence()
    {
        Assert.Throws<ArgumentException>(() => new LessonSkillResolution(
            "PED:TEST:EMPTY",
            MathematicsLessonSourceType.SupportingLesson,
            LessonSkillResolutionStatus.HighConfidenceCandidate));
    }

    [Fact]
    public void SkillContract_RejectsSelfPrerequisite()
    {
        var skill = new SkillId("algebra.linear.solve");

        Assert.Throws<ArgumentException>(() => new SkillContract(
            skill,
            "algebra",
            "Solve linear equations",
            prerequisites: [skill]));
    }

    [Fact]
    public void QuestionFamilyManifest_KeepsExactSkillBinding()
    {
        var skill = new SkillId("fractions.compare.unlike_denominators");
        var manifest = new QuestionFamilyManifest(
            "fractions.compare.numeric_pair",
            skill,
            [new CapabilityId("rational.compare")],
            "ComparisonOperator",
            "exact_rational",
            ["symbolic"]);

        Assert.Equal(skill, manifest.Skill);
        Assert.Contains(new CapabilityId("rational.compare"), manifest.RequiredCapabilities);
        Assert.Equal("exact_rational", manifest.VerificationPolicy);
    }
}
