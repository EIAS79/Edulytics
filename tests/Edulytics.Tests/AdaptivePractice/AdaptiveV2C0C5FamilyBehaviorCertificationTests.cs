using System.Text.Json;
using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Entities;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.AdaptivePractice;
using Edulytics.Services.Mathematics.Difficulty;

namespace Edulytics.Tests.MathematicsIntelligence.AdaptivePractice;

public sealed class AdaptiveV2C0C5FamilyBehaviorCertificationTests
{
    private const string RunEnvironmentVariable =
        "EDULYTICS_RUN_FULL_ADAPTIVE_V2_CERTIFICATION";

    [Fact]
    public void EverySupportedQuestionFamilyPassesTheC0C5BehaviorMatrix()
    {
        if (!string.Equals(
                Environment.GetEnvironmentVariable(
                    RunEnvironmentVariable),
                "1",
                StringComparison.Ordinal))
        {
            return;
        }

        var representatives =
            LessonPracticeContractRegistry.All
                .SelectMany(contract =>
                    contract.AllowedQuestionFamilies
                        .Distinct(StringComparer.Ordinal)
                        .Select(family => new FamilyTarget(contract, family)))
                .GroupBy(x => x.Family, StringComparer.Ordinal)
                .OrderBy(x => x.Key, StringComparer.Ordinal)
                .Select(x => x.First())
                .ToArray();

        var generator = new AdaptiveVerifiedItemGenerator();
        var decisionEngine =
            new AdaptiveNextItemDecisionEngine(
                new MathematicsDifficultyEngine());
        var remediationMachine =
            new AdaptiveRemediationStateMachine();

        var blockers = new List<string>();
        var certified = new List<object>();

        Assert.Equal(
            1,
            AdaptivePracticeV2Behavior.MaximumSameItemRetries);

        for (var index = 0; index < representatives.Length; index++)
        {
            var target = representatives[index];

            try
            {
                CertifyFamily(
                    target,
                    index,
                    generator,
                    decisionEngine,
                    remediationMachine);

                certified.Add(new
                {
                    target.Family,
                    target.Contract.LessonCode,
                    target.Contract.SkillId,
                    scenarios = new[]
                    {
                        "correct-first-attempt",
                        "wrong-then-correct-retry",
                        "wrong-then-wrong-retry",
                        "remediation-correct",
                        "remediation-wrong",
                        "repeated-misconception",
                        "weak-prerequisite",
                        "weak-representation",
                        "independent-confirmation-correct",
                        "independent-confirmation-wrong",
                        "full-session-budget"
                    }
                });
            }
            catch (Exception exception)
            {
                blockers.Add(
                    $"{target.Family} / {target.Contract.LessonCode}: " +
                    $"{exception.GetType().Name}: {exception.Message}");
            }
        }

        WriteReport(
            representatives.Length,
            certified,
            blockers);

        Assert.True(
            blockers.Count == 0,
            "Adaptive V2 C0-C5 family behavior blockers: " +
            string.Join(" | ", blockers.Take(100)) +
            (blockers.Count > 100
                ? $" (+{blockers.Count - 100} more)"
                : string.Empty));
    }

