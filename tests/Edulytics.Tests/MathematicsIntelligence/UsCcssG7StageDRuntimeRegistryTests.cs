using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage D: actual embedded Rich V2 and practice routing of all 145 G7 lessons.</summary>
public sealed class UsCcssG7StageDRuntimeRegistryTests
{
    private const string Prefix = "PED:US-CCSS-MATH:G7:U";
    private static readonly int[] Counts = [13,15,11,16,17,23,17,20,13];

    [Fact]
    public void All145LessonsResolveFromEmbeddedRuntimeAndHaveSupportedPracticeFamilies()
    {
        var codes = Enumerable.Range(1, Counts.Length)
            .SelectMany(u => Enumerable.Range(1, Counts[u-1])
                .Select(l => $"{Prefix}{u:00}:L{l:00}"))
            .ToArray();
        Assert.Equal(145, codes.Length);
        var unique = new HashSet<string>(StringComparer.Ordinal);
        foreach (var code in codes)
        {
            Assert.True(unique.Add(code), "Duplicate code: " + code);
            var rich = RichLessonContentV2Registry.Find(code, "en");
            Assert.NotNull(rich);
            Assert.Equal(code, rich.LessonCode);
            Assert.NotEmpty(rich.WorkedExamples);
            Assert.NotEmpty(rich.Visuals);
            Assert.All(rich.WorkedExamples, w =>
            {
                Assert.False(string.IsNullOrWhiteSpace(w.Question));
                Assert.False(string.IsNullOrWhiteSpace(w.Answer));
                Assert.False(string.IsNullOrWhiteSpace(w.Check));
            });
            Assert.True(LessonPracticeContractRegistry.TryResolve(code, out var contract),
                "Missing Practice runtime route: " + code);
            Assert.NotNull(contract);
            Assert.NotEmpty(contract.AllowedQuestionFamilies);
            Assert.All(contract.AllowedQuestionFamilies, family =>
                Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family),
                    "No engine for " + code + " -> " + family));
        }
    }
}