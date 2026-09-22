using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class PolishRichLessonLocalizationAuditTests
{
    [Fact]
    public void EmitVerifiedPolishQuestionFamilyCorpus()
    {
        var polishLessonCodes = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .Where(document =>
                string.Equals(
                    document.PackCode,
                    "PL-NATIONAL-MATH",
                    StringComparison.Ordinal))
            .SelectMany(document => document.Lessons)
            .Select(lesson => lesson.LessonCode)
            .ToHashSet(StringComparer.Ordinal);

        var families = LessonPracticeContractRegistry.All
            .Where(contract =>
                polishLessonCodes.Contains(contract.LessonCode) &&
                string.Equals(
                    contract.Readiness,
                    LessonPracticeCapabilityResolver.ReadyVerified,
                    StringComparison.Ordinal))
            .SelectMany(contract => contract.AllowedQuestionFamilies)
            .Distinct(StringComparer.Ordinal)
            .OrderBy(value => value, StringComparer.Ordinal)
            .ToArray();

        var engine = new ExactSkillContractQuestionEngine();
        var rows = new List<object>();

        foreach (var family in families)
        {
            Assert.True(
                ExactSkillContractQuestionEngine.SupportsFamily(family),
                $"Polish Practice family must remain exact-generation ready: {family}");

            var generated = engine.Generate(
                "rich-v2-polish-localization-audit",
                family,
                [family],
                ExactSkillQuestionDifficulty.Standard,
                1,
                StableSeed(family),
                [],
                0);

            var question = Assert.Single(generated);
            rows.Add(new
            {
                family,
                question.Prompt,
                question.Solution,
                question.CorrectAnswer,
                question.ItemType,
                question.Parameters
            });
        }

        Assert.Equal(138, rows.Count);

        var outputDirectory = Path.Combine(
            FindRoot(),
            "artifacts",
            "lesson-content-v2");
        Directory.CreateDirectory(outputDirectory);

        File.WriteAllText(
            Path.Combine(
                outputDirectory,
                "polish-question-family-corpus.json"),
            JsonSerializer.Serialize(
                new
                {
                    schemaVersion = 1,
                    familyCount = rows.Count,
                    rows
                },
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }));
    }

    private static int StableSeed(string value)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        var seed = BitConverter.ToInt32(hash, 0);
        return seed == 0 ? 1 : seed;
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(
                    Path.Combine(
                        directory.FullName,
                        "Edulytics.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException(
            "Edulytics solution root not found.");
    }
}
