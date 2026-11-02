using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCommonCoreFullLessonPracticeAuditTests
{
    [Fact]
    public void All1560UsLessonsGenerateAndVerifyEachFamilyAcrossThreeDifficulties()
    {
        var lessons = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(document => document.Lessons)
            .Where(lesson => lesson.LessonCode.StartsWith("PED:US-CCSS-MATH:", StringComparison.Ordinal))
            .GroupBy(lesson => lesson.LessonCode, StringComparer.Ordinal)
            .Select(group => group.First())
            .OrderBy(lesson => lesson.LessonCode, StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(1560, lessons.Length);

        var failures = new List<string>();
        var engine = new ExactSkillContractQuestionEngine();
        var seed = 7300000;
        var generatedCases = 0;

        foreach (var lesson in lessons)
        {
            if (!LessonPracticeCapabilityResolver.TryResolve(lesson.LessonCode, out var contract) || contract is null)
            {
                failures.Add($"{lesson.LessonCode}: missing SkillContract");
                continue;
            }

            if (contract.AllowedQuestionFamilies.Count == 0)
            {
                failures.Add($"{lesson.LessonCode}: no allowed families");
                continue;
            }

            foreach (var family in contract.AllowedQuestionFamilies.Distinct(StringComparer.Ordinal))
            {
                foreach (var difficulty in new[]
                         {
                             ExactSkillQuestionDifficulty.Standard,
                             ExactSkillQuestionDifficulty.Stretch,
                             ExactSkillQuestionDifficulty.Challenge
                         })
                {
                    try
                    {
                        var question = Assert.Single(engine.Generate(
                            "us-ccss-all-families",
                            lesson.LessonCode,
                            [family],
                            difficulty,
                            1,
                            seed++,
                            []));

                        generatedCases++;
                        if (!StringComparer.Ordinal.Equals(question.Family, family))
                        {
                            failures.Add($"{lesson.LessonCode}: requested {family} but generated {question.Family}");
                        }

                        if (!ExactSkillContractQuestionEngine.Verify(
                                question.Family, question.Parameters, question.CorrectAnswer))
                        {
                            failures.Add($"{lesson.LessonCode}/{family}/{difficulty}: independent verification failed");
                        }
                    }
                    catch (Exception e)
                    {
                        failures.Add($"{lesson.LessonCode}/{family}/{difficulty}: {e.GetType().Name} {e.Message}");
                    }
                }
            }
        }

        Assert.True(generatedCases >= lessons.Length * 3,
            $"Expected three verified difficulty cases per lesson; executed {generatedCases} cases.");
        Assert.True(failures.Count == 0,
            $"{failures.Count} US Practice family failures: {string.Join(" | ", failures.Take(45))}");
    }
}