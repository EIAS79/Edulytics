using System.Reflection;
using System.Text.Json;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.Phase29;

public sealed class UaeOfficialBookReferenceMappingTests
{
    private static readonly string[] ExpectedLessonCodes =
    [
        "PED:UAE-MOE-MATH:L3:COMMON:01:02:ROUNDING-AND-ESTIMATING",
        "PED:UAE-MOE-MATH:L3:COMMON:01:08:WRITTEN-DIVISION-FOUNDATIONS",
        "PED:UAE-MOE-MATH:L3:COMMON:04:01:ANGLES-AS-TURNS",
        "PED:UAE-MOE-MATH:L3:COMMON:04:03:TRIANGLES-AND-QUADRILATERALS",
        "PED:UAE-MOE-MATH:L4:COMMON:01:06:MULTIPLICATION-BY-ONE-DIGIT-NUMBERS",
        "PED:UAE-MOE-MATH:L4:COMMON:01:07:DIVISION-WITH-REMAINDERS",
        "PED:UAE-MOE-MATH:L4:COMMON:04:03:QUADRILATERALS",
        "PED:UAE-MOE-MATH:L4:COMMON:04:05:LINE-SYMMETRY",
        "PED:UAE-MOE-MATH:L4:COMMON:05:02:LINE-GRAPHS",
        "PED:UAE-MOE-MATH:L11:GENERAL:01:05:RATIO-AND-PROPORTION",
        "PED:UAE-MOE-MATH:L11:GENERAL:02:01:ALGEBRAIC-EXPRESSIONS",
        "PED:UAE-MOE-MATH:L11:GENERAL:02:05:LINEAR-EQUATIONS",
        "PED:UAE-MOE-MATH:L11:GENERAL:02:06:INEQUALITIES",
        "PED:UAE-MOE-MATH:L11:GENERAL:02:09:SIMULTANEOUS-EQUATIONS",
        "PED:UAE-MOE-MATH:L11:GENERAL:03:06:SURFACE-AREA-AND-VOLUME",
        "PED:UAE-MOE-MATH:L11:GENERAL:03:07:PYTHAGORAS-THEOREM",
        "PED:UAE-MOE-MATH:L11:GENERAL:03:09:TRIGONOMETRIC-RATIOS",
        "PED:UAE-MOE-MATH:L11:ADVANCED:05:09:RATES-OF-CHANGE"
    ];

    [Fact]
    public void Verified_book_references_are_exact_formal_targets()
    {
        var lessons = PedagogicalLessonBlueprintRegistry
            .LoadEmbeddedDocuments()
            .Where(x => x.PackCode == MathematicsCurriculumPackRegistry.UaeCode)
            .SelectMany(x => x.Lessons)
            .ToDictionary(x => x.LessonCode, StringComparer.Ordinal);

        foreach (var lessonCode in ExpectedLessonCodes)
        {
            var lesson = lessons[lessonCode];
            var outcome = Assert.Single(lesson.OutcomeCodes);
            Assert.StartsWith("UAE:REF:BOOK:", outcome, StringComparison.Ordinal);

            var alignment = Assert.Single(lesson.Alignments);
            Assert.Equal("Addressing", alignment.Role);
            Assert.Equal("OfficialReference", alignment.ReferenceKind);
            Assert.Equal("ExactAcceptedReference", alignment.ResolutionKind);
            Assert.Equal(outcome, alignment.ReferenceCode);
            Assert.Equal(outcome, alignment.OutcomeCode);
        }
    }

    [Fact]
    public void Verified_book_references_are_official_reference_nodes()
    {
        var assembly = typeof(MathematicsCurriculumPackRegistry).Assembly;
        var resourceName = assembly.GetManifestResourceNames().Single(x =>
            x.EndsWith(
                "uae-moe-math.curriculum-pack.json",
                StringComparison.OrdinalIgnoreCase));

        using var stream = assembly.GetManifestResourceStream(resourceName)!;
        using var json = JsonDocument.Parse(stream);

        var refs = json.RootElement
            .GetProperty("Nodes")
            .EnumerateArray()
            .Where(x =>
                x.GetProperty("Code").GetString()!
                    .StartsWith("UAE:REF:BOOK:", StringComparison.Ordinal))
            .ToArray();

        Assert.Equal(18, refs.Length);
        Assert.All(refs, node =>
        {
            Assert.Equal("Reference", node.GetProperty("Kind").GetString());
            Assert.True(node.GetProperty("IsOfficial").GetBoolean());
            Assert.True(node.GetProperty("IsActive").GetBoolean());
            Assert.Equal(
                "UAE Ministry of Education",
                node.GetProperty("SourceAuthority").GetString());
            Assert.Equal(JsonValueKind.Null, node.GetProperty("OfficialText").ValueKind);
            Assert.Contains(
                "supplied PDF SHA256",
                node.GetProperty("SourceLocator").GetString(),
                StringComparison.Ordinal);
        });
    }

    [Fact]
    public void Promoted_lessons_are_not_supporting_and_keep_verified_practice()
    {
        var documents = LoadCanonicalContentDocuments();
        var lessons = documents
            .Where(x => x.PackCode == MathematicsCurriculumPackRegistry.UaeCode)
            .SelectMany(x => x.Lessons)
            .ToDictionary(x => x.LessonCode, StringComparer.Ordinal);

        foreach (var lessonCode in ExpectedLessonCodes)
        {
            var lesson = lessons[lessonCode];
            Assert.False(lesson.IsSupporting);
            Assert.Single(lesson.OutcomeCodes);
            Assert.StartsWith(
                "UAE:REF:BOOK:",
                lesson.OutcomeCodes[0],
                StringComparison.Ordinal);

            Assert.True(
                LessonPracticeContractRegistry.TryResolve(
                    lessonCode,
                    out var practice),
                $"Practice contract disappeared after official mapping: {lessonCode}");
            Assert.NotNull(practice);
            Assert.Equal("READY_VERIFIED", practice!.Readiness);
            Assert.NotEmpty(practice.AllowedQuestionFamilies);
        }
    }

    private static IReadOnlyList<CanonicalLessonContentPackDocument>
        LoadCanonicalContentDocuments()
    {
        var assembly = typeof(CanonicalLessonContentPackDocument).Assembly;
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return assembly
            .GetManifestResourceNames()
            .Where(x => x.EndsWith(
                ".lesson-content-pack.json",
                StringComparison.OrdinalIgnoreCase))
            .Select(name =>
            {
                using var stream = assembly.GetManifestResourceStream(name)!;
                return JsonSerializer.Deserialize<CanonicalLessonContentPackDocument>(
                    stream,
                    options)!;
            })
            .ToArray();
    }
}