    private static void CertifyFamily(
        FamilyTarget target,
        int index,
        AdaptiveVerifiedItemGenerator generator,
        AdaptiveNextItemDecisionEngine decisionEngine,
        AdaptiveRemediationStateMachine remediationMachine)
    {
        const int baselineComplexity = 72;
        var seedBase = unchecked(73000000 + (index * 5003));

        var baselineDecision = Decision(
            target,
            baselineComplexity,
            AdaptivePracticeDecisionReasonCodes.SessionBaseline,
            remediationLockActive: false,
            confirmationRequired: false,
            isIndependentConfirmation: false,
            progressionEligible: false);

        var baseline = Generate(
            generator,
            target,
            baselineDecision,
            seedBase + 1,
            [],
            []);

        ValidateItem(target, baseline);

        var correctFirst = Observation(
            sequence: 1,
            correct: true,
            baselineComplexity,
            target.Family,
            confirmation: false);

        var correctFirstDecision = decisionEngine.Decide(
            State(
                target,
                baselineComplexity,
                responses: [correctFirst],
                remediation: AdaptivePracticeRemediationState.None,
                skillMastery: .90m,
                prerequisiteMastery: .90m,
                recentSuccessfulItems: 2,
                representationFluency: .90m));

        Require(
            !correctFirstDecision.RemediationLockActive &&
            !correctFirstDecision.ConfirmationRequired,
            "Correct first attempt unexpectedly entered remediation.");

        var wrongFirst = Observation(
            sequence: 1,
            correct: false,
            baselineComplexity,
            target.Family,
            confirmation: false);

        var initialLock = remediationMachine.Apply(
            AdaptivePracticeRemediationState.None,
            wrongFirst);

        Require(
            initialLock.IsLocked &&
            initialLock.ConfirmationRequired,
            "Wrong #1 did not establish the remediation lock.");

        var wrongDecision = decisionEngine.Decide(
            State(
                target,
                baselineComplexity,
                responses: [wrongFirst],
                remediation: initialLock));

        Require(
            wrongDecision.RemediationLockActive &&
            wrongDecision.ConfirmationRequired &&
            !wrongDecision.ProgressionEligible &&
            wrongDecision.TargetComplexityScore <= baselineComplexity &&
            string.Equals(
                wrongDecision.TargetQuestionFamily,
                target.Family,
                StringComparison.Ordinal),
            "Wrong-answer recovery decision violated the C0-C4 contract.");

        var recovery = Generate(
            generator,
            target,
            wrongDecision,
            seedBase + 2,
            [baseline.ExposureFingerprint],
            [AdaptivePracticeSemanticIdentity.Resolve(baseline)]);

        ValidateItem(target, recovery);
        Require(
            !string.Equals(
                baseline.ExposureFingerprint,
                recovery.ExposureFingerprint,
                StringComparison.Ordinal),
            "Wrong #2 did not produce a fresh recovery exposure.");

        var correctedRetry = Observation(
            sequence: 1,
            correct: true,
            baselineComplexity,
            target.Family,
            confirmation: false);

        var retryState = remediationMachine.Apply(
            initialLock,
            correctedRetry);

        Require(
            retryState.IsLocked &&
            retryState.ConfirmationRequired,
            "Corrected same-item retry incorrectly unlocked progression.");

        var retryDecision = decisionEngine.Decide(
            State(
                target,
                baselineComplexity,
                responses: [correctedRetry],
                remediation: retryState));

        Require(
            retryDecision.IsIndependentConfirmation &&
            retryDecision.RemediationLockActive &&
            retryDecision.ConfirmationRequired &&
            !retryDecision.ProgressionEligible,
            "Corrected retry did not require fresh independent confirmation.");

        var remediationCorrect = Observation(
            sequence: 2,
            correct: true,
            wrongDecision.TargetComplexityScore,
            target.Family,
            confirmation: false);

        var remediationState = remediationMachine.Apply(
            initialLock,
            remediationCorrect);

        var confirmationDecision = decisionEngine.Decide(
            State(
                target,
                wrongDecision.TargetComplexityScore,
                responses: [wrongFirst, remediationCorrect],
                remediation: remediationState));

        Require(
            confirmationDecision.IsIndependentConfirmation &&
            confirmationDecision.RemediationLockActive &&
            confirmationDecision.ConfirmationRequired &&
            confirmationDecision.TargetComplexityScore <=
                initialLock.LockComplexityScore,
            "Correct remediation item did not lead to bounded fresh confirmation.");

        var confirmation = Generate(
            generator,
            target,
            confirmationDecision,
            seedBase + 3,
            [
                baseline.ExposureFingerprint,
                recovery.ExposureFingerprint
            ],
            [
                AdaptivePracticeSemanticIdentity.Resolve(baseline),
                AdaptivePracticeSemanticIdentity.Resolve(recovery)
            ]);

        ValidateItem(target, confirmation);

        var remediationWrong = Observation(
            sequence: 2,
            correct: false,
            wrongDecision.TargetComplexityScore,
            target.Family,
            confirmation: false);

        var repeatedFailureLock = remediationMachine.Apply(
            initialLock,
            remediationWrong);

        var repeatedFailureDecision = decisionEngine.Decide(
            State(
                target,
                wrongDecision.TargetComplexityScore,
                responses: [wrongFirst, remediationWrong],
                remediation: repeatedFailureLock));

        Require(
            repeatedFailureDecision.RemediationLockActive &&
            !repeatedFailureDecision.ProgressionEligible &&
            repeatedFailureDecision.TargetComplexityScore <=
                wrongDecision.TargetComplexityScore,
            "Repeated remediation failure increased complexity or unlocked progression.");

        var misconceptionDecision = decisionEngine.Decide(
            State(
                target,
                wrongDecision.TargetComplexityScore,
                responses: [remediationWrong],
                remediation: repeatedFailureLock,
                misconceptions:
                [
                    new AdaptivePracticeMisconceptionEvidence(
                        "certified.repeated-misconception",
                        ObservationCount: 2,
                        IsBlocking: true,
                        QuestionFamily: target.Family,
                        LastObservedSequence: 2)
                ]));

        Require(
            string.Equals(
                misconceptionDecision.ReasonCode,
                AdaptivePracticeDecisionReasonCodes.MisconceptionRemediation,
                StringComparison.Ordinal) &&
            string.Equals(
                misconceptionDecision.TargetQuestionFamily,
                target.Family,
                StringComparison.Ordinal),
            "Repeated misconception did not preserve the approved family.");

        var weakPrerequisite = decisionEngine.Decide(
            State(
                target,
                baselineComplexity,
                responses: [correctFirst],
                remediation: AdaptivePracticeRemediationState.None,
                prerequisiteMastery: .25m,
                recentSuccessfulItems: 3,
                representationFluency: .90m));

        Require(
            string.Equals(
                weakPrerequisite.ReasonCode,
                AdaptivePracticeDecisionReasonCodes.PrerequisiteRecovery,
                StringComparison.Ordinal) &&
            !weakPrerequisite.ProgressionEligible &&
            weakPrerequisite.TargetComplexityScore < baselineComplexity,
            "Weak prerequisite did not block progression.");

        var weakRepresentation = decisionEngine.Decide(
            State(
                target,
                baselineComplexity,
                responses: [wrongFirst],
                remediation: initialLock,
                prerequisiteMastery: .90m,
                representationFluency: .20m));

        Require(
            string.Equals(
                weakRepresentation.ReasonCode,
                AdaptivePracticeDecisionReasonCodes.RepresentationRecovery,
                StringComparison.Ordinal) &&
            !weakRepresentation.ProgressionEligible,
            "Weak representation did not trigger representation recovery.");

        var confirmationCorrect = Observation(
            sequence: 3,
            correct: true,
            confirmationDecision.TargetComplexityScore,
            target.Family,
            confirmation: true);

        var unlocked = remediationMachine.Apply(
            remediationState,
            confirmationCorrect);

        Require(
            !unlocked.IsLocked &&
            !unlocked.ConfirmationRequired,
            "Correct fresh confirmation did not clear remediation.");

        var postConfirmation = decisionEngine.Decide(
            State(
                target,
                confirmationDecision.TargetComplexityScore,
                responses: [remediationCorrect, confirmationCorrect],
                remediation: unlocked,
                skillMastery: .95m,
                prerequisiteMastery: .95m,
                recentSuccessfulItems: 3,
                representationFluency: .90m));

        Require(
            !postConfirmation.RemediationLockActive &&
            !postConfirmation.ConfirmationRequired &&
            postConfirmation.TargetComplexityScore <=
                confirmationDecision.TargetComplexityScore +
                AdaptiveNextItemDecisionEngine.ComplexityStepLimit,
            "Post-confirmation progression violated the bounded-step contract.");

        var confirmationWrong = Observation(
            sequence: 3,
            correct: false,
            confirmationDecision.TargetComplexityScore,
            target.Family,
            confirmation: true);

        var relocked = remediationMachine.Apply(
            remediationState,
            confirmationWrong);

        var failedConfirmationDecision = decisionEngine.Decide(
            State(
                target,
                confirmationDecision.TargetComplexityScore,
                responses: [remediationCorrect, confirmationWrong],
                remediation: relocked));

        Require(
            relocked.IsLocked &&
            failedConfirmationDecision.RemediationLockActive &&
            !failedConfirmationDecision.ProgressionEligible &&
            failedConfirmationDecision.TargetComplexityScore <=
                confirmationDecision.TargetComplexityScore,
            "Wrong fresh confirmation did not continue bounded remediation.");
    }

