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
    public void ConcurrentTabs_DifferentLessons_KeepProtectedRoundsIsolated()
    {
        var contracts = Stage18PracticeSkillContracts.All.Take(2).ToArray();
        Assert.Equal(2, contracts.Length);

        var actor = Guid.NewGuid();
        var adoption = Guid.NewGuid();
        var lessonA = Guid.NewGuid();
        var lessonB = Guid.NewGuid();

        var roundA = runtime.CreateRound(
            actor,
            adoption,
            lessonA,
            contracts[0].LessonCode,
            contracts[0].Mechanic,
            roundIndex: 0);

        var roundB = runtime.CreateRound(
            actor,
            adoption,
            lessonB,
            contracts[1].LessonCode,
            contracts[1].Mechanic,
            roundIndex: 0);

        var correctA = CorrectChoice(actor, adoption, lessonA, roundA);
        var correctB = CorrectChoice(actor, adoption, lessonB, roundB);
        var wrongA = roundA.Choices.First(x => !string.Equals(x, correctA, StringComparison.Ordinal));
        var wrongB = roundB.Choices.First(x => !string.Equals(x, correctB, StringComparison.Ordinal));

        Assert.True(Evaluate(actor, adoption, lessonA, roundA, correctA).IsCorrect);
        Assert.False(Evaluate(actor, adoption, lessonB, roundB, wrongB).IsCorrect);
        Assert.True(Evaluate(actor, adoption, lessonB, roundB, correctB).IsCorrect);
        Assert.False(Evaluate(actor, adoption, lessonA, roundA, wrongA).IsCorrect);

        // Re-evaluating either tab after activity in the other must still use
        // that tab's own protected lesson/question parameters.
        Assert.True(Evaluate(actor, adoption, lessonA, roundA, correctA).IsCorrect);
        Assert.True(Evaluate(actor, adoption, lessonB, roundB, correctB).IsCorrect);
    }

    [Fact]
    public void ConcurrentTabs_SameLessonSeparateRounds_DoNotOverwriteEachOther()
    {
        var contract = Stage18PracticeSkillContracts.All
            .First(x => x.AllowedQuestionFamilies.Count > 1);
        var actor = Guid.NewGuid();
        var adoption = Guid.NewGuid();
        var lesson = Guid.NewGuid();

        var tabA = runtime.CreateRound(
            actor,
            adoption,
            lesson,
            contract.LessonCode,
            contract.Mechanic,
            roundIndex: 0);

        var tabB = runtime.CreateRound(
            actor,
            adoption,
            lesson,
            contract.LessonCode,
            contract.Mechanic,
            roundIndex: 1);

        Assert.NotEqual(tabA.RoundToken, tabB.RoundToken);

        var correctA = CorrectChoice(actor, adoption, lesson, tabA);
        var correctB = CorrectChoice(actor, adoption, lesson, tabB);

        Assert.True(Evaluate(actor, adoption, lesson, tabA, correctA).IsCorrect);
        Assert.True(Evaluate(actor, adoption, lesson, tabB, correctB).IsCorrect);

        // Starting additional rounds in one browser tab must not mutate the
        // server authority represented by the other tab's protected token.
        _ = runtime.CreateRound(
            actor,
            adoption,
            lesson,
            contract.LessonCode,
            contract.Mechanic,
            roundIndex: 2);
        _ = runtime.CreateRound(
            actor,
            adoption,
            lesson,
            contract.LessonCode,
            contract.Mechanic,
            roundIndex: 3);

        Assert.True(Evaluate(actor, adoption, lesson, tabA, correctA).IsCorrect);
        Assert.True(Evaluate(actor, adoption, lesson, tabB, correctB).IsCorrect);
    }

    private string CorrectChoice(
        Guid actor,
        Guid adoption,
        Guid lesson,
        Stage22GameRound round) =>
        round.Choices.Single(choice =>
            Evaluate(actor, adoption, lesson, round, choice).IsCorrect);

    private Stage22GameAnswerResult Evaluate(
        Guid actor,
        Guid adoption,
        Guid lesson,
        Stage22GameRound round,
        string answer) =>
        runtime.EvaluateAnswer(
            actor,
            new Stage22GameAnswerRequest(
                adoption,
                lesson,
                round.RoundToken,
                answer));


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
