using System.Reflection;
using System.Text.Json;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Practice;

namespace Edulytics.Tests.Acceptance;

/// <summary>
/// ALG2 L01 persisted scoring fixtures, AUTHORED_NOT_RUN.
/// Exercise the precise private grading branch without a database or Stage D.
/// </summary>
public sealed class UsCcssAlg2U01L01PersistedGradingTests
{
    private const string Hanoi = "usccss.alg2.u01.l01.hanoi_min_moves";
    private const string Checker = "usccss.alg2.u01.l01.checker_jump_optional";
    private const string Recurrence = "usccss.alg2.u01.l01.recursive_sequence";
    private static readonly Guid School = Guid.NewGuid();
    private static readonly Guid Adoption = Guid.NewGuid();
    private static readonly Guid Lesson = Guid.NewGuid();

    private static (AssessmentItem Item, PracticeAttempt Attempt) Issued(
        string family, int n, int variant, string canonical)
    {
        var item = new AssessmentItem
        {
            SchoolId = School,
            CurriculumAdoptionId = Adoption,
            CurriculumPedagogicalLessonId = Lesson,
            Source = AssessmentItemSource.SystemGenerated,
            ItemType = AssessmentItemType.ShortAnswer,
            GenerationFamily = family,
            GenerationMethod = Stage18PracticeSkillContracts.GenerationMethod,
            CorrectAnswer = canonical,
            GenerationParametersJson = JsonSerializer.Serialize(new
            {
                skillId = "sequences.core.recursive_nth_term",
                questionFamily = family,
                questionVariant = "v1",
                parameters = new { n, variant }
            }),
            ValidationMetadataJson = JsonSerializer.Serialize(new
            {
                stage = 18,
                alignment = "skill-contract-verified",
                readiness = "READY_VERIFIED",
                skillContract = "sequences.core.recursive_nth_term",
                allowedFamily = family,
                questionVariant = "v1",
                solverVerified = true,
                broadFallbackUsed = false,
                officialMasteryEvidence = false
            })
        };
        var attempt = new PracticeAttempt
        {
            IsPrivate = true,
            SchoolId = School,
            CurriculumAdoptionId = Adoption,
            CurriculumPedagogicalLessonId = Lesson
        };
        return (item, attempt);
    }

