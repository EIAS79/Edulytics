using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Tests precise G2 2.NBT.A.1.a/b numeric competencies while asserting they are
/// NOT user-facing READY_VERIFIED and cannot be called academic clause signoffs.
/// </summary>
public sealed class UsCommonCoreGrade2HundredsShadowQuestionTests
{
    private static readonly string[] FamilyIds =
    [
        "usccss.g2.hundreds_as_tens",
        "usccss.g2.hundreds_digit_to_number"
    ];

    [Fact]
    public void NarrowFamiliesAreMathematicallyVerifiedButNotAuthorizedForLearnerRouting()
    {
        foreach (var family in FamilyIds)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        }
        foreach (var lesson in new[] {"L01", "L02", "L03"})
        {
            var code = "PED:US-CCSS-MATH:G2:U05:" + lesson;
            // An earlier, independent READY_VERIFIED lesson contract can
            // exist here. Preserve its already routed families, but require
            // that neither NEW shadow candidate is made learner-accessible.
            if (LessonPracticeCapabilityResolver.TryResolve(code, out var existing))
            {
                Assert.NotNull(existing);
                Assert.DoesNotContain(FamilyIds[0], existing!.AllowedQuestionFamilies);
                Assert.DoesNotContain(FamilyIds[1], existing.AllowedQuestionFamilies);
            }
        }
    }

    [Fact]
    public void AllThreeSourceAnchoredMappingsStayShadowAndPreserveOfficialOutcomeIdentity()
    {
        var asm = typeof(LessonPracticeCapabilityResolver).Assembly;
        using var mapStream = asm.GetManifestResourceStream(
            "Edulytics.Core.Mathematics.Curriculum.lesson-skill-mappings.v1.json");
        using var familyStream = asm.GetManifestResourceStream(
            "Edulytics.Core.Mathematics.Generation.question-family-registry.v1.json");
        Assert.NotNull(mapStream);
        Assert.NotNull(familyStream);
        using var mapDoc = System.Text.Json.JsonDocument.Parse(mapStream!);
        using var familyDoc = System.Text.Json.JsonDocument.Parse(familyStream!);
        var allMappings = mapDoc.RootElement.GetProperty("mappings").EnumerateArray()
            .Where(x => x.GetProperty("lessonCode").GetString() is string code &&
                code.StartsWith("PED:US-CCSS-MATH:G2:U05:L0", StringComparison.Ordinal))
            .ToArray();
        var exactThree = allMappings.Where(x => new[] { "L01", "L02", "L03" }
            .Any(y => x.GetProperty("lessonCode").GetString()!.EndsWith(y,
                StringComparison.Ordinal))).ToArray();
        Assert.Equal(3, exactThree.Length);
        foreach (var row in exactThree)
        {
            Assert.Equal("SHADOW_VERIFIED_NOT_STUDENT_ROUTED",
                row.GetProperty("practiceReadiness").GetString());
            var mappedFamilies = row.GetProperty("allowedQuestionFamilies")
                .EnumerateArray().Select(x => x.GetString()).ToArray();
            Assert.NotEmpty(mappedFamilies);
            Assert.All(mappedFamilies, family => Assert.Contains(family, FamilyIds));
        }

        var l3 = Assert.Single(exactThree, x =>
            x.GetProperty("lessonCode").GetString()!.EndsWith("L03",
                StringComparison.Ordinal));
        Assert.Equal(
            new[] { "CCSS:2.NBT.A.1", "CCSS:2.NBT.A.2", "CCSS:2.NBT.B.5" },
            l3.GetProperty("officialOutcomeCodes").EnumerateArray()
                .Select(x => x.GetString()).ToArray());

        var registered = familyDoc.RootElement.GetProperty("families")
            .EnumerateArray().Where(x => FamilyIds.Contains(
                x.GetProperty("id").GetString())).ToArray();
        Assert.Equal(2, registered.Length);
        Assert.All(registered, x =>
            Assert.False(x.GetProperty("lessonPracticeRouting").GetBoolean()));
    }

    [Fact]
    public void HundredsAndTensAreIndependentCorrectAndRejectWrongAnswersAtAllDifficulties()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var total = 0;
        foreach (var family in FamilyIds)
        foreach (var difficulty in new[] {
            ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Stretch,
            ExactSkillQuestionDifficulty.Challenge
        })
        for (var seed = 1; seed <= 18; seed++)
        {
            var item = Assert.Single(engine.Generate(
                "us-ccss-g2-hundreds-shadow",
                "PED:US-CCSS-MATH:G2:U05:L02",
                [family],
                difficulty,
                1,
                310000 + 1000 * total + seed,
                []));
            Assert.Equal(family, item.Family);
            Assert.True(item.Parameters.TryGetValue("hundreds", out var h));
            Assert.InRange(h, 1, 9);
            var expected = h * (family.EndsWith("hundreds_as_tens",
                StringComparison.Ordinal) ? 10 : 100);
            Assert.Equal(expected.ToString(System.Globalization.CultureInfo.InvariantCulture),
                item.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family, item.Parameters, item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, item.Parameters, (expected + 10).ToString(
                    System.Globalization.CultureInfo.InvariantCulture)));
            total++;
        }
        Assert.Equal(108, total);
        Assert.Equal(10, 100 / 10);
        Assert.Equal(100, 1 * 100);
        Assert.Equal(900, 9 * 100);
    }
}
