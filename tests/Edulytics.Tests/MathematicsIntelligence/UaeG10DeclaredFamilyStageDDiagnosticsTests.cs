using System.Globalization;
using System.Text.Json;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

// Stage D.2 diagnostics over the actual mapping registry; no production
// routing, independent source-oracle or real HTTP acceptance claim.
public sealed class UaeG10DeclaredFamilyStageDDiagnosticsTests
{
    private const string Prefix = "PED:UAE-MOE-MATH:L10:GENERAL:";
    private static readonly Lazy<JsonElement[]> Rows = new(() =>
    {
        using var stream = typeof(PedagogicalLessonBlueprintRegistry).Assembly
            .GetManifestResourceStream("Edulytics.Core.Mathematics.Curriculum.lesson-skill-mappings.v1.json")
            ?? throw new InvalidOperationException("Missing embedded lesson mapping registry.");
        using var doc = JsonDocument.Parse(stream);
        return doc.RootElement.GetProperty("mappings").EnumerateArray()
            .Where(x => x.GetProperty("lessonCode").GetString()!.StartsWith(Prefix, StringComparison.Ordinal))
            .Select(x => x.Clone()).ToArray();
    });

    private static string[] Values(JsonElement row, string property) =>
        row.TryGetProperty(property, out var value) && value.ValueKind == JsonValueKind.Array
            ? value.EnumerateArray().Select(x => x.GetString()!).ToArray() : [];

    [Fact]
    public void CandidateInventoryIsExactAndNoTeacherOnlyLessonIsRelabeled()
    {
        var mappings = Rows.Value;
        var canonical = PedagogicalLessonBlueprintRegistry.LoadEmbeddedDocuments()
            .Single(x => x.PackCode == "UAE-MOE-MATH" && x.LogicalLevel == 10 && x.Pathway == "General");
        Assert.Equal(148, mappings.Length);
        Assert.True(canonical.Lessons.Select(x => x.LessonCode).ToHashSet(StringComparer.Ordinal)
            .SetEquals(mappings.Select(x => x.GetProperty("lessonCode").GetString()!)));
        Assert.Equal(148, mappings.Count(x => Values(x, "allowedQuestionFamilies").Length != 0));
        Assert.Equal(0, mappings.Count(x => Values(x, "allowedQuestionFamilies").Length == 0));
        Assert.Equal(691, mappings.Sum(x => Values(x, "allowedQuestionFamilies").Length));
        Assert.Equal(678, mappings.SelectMany(x => Values(x, "allowedQuestionFamilies")).Distinct(StringComparer.Ordinal).Count());
        foreach (var row in mappings)
        {
            var allowed = Values(row, "allowedQuestionFamilies");
            Assert.Equal(allowed.Length, allowed.Distinct(StringComparer.Ordinal).Count());
            Assert.Empty(allowed.Intersect(Values(row, "forbiddenQuestionFamilies"), StringComparer.Ordinal));
            if (allowed.Length == 0)
                Assert.NotEmpty(Values(row, "teacherReviewRequiredFor"));
        }
    }

    public static IEnumerable<object[]> DeclaredLessonFamilyPairs()
    {
        foreach (var row in Rows.Value)
        {
            string code = row.GetProperty("lessonCode").GetString()!;
            foreach (var family in Values(row, "allowedQuestionFamilies"))
                yield return [code, family];
        }
    }

    [Theory]
    [MemberData(nameof(DeclaredLessonFamilyPairs))]
    public void ExactFamilyDispatchRendererAlignmentAndSeedReplay(string lessonCode, string family)
    {
        var row = Rows.Value.Single(x => x.GetProperty("lessonCode").GetString() == lessonCode);
        var allowed = Values(row, "allowedQuestionFamilies");
        var skill = Values(row, "primarySkills").First();
        var mechanic = row.GetProperty("practiceMechanic").GetString();
        Assert.False(string.IsNullOrWhiteSpace(mechanic));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family), lessonCode + ":" + family);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var interaction), family);
        Assert.NotNull(interaction);
        var contract = new Stage18PracticeSkillContract(lessonCode, skill, mechanic!, allowed);
        var engine = new ExactSkillContractQuestionEngine();
        foreach (int seed in new[] { 20261127, 147 })
        {
            var task = Assert.Single(engine.Generate(lessonCode, lessonCode, [family],
                ExactSkillQuestionDifficulty.Standard, 1, seed, []));
            var replay = Assert.Single(engine.Generate(lessonCode, lessonCode, [family],
                ExactSkillQuestionDifficulty.Standard, 1, seed, []));
            Assert.Equal(family, task.Family);
            Assert.Equal(ExactSkillQuestionDifficulty.Standard, task.Difficulty);
            Assert.Equal(task.Family, replay.Family);
            Assert.Equal(task.Prompt, replay.Prompt);
            Assert.Equal(task.CorrectAnswer, replay.CorrectAnswer);
            Assert.Equal(task.ExposureFingerprint, replay.ExposureFingerprint);
            Assert.Equal(task.VariantId, replay.VariantId);
            Assert.Equal(task.Parameters.OrderBy(x => x.Key).ToArray(),
                replay.Parameters.OrderBy(x => x.Key).ToArray());
            Assert.False(string.IsNullOrWhiteSpace(task.Prompt));
            Assert.False(string.IsNullOrWhiteSpace(task.CorrectAnswer));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family, task.Parameters, task.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, task.Parameters, ""));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, task.Parameters, "G10_INVALID_RESPONSE"));
            Assert.True(LessonPracticeAlignmentValidator.Validate(contract, task).IsAligned);
            Assert.False(LessonPracticeAlignmentValidator.Validate(
                contract with { AllowedQuestionFamilies = ["G10_UNAUTHORIZED_FAMILY"] }, task).IsAligned);
            if (task.CorrectAnswer.Any(ch => ch is >= '0' and <= '9'))
            {
                var arabic = string.Concat(task.CorrectAnswer.Select(ch =>
                    ch is >= '0' and <= '9' ? (char)('٠' + ch - '0') : ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family, task.Parameters, arabic),
                    "Arabic digit representation rejected: " + lessonCode + ":" + family);
            }
            // Numerical negative control, not a substitute for source oracles.
            if (decimal.TryParse(task.CorrectAnswer, NumberStyles.Number,
                CultureInfo.InvariantCulture, out var numeric) && numeric < decimal.MaxValue - 1)
            {
                var wrong = (numeric + 1).ToString(CultureInfo.InvariantCulture);
                Assert.False(ExactSkillContractQuestionEngine.Verify(family, task.Parameters, wrong));
            }
        }
    }
}
