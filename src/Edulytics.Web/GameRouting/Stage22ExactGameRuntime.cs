using System.Globalization;
using System.Security.Cryptography;
using System.Text.Json;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Microsoft.AspNetCore.DataProtection;

namespace Edulytics.Web.GameRouting;

public sealed record Stage22GameRoundRequest(
    Guid CurriculumAdoptionId,
    Guid LessonId,
    int RoundIndex);

public sealed record Stage22GameAnswerRequest(
    Guid CurriculumAdoptionId,
    Guid LessonId,
    string RoundToken,
    string Answer);

public sealed record Stage22GameRound(
    string RoundToken,
    int RoundIndex,
    string SkillId,
    string Mechanic,
    string QuestionFamily,
    string Prompt,
    string Hint,
    IReadOnlyList<string> Choices);

public sealed record Stage22GameAnswerResult(
    bool IsCorrect,
    int Points,
    string Feedback,
    string? Solution);

/// <summary>
/// Server-authoritative mathematics runtime for Stage 22 exact games.
///
/// The browser receives a prompt, renderer choices and an opaque protected round
/// token. It never receives the authoritative answer. Generation, solving and
/// correctness verification remain on the server and reuse the same exact
/// SkillContract kernel as Stage 18 Practice.
/// </summary>
public sealed class Stage22ExactGameRuntime
{
    public const string RuntimeVersion = "stage22-server-authoritative-game-v1";
    public const string ProtectorPurpose = "Edulytics.Stage22.ExactGame.v1";
    private static readonly TimeSpan RoundLifetime = TimeSpan.FromMinutes(30);

    private readonly IDataProtector protector;

    public Stage22ExactGameRuntime(IDataProtectionProvider dataProtectionProvider)
    {
        ArgumentNullException.ThrowIfNull(dataProtectionProvider);
        protector = dataProtectionProvider.CreateProtector(ProtectorPurpose);
    }

    public Stage22GameRound CreateRound(
        Guid actorUserId,
        Guid curriculumAdoptionId,
        Guid lessonId,
        string lessonCode,
        string mechanic,
        int roundIndex)
    {
        if (actorUserId == Guid.Empty ||
            curriculumAdoptionId == Guid.Empty ||
            lessonId == Guid.Empty ||
            string.IsNullOrWhiteSpace(lessonCode) ||
            string.IsNullOrWhiteSpace(mechanic) ||
            roundIndex is < 0 or > 49)
        {
            throw new InvalidOperationException(
                "Stage 22 exact game runtime requires valid actor, curriculum, lesson, mechanic and round scope.");
        }

        if (!Stage18PracticeSkillContracts.TryResolve(lessonCode, out var contract) ||
            contract is null ||
            !string.Equals(contract.Mechanic, mechanic, StringComparison.Ordinal) ||
            contract.AllowedQuestionFamilies.Count == 0)
        {
            throw new InvalidOperationException(
                "Stage 22 exact game runtime is available only for an approved exact lesson SkillContract.");
        }

        var seed = RandomNumberGenerator.GetInt32(1, int.MaxValue);
        var difficulty = roundIndex switch
        {
            >= 6 => ExactSkillQuestionDifficulty.Challenge,
            >= 3 => ExactSkillQuestionDifficulty.Stretch,
            _ => ExactSkillQuestionDifficulty.Standard
        };

        var generated = new ExactSkillContractQuestionEngine()
            .Generate(
                "stage22-game",
                contract.LessonCode,
                contract.AllowedQuestionFamilies,
                difficulty,
                1,
                unchecked(seed ^ ((roundIndex + 1) * 7919)),
                [])[0];

        if (!ExactSkillContractQuestionEngine.Verify(
                generated.Family,
                generated.Parameters,
                generated.CorrectAnswer))
        {
            throw new InvalidOperationException(
                "Stage 22 server verifier rejected generated solver output.");
        }

        var payload = new ProtectedRoundPayload(
            RuntimeVersion,
            actorUserId,
            curriculumAdoptionId,
            lessonId,
            contract.LessonCode,
            contract.SkillId,
            contract.Mechanic,
            generated.Family,
            generated.Parameters.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal),
            generated.Solution,
            roundIndex,
            DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        var roundToken = protector.Protect(JsonSerializer.Serialize(payload));
        var choices = BuildChoices(generated, seed);

        return new Stage22GameRound(
            roundToken,
            roundIndex,
            contract.SkillId,
            contract.Mechanic,
            generated.Family,
            generated.Prompt,
            HintFor(generated.Family),
            choices);
    }

