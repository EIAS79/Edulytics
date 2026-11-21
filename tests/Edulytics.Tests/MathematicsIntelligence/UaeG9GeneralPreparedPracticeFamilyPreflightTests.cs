using System.Text.Json;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Diagnostic only. The 80 G9 General Arabic digital contract definitions
/// remain PREPARED and excluded from the runtime READY_VERIFIED registry.
/// An owner-approved Published content pack does NOT certify math grading.
/// For every Grade9 mapping, test every proposed family with actual generation,
/// independent acceptance and rejection, and Arabic learner prompts.
/// This is a necessary but not sufficient Stage D gate.
/// </summary>
public sealed class UaeG9GeneralPreparedPracticeFamilyPreflightTests
{
    private const string Prefix = "PED:UAE-MOE-MATH:L9:GENERAL:";

    [Fact]
    public void Every_proposed_Grade9_digital_family_generates_correctly_graded_Arabic_question()
    {
        var root = FindRepo();
        using var data = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root, "src", "Edulytics.Core", "Mathematics", "Curriculum",
            "lesson-skill-mappings.v1.json")));
        var mappings = data.RootElement.GetProperty("mappings")
            .EnumerateArray()
            .Where(row => row.GetProperty("lessonCode").GetString()!
                .StartsWith(Prefix, StringComparison.Ordinal))
            .ToArray();
        Assert.Equal(82, mappings.Length);

        var engine = new ExactSkillContractQuestionEngine();
        var failures = new List<string>();
        var checkedPairs = 0;
        var teacherOnly = 0;
        var digital = 0;

        foreach (var row in mappings.OrderBy(x =>
            x.GetProperty("lessonCode").GetString(), StringComparer.Ordinal))
        {
            var code = row.GetProperty("lessonCode").GetString()!;
            var families = row.GetProperty("allowedQuestionFamilies")
                .EnumerateArray().Select(x => x.GetString()!).ToArray();
            if (families.Length == 0)
            {
                teacherOnly++;
                continue;
            }
            digital++;
            if (families.Distinct(StringComparer.Ordinal).Count() != families.Length)
            {
                failures.Add($"{code}: duplicate family mappings");
                continue;
            }

            foreach (var family in families)
            {
                checkedPairs++;
                try
                {
                    if (!ExactSkillContractQuestionEngine.SupportsFamily(family))
                    {
                        failures.Add($"{code} / {family}: UNSUPPORTED_FAMILY");
                        continue;
                    }
                    var generated = engine.Generate(
                        "UAE-G9-GENERAL", code, [family],
                        ExactSkillQuestionDifficulty.Standard, 1,
                        unchecked(20261121 + checkedPairs * 73),
                        Array.Empty<string>());
                    if (generated.Count != 1 || generated[0].Family != family)
                    {
                        failures.Add($"{code} / {family}: NO_EXACT_QUESTION");
                        continue;
                    }
                    var item = generated[0];
                    if (!item.Prompt.Any(ch => ch is >= '\u0600' and <= '\u06ff'))
                        failures.Add($"{code} / {family}: NOT_ARABIC prompt=" +
                            item.Prompt.Replace('\n', ' ')[..Math.Min(140, item.Prompt.Length)] +
                            " params=" + JsonSerializer.Serialize(item.Parameters));
                    if (!ExactSkillContractQuestionEngine.Verify(
                        family, item.Parameters, item.CorrectAnswer))
                        failures.Add($"{code} / {family}: CORRECT_ANSWER_REJECTED");
                    if (ExactSkillContractQuestionEngine.Verify(
                        family, item.Parameters, "-999"))
                        failures.Add($"{code} / {family}: INCORRECT_ANSWER_ACCEPTED");
                }
                catch (Exception ex)
                {
                    failures.Add($"{code} / {family}: " +
                        $"{ex.GetType().Name}: {ex.Message}");
                }
            }
        }

        Assert.Equal(80, digital);
        Assert.Equal(2, teacherOnly);
        Assert.True(checkedPairs > 80,
            "A single hard-coded question per Grade9 lesson is insufficient.");
        Assert.True(failures.Count == 0,
            $"G9 Stage D proposed digital family readiness BLOCKED: " +
            $"digital={digital}, teacherOnly={teacherOnly}, " +
            $"checkedFamilies={checkedPairs}, failures={failures.Count}; " +
            string.Join(" | ", failures.Take(65)));
    }

    private static string FindRepo()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;
            directory = directory.Parent;
        }
        throw new InvalidOperationException("Cannot locate Edulytics repository root.");
    }
}
