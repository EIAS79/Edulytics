using System.Text.Json;
using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Entities;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.AdaptivePractice;

namespace Edulytics.Tests.MathematicsIntelligence.AdaptivePractice;

public sealed class AdaptiveV2FullCatalogueCertificationTests
{
    private const int ExpectedLessonCount = 4453;
    private const int CertifiedSessionLength = 8;
    private const string RunEnvironmentVariable =
        "EDULYTICS_RUN_FULL_ADAPTIVE_V2_CERTIFICATION";

    [Fact]
    public void EveryReadyVerifiedLessonAndAllowedFamilyCanRunThroughAdaptiveV2()
    {
        if (!string.Equals(
                Environment.GetEnvironmentVariable(
                    RunEnvironmentVariable),
                "1",
                StringComparison.Ordinal))
        {
            return;
        }

        var contracts =
            LessonPracticeContractRegistry.All
                .OrderBy(
                    contract => contract.LessonCode,
                    StringComparer.Ordinal)
                .ToArray();

        Assert.Equal(
            ExpectedLessonCount,
            contracts.Length);

        var pairs =
            contracts
                .SelectMany(
                    contract =>
                        contract.AllowedQuestionFamilies
                            .Distinct(StringComparer.Ordinal)
                            .OrderBy(
                                family => family,
                                StringComparer.Ordinal)
                            .Select(
                                family =>
                                    new ContractFamilyPair(
                                        contract,
                                        family)))
                .ToArray();

        var distinctFamilyCount =
            pairs
                .Select(pair => pair.Family)
                .Distinct(StringComparer.Ordinal)
                .Count();

        var generator =
            new AdaptiveVerifiedItemGenerator();
        var blockers =
            new List<string>();

        var certifiedPairCount = 0;

        // Every lesson-family pairing is a possible adaptive target. Certify
        // the exact pairing at maximum requested complexity; the generator is
        // responsible for truthfully bounding to the family's supported form.
        for (var index = 0;
             index < pairs.Length;
             index++)
        {
            var pair = pairs[index];

            try
            {
                var item =
                    Generate(
                        generator,
                        pair.Contract,
                        pair.Family,
                        complexity:
                            AdaptiveNextItemDecisionEngine
                                .MaximumComplexityScore,
                        seed:
                            unchecked(
                                90260929 +
                                (index * 257)),
                        identityIndex:
                            index,
                        sequence:
                            1,
                        excludedExposureFingerprints:
                            [],
                        excludedSemanticIdentityKeys:
                            []);

                ValidateGeneratedItem(
                    pair.Contract,
                    pair.Family,
                    item);

                certifiedPairCount++;
            }
            catch (Exception exception)
            {
                blockers.Add(
                    $"{pair.Contract.LessonCode} / {pair.Family}: " +
                    $"{exception.GetType().Name}: {exception.Message}");
            }
        }

        var certifiedSessionCount = 0;
        var generatedSessionItemCount = 0;

        // A one-item smoke test cannot prove runtime freshness. For every
        // lesson, generate a complete default learner session while carrying
        // exposure fingerprints and semantic identities forward exactly as
        // the real Adaptive V2 service does. Families rotate deterministically
        // so multi-family contracts exercise more than their first family.
        for (var contractIndex = 0;
             contractIndex < contracts.Length;
             contractIndex++)
        {
            var contract = contracts[contractIndex];
            var families =
                contract.AllowedQuestionFamilies
                    .Distinct(StringComparer.Ordinal)
                    .OrderBy(
                        family => family,
                        StringComparer.Ordinal)
                    .ToArray();

            if (families.Length == 0)
            {
                blockers.Add(
                    $"{contract.LessonCode}: no allowed question family.");
                continue;
            }

            var exposureExclusions =
                new HashSet<string>(
                    StringComparer.Ordinal);
            var semanticExclusions =
                new HashSet<string>(
                    StringComparer.Ordinal);

            var sessionSucceeded = true;

            for (var sequence = 1;
                 sequence <= CertifiedSessionLength;
                 sequence++)
            {
                var family =
                    families[
                        (sequence - 1) %
                        families.Length];

                try
                {
                    var item =
                        Generate(
                            generator,
                            contract,
                            family,
                            complexity:
                                sequence <= 4
                                    ? 42
                                    : AdaptiveNextItemDecisionEngine
                                        .MaximumComplexityScore,
                            seed:
                                unchecked(
                                    20260929 +
                                    (contractIndex * 1009) +
                                    (sequence * 131)),
                            identityIndex:
                                pairs.Length +
                                contractIndex,
                            sequence,
                            exposureExclusions,
                            semanticExclusions);

                    ValidateGeneratedItem(
                        contract,
                        family,
                        item);

                    if (!string.IsNullOrWhiteSpace(
                            item.ExposureFingerprint))
                    {
                        if (!exposureExclusions.Add(
                                item.ExposureFingerprint))
                        {
                            throw new InvalidOperationException(
                                "Adaptive session repeated an excluded exposure fingerprint.");
                        }
                    }

                    var semanticIdentity =
                        AdaptivePracticeSemanticIdentity
                            .Resolve(item);

                    if (!string.IsNullOrWhiteSpace(
                            semanticIdentity) &&
                        !semanticExclusions.Add(
                            semanticIdentity))
                    {
                        throw new InvalidOperationException(
                            "Adaptive session repeated an excluded semantic identity.");
                    }

                    generatedSessionItemCount++;
                }
                catch (Exception exception)
                {
                    blockers.Add(
                        $"{contract.LessonCode} session turn {sequence} " +
                        $"({family}): {exception.GetType().Name}: " +
                        exception.Message);
                    sessionSucceeded = false;
                    break;
                }
            }

            if (sessionSucceeded)
                certifiedSessionCount++;
        }

        var summary =
            new CertificationSummary(
                ExpectedLessonCount,
                certifiedSessionCount,
                CertifiedSessionLength,
                generatedSessionItemCount,
                pairs.Length,
                certifiedPairCount,
                distinctFamilyCount,
                blockers.Count);

        WriteReport(
            summary,
            blockers);

        Assert.True(
            blockers.Count == 0,
            "Adaptive V2 full catalogue certification blockers: " +
            string.Join(
                " | ",
                blockers.Take(100)) +
            (blockers.Count > 100
                ? $" (+{blockers.Count - 100} more)"
                : string.Empty));
    }

