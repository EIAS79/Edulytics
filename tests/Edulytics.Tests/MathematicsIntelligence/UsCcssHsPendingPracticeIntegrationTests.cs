using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Exercises seven Fourth Courses pending contracts through the same real
/// exact-question engine used by selected Practice. The direct-kernel path
/// is test-only; learner routing, HTTP and persistence must stay denied
/// until independently approved and authenticated.
/// </summary>
public sealed class UsCcssHsPendingPracticeIntegrationTests
{
    private const string Prefix = "PED:US-CCSS-MATH:HS:FOURTH:";

    private static LessonPracticeContract[] PendingFourthCourseContracts() =>
        LessonPracticeContractRegistry.PendingForAudit
            .Where(x => x.LessonCode.StartsWith(Prefix, StringComparison.Ordinal))
            .OrderBy(x => x.LessonCode, StringComparer.Ordinal)
            .ToArray();

    [Fact]
    public void EveryPendingFourthCourseLessonGeneratesGradesAndRejectsMalformedAnswersDirectly()
    {
        var pending = PendingFourthCourseContracts();
        Assert.Equal(7, pending.Length);
        var engine = new ExactSkillContractQuestionEngine();
        var checkedQuestions = 0;
        for (var index = 0; index < pending.Length; index++)
        {
            var contract = pending[index];
            Assert.Equal("INTEGRATION_PENDING", contract.Readiness);
            Assert.NotEmpty(contract.AllowedQuestionFamilies);
            Assert.DoesNotContain(contract.AllowedQuestionFamilies,
                x => x.Contains("one-step-formula-equation", StringComparison.Ordinal));

            // This explicit direct-kernel QA does not call TryResolve and must
            // not be confused with a real authenticated selected session.
            var questions = engine.Generate(
                "fourth-courses-pending-direct-kernel-qa",
                contract.LessonCode,
                contract.AllowedQuestionFamilies,
                ExactSkillQuestionDifficulty.Standard,
                10,
                870000 + index,
                []);

            Assert.Equal(10, questions.Count);
            Assert.Equal(10, questions
                .Select(x => x.ExposureFingerprint)
                .Distinct(StringComparer.Ordinal).Count());
            foreach (var question in questions)
            {
                checkedQuestions++;
                Assert.Contains(question.Family, contract.AllowedQuestionFamilies);
                Assert.False(string.IsNullOrWhiteSpace(question.Prompt));
                Assert.False(string.IsNullOrWhiteSpace(question.CorrectAnswer));
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    question.Family, question.Parameters, question.CorrectAnswer),
                    $"{contract.LessonCode}/{question.Family} rejected its generated answer");
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    question.Family, question.Parameters, "<script>"),
                    $"{contract.LessonCode}/{question.Family} accepted untrusted markup");
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    question.Family, question.Parameters, new string('9', 400)),
                    $"{contract.LessonCode}/{question.Family} accepted overlong input");
            }
        }
        Assert.Equal(70, checkedQuestions);
    }

    [Fact]
    public void PendingQuestionsRemainUnavailableToSelectedStudentsAndOtherCourseContractsStayReady()
    {
        var pending = PendingFourthCourseContracts();
        Assert.Equal(7, pending.Length);
        foreach (var contract in pending)
        {
            Assert.True(LessonPracticeContractRegistry.TryResolve(
                contract.LessonCode, out var auditContract));
            Assert.Same(contract, auditContract);
            Assert.DoesNotContain(LessonPracticeContractRegistry.All,
                x => x.LessonCode == contract.LessonCode);
            Assert.False(LessonPracticeCapabilityResolver.TryResolve(
                contract.LessonCode, out var selected));
            Assert.Null(selected);

            foreach (var family in contract.AllowedQuestionFamilies)
                Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        }

        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G1:U01:L01", out var gradeOne));
        Assert.NotNull(gradeOne);
        Assert.Equal("READY_VERIFIED", gradeOne!.Readiness);
    }
}
