using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Nineteen original-source-anchored corrections of literal HTML &amp;nbsp;
/// spellings in learner worked examples. The 20th previously flagged lesson,
/// G4/U01/L01, already has a complete source-grounded mathematical rewrite.
/// Original publisher files remain unchanged. Not academic sign-off.
/// </summary>
public static class CommonCoreSourceBoundHtmlEntityCorrections
{
    public const string CorrectedContentVersion =
        "us-ccss-source-bound-nbsp-display-v1";

    private sealed record Repair(
        string Code, string SourceUrl, string SourceSha256,
        string OriginalExamplesSha256, int ExpectedEntityCount,
        string[] OutcomeCodes);

    private static readonly Repair[] Repairs =
    [
        new Repair("PED:US-CCSS-MATH:G2:U04:L03", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-3/preparation.html",
            "f52cfba81e09474a951333db5d4a62d83c3145c74d8dd7c5ecbcaadb97bcdb99", "3bdbde96737fab1765d25ff597c09315e49b1e90755e976a3c940d9564f7addf", 1,
            ["CCSS:2.MD.B.6", "CCSS:2.NBT.A.2"]),
        new Repair("PED:US-CCSS-MATH:G2:U06:L19", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-6/lesson-19/preparation.html",
            "5ed030269c542b6f0a1a7bb27c7108712dbb1c15daea05a36e9e424228de2a36", "b1b13677739ad9a3b02b85489259a8b0086660fc9d0fd73a5c6efe0415693b74", 3,
            ["CCSS:2.MD.C.8", "CCSS:2.NBT.B.5", "CCSS:2.OA.A.1"]),
        new Repair("PED:US-CCSS-MATH:G2:U09:L04", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-9/lesson-4/preparation.html",
            "39879a4c0f2c36d601282549dce30b6d37eb3fc19f757d10f3dd35eb87d55272", "dfc212ad39f80fde03263b0a20566552c2472271997a7418a5826caca177445a", 2,
            ["CCSS:2.MD.A.1", "CCSS:2.MD.D.9", "CCSS:2.NBT.B.5", "CCSS:2.OA.B.2"]),
        new Repair("PED:US-CCSS-MATH:G4:U02:L03", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-2/lesson-3/preparation.html",
            "e6adaba46979c40b780994cdcf7abd16d0d54f5b12e2f8b585fbe32897685985", "62ad8279648d6e06a607c59599dabf26c2c837a6273eacd28b0c1d2f3dd8b971", 1,
            ["CCSS:4.NBT.B.4", "CCSS:4.NF.A.2"]),
        new Repair("PED:US-CCSS-MATH:G4:U02:L17", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-2/lesson-17/preparation.html",
            "36088c8ecb3463a6d2297d5bb1cd32ed4920bf2ecb7758cae6c89b4ee323eb2d", "f389f4e5b1ccfd0b9f1bdaec677d77509af334f0446ba949df0479768c323c66", 1,
            ["CCSS:MP.4", "CCSS:MP.6"]),
        new Repair("PED:US-CCSS-MATH:G4:U05:L04", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-5/lesson-4/preparation.html",
            "f4828584eabc336f9605cf2d606338f829e64062aa685a98335ef37e6ce3d224", "09660313d05d27646356ad8b1dc12618f600116c7ecd9dd543a7e2e710c681eb", 1,
            ["CCSS:4.OA.A.1", "CCSS:4.OA.A.2"]),
        new Repair("PED:US-CCSS-MATH:G4:U06:L01", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-6/lesson-1/preparation.html",
            "041f3a9f4d1101a1e0972dc6f7fddd4018551a27255d59716b53335e258c266c", "11970532b70910fdccfb09c8216eaf14792577409f0221f7804d7397dcbeb0fd", 1,
            ["CCSS:4.OA.C.5"]),
        new Repair("PED:US-CCSS-MATH:G4:U06:L22", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-6/lesson-22/preparation.html",
            "8645cb7159aedb37ed535a2e544dd5189d2c7addc7ea36058926a8e33d504b30", "afd6eb4c15be01e7f6cb15556651c6affcd124c8291d6f3395890e815e784ac3", 1,
            ["CCSS:4.MD.A.2", "CCSS:4.MD.A.3", "CCSS:4.OA.A.3"]),
        new Repair("PED:US-CCSS-MATH:G4:U06:L25", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-6/lesson-25/preparation.html",
            "4fdfedde9a3eb1d872ddad045dbdff37cf7d12fec42008ff7dde655246cf5280", "5a2d26ac8aaa617b6d8fa5c8014aeaa183db15a176818492a9d6d4627e342144", 1,
            ["CCSS:4.OA.A.3", "CCSS:4.OA.C.5"]),
        new Repair("PED:US-CCSS-MATH:G4:U07:L04", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-7/lesson-4/preparation.html",
            "c173762449a6c0af76c7e5139c899a67d0cde32e81dfb87828504f01b105d2d8", "81725c84bbca9b859d2190d9cc57d5a466a4c39fc3a13b11b1a25a768f35fe14", 1,
            ["CCSS:4.G.A.1"]),
        new Repair("PED:US-CCSS-MATH:G4:U07:L14", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-7/lesson-14/preparation.html",
            "9ec17466686b008c40dac25694462d655a6339b0f7b4df65da90ef964c9b6909", "6a4bbfec28c43e5b6fc94945e8ec75a308befe9cafb107fa1d822aff5b5e98b9", 1,
            ["CCSS:4.MD.C.6", "CCSS:4.MD.C.7"]),
        new Repair("PED:US-CCSS-MATH:G5:U01:L10", "https://im.kendallhunt.com/k5/teachers/grade-5/unit-1/lesson-10/preparation.html",
            "4744e4e4ac2c8160b9c2be9df08d8e60f58e9aa1a7d84fcf4814b0f3b79790f2", "ab38cd9edbd8424883bcefbcc198ba67c007cfba935aa690124cb40b0523cadf", 2,
            ["CCSS:5.MD.C.5", "CCSS:5.OA.A.1", "CCSS:5.OA.A.2"]),
        new Repair("PED:US-CCSS-MATH:G5:U04:L14", "https://im.kendallhunt.com/k5/teachers/grade-5/unit-4/lesson-14/preparation.html",
            "a77ee7ad4e2aaddecc799e82d71962cf2044aaa55c6d09f793a3014e46f5b488", "e71d960cbdc0e60701dea727c32c166f36be190d7095950f73ebd06867a760f3", 1,
            ["CCSS:5.NBT.B.6"]),
        new Repair("PED:US-CCSS-MATH:G5:U05:L11", "https://im.kendallhunt.com/k5/teachers/grade-5/unit-5/lesson-11/preparation.html",
            "ea865686d66650bc0839c02be784beae1869cb53f87b2f98f2a8bd2988d7125c", "f1c0db2f741b930c3c178c62ae9a538d91df106bc4a8cb77d9a5cecdf6d291df", 1,
            ["CCSS:5.NBT.B.7"]),
        new Repair("PED:US-CCSS-MATH:G5:U05:L25", "https://im.kendallhunt.com/k5/teachers/grade-5/unit-5/lesson-25/preparation.html",
            "021f427804bd438ea06b81d53b3c7ceb316f0f6c1fe4f45e75af09d4c989fb05", "fc5b459251c70614b6af219e0cf035c5ca32984a1df090a9805f75ad410539f4", 3,
            ["CCSS:5.NBT.B.7"]),
        new Repair("PED:US-CCSS-MATH:G5:U06:L08", "https://im.kendallhunt.com/k5/teachers/grade-5/unit-6/lesson-8/preparation.html",
            "40e8aea0a28687ee8134aafec5b3244fe6f3a6b0fb897d4ae9b380c33f8c2339", "3cee4147ac5ef353275068f50c3c524eb41edee97325bcf8977db9afad3db241", 2,
            ["CCSS:5.NF.A.1"]),
        new Repair("PED:US-CCSS-MATH:G5:U08:L02", "https://im.kendallhunt.com/k5/teachers/grade-5/unit-8/lesson-2/preparation.html",
            "e8781c426e8967fdcd229d94b8d9b08cc29d4e07090f29ca4c76eefb1b6c1202", "611dda8aaef8d1122a111b24bb4c2f5b56a89dd39efd7e0e99c9d5984daa343a", 1,
            ["CCSS:5.NBT.B.5"]),
        new Repair("PED:US-CCSS-MATH:G5:U08:L05", "https://im.kendallhunt.com/k5/teachers/grade-5/unit-8/lesson-5/preparation.html",
            "2fe223a94bf1765ab6faf5c5bbbf10a810d5439b1b40ddce34d54f3b960fa2bb", "633ef3431611fd4e5f32758cca51ac3474542301e791f7f5bc37321f7b758f10", 2,
            ["CCSS:5.NBT.B.6"]),
        new Repair("PED:US-CCSS-MATH:G5:U08:L09", "https://im.kendallhunt.com/k5/teachers/grade-5/unit-8/lesson-9/preparation.html",
            "170f76febe61ca9a9188be5a618211c71bc711c0822d004d0c9e424871938b53", "0a4266ecffa20bd0359182c24b24ef81d2c06545c44d440b7fabd1fd43117ab9", 1,
            ["CCSS:5.MD.C.5"])
    ];

