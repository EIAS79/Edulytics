using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Versions the nineteen raw Cambridge example corrections introduced in
/// c4a4592d (Supporting Practice remediation). That change retained the pack's
/// original version, so databases seeded earlier correctly reject body drift.
/// Only the exact historical body or the exact reviewed replacement can be
/// promoted; unrelated content and unexpected edits remain fail-closed.
/// </summary>
public static class CambridgeReviewedExampleContentCorrections
{
    public const string CorrectionContentVersion =
        "cambridge-reviewed-examples-20260919-v1";

    private sealed record ReviewedUpgrade(
        string PriorVersion,
        string PriorBodySha256,
        string ReviewedBodySha256);

    private static readonly IReadOnlyDictionary<string, ReviewedUpgrade> Upgrades =
        new Dictionary<string, ReviewedUpgrade>(StringComparer.Ordinal)
        {
            ["PED:CAMBRIDGE-INTL-MATH:S2:2AS-1:APPLY"] = new("phase29-cambridge-primary-stage2-dfe-ogl-v1",
                "c1cbac22ff9cc7bc1dd26a7a0a02a1b339d7d332f41b9e9fdd037a988ff958ea",
                "754dbbe306b1f828301482f2af937487f50d9e3ad61d56caad054fce79bb651f"),
            ["PED:CAMBRIDGE-INTL-MATH:S2:2AS-1:BUILD"] = new("phase29-cambridge-primary-stage2-dfe-ogl-v1",
                "6368092a59325d1b72e53aed9189c09125f507e8a9ade2b569fbc709b01e3a8b",
                "0826580dcfdfe8f205b5328bd4ab11b84181fe1e9afbb9007a968a3ac691661c"),
            ["PED:CAMBRIDGE-INTL-MATH:S2:2AS-3:APPLY"] = new("phase29-cambridge-primary-stage2-dfe-ogl-v1",
                "301e2b912e7d2657737dbb568c40b78476715bfa296e23e8d8de1efb2347ed0f",
                "b0b1dff224cb1d30313a35f4f6cd0c9e06ada8c5bb4b199a3593fcde1b59e203"),
            ["PED:CAMBRIDGE-INTL-MATH:S2:2AS-3:BUILD"] = new("phase29-cambridge-primary-stage2-dfe-ogl-v1",
                "70ca51173a0adf2a05c8dfd19a3107962b636ae400fbaaf61ed46a1117ba953c",
                "8eb0cee480afc64a5d1bf857dc7e9053bb5d16b41d8e86f14dbf1455517826d3"),
            ["PED:CAMBRIDGE-INTL-MATH:S2:2AS-4:APPLY"] = new("phase29-cambridge-primary-stage2-dfe-ogl-v1",
                "6e31aefe9a59957be78510448e2fa21daed05f0b2464c9032f808ab00b4b64c1",
                "6d1c9fc99c7cf4717b7e71fd234c0c6abf531702d6e25421a6bf9c483717dddf"),
            ["PED:CAMBRIDGE-INTL-MATH:S2:2AS-4:BUILD"] = new("phase29-cambridge-primary-stage2-dfe-ogl-v1",
                "fe974cf8afe4c717d420a188b1c20ee7f1c2ff82ae7296ecaf9c41c3b9f2ce91",
                "f95c40028dda9babdd7c16ef63c3d8606178702bb92e0be0a3d800e6de4c5b9d"),
            ["PED:CAMBRIDGE-INTL-MATH:S2:2NF-1:APPLY"] = new("phase29-cambridge-primary-stage2-dfe-ogl-v1",
                "edd8bc2ab0cdaa976992c599af1cb5e226536b70a9232abda60d332b65fc087e",
                "f8a06f1615c946a94edaf08c422c10687f17c281fb0eabd5f39fbd268f913d52"),
            ["PED:CAMBRIDGE-INTL-MATH:S2:2NF-1:BUILD"] = new("phase29-cambridge-primary-stage2-dfe-ogl-v1",
                "0e9d07ec95afc88e2b8c93454f1be19ef783ceb17a46c665eceb7def688322ec",
                "bffe2bcd3ea834d210bb5aa0c3a3d7e4828f3278c8e22d70f97a7ddc43d136c5"),
            ["PED:CAMBRIDGE-INTL-MATH:S3:3AS-1:APPLY"] = new("phase29-cambridge-primary-stage3-dfe-ogl-v1",
                "f10b13aa61d2c5ab4f6689368a0a24c73839bccfaf0d34b5497a75f94a97c2b4",
                "29718cdf7c9b6d0dd1659dc95671f69b1d2ba8940d7872783ee6b62b4a30343b"),
            ["PED:CAMBRIDGE-INTL-MATH:S3:3AS-1:BUILD"] = new("phase29-cambridge-primary-stage3-dfe-ogl-v1",
                "f8401189f161aa68f0439bdbf4417bfc79d20fa5ed7348de2afbf3e3668581fc",
                "0e26e839796642d4cbde13646601af7dd062a394431db1adeec3ae3916558071"),
            ["PED:CAMBRIDGE-INTL-MATH:S3:3NF-1:APPLY"] = new("phase29-cambridge-primary-stage3-dfe-ogl-v1",
                "fa4261ab799105559924fb1f012ca9a477801c42f75b60399d7c84889bdd46f4",
                "2a12f1e86526958145fc6c42e937e137fd6f885fbc9e0d4afbcc9c38608fbfb7"),
            ["PED:CAMBRIDGE-INTL-MATH:S3:3NF-1:BUILD"] = new("phase29-cambridge-primary-stage3-dfe-ogl-v1",
                "98db84a3255ad17417cae7c9e690fab640e4501df5893241ee543710facac276",
                "a743eeda6cc7f00fc35d8cbbdb38e22229080d36c574bed7373d3f875be0e09f"),
            ["PED:CAMBRIDGE-INTL-MATH:S3:3NF-2:APPLY"] = new("phase29-cambridge-primary-stage3-dfe-ogl-v1",
                "e41ea486a95ca7067eb82bcb4751438855ea9f368d511adb153a07abdef2ca32",
                "a7124d62b7af5df2674301c60b3c23ced7db8ee0d524a5251d16aab9ef1dedf4"),
            ["PED:CAMBRIDGE-INTL-MATH:S3:3NF-2:BUILD"] = new("phase29-cambridge-primary-stage3-dfe-ogl-v1",
                "c665099b0a05ce15d55715bf02a7d2c7329c784e07478e3dd6f00afccc9b3c70",
                "0e5cd8a256b09d2ab7766817b0ced6b96f2702c17fe1d3996aad43df8c4b81c5"),
            ["PED:CAMBRIDGE-INTL-MATH:S4:4NF-1:APPLY"] = new("phase29-cambridge-primary-stage4-dfe-ogl-v1",
                "e3b5d35411c25a6ee62adda6f8b0833b2c533b245331066b31ece91232e9b720",
                "0744f660d4670380e030f504a17f3116b14299f38ba68c0d24f313df907a9f65"),
            ["PED:CAMBRIDGE-INTL-MATH:S4:4NF-1:BUILD"] = new("phase29-cambridge-primary-stage4-dfe-ogl-v1",
                "f7377b658fa28feec08f4a99ce6308fe3cd309143a3ae7b032b5a4d3202ebf83",
                "9e84476b594cc690e2e011ee3a88b312a8bdce40040dd084eba3b57d9c25bbe3"),
            ["PED:CAMBRIDGE-INTL-MATH:S4:4NF-2:APPLY"] = new("phase29-cambridge-primary-stage4-dfe-ogl-v1",
                "e19a529a9a5255fd863f9118ed2b7fa9182f6cff16613690acba4f3114fb7a18",
                "9301a968e328b66c1f3e7d9086897430e67fc2719d5fd8ef68cad4418a317861"),
            ["PED:CAMBRIDGE-INTL-MATH:S4:4NF-2:BUILD"] = new("phase29-cambridge-primary-stage4-dfe-ogl-v1",
                "b074e7e326450037e653d290e36e81e36341aebb19b744ae8b69b0ce03e003f4",
                "9c6ec01b4c65127ec38f32da1bbc4050c15d9b2de28a4ecd468e55599f18bb4c"),
            ["PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:BUILD"] = new("phase29-cambridge-primary-stage6-dfe-ogl-v1",
                "b8edf99b5faefd7ad7d1994888d02943faf7d359bd20a8d39cb7d1695f930507",
                "57d03a074fc5dc74dc6453d82c538ee9fa7144529a7a8b085f3644bb4d4b122e"),
        };