    private static AssessmentItem Generate(
        AdaptiveVerifiedItemGenerator generator,
        LessonPracticeContract contract,
        string family,
        int complexity,
        int seed,
        int identityIndex,
        int sequence,
        IReadOnlyCollection<string> excludedExposureFingerprints,
        IReadOnlyCollection<string> excludedSemanticIdentityKeys)
    {
        var decision =
            new AdaptiveNextItemDecision(
                TargetSkillId:
                    contract.SkillId,
                TargetComplexityScore:
                    complexity,
                TargetQuestionFamily:
                    family,
                TargetRepresentation:
                    "symbolic",
                MisconceptionFocusId:
                    null,
                ReasonCode:
                    sequence == 1
                        ? AdaptivePracticeDecisionReasonCodes
                            .SessionBaseline
                        : AdaptivePracticeDecisionReasonCodes
                            .ComplexityConsolidate,
                RequiresFreshExposure:
                    true,
                RemediationLockActive:
                    false,
                ConfirmationRequired:
                    false,
                IsIndependentConfirmation:
                    false,
                ProgressionEligible:
                    false,
                EngineVersion:
                    AdaptivePracticeV2Versions
                        .EngineVersion,
                PolicyVersion:
                    AdaptivePracticeV2Versions
                        .PolicyVersion);

        return generator.GenerateOne(
            DeterministicGuid(
                identityIndex,
                1),
            DeterministicGuid(
                identityIndex,
                2),
            DeterministicGuid(
                identityIndex,
                3),
            DeterministicGuid(
                identityIndex,
                4),
            contract,
            decision,
            seed,
            excludedExposureFingerprints,
            excludedSemanticIdentityKeys);
    }

    private static void ValidateGeneratedItem(
        LessonPracticeContract contract,
        string expectedFamily,
        AssessmentItem item)
    {
        if (!string.Equals(
                item.GenerationFamily,
                expectedFamily,
                StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Generated family {item.GenerationFamily} " +
                $"did not preserve {expectedFamily}.");
        }

        if (string.IsNullOrWhiteSpace(
                item.ExposureFingerprint) ||
            string.IsNullOrWhiteSpace(
                item.ValidationMetadataJson) ||
            !item.ValidationMetadataJson.Contains(
                "solverVerified",
                StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Generated item for {contract.LessonCode} " +
                "is missing solver-verified provenance.");
        }
    }

    private static Guid DeterministicGuid(
        int index,
        byte discriminator)
    {
        Span<byte> bytes =
            stackalloc byte[16];

        BitConverter.TryWriteBytes(
            bytes[..4],
            index + 1);

        bytes[8] =
            discriminator;
        bytes[15] =
            1;

        return new Guid(bytes);
    }

    private static void WriteReport(
        CertificationSummary summary,
        IReadOnlyList<string> blockers)
    {
        var root =
            FindRepositoryRoot();
        var outputDirectory =
            Path.Combine(
                root,
                "artifacts",
                "math-intelligence");

        Directory.CreateDirectory(
            outputDirectory);

        var path =
            Path.Combine(
                outputDirectory,
                "adaptive-v2-full-catalogue-certification.json");

        var payload =
            new
            {
                schemaVersion = 2,
                audit =
                    "Adaptive Practice V2 full READY_VERIFIED catalogue runtime certification",
                generatedAtUtc =
                    DateTime.UtcNow,
                summary,
                blockers
            };

        File.WriteAllText(
            path,
            JsonSerializer.Serialize(
                payload,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }) +
            Environment.NewLine);
    }

    private static string FindRepositoryRoot()
    {
        var workspace =
            Environment.GetEnvironmentVariable(
                "GITHUB_WORKSPACE");

        if (!string.IsNullOrWhiteSpace(
                workspace) &&
            File.Exists(
                Path.Combine(
                    workspace,
                    "Edulytics.sln")))
        {
            return workspace;
        }

        var directory =
            new DirectoryInfo(
                Directory.GetCurrentDirectory());

        while (directory is not null)
        {
            if (File.Exists(
                    Path.Combine(
                        directory.FullName,
                        "Edulytics.sln")))
            {
                return directory.FullName;
            }

            directory =
                directory.Parent;
        }

        throw new InvalidOperationException(
            "Unable to locate Edulytics repository root " +
            "for Adaptive V2 certification report.");
    }

    private sealed record ContractFamilyPair(
        LessonPracticeContract Contract,
        string Family);

    private sealed record CertificationSummary(
        int ExpectedLessonCount,
        int CertifiedSessionCount,
        int CertifiedSessionLength,
        int GeneratedSessionItemCount,
        int ExpectedLessonFamilyPairCount,
        int CertifiedLessonFamilyPairCount,
        int DistinctQuestionFamilyCount,
        int BlockerCount);
}
