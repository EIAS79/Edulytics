using System.Text.Json;
using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.AdaptivePractice;

namespace Edulytics.Tests.MathematicsIntelligence.AdaptivePractice;

public sealed class AdaptiveV2FullCatalogueCertificationTests
{
    private const int ExpectedLessonCount = 4453;
    private const string RunEnvironmentVariable =
        "EDULYTICS_RUN_FULL_ADAPTIVE_V2_CERTIFICATION";

    [Fact]
    public void EveryReadyVerifiedLessonCanGenerateThroughAdaptiveV2()
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

        var generator =
            new AdaptiveVerifiedItemGenerator();
        var blockers =
            new List<string>();
        var generatedLessonCount = 0;

        for (var index = 0;
             index < contracts.Length;
             index++)
        {
            var contract = contracts[index];
            var family =
                contract.AllowedQuestionFamilies
                    .Distinct(StringComparer.Ordinal)
                    .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(family))
            {
                blockers.Add(
                    $"{contract.LessonCode}: no allowed question family.");
                continue;
            }

            try
            {
                var item =
                    Generate(
                        generator,
                        contract,
                        family,
                        complexity: 42,
                        seed:
                            unchecked(
                                20260929 +
                                (index * 131)),
                        identityIndex:
                            index);

                if (!string.Equals(
                        item.GenerationFamily,
                        family,
                        StringComparison.Ordinal))
                {
                    blockers.Add(
                        $"{contract.LessonCode}: generated family " +
                        $"{item.GenerationFamily} != {family}.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(
                        item.ExposureFingerprint) ||
                    string.IsNullOrWhiteSpace(
                        item.ValidationMetadataJson) ||
                    !item.ValidationMetadataJson.Contains(
                        "solverVerified",
                        StringComparison.Ordinal))
                {
                    blockers.Add(
                        $"{contract.LessonCode}: generated item is missing " +
                        "verified provenance.");
                    continue;
                }

                generatedLessonCount++;
            }
            catch (Exception exception)
            {
                blockers.Add(
                    $"{contract.LessonCode}: " +
                    $"{exception.GetType().Name}: {exception.Message}");
            }
        }

        var familyRepresentatives =
            contracts
                .SelectMany(
                    contract =>
                        contract.AllowedQuestionFamilies
                            .Distinct(StringComparer.Ordinal)
                            .Select(
                                family =>
                                    new
                                    {
                                        Contract = contract,
                                        Family = family
                                    }))
                .GroupBy(
                    row => row.Family,
                    StringComparer.Ordinal)
                .OrderBy(
                    group => group.Key,
                    StringComparer.Ordinal)
                .Select(group => group.First())
                .ToArray();

        var certifiedFamilyCount = 0;

        for (var index = 0;
             index < familyRepresentatives.Length;
             index++)
        {
            var representative =
                familyRepresentatives[index];

            try
            {
                var item =
                    Generate(
                        generator,
                        representative.Contract,
                        representative.Family,
                        complexity:
                            AdaptiveNextItemDecisionEngine
                                .MaximumComplexityScore,
                        seed:
                            unchecked(
                                90260929 +
                                (index * 257)),
                        identityIndex:
                            contracts.Length + index);

                if (!string.Equals(
                        item.GenerationFamily,
                        representative.Family,
                        StringComparison.Ordinal))
                {
                    blockers.Add(
                        $"family {representative.Family}: generated " +
                        $"{item.GenerationFamily}.");
                    continue;
                }

                certifiedFamilyCount++;
            }
            catch (Exception exception)
            {
                blockers.Add(
                    $"family {representative.Family}: " +
                    $"{exception.GetType().Name}: {exception.Message}");
            }
        }

        var summary =
            new CertificationSummary(
                ExpectedLessonCount,
                generatedLessonCount,
                familyRepresentatives.Length,
                certifiedFamilyCount,
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

    private static Edulytics.Core.Entities.AssessmentItem Generate(
        AdaptiveVerifiedItemGenerator generator,
        LessonPracticeContract contract,
        string family,
        int complexity,
        int seed,
        int identityIndex)
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
                    AdaptivePracticeDecisionReasonCodes
                        .SessionBaseline,
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
            excludedExposureFingerprints:
                [],
            excludedSemanticIdentityKeys:
                []);
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
                schemaVersion = 1,
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

    private sealed record CertificationSummary(
        int ExpectedLessonCount,
        int CertifiedLessonCount,
        int DistinctQuestionFamilyCount,
        int CertifiedQuestionFamilyCount,
        int BlockerCount);
}