    public Stage22GameAnswerResult EvaluateAnswer(
        Guid actorUserId,
        Stage22GameAnswerRequest request)
    {
        if (actorUserId == Guid.Empty ||
            request.CurriculumAdoptionId == Guid.Empty ||
            request.LessonId == Guid.Empty ||
            string.IsNullOrWhiteSpace(request.RoundToken) ||
            string.IsNullOrWhiteSpace(request.Answer) ||
            request.Answer.Length > 64)
        {
            throw new InvalidOperationException(
                "Stage 22 game answer request is invalid.");
        }

        ProtectedRoundPayload payload;
        try
        {
            var json = protector.Unprotect(request.RoundToken);
            payload = JsonSerializer.Deserialize<ProtectedRoundPayload>(json)
                ?? throw new InvalidOperationException("Stage 22 round token is empty.");
        }
        catch (Exception exception) when (
            exception is CryptographicException or
            JsonException or
            InvalidOperationException)
        {
            throw new InvalidOperationException(
                "Stage 22 round token is invalid.",
                exception);
        }

        if (!string.Equals(payload.RuntimeVersion, RuntimeVersion, StringComparison.Ordinal) ||
            payload.ActorUserId != actorUserId ||
            payload.CurriculumAdoptionId != request.CurriculumAdoptionId ||
            payload.LessonId != request.LessonId)
        {
            throw new InvalidOperationException(
                "Stage 22 round token is outside the authenticated game scope.");
        }

        var issuedAt = DateTimeOffset.FromUnixTimeSeconds(payload.IssuedAtUnixSeconds);
        var now = DateTimeOffset.UtcNow;
        if (issuedAt > now.AddMinutes(1) || now - issuedAt > RoundLifetime)
        {
            throw new InvalidOperationException(
                "Stage 22 round token has expired.");
        }

        if (!Stage18PracticeSkillContracts.TryResolve(payload.LessonCode, out var contract) ||
            contract is null ||
            !string.Equals(contract.SkillId, payload.SkillId, StringComparison.Ordinal) ||
            !string.Equals(contract.Mechanic, payload.Mechanic, StringComparison.Ordinal) ||
            !contract.AllowedQuestionFamilies.Contains(payload.QuestionFamily, StringComparer.Ordinal))
        {
            throw new InvalidOperationException(
                "Stage 22 protected round no longer matches the approved SkillContract.");
        }

        var normalized = request.Answer.Trim();
        var correct = ExactSkillContractQuestionEngine.Verify(
            payload.QuestionFamily,
            payload.Parameters,
            normalized);

        return correct
            ? new Stage22GameAnswerResult(
                true,
                200,
                "Correct. The server verified the answer against the exact SkillContract problem.",
                payload.Solution)
            : new Stage22GameAnswerResult(
                false,
                0,
                "Try again. Re-check the mathematical relationship shown in the problem.",
                null);
    }

    private static IReadOnlyList<string> BuildChoices(
        ExactSkillGeneratedQuestion generated,
        int seed)
    {
        var values = new List<string>();

        if (string.Equals(
                generated.Family,
                "fractions.compare.unlike.common_denominator",
                StringComparison.Ordinal))
        {
            values.AddRange(["<", "=", ">"]);
        }
        else
        {
            if (!int.TryParse(
                    generated.CorrectAnswer,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out var correct))
            {
                throw new InvalidOperationException(
                    "Stage 22 exact game currently requires integer or comparison-symbol answers.");
            }

            var numeric = new HashSet<int> { correct };
            foreach (var delta in new[] { -1, 1, -2, 2, -3, 3 })
            {
                var candidate = correct + delta;
                if (candidate >= 0)
                    numeric.Add(candidate);
                if (numeric.Count >= 4)
                    break;
            }

            while (numeric.Count < 4)
                numeric.Add(correct + numeric.Count + 1);

            values.AddRange(
                numeric.Select(x => x.ToString(CultureInfo.InvariantCulture)));
        }

        var random = new Random(seed);
        for (var i = values.Count - 1; i > 0; i--)
        {
            var j = random.Next(i + 1);
            (values[i], values[j]) = (values[j], values[i]);
        }

        return values;
    }

    private static string HintFor(string family) => family switch
    {
        "algebra.relationships.two_unknowns.total_difference" =>
            "Use both relationships together; one equation alone is not enough.",

        "measurement.scale.equal_intervals.read_value" =>
            "Count equal intervals, determine the interval value, then locate the pointer.",

        "fractions.compare.unlike.common_denominator" =>
            "Compare the values using a common denominator or cross-products.",

        "fractions.equivalent.missing_value" or
        "fractions.equivalent.recognize" or
        "fractions.equivalent.generate_multiple" or
        "fractions.equivalent.number_line" or
        "fractions.equivalent.reduce_common_factor" =>
            "Equivalent fractions preserve value when numerator and denominator change by the same factor.",

        "ratio.unit_rate.direct" or
        "ratio.unit_rate.equivalent_ratio" =>
            "Use the same multiplicative relationship on both parts of the ratio.",

        _ => "Use the exact mathematical relationship in the question."
    };

    private sealed record ProtectedRoundPayload(
        string RuntimeVersion,
        Guid ActorUserId,
        Guid CurriculumAdoptionId,
        Guid LessonId,
        string LessonCode,
        string SkillId,
        string Mechanic,
        string QuestionFamily,
        Dictionary<string, int> Parameters,
        string Solution,
        int RoundIndex,
        long IssuedAtUnixSeconds);
}