    private static bool Grade(AssessmentItem item, PracticeAttempt attempt, string answer)
    {
        var method = typeof(PracticeService).GetMethod(
            "GradeAlg2L01PersistedAnswer",
            BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(method);
        return (bool)method.Invoke(null, [item, attempt, answer])!;
    }

    [Theory]
    [InlineData(Hanoi, 64, 0, "18446744073709551615", "2^(64)-1")]
    [InlineData(Hanoi, 64, 0, "18446744073709551615", "2 ^ 64 - 1")]
    [InlineData(Checker, 3, 1, "15", "3*(3+2)")]
    [InlineData(Recurrence, 3, 2, "9", "9")]
    [InlineData(Recurrence, 2, 3, "1/4", "0.25")]
    [InlineData(Recurrence, 4, 4, "-7", "-7")]
    [InlineData(Recurrence, 6, 5, "5", "5")]
    public void IssuedNumericFamily_AcceptsExactEquivalent(
        string family, int n, int variant, string canonical, string equivalent)
    {
        var (item, attempt) = Issued(family, n, variant, canonical);
        Assert.True(Grade(item, attempt, equivalent));
    }

    [Theory]
    [InlineData(Hanoi, 64, 0, "18446744073709551615", "2^(63)-1")]
    [InlineData(Hanoi, 64, 0, "18446744073709551615", "2^(6 4)-1")]
    [InlineData(Hanoi, 65, 0, "36893488147419103231", "2^65-1")]
    [InlineData(Hanoi, 1, 6, "1", "1")]
    [InlineData(Checker, 3, 1, "15", "17")]
    [InlineData(Checker, 3, 7, "15", "15")]
    [InlineData(Recurrence, 3, 8, "9", "9")]
    public void InvalidOrTeacherOnlyQuestion_FailsClosed(
        string family, int n, int variant, string canonical, string answer)
    {
        var (item, attempt) = Issued(family, n, variant, canonical);
        Assert.False(Grade(item, attempt, answer));
    }

    [Fact]
    public void ForgedStoredAnswerAndParameters_DoNotFallBackToGenericComparator()
    {
        var (item, attempt) = Issued(Hanoi, 64, 0, "18446744073709551615");
        item.CorrectAnswer = "1";
        Assert.False(Grade(item, attempt, "1"));

        (item, attempt) = Issued(Hanoi, 64, 0, "18446744073709551615");
        item.GenerationParametersJson = "{bad-json";
        Assert.False(Grade(item, attempt, "2^64-1"));

        (item, attempt) = Issued(Hanoi, 64, 0, "18446744073709551615");
        item.GenerationParametersJson = item.GenerationParametersJson.Replace(
            "\"variant\":0", "\"variant\":6");
        Assert.False(Grade(item, attempt, "2^64-1"));

        (item, attempt) = Issued(Hanoi, 64, 0, "18446744073709551615");
        item.ValidationMetadataJson = "{}";
        Assert.False(Grade(item, attempt, "2^64-1"));

        (item, attempt) = Issued(Hanoi, 64, 0, "18446744073709551615");
        item.GenerationFamily = "usccss.alg2.u01.l02.foo";
        Assert.False(Grade(item, attempt, "2^64-1"));
    }

    [Fact]
    public void ForeignLessonSchoolAndAdoption_AreDenied()
    {
        var (item, attempt) = Issued(Hanoi, 64, 0, "18446744073709551615");
        attempt.CurriculumPedagogicalLessonId = Guid.NewGuid();
        Assert.False(Grade(item, attempt, "2^64-1"));
        attempt.CurriculumPedagogicalLessonId = Lesson;
        attempt.SchoolId = Guid.NewGuid();
        Assert.False(Grade(item, attempt, "2^64-1"));
        attempt.SchoolId = School;
        attempt.CurriculumAdoptionId = Guid.NewGuid();
        Assert.False(Grade(item, attempt, "2^64-1"));
        attempt.CurriculumAdoptionId = Adoption;
        attempt.IsPrivate = false;
        Assert.False(Grade(item, attempt, "2^64-1"));
    }

    // AUTHORED_NOT_RUN / EXPECTED_RED on f39c223e:
    // Stage18 GenerateComposed and GenerateProgressiveLesson do NOT put
    // questionVariant in ValidationMetadataJson; they place it in the
    // persisted GenerationParametersJson envelope instead.
    [Fact]
    public void ActualComposedValidationMetadataWithoutVariant_MustNotRejectIssuedAnswer()
    {
        var (item, attempt) = Issued(Hanoi, 64, 0, "18446744073709551615");
        item.ValidationMetadataJson = JsonSerializer.Serialize(new
        {
            stage = 18,
            alignment = "skill-contract-verified",
            readiness = "READY_VERIFIED",
            skillContract = "sequences.core.recursive_nth_term",
            allowedFamily = Hanoi,
            composer = "practice-assessment-v2",
            questionForm = "Calculate",
            solverVerified = true,
            broadFallbackUsed = false,
            officialMasteryEvidence = false
        });
        Assert.True(Grade(item, attempt, "2^(64)-1"));
    }

    [Theory]
    [InlineData("E,A,C", true)]
    [InlineData(" c, e, a ", true)]
    [InlineData("A,C,E", true)]
    [InlineData("A,A,E", false)]
    [InlineData("A,C,F", false)]
    [InlineData("A,C", false)]
    [InlineData("A,C,E,F", false)]
    [InlineData("A;C;E", false)]
    public void Algebra1P3_UnorderedChoiceCompatibilityIsUnchanged(
        string response, bool expectedCorrect)
    {
        var method = typeof(PracticeService).GetMethod(
            "GradeAlg1P3SelectionAnswer",
            BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(method);
        var actual = (bool)method.Invoke(null, ["A,C,E", response])!;
        Assert.Equal(expectedCorrect, actual);
        Assert.False((bool)method.Invoke(null, ["A,C,F", response])!);
    }
}