    public static bool IsTarget(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        document.PackCode == MathematicsCurriculumPackRegistry.CambridgeCode &&
        document.VersionCode == "CAMBRIDGE-PATHWAY-2026" &&
        Upgrades.TryGetValue(lesson.LessonCode, out var upgrade) &&
        document.ContentVersion == upgrade.PriorVersion;

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string priorExpectedVersion)
    {
        if (!IsTarget(document, lesson))
            return priorExpectedVersion;

        var upgrade = Upgrades[lesson.LessonCode];
        if (lesson.Translations.Count != 1 ||
            lesson.Translations[0].CultureCode != "en" ||
            ComputeBodySha256(lesson.Translations[0]) != upgrade.ReviewedBodySha256)
        {
            throw new InvalidOperationException(
                $"Unreviewed Cambridge example correction: {lesson.LessonCode}.");
        }

        return CorrectionContentVersion;
    }

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingContentVersion) =>
        IsTarget(document, lesson) &&
        existingContentVersion == Upgrades[lesson.LessonCode].PriorVersion;

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingContentVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> translations)
    {
        if (!CanUpgradeExisting(document, lesson, existingContentVersion))
        {
            throw new InvalidOperationException(
                $"Unknown Cambridge example correction version: {lesson.LessonCode}.");
        }

        if (translations.Count != 1 || translations.Single().CultureCode != "en")
        {
            throw new InvalidOperationException(
                $"Unexpected Cambridge example correction cultures: {lesson.LessonCode}.");
        }

        var current = translations.Single();
        var hash = ComputeBodySha256(new CanonicalLessonContentPackTranslation
        {
            CultureCode = current.CultureCode,
            Title = current.Title,
            Explanation = current.Explanation,
            KeyConceptsAndRules = current.KeyConceptsAndRules,
            WorkedExamples = current.WorkedExamples,
            StepByStepSolutions = current.StepByStepSolutions,
            CommonMistakes = current.CommonMistakes,
            QuickSummary = current.QuickSummary
        });
        var upgrade = Upgrades[lesson.LessonCode];

        if (hash != upgrade.PriorBodySha256 && hash != upgrade.ReviewedBodySha256)
        {
            throw new InvalidOperationException(
                $"Unrecognized stored Cambridge lesson body: {lesson.LessonCode}. " +
                "Refusing to overwrite content outside the reviewed historical upgrade.");
        }
    }

    private static string ComputeBodySha256(
        CanonicalLessonContentPackTranslation translation) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(
            string.Join("\n",
                translation.CultureCode,
                translation.Title,
                translation.Explanation,
                translation.KeyConceptsAndRules,
                translation.WorkedExamples,
                translation.StepByStepSolutions,
                translation.CommonMistakes,
                translation.QuickSummary) + "\n")))
            .ToLowerInvariant();
}

