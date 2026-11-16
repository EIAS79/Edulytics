using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Real engine-family preflight over all UAE Grade7 General published lessons.
/// Fails before authenticated E2E if any lesson contract names an unsupported family.</summary>
public sealed class UaeGrade7PracticeGenerationPreflightTests
{
    [Fact]
    public void All78ContractsUseRuntimeSupportedFamiliesAndGenerateAnExactVerifiableQuestion()
    {
        var lessons = PedagogicalLessonBlueprintRegistry.LoadEmbeddedDocuments()
            .Where(x => x.PackCode == MathematicsCurriculumPackRegistry.UaeCode &&
                        x.LogicalLevel == 7 && x.Pathway == "General")
            .SelectMany(x => x.Lessons).ToArray();
        Assert.Equal(78, lessons.Length);
        var engine = new ExactSkillContractQuestionEngine();
        var failures = new List<string>();
        for (var i = 0; i < lessons.Length; i++)
        {
            var code = lessons[i].LessonCode;
            if (!LessonPracticeCapabilityResolver.TryResolve(code, out var contract)
                || contract is null)
            {
                failures.Add(code + ": No READY_VERIFIED Practice contract");
                continue;
            }
            foreach (var family in contract.AllowedQuestionFamilies)
            {
                if (!ExactSkillContractQuestionEngine.SupportsFamily(family))
                    failures.Add(code + ": unsupported family: " + family);
            }
            if (contract.AllowedQuestionFamilies.Any(x =>
                !ExactSkillContractQuestionEngine.SupportsFamily(x)))
                continue;
            try
            {
                var question = Assert.Single(engine.Generate(
                    "uae-grade7-practice-stage-d", code, contract.AllowedQuestionFamilies,
                    ExactSkillQuestionDifficulty.Standard, 1, 702010 + i, []));
                if (!ExactSkillContractQuestionEngine.Verify(question.Family,
                        question.Parameters, question.CorrectAnswer))
                    failures.Add(code + ": independent answer verification failed");
            }
            catch (Exception e)
            {
                failures.Add(code + ": " + e.GetType().Name + ": " + e.Message);
            }
        }
        Assert.True(failures.Count == 0,
            "G7 Practice engine defects: " + string.Join(" | ", failures.Take(60)));
    }
}
