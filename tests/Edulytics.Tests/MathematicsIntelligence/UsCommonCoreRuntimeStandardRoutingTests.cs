using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCommonCoreRuntimeStandardRoutingTests
{

    [Fact]
    public void FiveAdditionalMultiOutcomeLessonsKeepTheirPrimaryAcademicQuestionScope()
    {
        var expected = new Dictionary<string, string[]>(StringComparer.Ordinal)
        {
            ["PED:US-CCSS-MATH:G6:U06:L10"] = ["supporting.algebra.expand", "supporting.algebra.simplify"],
            ["PED:US-CCSS-MATH:G6:U06:L11"] = ["supporting.algebra.expand", "supporting.algebra.simplify"],
            ["PED:US-CCSS-MATH:G6:U06:L15"] = ["usccss.algebra.exponent_product_value"],
            ["PED:US-CCSS-MATH:G6:U06:L19"] = ["supporting.functions.evaluate"],
            ["PED:US-CCSS-MATH:G7:U05:L08"] = ["usccss.number.signed_rate_displacement"]
        };
        foreach (var (lessonCode, families) in expected)
        {
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(lessonCode, out var contract));
            Assert.NotNull(contract);
            Assert.Equal(families, contract!.AllowedQuestionFamilies);
            Assert.DoesNotContain("supporting.logarithms.evaluate", contract.AllowedQuestionFamilies);
        }
    }

    [Fact]
    public void LessonsWithOneOfFourCorrectedContentOutcomesUseNarrowPracticeRouting()
    {
        var narrowByOutcome = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["CCSS:5.G.A.1"] = "usccss.geometry.",
            ["CCSS:5.NF.B.7"] = "usccss.fractions.",
            ["CCSS:6.EE.A.2"] = "usccss.algebra.",
            ["CCSS:7.NS.A.2"] = "usccss.number."
        };
        // Only lessons with independent publisher-title evidence for this exact
        // family are elevated. A secondary OutcomeCode alone does not prove
        // every family belongs in that particular lesson's Practice session.
        var preciseLessonCodes = new HashSet<string>(StringComparer.Ordinal);
        for (var lesson = 10; lesson <= 20; lesson++)
            preciseLessonCodes.Add($"PED:US-CCSS-MATH:G5:U03:L{lesson:00}");
        for (var lesson = 1; lesson <= 3; lesson++)
            preciseLessonCodes.Add($"PED:US-CCSS-MATH:G5:U07:L{lesson:00}");
        foreach (var lesson in new[] { 6, 14 })
            preciseLessonCodes.Add($"PED:US-CCSS-MATH:G6:U06:L{lesson:00}");
        preciseLessonCodes.Add("PED:US-CCSS-MATH:G7:U04:L05");
        foreach (var lesson in new[] { 9, 10, 11 })
            preciseLessonCodes.Add($"PED:US-CCSS-MATH:G7:U05:L{lesson:00}");

        var lessons = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(p => p.Lessons)
            .Where(l => l.LessonCode.StartsWith("PED:US-CCSS-MATH:", StringComparison.Ordinal))
            .GroupBy(l => l.LessonCode, StringComparer.Ordinal)
            .Select(g => g.First())
            .ToArray();
        var mismatched = new List<string>();
        var examined = 0;
        foreach (var lesson in lessons)
        {
            if (!preciseLessonCodes.Contains(lesson.LessonCode))
                continue;

            var matched = narrowByOutcome
                .Where(pair => lesson.OutcomeCodes.Contains(pair.Key))
                .ToArray();
            if (matched.Length == 0) continue;
            examined++;
            if (!LessonPracticeCapabilityResolver.TryResolve(lesson.LessonCode, out var contract) || contract is null)
            {
                mismatched.Add(lesson.LessonCode + ": no runtime contract");
                continue;
            }
            if (matched.All(pair => !contract.AllowedQuestionFamilies.Any(family => family.StartsWith(pair.Value, StringComparison.Ordinal))))
                mismatched.Add(lesson.LessonCode + ": "+string.Join(",",matched.Select(p=>p.Key))+
                    " routed to "+string.Join(",",contract.AllowedQuestionFamilies));
        }
        Assert.Equal(20, examined);
        Assert.True(mismatched.Count == 0,
            examined + " matching lessons; "+mismatched.Count+
            " missing grade-specific runtime family: "+string.Join(" | ", mismatched.Take(30)));
    }
}