    public static IReadOnlyCollection<string> TargetLessonCodes =>
        Repairs.Select(x => x.Code).ToArray();

    private static Repair? Find(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        document.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
        document.VersionCode == "CCSSM-2010" &&
        document.ContentVersion == "phase30-common-core-rich-content-v1" &&
        document.AcademicLanguage == "en" &&
        !document.CurriculumTranslationRequired
            ? // Source-specific later revisions supersede the old display-only
              // HTML normalization for these exactly identified original lessons.
              // Their newer rich examples do not reproduce the original &nbsp;
              // string, and each replacement is separately source-SHA guarded.
              (CommonCoreG2Unit4UnlabeledTicksTeachingCorrection.IsTarget(document,lesson) ||
               CommonCoreG2Unit6SourceCorrections.IsTarget(document,lesson) ||
               CommonCoreG2Unit9SourceCorrections.IsTarget(document,lesson)
                  ? null : Repairs.SingleOrDefault(x => x.Code == lesson.LessonCode))
            : null;

    public static bool IsTarget(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        Find(document, lesson) is not null;

    private static void AssertIdentity(
        CanonicalLessonContentPackLesson lesson, Repair repair)
    {
        if (lesson.SourceUrl != repair.SourceUrl ||
            !string.Equals(lesson.CanonicalBodySha256,
                repair.SourceSha256, StringComparison.OrdinalIgnoreCase) ||
            !lesson.OutcomeCodes.SequenceEqual(repair.OutcomeCodes))
            throw new InvalidOperationException(
                "Source and official outcome drift in HTML entity correction: " +
                lesson.LessonCode);
    }

    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        foreach (var lesson in document.Lessons)
        {
            var repair = Find(document, lesson);
            if (repair is null)
                continue;

            AssertIdentity(lesson, repair);
            var english = lesson.Translations.Single(x => x.CultureCode == "en");
            var beforeHash = Sha(english.WorkedExamples);
            var alreadyCorrected = !english.WorkedExamples.Contains("&nbsp;",
                StringComparison.Ordinal);

            if (alreadyCorrected)
            {
                // Only accept an idempotent second pass if reverting exactly
                // these ASCII spaces would reconstruct the expected old hash.
                // Full corrected-body hash is derived by validating the old
                // hash on first pass and remains checked via canonical tests.
                if (!string.Equals(beforeHash,
                        CorrectedShaFor(repair), StringComparison.Ordinal))
                    throw new InvalidOperationException(
                        "Previously corrected HTML entity examples have drifted.");
                continue;
            }

            if (beforeHash != repair.OriginalExamplesSha256 ||
                english.WorkedExamples.Split("&nbsp;", StringSplitOptions.None)
                    .Length != repair.ExpectedEntityCount + 1)
                throw new InvalidOperationException(
                    "Unexpected original HTML entity worked examples: " +
                    lesson.LessonCode);

            english.WorkedExamples =
                english.WorkedExamples.Replace("&nbsp;", " ",
                    StringComparison.Ordinal);

            if (Sha(english.WorkedExamples) != CorrectedShaFor(repair))
                throw new InvalidOperationException(
                    "HTML entity source-bound post-rewrite hash mismatch.");
        }
    }

    private static string CorrectedShaFor(Repair repair)
    {
        // SHA-256 of the exact ASCII-space-normalized original examples.
        // Manifest pins the authorized corrected output independently.
        return CorrectedHashes[repair.Code];
    }

    private static readonly IReadOnlyDictionary<string, string> CorrectedHashes =
        new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["PED:US-CCSS-MATH:G2:U04:L03"] = "78f6c2bdb22e9b677ba6d72200d35af530b41258f539073bdc4cfbadafdbee94",
            ["PED:US-CCSS-MATH:G2:U06:L19"] = "423da6bb730026eea9f9c076217da3ee2bac7527899f0b3742fcf876662faa37",
            ["PED:US-CCSS-MATH:G2:U09:L04"] = "efced304cb03f32c9e19197c26f1c94bae182d3251a07bde662bc21f7a3b4398",
            ["PED:US-CCSS-MATH:G4:U02:L03"] = "a32313a9aa0f64d28835d3cc9dedd8838d1f0e2fce4b67edc58e547440a1bd87",
            ["PED:US-CCSS-MATH:G4:U02:L17"] = "1281bf1f6dcffb3a8362ebec09817053a9d8598e36b6d1ab3ec05b9b003f6cdf",
            ["PED:US-CCSS-MATH:G4:U05:L04"] = "ea94c86f1a69c73b092330dd454ce4aeeec81859f0c4b976b47cb57a3b3bf351",
            ["PED:US-CCSS-MATH:G4:U06:L01"] = "690d5175863b7967f8e39932b947733a3afb72c7ab42430ca61ea4e6bda1be30",
            ["PED:US-CCSS-MATH:G4:U06:L22"] = "85011bad7e895e91bafa662bed14714b8f99f2682f36c007d3d5582ba85c3724",
            ["PED:US-CCSS-MATH:G4:U06:L25"] = "88b64983592d9427c1a265ea5b4527b69736d1f54f7aa0fe180e0a0764489f7a",
            ["PED:US-CCSS-MATH:G4:U07:L04"] = "2bb53c5ebc533b5025f72d0a2b7657a7367f7e2639b73af7bf0e926b8c96485e",
            ["PED:US-CCSS-MATH:G4:U07:L14"] = "88b8494448d193081fcba1ef3eff8f38839537c2e47e22aed56b1c446615234b",
            ["PED:US-CCSS-MATH:G5:U01:L10"] = "96052fc4d70ae3cbf4d8509e41cc0652c52663cb263e81aa85406fd9ce066a73",
            ["PED:US-CCSS-MATH:G5:U04:L14"] = "4c3f66ac1ce8c89496f45b51c03284cbb47cd66aac25b423817eb6501eb6b001",
            ["PED:US-CCSS-MATH:G5:U05:L11"] = "eca0e68663a3d766f5db6cea4547fc81c86ebef726b4e01e73e6a6ed01b487dc",
            ["PED:US-CCSS-MATH:G5:U05:L25"] = "2e8a43400e3fc84eb2cc8be9a569dc26373620fd104df2ef3f1b8e54e15f0dc0",
            ["PED:US-CCSS-MATH:G5:U06:L08"] = "3b5e2ed71377af541a3814891414c7e2731e0ea2ada5513737c0f1c111985125",
            ["PED:US-CCSS-MATH:G5:U08:L02"] = "d0c7f42fa1873f0097611640ea403b0d0fd0c91c1204d65e405faff4a13b2510",
            ["PED:US-CCSS-MATH:G5:U08:L05"] = "bc4a7535493bd0182ff43ff0a73ce284e1523651125dda96c5daaa15d6ba74e6",
            ["PED:US-CCSS-MATH:G5:U08:L09"] = "c7cfe072412f702bf8724f4e2b86a9032dd0621059fee5214fea028f6d13f8ac"
        };

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string priorVersion) =>
        IsTarget(document, lesson) ? CorrectedContentVersion : priorVersion;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string priorVersion) =>
        IsTarget(document, lesson) &&
        priorVersion == CommonCoreRichContentCorrections.CorrectionContentVersion;

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persistedTranslations)
    {
        var repair = Find(document, lesson) ??
            throw new InvalidOperationException(
                "Not a source-bound Common Core entity correction target.");
        AssertIdentity(lesson, repair);
        if (!CanUpgradeExisting(document, lesson, existingVersion))
            throw new InvalidOperationException(
                "Entity correction requires exact original content version.");

        // ReadFromJson production contains metadata, not persisted lesson prose.
        if (persistedTranslations.Count == 0)
            return;

        if (persistedTranslations.Count != 1 ||
            persistedTranslations.Single().CultureCode != "en")
            throw new InvalidOperationException(
                "Unexpected persisted translation set for HTML entity correction.");

        var row = persistedTranslations.Single();
        var expected = lesson.Translations.Single(x => x.CultureCode == "en");
        if (row.Title != expected.Title ||
            row.Explanation != expected.Explanation ||
            row.KeyConceptsAndRules != expected.KeyConceptsAndRules ||
            row.StepByStepSolutions != expected.StepByStepSolutions ||
            row.CommonMistakes != expected.CommonMistakes ||
            row.QuickSummary != expected.QuickSummary)
            throw new InvalidOperationException(
                "Persisted learner prose drift outside entity-only patch.");

        var persistedHash = Sha(row.WorkedExamples);
        if (persistedHash != repair.OriginalExamplesSha256 &&
            persistedHash != CorrectedShaFor(repair))
            throw new InvalidOperationException(
                "Persisted learner worked examples failed exact SHA-256 gate.");
    }

    private static string Sha(string text) =>
        Convert.ToHexString(SHA256.HashData(
            Encoding.UTF8.GetBytes(text))).ToLowerInvariant();
}
