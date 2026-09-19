using System.Text.Json;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class SupportingLessonPracticeR7R9ClosureTests
{
    [Fact]
    public void EveryReadyLessonContractUsesRegisteredQuestionFamilies()
    {
        var root = FindRoot();
        using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json")));

        var registered = document.RootElement
            .GetProperty("families")
            .EnumerateArray()
            .Select(x => x.GetProperty("id").GetString()!)
            .ToHashSet(StringComparer.Ordinal);

        Assert.NotEmpty(LessonPracticeContractRegistry.All);
        Assert.All(LessonPracticeContractRegistry.All, contract =>
        {
            Assert.Equal("READY_VERIFIED", contract.Readiness);
            Assert.NotEmpty(contract.AllowedQuestionFamilies);
            Assert.All(
                contract.AllowedQuestionFamilies,
                family => Assert.Contains(family, registered));
        });
    }

    [Fact]
    public void EveryReadyLessonContractGeneratesStructuredVerifiedAlignedQuestion()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var seed = 31000;

        foreach (var contract in LessonPracticeContractRegistry.All)
        {
            var question = Assert.Single(engine.Generate(
                "r7-r9-closure",
                contract.LessonCode,
                contract.AllowedQuestionFamilies,
                ExactSkillQuestionDifficulty.Stretch,
                1,
                seed++,
                []));

            Assert.Contains(
                contract.AllowedQuestionFamilies,
                family => string.Equals(family, question.Family, StringComparison.Ordinal));
            Assert.NotEmpty(question.Parameters);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                question.Family,
                question.Parameters,
                question.CorrectAnswer));
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
