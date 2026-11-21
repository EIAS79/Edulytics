using System.Text.Json;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Stage D diagnostic only; verifies 80 prepared G9 General lesson mappings
/// through the actual Stage18 composed and progressive generators, quality
/// gates, and persisted-item independent verification. Does not register
/// PREPARED contracts as publicly READY_VERIFIED and does not access Neon.
/// </summary>
public sealed class UaeG9GeneralPreparedPracticeSessionGateTests
{
    private const string Prefix = "PED:UAE-MOE-MATH:L9:GENERAL:";

    [Fact]
    public void All_80_proposed_digital_lessons_create_Arabic_quality_checked_8_item_sessions()
    {
        var root = FindRepo();
        using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(root,
            "src", "Edulytics.Core", "Mathematics", "Curriculum",
            "lesson-skill-mappings.v1.json")));
        var rows = document.RootElement.GetProperty("mappings")
            .EnumerateArray().Where(x => x.GetProperty("lessonCode").GetString()!
                .StartsWith(Prefix, StringComparison.Ordinal))
            .OrderBy(x => x.GetProperty("lessonCode").GetString(), StringComparer.Ordinal)
            .ToArray();
        Assert.Equal(82, rows.Length);

        var failures = new List<string>();
        var engine = new Stage18SkillContractPracticeEngine();
        var validator = new PracticeSessionQualityValidator();
        var sessions = 0;
        var examples = 0;
        var teacherOnly = 0;
        var index = 0;
        foreach (var entry in rows)
        {
            index++;
            var lessonCode = entry.GetProperty("lessonCode").GetString()!;
            var families = entry.GetProperty("allowedQuestionFamilies")
                .EnumerateArray().Select(x => x.GetString()!).ToArray();
            if (families.Length == 0)
            {
                teacherOnly++;
                continue;
            }
            var primarySkill = entry.GetProperty("primarySkills")
                .EnumerateArray().First().GetString()!;
            var mechanic = entry.GetProperty("practiceMechanic").GetString()!;
            var proposed = new Stage18PracticeSkillContract(
                lessonCode, primarySkill, mechanic, families);
            var schoolId = Guid.Parse("b1000000-0000-4000-8000-000000000001");
            var adoptionId = Guid.Parse("b1000000-0000-4000-8000-000000000002");
            var lessonId = Guid.Parse("b1000000-0000-4000-8000-000000000003");
            var studentId = Guid.Parse("b1000000-0000-4000-8000-000000000004");

            foreach (var progressive in new[] { false, true })
            {
                try
                {
                    var seed = unchecked(20261121 + index * 541 + (progressive ? 10000 : 0));
                    var items = progressive
                        ? engine.GenerateProgressiveLesson(schoolId, adoptionId, lessonId,
                            proposed, seed, [], studentId)
                        : engine.GenerateComposed(schoolId, adoptionId, lessonId, proposed,
                            StudentPrivatePracticeDifficulty.MyLevel, 8,
                            seed, [], studentId);
                    var quality = validator.Validate(proposed, items, 8);
                    if (!quality.IsReady)
                    {
                        failures.Add($"{lessonCode} {(progressive ? "progressive" : "standard")}: " +
                            $"{quality.StatusCode} [{string.Join(",", quality.ReasonCodes)}]");
                        continue;
                    }
                    foreach (var item in items)
                    {
                        if (!item.Prompt.Any(ch => ch is >= '\u0600' and <= '\u06ff'))
                            failures.Add($"{lessonCode}: item not Arabic: {item.GenerationFamily} / {item.Prompt}");
                        if (!Stage18SkillContractPracticeEngine.VerifyPersistedItem(proposed, item))
                            failures.Add($"{lessonCode}: stored-item independent verification failed: {item.GenerationFamily}");
                        examples++;
                    }
                    sessions++;
                }
                catch (Exception e)
                {
                    failures.Add($"{lessonCode} {(progressive ? "progressive" : "standard")}: " +
                        $"{e.GetType().Name}: {e.Message}");
                }
            }
        }
        Assert.Equal(2, teacherOnly);
        Assert.True(failures.Count == 0,
            $"Grade9 80-lesson Stage18 Arabic session release PREPARED/NOT READY: " +
            $"sessions={sessions}/160 items={examples}, blockers={failures.Count}. " +
            string.Join(" | ", failures.Take(75)));
        Assert.Equal(160, sessions);
        Assert.True(examples >= 480, "Too few Grade9 unique verified items.");
    }

    private static string FindRepo()
    {
        var current = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "Edulytics.sln")))
                return current.FullName;
            current = current.Parent;
        }
        throw new InvalidOperationException("Edulytics.sln not found.");
    }
}