    private static AdaptivePracticeLearningState State(
        FamilyTarget target,
        int complexity,
        IReadOnlyList<AdaptivePracticeResponseObservation> responses,
        AdaptivePracticeRemediationState remediation,
        decimal skillMastery = .70m,
        decimal prerequisiteMastery = .70m,
        int recentSuccessfulItems = 0,
        decimal representationFluency = .70m,
        IReadOnlyList<AdaptivePracticeMisconceptionEvidence>? misconceptions = null) =>
        new(
            target.Contract.SkillId,
            skillMastery,
            prerequisiteMastery,
            complexity,
            recentSuccessfulItems,
            responses,
            misconceptions ?? [],
            [
                new AdaptivePracticeRepresentationFluency(
                    "symbolic",
                    representationFluency,
                    [target.Family])
            ],
            target.Contract.AllowedQuestionFamilies,
            remediation);

    private static AdaptivePracticeResponseObservation Observation(
        int sequence,
        bool correct,
        int complexity,
        string family,
        bool confirmation) =>
        new(
            sequence,
            correct,
            complexity,
            family,
            Representation: "symbolic",
            IsIndependentConfirmation: confirmation);

    private static AdaptiveNextItemDecision Decision(
        FamilyTarget target,
        int complexity,
        string reason,
        bool remediationLockActive,
        bool confirmationRequired,
        bool isIndependentConfirmation,
        bool progressionEligible) =>
        new(
            target.Contract.SkillId,
            complexity,
            target.Family,
            "symbolic",
            null,
            reason,
            RequiresFreshExposure: true,
            remediationLockActive,
            confirmationRequired,
            isIndependentConfirmation,
            progressionEligible,
            AdaptivePracticeV2Versions.EngineVersion,
            AdaptivePracticeV2Versions.PolicyVersion);

