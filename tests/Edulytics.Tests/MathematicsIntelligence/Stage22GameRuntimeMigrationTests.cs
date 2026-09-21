using System.Text.Json;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Web.GameRouting;
using Microsoft.AspNetCore.DataProtection;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Stage22GameRuntimeMigrationTests
{
    private readonly Stage22ExactGameRuntime runtime =
        new(new EphemeralDataProtectionProvider());

    public static IEnumerable<object[]> ExactLessons() =>
        Stage18PracticeSkillContracts.All.Select(
            x => new object[] { x.LessonCode, x.SkillId, x.Mechanic });

    [Theory]
    [MemberData(nameof(ExactLessons))]
    public void EveryStage18ExactLessonUsesServerGeneratedVerifiedRound(
        string lessonCode,
        string skillId,
        string mechanic)
    {
        var actor = Guid.NewGuid();
        var adoption = Guid.NewGuid();
        var lesson = Guid.NewGuid();

        var round = runtime.CreateRound(
            actor,
            adoption,
            lesson,
            lessonCode,
            mechanic,
            roundIndex: 2);

        Assert.Equal(skillId, round.SkillId);
        Assert.Equal(mechanic, round.Mechanic);
        Assert.False(string.IsNullOrWhiteSpace(round.RoundToken));
        Assert.False(string.IsNullOrWhiteSpace(round.Prompt));
        Assert.True(round.Choices.Count >= 3);

        var serialized = JsonSerializer.Serialize(round);
        Assert.DoesNotContain("CorrectAnswer", serialized, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("correctAnswer", serialized, StringComparison.OrdinalIgnoreCase);

        var outcomes = round.Choices
            .Select(choice => runtime.EvaluateAnswer(
                actor,
                new Stage22GameAnswerRequest(
                    adoption,
                    lesson,
                    round.RoundToken,
                    choice)))
            .ToArray();

        var correctResult = Assert.Single(outcomes, x => x.IsCorrect);
        Assert.All(
            outcomes.Where(x => !x.IsCorrect),
            x => Assert.Equal(0, x.Points));
        Assert.Equal(200, correctResult.Points);
    }

    [Fact]
    public void MultiFamilyLessonRotatesQuestionFamilyAcrossRounds()
    {
        var contract = Stage18PracticeSkillContracts.All
            .First(x => x.AllowedQuestionFamilies.Count > 1);
        var actor = Guid.NewGuid();
        var adoption = Guid.NewGuid();
        var lesson = Guid.NewGuid();

        var first = runtime.CreateRound(
            actor,
            adoption,
            lesson,
            contract.LessonCode,
            contract.Mechanic,
            roundIndex: 0);

        var second = runtime.CreateRound(
            actor,
            adoption,
            lesson,
            contract.LessonCode,
            contract.Mechanic,
            roundIndex: 1);

        Assert.Equal(contract.AllowedQuestionFamilies[0], first.QuestionFamily);
        Assert.Equal(contract.AllowedQuestionFamilies[1], second.QuestionFamily);
        Assert.NotEqual(first.QuestionFamily, second.QuestionFamily);
    }

    [Fact]
    public void BrowserCannotForgeRoundScopeAcrossStudent()
    {
        var contract = Stage18PracticeSkillContracts.All[0];
        var actor = Guid.NewGuid();
        var adoption = Guid.NewGuid();
        var lesson = Guid.NewGuid();

        var round = runtime.CreateRound(
            actor,
            adoption,
            lesson,
            contract.LessonCode,
            contract.Mechanic,
            roundIndex: 0);

        var error = Assert.Throws<InvalidOperationException>(() =>
            runtime.EvaluateAnswer(
                Guid.NewGuid(),
                new Stage22GameAnswerRequest(
                    adoption,
                    lesson,
                    round.RoundToken,
                    round.Choices[0])));

        Assert.Contains(
            "authenticated game scope",
            error.Message,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BrowserCannotForgeCurriculumOrLessonScope()
    {
        var contract = Stage18PracticeSkillContracts.All[1];
        var actor = Guid.NewGuid();
        var adoption = Guid.NewGuid();
        var lesson = Guid.NewGuid();

        var round = runtime.CreateRound(
            actor,
            adoption,
            lesson,
            contract.LessonCode,
            contract.Mechanic,
            roundIndex: 0);

        Assert.Throws<InvalidOperationException>(() =>
            runtime.EvaluateAnswer(
                actor,
                new Stage22GameAnswerRequest(
                    Guid.NewGuid(),
                    lesson,
                    round.RoundToken,
                    round.Choices[0])));

        Assert.Throws<InvalidOperationException>(() =>
            runtime.EvaluateAnswer(
                actor,
                new Stage22GameAnswerRequest(
                    adoption,
                    Guid.NewGuid(),
                    round.RoundToken,
                    round.Choices[0])));
    }

    [Fact]
    public void TamperedRoundTokenFailsClosed()
    {
        var contract = Stage18PracticeSkillContracts.All[2];
        var actor = Guid.NewGuid();
        var adoption = Guid.NewGuid();
        var lesson = Guid.NewGuid();

        var round = runtime.CreateRound(
            actor,
            adoption,
            lesson,
            contract.LessonCode,
            contract.Mechanic,
            roundIndex: 0);

        var tampered = round.RoundToken[..^1] +
            (round.RoundToken[^1] == 'A' ? "B" : "A");

        Assert.Throws<InvalidOperationException>(() =>
            runtime.EvaluateAnswer(
                actor,
                new Stage22GameAnswerRequest(
                    adoption,
                    lesson,
                    tampered,
                    round.Choices[0])));
    }

    [Fact]
    public void InvalidMechanicCannotEnterServerAuthoritativeExactRuntime()
    {
        var contract = Stage18PracticeSkillContracts.All[0];

        Assert.Throws<InvalidOperationException>(() =>
            runtime.CreateRound(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                contract.LessonCode,
                "WRONG_MECHANIC",
                roundIndex: 0));
    }

    [Fact]
    public void ArbitraryWrongAnswerIsRejectedByServer()
    {
        var contract = Stage18PracticeSkillContracts.All
            .Single(x => x.Mechanic == "UNIT_RATE");

        var actor = Guid.NewGuid();
        var adoption = Guid.NewGuid();
        var lesson = Guid.NewGuid();

        var round = runtime.CreateRound(
            actor,
            adoption,
            lesson,
            contract.LessonCode,
            contract.Mechanic,
            roundIndex: 4);

        var result = runtime.EvaluateAnswer(
            actor,
            new Stage22GameAnswerRequest(
                adoption,
                lesson,
                round.RoundToken,
                "__not_a_mathematical_answer__"));

        Assert.False(result.IsCorrect);
        Assert.Equal(0, result.Points);
        Assert.Null(result.Solution);
    }
}
