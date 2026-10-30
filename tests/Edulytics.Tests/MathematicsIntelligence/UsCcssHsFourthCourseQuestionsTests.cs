using Edulytics.Services.Mathematics;
using Edulytics.Core.Mathematics.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Deterministic service-kernel checks. Does not substitute for authenticated
/// two-school score persistence, academic review, or production readiness.
/// </summary>
public sealed class UsCcssHsFourthCourseQuestionsTests
{
    private static readonly string[] Families =
    [
        UsCcssHsFourthCourseQuestions.MatrixCoefficients,
        UsCcssHsFourthCourseQuestions.MatrixSolve,
        UsCcssHsFourthCourseQuestions.MatrixInverse,
        UsCcssHsFourthCourseQuestions.MatrixClassification,
        UsCcssHsFourthCourseQuestions.LogEvaluate,
        UsCcssHsFourthCourseQuestions.LogCompare,
        UsCcssHsFourthCourseQuestions.TrigReflection,
        UsCcssHsFourthCourseQuestions.TrigPrincipalInverse,
        UsCcssHsFourthCourseQuestions.TrigModel,
        UsCcssHsFourthCourseQuestions.CavalieriVolume,
        UsCcssHsFourthCourseQuestions.FocalConics
    ];

    [Fact]
    public void AllElevenConcreteFamiliesGenerateAndVerifyBoundedQuestions()
    {
        foreach (var family in Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            for (var seed = 1; seed <= 15; seed++)
            {
                var item = UsCcssHsFourthCourseQuestions.Build(family, new Random(seed));
                var answer = UsCcssHsFourthCourseQuestions.Solve(item.Family, item.Parameters);
                Assert.True(UsCcssHsFourthCourseQuestions.Verify(item.Family, item.Parameters, answer));
                Assert.True(ExactSkillContractQuestionEngine.Verify(item.Family, item.Parameters, answer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(item.Family, item.Parameters, "1+1"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(item.Family, item.Parameters, "<script>"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(item.Family, item.Parameters, new string('9', 129)));
            }
        }
    }

    [Fact]
    public void MatrixClassificationSeparatesUniqueInfiniteAndInconsistentSystems()
    {
        for (var seed = 0; seed < 40; seed++)
        {
            var p = UsCcssHsFourthCourseQuestions.Build(
                UsCcssHsFourthCourseQuestions.MatrixClassification,
                new Random(seed)).Parameters;
            var answer = UsCcssHsFourthCourseQuestions.Solve(
                UsCcssHsFourthCourseQuestions.MatrixClassification, p);
            Assert.Equal(p["mode"] switch { 0 => "unique", 1 => "infinite", _ => "none" }, answer);
            Assert.False(UsCcssHsFourthCourseQuestions.Verify(
                UsCcssHsFourthCourseQuestions.MatrixClassification, p, "invalid"));
        }
    }

    [Fact]
    public void MatrixInverseAndSolutionPermitEquivalentFractionsAndRejectWrongDimension()
    {
        var p = new Dictionary<string, int>
        {
            ["a"] = 2, ["b"] = 1, ["c"] = 1, ["d"] = 1,
            ["e"] = 5, ["f"] = 3, ["mode"] = 0
        };
        Assert.Equal("2,1", UsCcssHsFourthCourseQuestions.Solve(UsCcssHsFourthCourseQuestions.MatrixSolve, p));
        Assert.True(UsCcssHsFourthCourseQuestions.Verify(UsCcssHsFourthCourseQuestions.MatrixSolve, p, "4/2,2/2"));
        Assert.False(UsCcssHsFourthCourseQuestions.Verify(UsCcssHsFourthCourseQuestions.MatrixSolve, p, "2,1,0"));
        Assert.True(UsCcssHsFourthCourseQuestions.Verify(UsCcssHsFourthCourseQuestions.MatrixInverse, p, "1,-1;-1,2"));
        Assert.False(UsCcssHsFourthCourseQuestions.Verify(UsCcssHsFourthCourseQuestions.MatrixInverse, p, "1,-1,-1,2"));
        Assert.False(UsCcssHsFourthCourseQuestions.Verify(UsCcssHsFourthCourseQuestions.MatrixInverse, p, "1,0;0,1"));
        var singular = new Dictionary<string, int>(p) { ["a"] = 2, ["b"] = 2, ["c"] = 4, ["d"] = 4 };
        Assert.False(UsCcssHsFourthCourseQuestions.Verify(UsCcssHsFourthCourseQuestions.MatrixInverse, singular, "1,0;0,1"));
    }

    [Fact]
    public void LogComparisonReversesBelowBaseOneAndRejectsOpenJustificationAutograding()
    {
        var p = new Dictionary<string, int> { ["decreasing"] = 1, ["low"] = 3, ["high"] = 5 };
        Assert.Equal(">", UsCcssHsFourthCourseQuestions.Solve(UsCcssHsFourthCourseQuestions.LogCompare, p));
        Assert.False(UsCcssHsFourthCourseQuestions.Verify(UsCcssHsFourthCourseQuestions.LogCompare, p, "<"));
        Assert.False(UsCcssHsFourthCourseQuestions.Verify(UsCcssHsFourthCourseQuestions.LogCompare, p, "because it decreases"));
    }

    [Fact]
    public void FiveTrigonometryGeometryFamiliesUseExactTypedAnswers()
    {
        var model = new Dictionary<string, int> { ["period"] = 12, ["amplitude"] = 3, ["offset"] = 2 };
        Assert.Equal("0,6,12", UsCcssHsFourthCourseQuestions.Solve(UsCcssHsFourthCourseQuestions.TrigModel, model));
        Assert.False(UsCcssHsFourthCourseQuestions.Verify(UsCcssHsFourthCourseQuestions.TrigModel, model, "0,12"));
        var ellipse = new Dictionary<string, int> { ["a"] = 5, ["c"] = 3, ["hyperbola"] = 0 };
        var hyperbola = new Dictionary<string, int> { ["a"] = 3, ["c"] = 5, ["hyperbola"] = 1 };
        Assert.Equal("16", UsCcssHsFourthCourseQuestions.Solve(UsCcssHsFourthCourseQuestions.FocalConics, ellipse));
        Assert.Equal("16", UsCcssHsFourthCourseQuestions.Solve(UsCcssHsFourthCourseQuestions.FocalConics, hyperbola));
        Assert.False(UsCcssHsFourthCourseQuestions.Verify(
            UsCcssHsFourthCourseQuestions.FocalConics,
            new Dictionary<string, int> { ["a"] = 3, ["c"] = 3, ["hyperbola"] = 1 }, "0"));
    }

    [Fact]
    public void PendingKernelsAreDirectlyTestableButAbsentFromLearnerReadinessCatalogues()
    {
        Assert.All(LessonPracticeContractRegistry.All,
            x => Assert.Equal("READY_VERIFIED", x.Readiness));
        foreach (var id in Families.Concat(new[]
        {
            UsCcssHsFourthCourse3x3Questions.Coefficients,
            UsCcssHsFourthCourse3x3Questions.Inverse,
            UsCcssHsFourthCourse3x3Questions.SolveSystem,
            UsCcssHsFourthCourse3x3Questions.RowReduce
        }))
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(id));
            Assert.False(LessonPracticeInteractionRegistry.TryResolve(id, out _));
        }
    }

    [Fact]
    public void PendingFourthCourseContractsAreDiscoverableForAuditOnly()
    {
        var pending = LessonPracticeContractRegistry.PendingForAudit;
        var fourth = pending.Where(x =>
            x.LessonCode.StartsWith("PED:US-CCSS-MATH:HS:FOURTH:", StringComparison.Ordinal))
            .OrderBy(x => x.LessonCode, StringComparer.Ordinal)
            .ToArray();
        Assert.Equal(7, fourth.Length);
        Assert.Equal(2, fourth.Count(x => x.LessonCode.Contains(":ADV-ALG-FUNC:", StringComparison.Ordinal)));
        Assert.Equal(5, fourth.Count(x => x.LessonCode.Contains(":ADV-TRIG-GEO:", StringComparison.Ordinal)));
        foreach (var candidate in fourth)
        {
            Assert.Equal("INTEGRATION_PENDING", candidate.Readiness);
            Assert.NotEmpty(candidate.AllowedQuestionFamilies);
            Assert.True(LessonPracticeContractRegistry.TryResolve(candidate.LessonCode, out var resolved));
            Assert.Same(candidate, resolved);
            Assert.DoesNotContain(LessonPracticeContractRegistry.All,
                row => row.LessonCode == candidate.LessonCode);
            Assert.False(LessonPracticeCapabilityResolver.TryResolve(candidate.LessonCode, out _));
            foreach (var family in candidate.AllowedQuestionFamilies)
                Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family), family);
        }

        Assert.Contains(LessonPracticeContractRegistry.All,
            row => row.LessonCode == "PED:US-CCSS-MATH:G1:U01:L01" &&
                   row.Readiness == "READY_VERIFIED");
    }

    [Fact]
    public void StudentRoutingIsStillFailClosedUntilFullIntegrationApproval()
    {
        foreach (var course in new[] { "ADV-ALG-FUNC", "ADV-TRIG-GEO" })
            for (var lesson = 1; lesson <= (course == "ADV-ALG-FUNC" ? 2 : 5); lesson++)
                Assert.False(LessonPracticeCapabilityResolver.TryResolve(
                    $"PED:US-CCSS-MATH:HS:FOURTH:{course}:U01:L{lesson:D2}", out _));
    }
}
