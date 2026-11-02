using System.Text.Json;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class SupportingLessonPracticeR3Tests
{
    [Fact]
    public void ReviewDecisionRegistryCoversRemainingBaselineReviewRequiredLessons()
    {
        var root = FindRoot();
        using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/supporting-review-decisions.r3.v1.json")));

        var rootElement = document.RootElement;
        Assert.Equal(68, rootElement.GetProperty("total").GetInt32());
        Assert.Equal(23, rootElement.GetProperty("approvedExistingSkill").GetInt32());
        Assert.Equal(45, rootElement.GetProperty("deferredToOntology").GetInt32());

        var decisions = rootElement.GetProperty("decisions").EnumerateArray().ToArray();
        Assert.Equal(68, decisions.Length);
        Assert.Equal(68, decisions.Select(x => x.GetProperty("lessonCode").GetString()).Distinct().Count());

        Assert.All(decisions, decision =>
        {
            var value = decision.GetProperty("decision").GetString();
            Assert.True(value is "APPROVED_EXISTING_SKILL" or "DEFER_TO_R5_ONTOLOGY");
            Assert.False(string.IsNullOrWhiteSpace(decision.GetProperty("reviewEvidence").GetString()));
            Assert.False(string.IsNullOrWhiteSpace(decision.GetProperty("nextAction").GetString()));
        });
    }

    [Fact]
    public void ApprovedR3LessonsHaveExactPracticeContracts()
    {
        var root = FindRoot();
        using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/supporting-review-decisions.r3.v1.json")));

        var approvedCodes = document.RootElement
            .GetProperty("decisions")
            .EnumerateArray()
            .Where(x => x.GetProperty("decision").GetString() == "APPROVED_EXISTING_SKILL")
            .Select(x => x.GetProperty("lessonCode").GetString()!)
            .ToArray();

        Assert.Equal(23, approvedCodes.Length);
        foreach (var code in approvedCodes)
        {

            Assert.True(LessonPracticeContractRegistry.TryResolve(code, out var contract));
            Assert.NotNull(contract);
            Assert.Equal("READY_VERIFIED", contract!.Readiness);
            Assert.NotEmpty(contract.AllowedQuestionFamilies);
        }
    }

    [Fact]
    public void EveryApprovedR3ContractGeneratesSolverVerifiedItems()
    {
        var root = FindRoot();
        using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/supporting-review-decisions.r3.v1.json")));

        var approvedCodes = document.RootElement
            .GetProperty("decisions")
            .EnumerateArray()
            .Where(x => x.GetProperty("decision").GetString() == "APPROVED_EXISTING_SKILL")
            .Select(x => x.GetProperty("lessonCode").GetString()!)
            .ToHashSet(StringComparer.Ordinal);

        var engine = new Stage18SkillContractPracticeEngine();
        var seed = 12000;
        foreach (var contract in LessonPracticeContractRegistry.All.Where(x => approvedCodes.Contains(x.LessonCode)))
        {
            var legacy = contract.ToLegacyStage18Contract();
            var items = engine.Generate(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                legacy,
                StudentPrivatePracticeDifficulty.Stretch,
                Math.Max(2, legacy.AllowedQuestionFamilies.Count),
                seed++,
                [],
                Guid.NewGuid());

            Assert.All(items, item =>
            {
                Assert.True(Stage18SkillContractPracticeEngine.VerifyPersistedItem(legacy, item));
                Assert.Contains(@"""solverVerified"":true", item.ValidationMetadataJson, StringComparison.Ordinal);
                Assert.Contains(@"""broadFallbackUsed"":false", item.ValidationMetadataJson, StringComparison.Ordinal);
            });
        }
    }

    [Theory]
    [InlineData("PED:CAMBRIDGE-INTL-MATH:S4:4NF-2:BUILD", "number.whole.divide.with_remainder.build")]
    [InlineData("PED:CAMBRIDGE-INTL-MATH:S5:5F-1:APPLY", "fractions.of_quantity.apply")]
     [InlineData("PED:US-CCSS-MATH:G7:U06:L13", "algebra.linear.inequality.ax_plus_b_relation_c")]
    public void ReviewedLessonUsesNarrowExactFamily(string lessonCode, string family)
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(lessonCode, out var contract));
        Assert.NotNull(contract);
        Assert.Contains(contract!.AllowedQuestionFamilies, x => string.Equals(x, family, StringComparison.Ordinal));
    }


    [Fact]
    public void Grade8TwoVariableLessonRequiresExactOrderedPairMembershipPractice()
    {
        const string blocked = LessonPracticeContractRegistry.Grade8TwoVariableMembershipLesson;
        Assert.Equal("PED:US-CCSS-MATH:G8:U03:L12", blocked);

        // A correct Rich V2 teaching asset does not authorize wrong Practice.
        Assert.True(LessonPracticeContractRegistry.TryResolve(blocked, out var exact));
        Assert.NotNull(exact);
        Assert.Equal("algebra.linear.two_variable.membership", exact!.SkillId);
        Assert.Equal(new[] { "usccss.g8.u03.l12.ordered_pair_membership" }, exact.AllowedQuestionFamilies);
        Assert.DoesNotContain("algebra.linear.ax_plus_b_equals_c", exact.AllowedQuestionFamilies);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(blocked, out _));

        var root = FindRoot();
        using var mapping = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root, "src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json")));
        var row = Assert.Single(mapping.RootElement
            .GetProperty("mappings").EnumerateArray(),
            x => x.GetProperty("lessonCode").GetString() == blocked);
        Assert.Equal("READY_VERIFIED", row.GetProperty("practiceReadiness").GetString());
        Assert.Single(row.GetProperty("allowedQuestionFamilies").EnumerateArray());

        // Do not remove legitimate Grade 8 single-variable linear-equation Practice.
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G8:U04:L05", out var valid));
        Assert.NotNull(valid);
        Assert.Contains("algebra.linear.ax_plus_b_equals_c", valid!.AllowedQuestionFamilies);
    }

    [Fact]
    public void Grade8TwoVariableMembershipQuestionsUseIndependentPointSubstitution()
    {
        const string code = "PED:US-CCSS-MATH:G8:U03:L12";
        const string family = "usccss.g8.u03.l12.ordered_pair_membership";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code, out var contract));
        Assert.NotNull(contract);
        var engine = new ExactSkillContractQuestionEngine();
        var items = engine.Generate(
            "usccss-grade8-regression", code,
            contract!.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard, 24, 820312, []);
        Assert.Equal(24, items.Count);
        Assert.Contains(items, item => item.CorrectAnswer == "yes");
        Assert.Contains(items, item => item.CorrectAnswer == "no");
        foreach (var item in items)
        {
            Assert.Equal(family, item.Family);
            var p = item.Parameters;
            var onLine = (long)p["a"] * p["x"] + (long)p["b"] * p["y"] == p["c"];
            Assert.Equal(onLine ? "yes" : "no", item.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family, p, item.CorrectAnswer));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family, p, item.CorrectAnswer.ToUpperInvariant()));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, p, onLine ? "no" : "yes"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, p, "maybe; 1=1"));
            var malformed = p.ToDictionary(pair => pair.Key, pair => pair.Value);
            malformed.Remove("x");
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, malformed, item.CorrectAnswer));
        }
    }

    [Theory]
    [InlineData(0, 3, 9, 11, 3, true)]  // horizontal line y = 3
    [InlineData(0, 3, 9, 11, 2, false)]
    [InlineData(-2, 0, 8, -4, 13, true)] // vertical line x = -4
    [InlineData(-2, 0, 8, 3, 13, false)]
    [InlineData(0, 0, 0, 0, 0, false)]   // reject the degenerate non-line
    public void Grade8MembershipSupportsAxisAlignedLinesButRejectsDegenerateEquations(
        int a, int b, int c, int x, int y, bool expectedMember)
    {
        const string family = "usccss.g8.u03.l12.ordered_pair_membership";
        var p = new Dictionary<string, int>
        {
            ["a"] = a, ["b"] = b, ["c"] = c, ["x"] = x, ["y"] = y
        };
        var nonDegenerate = a != 0 || b != 0;
        Assert.Equal(nonDegenerate && expectedMember,
            ExactSkillContractQuestionEngine.Verify(family, p, "yes"));
        Assert.Equal(nonDegenerate && !expectedMember,
            ExactSkillContractQuestionEngine.Verify(family, p, "no"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family, p, "maybe"));
    }

    [Fact]
    public void Grade8GeneratedMembershipIncludesAxisAlignedLinesAndNoDegeneratePairs()
    {
        const string code = "PED:US-CCSS-MATH:G8:U03:L12";
        const string family = "usccss.g8.u03.l12.ordered_pair_membership";
        var engine = new ExactSkillContractQuestionEngine();
        var items = engine.Generate("g8-zero-coefficients", code,
            [family], ExactSkillQuestionDifficulty.Standard, 48, 820313, []);
        Assert.Equal(48, items.Count);
        Assert.Contains(items, item => item.Parameters["a"] == 0);
        Assert.Contains(items, item => item.Parameters["b"] == 0);
        foreach (var item in items)
        {
            var p = item.Parameters;
            Assert.True(p["a"] != 0 || p["b"] != 0);
            var expected = (long)p["a"] * p["x"] + (long)p["b"] * p["y"] == p["c"];
            Assert.Equal(expected ? "yes" : "no", item.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family, p, item.CorrectAnswer));
        }
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null &&
               !File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
            ?? throw new InvalidOperationException("Repository root not found.");
    }
}