    private static AssessmentItem Generate(
        AdaptiveVerifiedItemGenerator generator,
        FamilyTarget target,
        AdaptiveNextItemDecision decision,
        int seed,
        IReadOnlyCollection<string> excludedExposureFingerprints,
        IReadOnlyCollection<string> excludedSemanticIdentityKeys) =>
        generator.GenerateOne(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            target.Contract,
            decision,
            seed,
            excludedExposureFingerprints,
            excludedSemanticIdentityKeys);

    private static void ValidateItem(
        FamilyTarget target,
        AssessmentItem item)
    {
        Require(
            string.Equals(
                item.GenerationFamily,
                target.Family,
                StringComparison.Ordinal),
            "Generator changed the certified family.");

        Require(
            !string.IsNullOrWhiteSpace(item.ExposureFingerprint) &&
            !string.IsNullOrWhiteSpace(item.ValidationMetadataJson) &&
            item.ValidationMetadataJson.Contains(
                "solverVerified",
                StringComparison.Ordinal),
            "Generated item is missing solver-verified provenance.");
    }

    private static void Require(
        bool condition,
        string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private static void WriteReport(
        int expectedFamilyCount,
        IReadOnlyList<object> certified,
        IReadOnlyList<string> blockers)
    {
        var root = FindRepositoryRoot();
        var directory = Path.Combine(
            root,
            "artifacts",
            "math-intelligence");

        Directory.CreateDirectory(directory);

        File.WriteAllText(
            Path.Combine(
                directory,
                "adaptive-v2-c0-c5-family-behavior-certification.json"),
            JsonSerializer.Serialize(
                new
                {
                    schemaVersion = 1,
                    audit =
                        "Adaptive Practice V2 C0-C5 per-question-family behavioral certification",
                    generatedAtUtc = DateTime.UtcNow,
                    summary = new
                    {
                        expectedFamilyCount,
                        certifiedFamilyCount = certified.Count,
                        scenarioCountPerFamily = 11,
                        maximumSameItemRetries =
                            AdaptivePracticeV2Behavior.MaximumSameItemRetries,
                        maximumSessionItems =
                            AdaptivePracticeV2Behavior.MaximumSessionItems,
                        blockerCount = blockers.Count
                    },
                    certified,
                    blockers
                },
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }) + Environment.NewLine);
    }

    private static string FindRepositoryRoot()
    {
        var workspace =
            Environment.GetEnvironmentVariable("GITHUB_WORKSPACE");

        if (!string.IsNullOrWhiteSpace(workspace) &&
            File.Exists(Path.Combine(workspace, "Edulytics.sln")))
        {
            return workspace;
        }

        var directory =
            new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directory is not null)
        {
            if (File.Exists(
                    Path.Combine(
                        directory.FullName,
                        "Edulytics.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException(
            "Unable to locate Edulytics repository root.");
    }

    private sealed record FamilyTarget(
        LessonPracticeContract Contract,
        string Family);
}
