using System.Text.Json;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Evidence inventory: verifies real generator and composed learner Practice for
/// all 76 Grade 8 General Arabic contracts. This does NOT assert full
/// lesson-to-question semantic alignment; the teacher must review each sample.
/// </summary>
public sealed class UaeGrade8GeneralFullPracticeSamplingTests
{
    private const string Prefix = "PED:UAE-MOE-MATH:L8:GENERAL:";

    [Fact]
    public void InventoryEveryActualLearnerPracticeFamilyWithoutClaimingFullAcademicSignoff()
    {
        var root = FindRoot();
        var auditPath = Path.Combine(root, "artifacts", "curriculum-delivery",
            "UAE-MOE-MATH", "G8", "general-practice-semantic-audit.json");
        using var document = JsonDocument.Parse(File.ReadAllText(auditPath));
        var sourceRows = document.RootElement.GetProperty("lessons").EnumerateArray()
            .ToArray();
        Assert.Equal(76, sourceRows.Length);

        var contracts = LessonPracticeContractRegistry.All
            .Where(x => x.LessonCode.StartsWith(Prefix, StringComparison.Ordinal))
            .OrderBy(x => x.LessonCode, StringComparer.Ordinal).ToArray();
        Assert.Equal(76, contracts.Length);
        var engine = new ExactSkillContractQuestionEngine();
        var samples = new List<object>();

        for (var i = 0; i < sourceRows.Length; i++)
        {
            var source = sourceRows[i];
            var code = source.GetProperty("lessonCode").GetString()!;
            var contract = Assert.Single(contracts, c => c.LessonCode == code);
            var expected = source.GetProperty("allowedFamilies").EnumerateArray()
                .Select(x => x.GetString()!).ToArray();
            Assert.Equal(expected, contract.AllowedQuestionFamilies);

            var families = new List<object>();
            for (var j = 0; j < expected.Length; j++)
            {
                var family = expected[j];
                Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family),
                    $"Unsupported: {code}/{family}");
                var questions = engine.Generate(code, family, [family],
                    ExactSkillQuestionDifficulty.Standard, 2,
                    94137 + i * 37 + j * 103, []);
                Assert.Equal(2, questions.Count);
                foreach (var q in questions)
                {
                    Assert.False(string.IsNullOrWhiteSpace(q.Prompt));
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        q.Family, q.Parameters, q.CorrectAnswer));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        q.Family, q.Parameters, "not-an-answer"));
                }
                families.Add(new
                {
                    family,
                    samples = questions.Select(q => new
                    {
                        q.Prompt,
                        q.Solution,
                        q.CorrectAnswer,
                        q.Parameters,
                        q.ExposureFingerprint
                    }).ToArray()
                });
            }

            var studentPractice = new Stage18SkillContractPracticeEngine().GenerateComposed(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                contract.ToLegacyStage18Contract(),
                StudentPrivatePracticeDifficulty.MyLevel,
                3, 20261118 + i * 101, [], Guid.NewGuid());
            Assert.Equal(3, studentPractice.Count);
            samples.Add(new
            {
                ordinal = source.GetProperty("ordinal").GetInt32(),
                lessonCode = code,
                arabicTitle = source.GetProperty("arabicTitle").GetString(),
                targetMechanic = source.GetProperty("mechanic").GetString(),
                semanticAuditStatus = source.GetProperty("semanticAuditStatus").GetString(),
                sourceLocator = source.GetProperty("sourceLocator").GetString(),
                familyQuestions = families,
                actualLearnerPractice = studentPractice.Select(q => new
                {
                    q.Prompt,
                    q.CorrectAnswer,
                    q.GenerationFamily
                }).ToArray()
            });
        }

        var output = Path.Combine(root, "artifacts", "curriculum-delivery",
            "UAE-MOE-MATH", "G8", "general-practice-actual-learner-samples.json");
        File.WriteAllText(output, JsonSerializer.Serialize(new
        {
            schemaVersion = 1,
            lessonCount = samples.Count,
            purpose = "Generated question and actual composed student Practice evidence; objective-to-question alignment requires separate teacher review",
            academicSignoff = false,
            lessons = samples
        }, new JsonSerializerOptions { WriteIndented = true }) + "\n");
    }

    private static string FindRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "Edulytics.sln")))
                return dir.FullName;
            dir = dir.Parent;
        }
        throw new DirectoryNotFoundException("Edulytics solution root not found.");
    }
}
