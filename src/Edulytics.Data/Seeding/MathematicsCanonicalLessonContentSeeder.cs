using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Imports reviewed Edulytics canonical lesson bodies.
///
/// It never creates official curriculum identities or alignments.
/// Pedagogical LessonCode and OutcomeCodes must already exist and must match
/// the accepted official curriculum graph exactly.
/// </summary>
public sealed class MathematicsCanonicalLessonContentSeeder
{
    private const long AdvisoryLockKey = 27500029;

    private static readonly JsonSerializerOptions JsonOptions =
        new()
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

    private readonly EdulyticsDbContext _db;

    public MathematicsCanonicalLessonContentSeeder(
        EdulyticsDbContext db) =>
        _db = db;

    public async Task SeedAsync(
        CancellationToken ct = default)
    {
        var documents = LoadEmbeddedDocuments();

        // No reviewed runtime pack is safer than fabricated content.
        // Phase 29 remains open until approved packs are supplied.
        if (documents.Count == 0)
            return;

        await SeedDocumentsAsync(documents, ct);
    }

    /// <summary>
    /// For a newly created clean database: seed validated canonical identities,
    /// publication state and version without duplicating JSON lesson prose in
    /// PostgreSQL. Only use together with ReadFromJson=true.
    /// </summary>
    public async Task SeedMetadataOnlyAsync(CancellationToken ct = default)
    {
        var documents = LoadEmbeddedDocuments();
        if (documents.Count == 0)
            return;

        await SeedDocumentsAsync(documents, ct, metadataOnly: true);
    }

    /// <summary>
    /// Metadata-only reconciliation for the narrowly source-hash-guarded CCSS
    /// editorial candidates when ReadFromJson=true and the host intentionally
    /// skips full startup data maintenance. Never insert lesson prose or update
    /// learner, adoption or assessment records. Existing PostgreSQL content
    /// identities/versions and the absence of persisted translations must be
    /// checked BEFORE any version mutation; the actual seeder uses one
    /// advisory-locked transaction and revalidates outcomes and source hashes.
    /// This is a technical content release, not academic certification.
    /// </summary>
    /// <summary>
    /// Production ReadFromJson may contain only the V1 metadata marker for this
    /// original-source-reviewed lesson.  This exception must never authorize
    /// overwriting persisted student-visible lesson translations.
    /// </summary>
    public static bool IsGuardedG1U04L21JsonMetadataPredecessor(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingContentVersion,
        string expectedContentVersion,
        bool verifiedNoPersistedTranslations) =>
        verifiedNoPersistedTranslations &&
        document.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
        document.VersionCode == "CCSSM-2010" &&
        lesson.LessonCode ==
            CommonCoreG1SourceGroundedInstructionalCorrections.CompareRepresentedNumbersLessonCode &&
        CommonCoreG1SourceGroundedInstructionalCorrections.IsTarget(document, lesson) &&
        existingContentVersion ==
            CommonCoreG1SourceGroundedInstructionalCorrections.CompareRepresentedNumbersVersionV1 &&
        expectedContentVersion ==
            CommonCoreG1SourceGroundedInstructionalCorrections.CompareRepresentedNumbersVersion;

    /// <summary>
    /// Accept only the 2026-10-23 read-only-verified Khalid production JSON
    /// metadata predecessors for source-aligned Grade 2 lessons. All candidates
    /// still require the canonical pack's pinned 2021 lesson identity, source
    /// materializer version, exact stored predecessor, status and absence of
    /// persisted translations. This does not approve human classroom work.
    /// </summary>
    /// <summary>
    /// The 2026-10-26 production SELECT-only inventory confirmed exactly two
    /// Grade 3 B001-corrected JSON metadata predecessors (U01:L12, U01:L15).
    /// This is not a general Grade 3 migration: allow these exact old markers
    /// only when the pinned 2021 Grade 3 source hash/edition and effective
    /// Rich V2 successor match, and ONLY after verifying no persisted prose.
    /// </summary>
    public static bool IsGuardedG3KnownProductionJsonMetadataPredecessor(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingContentVersion,
        string expectedContentVersion,
        bool verifiedNoPersistedTranslations) =>
        verifiedNoPersistedTranslations &&
        document.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
        document.VersionCode == "CCSSM-2010" &&
        document.ContentVersion == CommonCoreG3SourceTeachingCorrections.BaseContentVersion &&
        (lesson.LessonCode is
            "PED:US-CCSS-MATH:G3:U01:L12" or
            "PED:US-CCSS-MATH:G3:U01:L15") &&
        CommonCoreG3SourceTeachingCorrections.IsTarget(document, lesson) &&
        existingContentVersion == "us-ccss-b001-original-source-goals-repaired-v1" &&
        expectedContentVersion ==
            CommonCoreG3SourceTeachingCorrections.CorrectionContentVersion;

    /// <summary>
    /// The 2026-10-28 SELECT-only Neon production inventory verified that
    /// G4 U01:L01 still holds the prior 2021 source correction marker and
    /// contains zero persisted lesson translations. Allow ONLY this exact
    /// source-SHA-bound transition to the reviewed owner-authored A-B-C body.
    /// Other G4 lessons, unexpected predecessor versions and saved prose
    /// must remain fail closed. This is metadata-only, not a copy of publisher work.
    /// </summary>
    public static bool IsGuardedG4KnownProductionJsonMetadataPredecessor(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingContentVersion,
        string expectedContentVersion,
        bool verifiedNoPersistedTranslations) =>
        verifiedNoPersistedTranslations &&
        document.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
        document.VersionCode == "CCSSM-2010" &&
        document.ContentVersion == "phase30-common-core-rich-content-v1" &&
        lesson.LessonCode == CommonCoreG4MultiplesSourceCorrection.LessonCode &&
        lesson.SourceUrl ==
            "https://im.kendallhunt.com/k5/teachers/grade-4/unit-1/lesson-1/preparation.html" &&
        lesson.CanonicalBodySha256 ==
            "b18e1bed2b1e3ad2d363bf493e5327be0f117e834958a6779ef018cc8c2e8b81" &&
        lesson.OutcomeCodes.SequenceEqual(["CCSS:4.OA.B.4"]) &&
        existingContentVersion ==
            CommonCoreG4MultiplesSourceCorrection.CorrectedContentVersion &&
        expectedContentVersion ==
            CommonCoreG4MultiplesSourceCorrection.Reviewed2021AuthoringContentVersion &&
        CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
            document, lesson) == expectedContentVersion;

    public static bool IsGuardedG2KnownProductionJsonMetadataPredecessor(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingContentVersion,
        string expectedContentVersion,
        bool verifiedNoPersistedTranslations)
    {
        if (!verifiedNoPersistedTranslations ||
            document.PackCode != MathematicsCurriculumPackRegistry.CommonCoreCode ||
            document.VersionCode != "CCSSM-2010" ||
            document.ContentVersion != "phase30-common-core-rich-content-v1" ||
            !lesson.SourceUrl.StartsWith(
                "https://im.kendallhunt.com/k5/teachers/grade-2/",
                StringComparison.Ordinal) ||
            lesson.CanonicalBodySha256.Length != 64)
            return false;

        const string oldB001 = "us-ccss-b001-original-source-goals-repaired-v1";
        const string oldDisplay = "us-ccss-source-bound-nbsp-display-v1";
        const string oldFoundation = "us-ccss-g2-source-grounded-foundations-v1";
        const string oldHundreds = "us-ccss-g2-u05-hundreds-source-goal-v1";
        const string oldStory11 =
            "us-ccss-g2-u09-l11-tape-diagram-relationships-correction-v1";
        const string oldStory12 =
            "us-ccss-g2-u09-l12-story-writing-source-correction-v1";
        const string oldStory13 =
            "us-ccss-g2-u09-l13-solve-represent-story-correction-v1";

        // These exact lesson-code/old-version combinations were observed by a
        // SELECT-only inventory. No unknown, duplicate, or generic predecessor
        // is eligible for this exception.
        var documentedPredecessor = lesson.LessonCode switch
        {
            "PED:US-CCSS-MATH:G2:U02:L01" => oldB001,
            "PED:US-CCSS-MATH:G2:U03:L12" => oldB001,
            "PED:US-CCSS-MATH:G2:U04:L03" => oldDisplay,
            "PED:US-CCSS-MATH:G2:U05:L01" => oldHundreds,
            "PED:US-CCSS-MATH:G2:U05:L02" => oldHundreds,
            "PED:US-CCSS-MATH:G2:U05:L03" => oldHundreds,
            "PED:US-CCSS-MATH:G2:U06:L07" => oldFoundation,
            "PED:US-CCSS-MATH:G2:U06:L08" => oldFoundation,
            "PED:US-CCSS-MATH:G2:U06:L09" => oldFoundation,
            "PED:US-CCSS-MATH:G2:U06:L19" => oldDisplay,
            "PED:US-CCSS-MATH:G2:U09:L04" => oldDisplay,
            "PED:US-CCSS-MATH:G2:U09:L05" => oldFoundation,
            "PED:US-CCSS-MATH:G2:U09:L11" => oldStory11,
            "PED:US-CCSS-MATH:G2:U09:L12" => oldStory12,
            "PED:US-CCSS-MATH:G2:U09:L13" => oldStory13,
            _ => null
        };
        if (documentedPredecessor is null ||
            existingContentVersion != documentedPredecessor)
            return false;

        const string noMatchedSourceTarget = "__g2-no-source-target__";
        var sourceBoundExpected = lesson.LessonCode switch
        {
            "PED:US-CCSS-MATH:G2:U02:L01" when
                CommonCoreG2Unit2AddSubtractCompareTeachingCorrection.IsTarget(
                    document, lesson) =>
                CommonCoreG2Unit2AddSubtractCompareTeachingCorrection.Version,
            "PED:US-CCSS-MATH:G2:U03:L12" when
                CommonCoreG2Unit3FriendshipBraceletsTeachingCorrection.IsTarget(
                    document, lesson) =>
                CommonCoreG2Unit3FriendshipBraceletsTeachingCorrection.Version,
            "PED:US-CCSS-MATH:G2:U04:L03" when
                CommonCoreG2Unit4UnlabeledTicksTeachingCorrection.IsTarget(
                    document, lesson) =>
                CommonCoreG2Unit4UnlabeledTicksTeachingCorrection.Version,
            "PED:US-CCSS-MATH:G2:U05:L01" or
            "PED:US-CCSS-MATH:G2:U05:L02" or
            "PED:US-CCSS-MATH:G2:U05:L03" =>
                CommonCoreG2Unit5OriginalRichContentCorrections.GetExpectedContentVersion(
                    document, lesson, noMatchedSourceTarget),
            "PED:US-CCSS-MATH:G2:U06:L07" or
            "PED:US-CCSS-MATH:G2:U06:L08" or
            "PED:US-CCSS-MATH:G2:U06:L09" or
            "PED:US-CCSS-MATH:G2:U06:L19" =>
                CommonCoreG2Unit6SourceCorrections.GetExpectedContentVersion(
                    document, lesson, noMatchedSourceTarget),
            "PED:US-CCSS-MATH:G2:U09:L04" or
            "PED:US-CCSS-MATH:G2:U09:L05" or
            "PED:US-CCSS-MATH:G2:U09:L11" or
            "PED:US-CCSS-MATH:G2:U09:L12" or
            "PED:US-CCSS-MATH:G2:U09:L13" =>
                CommonCoreG2Unit9SourceCorrections.GetExpectedContentVersion(
                    document, lesson, noMatchedSourceTarget),
            _ => noMatchedSourceTarget
        };

        return sourceBoundExpected != noMatchedSourceTarget &&
            sourceBoundExpected != existingContentVersion &&
            sourceBoundExpected == expectedContentVersion;
    }

    public async Task SeedSourceBoundJsonMetadataCorrectionsAsync(
        CancellationToken ct = default)
    {
        var accepted = await _db.CurriculumPackImportStates.AsNoTracking()
            .SingleOrDefaultAsync(x =>
                x.FrameworkCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
                x.VersionCode == "CCSSM-2010" &&
                x.IsComplete, ct);

        if (accepted is null)
            return;

        var documents = LoadEmbeddedDocuments()
            .Where(d => d.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
                d.VersionCode == "CCSSM-2010")
            .ToArray();

        foreach (var document in documents)
        {
            document.Lessons = document.Lessons
                .Where(lesson => IsSourceBoundJsonMetadataCandidate(document, lesson))
                .ToList();
        }

        var selected = documents.Where(d => d.Lessons.Count > 0).ToArray();
        var targets = selected.SelectMany(d => d.Lessons.Select(lesson =>
            (Document: d, Lesson: lesson))).ToArray();
        var codes = targets.Select(x => x.Lesson.LessonCode).ToArray();

        // Fail closed on source-bound technical release inventory drift:
        // 12 previous mathematics repairs + 19 entity display corrections
        // + 3 first Grade 1 repairs + 7 more + 6 locally verified G1 + 6 further source-goal G1 content repairs.
        // Exactly 294 individually SHA-guarded source-bound lesson identities currently require metadata-only synchronization. Older inventory was 179; 115 additional Grade2 source repairs have explicit official lesson identities. Fail closed on any discrepancy.
        const int expectedSourceBoundCorrectionCount = 294;
        var distinctCount = codes.Distinct(StringComparer.Ordinal).Count();
        if (codes.Length != expectedSourceBoundCorrectionCount ||
            distinctCount != expectedSourceBoundCorrectionCount)
            throw new InvalidOperationException(
                "Source-bound Common Core JSON metadata correction inventory drift." +
                $" expected={expectedSourceBoundCorrectionCount} actual={codes.Length}" +
                $" unique={distinctCount}" +
                " byGrade=" + string.Join(",",
                    codes.GroupBy(code =>
                        System.Text.RegularExpressions.Regex.Match(
                            code, @":G\d+:").Value,
                        StringComparer.Ordinal)
                    .OrderBy(group => group.Key, StringComparer.Ordinal)
                    .Select(group => $"{group.Key}:{group.Count()}")));

        var lessons = await _db.CurriculumPedagogicalLessons.AsNoTracking()
            .Where(x =>
                x.FrameworkVersionId == accepted.FrameworkVersionId &&
                codes.Contains(x.Code))
            .Select(x => new { x.Id, x.Code })
            .ToArrayAsync(ct);

        if (lessons.Length != codes.Length)
            throw new InvalidOperationException(
                "Refusing to create missing Common Core production lesson identities.");

        var ids = lessons.Select(x => x.Id).ToArray();
        var contents = await _db.CurriculumLessonContents.AsNoTracking()
            .Where(x =>
                x.FrameworkVersionId == accepted.FrameworkVersionId &&
                ids.Contains(x.PedagogicalLessonId))
            .ToArrayAsync(ct);

        if (contents.Length != codes.Length)
            throw new InvalidOperationException(
                "Refusing to create missing Common Core production JSON content metadata.");

        var contentIds = contents.Select(x => x.Id).ToArray();
        if (await _db.CurriculumLessonContentTranslations.AsNoTracking()
            .AnyAsync(x => contentIds.Contains(x.CurriculumLessonContentId), ct))
            throw new InvalidOperationException(
                "Persisted lesson prose exists: refusing a JSON-only metadata upgrade.");

        var contentsByCode = (
                from lesson in lessons
                join content in contents on lesson.Id equals content.PedagogicalLessonId
                select new { lesson.Code, Content = content })
            .ToDictionary(x => x.Code, x => x.Content, StringComparer.Ordinal);

        var hasMismatch = false;
        foreach (var target in targets)
        {
            if (!contentsByCode.TryGetValue(target.Lesson.LessonCode,
                    out var content) || content.Status != target.Document.Status)
                throw new InvalidOperationException(
                    "CCSS lesson publication metadata status or identity drift.");

            var expectedVersion = CanonicalLessonContentMaterializer
                .GetEffectiveContentVersion(target.Document, target.Lesson);

            if (string.Equals(content.ContentVersion, expectedVersion,
                    StringComparison.Ordinal))
                continue;

            if (!CanonicalLessonContentMaterializer.CanUpgradeExisting(
                    target.Document, target.Lesson, content.ContentVersion) &&
                !IsGuardedG1U04L21JsonMetadataPredecessor(
                    target.Document, target.Lesson, content.ContentVersion,
                    expectedVersion, verifiedNoPersistedTranslations: true) &&
                !IsGuardedG4KnownProductionJsonMetadataPredecessor(
                    target.Document, target.Lesson, content.ContentVersion,
                    expectedVersion, verifiedNoPersistedTranslations: true) &&
                !IsGuardedG2KnownProductionJsonMetadataPredecessor(
                    target.Document, target.Lesson, content.ContentVersion,
                    expectedVersion, verifiedNoPersistedTranslations: true) &&
                !IsGuardedG3KnownProductionJsonMetadataPredecessor(
                    target.Document, target.Lesson, content.ContentVersion,
                    expectedVersion, verifiedNoPersistedTranslations: true))
                throw new InvalidOperationException(
                    "CCSS lesson content version is not the guarded predecessor: " +
                    target.Lesson.LessonCode);

            hasMismatch = true;
        }

        if (!hasMismatch)
            return;

        // The full 143-lesson Grade 3 pack was already materialized and
        // source-SHA checked by LoadEmbeddedDocuments. The 294-row list above
        // is a FILTERED subset (only two Grade 3 B001 rows): re-running the
        // whole-grade authoring transform on it fails its 143-lesson guard.
        // Preserve this narrow preflight and seed the prepared rows directly.
        await SeedAlreadyMaterializedDocumentsAsync(
            selected, ct, metadataOnly: true);

        var after = await _db.CurriculumLessonContents.AsNoTracking()
            .Where(x => ids.Contains(x.PedagogicalLessonId))
            .Select(x => new { x.PedagogicalLessonId, x.ContentVersion })
            .ToArrayAsync(ct);
        var versions = after.ToDictionary(x => x.PedagogicalLessonId,
            x => x.ContentVersion);

        foreach (var target in targets)
        {
            var lessonId = lessons.Single(x =>
                x.Code == target.Lesson.LessonCode).Id;
            if (!versions.TryGetValue(lessonId, out var version) ||
                version != CanonicalLessonContentMaterializer
                    .GetEffectiveContentVersion(target.Document, target.Lesson))
                throw new InvalidOperationException(
                    "Post-reconciliation CCSS JSON metadata version mismatch: " +
                    target.Lesson.LessonCode);
        }
    }

    /// <summary>
    /// Deliberately exclude all other Common Core content and unrelated
    /// curriculum packs. Audit-only risk flags do not authorize an upgrade.
    /// </summary>
    public static bool IsSourceBoundJsonMetadataCandidate(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        document.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
        document.VersionCode == "CCSSM-2010" &&
        (CommonCoreG4MultiplesSourceCorrection.IsTarget(document, lesson) ||
         CommonCoreG2StoryProblemsSourceCorrection.IsTarget(document, lesson) ||
         CommonCoreG2TapeDiagramAndStoryResolutionCorrection.IsTarget(document, lesson) ||
         CommonCoreG1SourceGroundedInstructionalCorrections.IsTarget(document, lesson) ||
         CommonCoreG1CountAndAddPilotCorrection.IsTarget(document, lesson) ||
         CommonCoreG2SourceGroundedFoundationsCorrections.IsTarget(document, lesson) ||
         CommonCoreG2HundredsPlaceValueSourceCorrections.IsTarget(document, lesson) ||
         CommonCoreG2Unit5OriginalRichContentCorrections.IsTarget(document, lesson) ||
         CommonCoreG2Unit5ComparisonSourceCorrections.IsTarget(document, lesson) ||
         CommonCoreG2Unit6SourceCorrections.IsTarget(document, lesson) ||
         CommonCoreG2Unit7FirstFourCorrections.IsTarget(document, lesson) ||
         CommonCoreG2Unit7NextFourCorrections.IsTarget(document, lesson) ||
         CommonCoreG2Unit7LastTenCorrections.IsTarget(document, lesson) ||
         CommonCoreG2Unit8SourceCorrections.IsTarget(document, lesson) ||
         CommonCoreG2Unit9SourceCorrections.IsTarget(document, lesson) ||
         CommonCoreG2Unit1CountingCollectionsCenterTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit1ClassSurveysTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit2AddSubtractCompareTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit2UnknownAddendTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit2ZooStoryProblemsTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit2CaptureAndFiveCentersTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit2DecomposeTenTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit2TargetNumbersStage4TeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit2TwoDigitTradeTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit2DifferentWaysDecomposeTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit2MixedAddSubtractTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit2CenterDay2TargetStage5TeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit2SeedsStoryTypesTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit2MancalaTapeDiagramsTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit2StoryEquationsTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit2TwoStepSeedStoriesTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit2TapeStoriesCenterTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit2MarketInventoryTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit3StandardUnitsTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit3BeardedDragonCmTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit3BuildRulerTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit3EstimateMeasureCmTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit3MetersReptileTapeTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit3ReptileCompareStoryTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit3EstimateMeasureStage2TeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit3WhatIsAnInchTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit3FeetFishTapeTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit3TornTapeTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit3SareeRibbonStoriesTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit3FriendshipBraceletsTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit3TargetMeasurementsTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit3HandSpanLinePlotTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit3PencilLinePlotTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit3PlantHeightsTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit3CenterDay3LinePlotsTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit3YardstickTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit4NumberLineTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit4NumberLineFeaturesTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit4UnlabeledTicksTeachingCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit4CompareNumberLinesCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit4EstimateNumberLineCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit4CenterDay1NumberLineCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit4DirectionalJumpCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit4EquationArrowCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit4DifferenceStrategiesCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit4PlaceValueLineCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit4MakeTenStrategiesCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit4UnknownArrowRiddlesCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit4StoryRepresentationCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit4JumpTheLineCenterCorrection.IsTarget(document, lesson) ||
         CommonCoreG2Unit4AgeRiddlesCorrection.IsTarget(document, lesson) ||
         CommonCoreG1Unit2StorySourceCorrections.IsTarget(document, lesson) ||
         CommonCoreB001SourceGoalMismatchCorrections.IsTarget(document, lesson) ||
         // Retain exactly the TWO historical B001 Grade 3 metadata identities
         // after the newer SHA-verified Grade 3 teaching correction supersedes
         // their obsolete generic prose. Do not admit the remaining 141 Grade 3
         // candidates into this previously approved production inventory.
         ((lesson.LessonCode is
             "PED:US-CCSS-MATH:G3:U01:L12" or
             "PED:US-CCSS-MATH:G3:U01:L15") &&
             CommonCoreG3SourceTeachingCorrections.IsTarget(document, lesson)) ||
         CommonCoreG1Unit2Part2SourceTeachingCorrections.IsTarget(document, lesson) ||
         CommonCoreG1Unit2NextFiveSourceTeachingCorrections.IsTarget(document, lesson) ||
         CommonCoreG1Unit2CompareFiveSourceTeachingCorrections.IsTarget(document, lesson) ||
         CommonCoreG1Unit2EquationsAndStoriesSourceCorrections.IsTarget(document, lesson) ||
         CommonCoreG1NextSourceFiveCorrections.IsTarget(document, lesson) ||
         CommonCoreG1SourceFiveNextCorrections.IsTarget(document, lesson) ||
         CommonCoreG1TenAndTeenSourceFiveCorrections.IsTarget(document, lesson) ||
         CommonCoreG1FullGradeStartingContentCorrections.IsTarget(document, lesson) ||
         CommonCoreG1FullGradeFinalUnit4Corrections.IsTarget(document, lesson) ||
         CommonCoreG1FullGradeUnit5ContentCorrections.IsTarget(document, lesson) ||
         CommonCoreG1FullGradeUnit6To8ContentCorrections.IsTarget(document, lesson) ||
         CommonCoreSourceBoundHtmlEntityCorrections.IsTarget(document, lesson) ||
         CommonCoreT001MixedAddSubtractContentCorrection.IsTarget(document, lesson) ||
         CommonCoreT001FindThePairContentCorrection.IsTarget(document, lesson));

    public async Task SeedApprovedProductionCorrectionsAsync(
        CancellationToken ct = default)
    {
        var hasAcceptedCurriculum =
            await _db.CurriculumPackImportStates
                .AsNoTracking()
                .AnyAsync(state => state.IsComplete, ct);

        if (!hasAcceptedCurriculum)
            return;

        var documents = LoadEmbeddedDocuments()
            .Where(document =>
                document.Lessons.Any(lesson =>
                    IsApprovedProductionCorrectionTarget(
                        document,
                        lesson)))
            .ToArray();

        foreach (var document in documents)
        {
            document.Lessons = document.Lessons
                .Where(lesson =>
                    IsApprovedProductionCorrectionTarget(
                        document,
                        lesson))
                .ToList();
        }

        var targeted = documents
            .Where(document => document.Lessons.Count != 0)
            .ToArray();

        if (targeted.Length == 0)
            return;

        // Production can start multiple instances during a rolling deploy.
        // Avoid taking the PostgreSQL advisory write lock when the reviewed
        // correction set is already at exact parity. Without this fast path,
        // a second instance can wait behind a long-running idempotent seed and
        // hit the database command timeout before the web host starts.
        var parityMismatches =
            await FindReviewedProductionParityMismatchesAsync(ct);

        if (parityMismatches.Count == 0)
            return;

        await SeedDocumentsAsync(targeted, ct);

        parityMismatches =
            await FindReviewedProductionParityMismatchesAsync(ct);

        if (parityMismatches.Count != 0)
        {
            throw new InvalidOperationException(
                "Reviewed learner-content Production parity failed: " +
                string.Join(" | ", parityMismatches.Take(50)) +
                (parityMismatches.Count > 50
                    ? $" (+{parityMismatches.Count - 50} more)"
                    : string.Empty));
        }
    }

    public async Task<IReadOnlyList<string>>
        FindReviewedProductionParityMismatchesAsync(
            CancellationToken ct = default)
    {
        var expected = LoadEmbeddedDocuments()
            .SelectMany(document =>
                document.Lessons
                    .Where(lesson =>
                        CanonicalLessonContentMaterializer
                            .IsReviewedCorrectionTarget(
                                document,
                                lesson))
                    .Select(lesson => new
                    {
                        Document = document,
                        Lesson = lesson
                    }))
            .ToArray();

        var codes = expected
            .Select(x => x.Lesson.LessonCode)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        var persistedLessons = await _db.CurriculumPedagogicalLessons
            .AsNoTracking()
            .Where(x => codes.Contains(x.Code))
            .ToArrayAsync(ct);

        var lessonByCode = persistedLessons
            .ToDictionary(x => x.Code, StringComparer.Ordinal);

        var lessonIds = persistedLessons
            .Select(x => x.Id)
            .ToArray();

        var contents = await _db.CurriculumLessonContents
            .AsNoTracking()
            .Where(x => lessonIds.Contains(x.PedagogicalLessonId))
            .ToArrayAsync(ct);

        var contentByLessonId = contents
            .ToDictionary(x => x.PedagogicalLessonId);

        var contentIds = contents
            .Select(x => x.Id)
            .ToArray();

        var translations = await _db.CurriculumLessonContentTranslations
            .AsNoTracking()
            .Where(x => contentIds.Contains(x.CurriculumLessonContentId))
            .ToArrayAsync(ct);

        var translationsByContentId = translations
            .GroupBy(x => x.CurriculumLessonContentId)
            .ToDictionary(
                group => group.Key,
                group => group.ToDictionary(
                    x => x.CultureCode,
                    StringComparer.Ordinal));

        var mismatches = new List<string>();

        foreach (var item in expected)
        {
            var code = item.Lesson.LessonCode;

            if (!lessonByCode.TryGetValue(code, out var persistedLesson))
            {
                mismatches.Add($"{code}: lesson missing");
                continue;
            }

            if (!contentByLessonId.TryGetValue(
                    persistedLesson.Id,
                    out var content))
            {
                mismatches.Add($"{code}: content missing");
                continue;
            }

            var expectedVersion =
                CanonicalLessonContentMaterializer
                    .GetEffectiveContentVersion(
                        item.Document,
                        item.Lesson);

            if (!string.Equals(
                    content.ContentVersion,
                    expectedVersion,
                    StringComparison.Ordinal))
            {
                mismatches.Add(
                    $"{code}: version {content.ContentVersion} != {expectedVersion}");
                continue;
            }

            if (!translationsByContentId.TryGetValue(
                    content.Id,
                    out var persistedByCulture))
            {
                mismatches.Add($"{code}: translations missing");
                continue;
            }

            foreach (var expectedTranslation in item.Lesson.Translations)
            {
                if (!persistedByCulture.TryGetValue(
                        expectedTranslation.CultureCode,
                        out var current))
                {
                    mismatches.Add(
                        $"{code}:{expectedTranslation.CultureCode}: translation missing");
                    continue;
                }

                if (!TranslationMatches(
                        current,
                        expectedTranslation))
                {
                    mismatches.Add(
                        $"{code}:{expectedTranslation.CultureCode}: body mismatch");
                }
            }
        }

        return mismatches;
    }

    private static bool TranslationMatches(
        CurriculumLessonContentTranslation current,
        CanonicalLessonContentPackTranslation expected) =>
        string.Equals(current.Title, expected.Title, StringComparison.Ordinal) &&
        string.Equals(current.Explanation, expected.Explanation, StringComparison.Ordinal) &&
        string.Equals(current.KeyConceptsAndRules, expected.KeyConceptsAndRules, StringComparison.Ordinal) &&
        string.Equals(current.WorkedExamples, expected.WorkedExamples, StringComparison.Ordinal) &&
        string.Equals(current.StepByStepSolutions, expected.StepByStepSolutions, StringComparison.Ordinal) &&
        string.Equals(current.CommonMistakes, expected.CommonMistakes, StringComparison.Ordinal) &&
        string.Equals(current.QuickSummary, expected.QuickSummary, StringComparison.Ordinal);

    private static bool IsApprovedProductionCorrectionTarget(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        CanonicalLessonContentMaterializer
            .IsReviewedCorrectionTarget(document, lesson);

    public static IReadOnlyList<CanonicalLessonContentPackDocument>
        LoadEmbeddedDocuments()
    {
        var assembly =
            typeof(MathematicsCurriculumPackRegistry).Assembly;

        var names =
            assembly.GetManifestResourceNames()
                .Where(
                    x => x.EndsWith(
                        ".lesson-content-pack.json",
                        StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x, StringComparer.Ordinal)
                .ToArray();

        var result =
            new List<CanonicalLessonContentPackDocument>();

        foreach (var name in names)
        {
            using var stream =
                assembly.GetManifestResourceStream(name)
                ?? throw new InvalidOperationException(
                    $"Cannot open canonical lesson content resource {name}.");

            var document =
                JsonSerializer.Deserialize<
                    CanonicalLessonContentPackDocument>(
                    stream,
                    JsonOptions)
                ?? throw new InvalidOperationException(
                    $"Invalid canonical lesson content resource {name}.");

            CanonicalLessonContentMaterializer
                .Materialize(document);
            result.Add(document);
        }

        return result;
    }

    public async Task SeedDocumentsAsync(
        IReadOnlyCollection<CanonicalLessonContentPackDocument> documents,
        CancellationToken ct = default,
        bool metadataOnly = false)
    {
        if (documents.Count == 0)
            return;

        foreach (var document in documents)
        {
            CanonicalLessonContentMaterializer
                .Materialize(document);
        }

        await SeedAlreadyMaterializedDocumentsAsync(documents, ct, metadataOnly);
    }

    // Only source packages already validated and materialized as COMPLETE
    // canonical packs by LoadEmbeddedDocuments may use this private method.
    // The source-bound caller has additionally preflighted all 294 exact
    // identities, status, stored predecessor and absence of saved prose.
    // Never apply whole-grade transforms to the subsequently filtered rows.
    private async Task SeedAlreadyMaterializedDocumentsAsync(
        IReadOnlyCollection<CanonicalLessonContentPackDocument> documents,
        CancellationToken ct,
        bool metadataOnly)
    {
        ValidateDistinctTargets(documents);

        if (_db.Database.IsNpgsql())
        {
            await using var transaction =
                await _db.Database.BeginTransactionAsync(ct);

            await _db.Database.ExecuteSqlRawAsync(
                $"SELECT pg_advisory_xact_lock({AdvisoryLockKey});",
                ct);

            foreach (var document in documents
                         .OrderBy(x => x.PackCode, StringComparer.Ordinal)
                         .ThenBy(x => x.VersionCode, StringComparer.Ordinal))
            {
                await SeedOneAsync(document, ct, metadataOnly);
            }

            await transaction.CommitAsync(ct);
            return;
        }

        foreach (var document in documents
                     .OrderBy(x => x.PackCode, StringComparer.Ordinal)
                     .ThenBy(x => x.VersionCode, StringComparer.Ordinal))
        {
            await SeedOneAsync(document, ct, metadataOnly);
        }
    }

    private async Task SeedOneAsync(
        CanonicalLessonContentPackDocument document,
        CancellationToken ct,
        bool metadataOnly)
    {
        var state =
            await _db.CurriculumPackImportStates
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    x =>
                        x.FrameworkCode == document.PackCode &&
                        x.VersionCode == document.VersionCode &&
                        x.IsComplete,
                    ct)
            ?? throw new InvalidOperationException(
                $"Accepted curriculum pack not found for canonical content: " +
                $"{document.PackCode}/{document.VersionCode}.");

        var requestedCodes =
            document.Lessons
                .Select(x => x.LessonCode)
                .ToArray();

        var persistedLessons =
            await _db.CurriculumPedagogicalLessons
                .Where(
                    x =>
                        x.FrameworkVersionId ==
                            state.FrameworkVersionId &&
                        requestedCodes.Contains(x.Code))
                .ToArrayAsync(ct);

        var lessonByCode =
            persistedLessons.ToDictionary(
                x => x.Code,
                StringComparer.Ordinal);

        if (lessonByCode.Count != requestedCodes.Length)
        {
            var missing =
                requestedCodes
                    .Where(x => !lessonByCode.ContainsKey(x))
                    .OrderBy(x => x, StringComparer.Ordinal);

            throw new InvalidOperationException(
                $"Canonical content references unknown pedagogical lesson(s) " +
                $"in {document.PackCode}: {string.Join(", ", missing)}.");
        }

        var lessonIds =
            persistedLessons
                .Select(x => x.Id)
                .ToArray();

        /*
         * Bulk-load the complete document state before per-lesson validation.
         *
         * The validation semantics remain fail-closed:
         * - exact official OutcomeCode equality;
         * - exact pedagogical lesson identity;
         * - no silent content-version replacement;
         * - no status downgrade;
         * - no silent reviewed-body rewrite;
         * - no unexpected translation cultures.
         *
         * PostgreSQL transaction/advisory-lock behaviour remains owned by
         * SeedDocumentsAsync. This method batches persistence per canonical
         * content-pack document.
         */

        var mappingRows =
            await (
                from mapping in
                    _db.CurriculumPedagogicalLessonOutcomes
                        .AsNoTracking()
                join node in
                    _db.CurriculumPackContentNodes
                        .AsNoTracking()
                    on mapping.OutcomeNodeId equals node.Id
                where
                    mapping.FrameworkVersionId ==
                        state.FrameworkVersionId &&
                    lessonIds.Contains(
                        mapping.PedagogicalLessonId)
                select new
                {
                    mapping.PedagogicalLessonId,
                    mapping.SortOrder,
                    node.Code
                })
                .ToArrayAsync(ct);

        var outcomesByLessonId =
            mappingRows
                .GroupBy(x => x.PedagogicalLessonId)
                .ToDictionary(
                    group => group.Key,
                    group =>
                        group
                            .OrderBy(x => x.SortOrder)
                            .Select(x => x.Code)
                            .ToArray());

        var expectedContentIdByLessonId =
            persistedLessons.ToDictionary(
                lesson => lesson.Id,
                lesson =>
                    G(
                        $"canonical-content|" +
                        $"{state.FrameworkVersionId}|" +
                        $"{lesson.Id}"));

        var expectedContentIds =
            expectedContentIdByLessonId
                .Values
                .ToArray();

        var existingContents =
            await _db.CurriculumLessonContents
                .Where(
                    x =>
                        lessonIds.Contains(
                            x.PedagogicalLessonId) ||
                        expectedContentIds.Contains(x.Id))
                .ToArrayAsync(ct);

        var requestedLessonIdSet =
            lessonIds.ToHashSet();

        var identityCollision =
            existingContents
                .FirstOrDefault(
                    x =>
                        !requestedLessonIdSet.Contains(
                            x.PedagogicalLessonId));

        if (identityCollision is not null)
        {
            throw new InvalidOperationException(
                "Canonical deterministic content identity collision detected.");
        }

        var contentByLessonId =
            existingContents.ToDictionary(
                x => x.PedagogicalLessonId);

        var translationContentIds =
            existingContents
                .Select(x => x.Id)
                .Concat(expectedContentIds)
                .Distinct()
                .ToArray();

        var existingTranslations = metadataOnly
            ? Array.Empty<CurriculumLessonContentTranslation>()
            : await _db
                .CurriculumLessonContentTranslations
                .Where(
                    x =>
                        translationContentIds.Contains(
                            x.CurriculumLessonContentId))
                .ToArrayAsync(ct);

        var translationsByContentId =
            existingTranslations
                .GroupBy(
                    x =>
                        x.CurriculumLessonContentId)
                .ToDictionary(
                    group => group.Key,
                    group => group.ToArray());

        var now = DateTime.UtcNow;
        var isApprovedCommonCoreReplacement =
            document.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
            document.ContentVersion == "phase29-source-faithful-en-final-v1" &&
            document.AcademicLanguage == "en" &&
            !document.CurriculumTranslationRequired;

        var isApprovedUaeOfficialRebuildReplacement =
            document.PackCode == MathematicsCurriculumPackRegistry.UaeCode &&
            document.VersionCode == "MOE-2026-2027-T1" &&
            document.AcademicLanguage == "en" &&
            !document.CurriculumTranslationRequired;

        foreach (var sourceLesson in document.Lessons)
        {
            var lesson =
                lessonByCode[sourceLesson.LessonCode];

            var expectedContentVersion =
                CanonicalLessonContentMaterializer
                    .GetEffectiveContentVersion(
                        document,
                        sourceLesson);

            var actualOutcomeCodes =
                outcomesByLessonId.TryGetValue(
                    lesson.Id,
                    out var mappedOutcomeCodes)
                    ? mappedOutcomeCodes
                    : Array.Empty<string>();

            var expectedOutcomes =
                sourceLesson.OutcomeCodes.ToHashSet(
                    StringComparer.Ordinal);

            var actualOutcomes =
                actualOutcomeCodes.ToHashSet(
                    StringComparer.Ordinal);

            if (!expectedOutcomes.SetEquals(actualOutcomes))
            {
                throw new InvalidOperationException(
                    $"Canonical content OutcomeCode drift for " +
                    $"{document.PackCode}/{sourceLesson.LessonCode}. " +
                    $"Expected [{string.Join(", ", expectedOutcomes.OrderBy(x => x))}], " +
                    $"actual [{string.Join(", ", actualOutcomes.OrderBy(x => x))}].");
            }

            var expectedContentId =
                expectedContentIdByLessonId[lesson.Id];

            var didUpgradeReviewedCorrection = false;

            if (!contentByLessonId.TryGetValue(
                    lesson.Id,
                    out var content))
            {
                content =
                    new CurriculumLessonContent
                    {
                        Id = expectedContentId,
                        FrameworkVersionId =
                            state.FrameworkVersionId,
                        PedagogicalLessonId = lesson.Id,
                        Status = document.Status,
                        ContentVersion =
                            expectedContentVersion,
                        CreatedAtUtc = now,
                        UpdatedAtUtc = now,
                        RowVersion = []
                    };

                ApplyStatusMetadata(
                    content,
                    document.Status,
                    now);

                _db.CurriculumLessonContents.Add(
                    content);

                contentByLessonId[lesson.Id] =
                    content;
            }
            else
            {
                if (content.FrameworkVersionId !=
                        state.FrameworkVersionId ||
                    content.PedagogicalLessonId !=
                        lesson.Id)
                {
                    throw new InvalidOperationException(
                        $"Canonical lesson identity drift: " +
                        $"{sourceLesson.LessonCode}.");
                }

                if (!string.Equals(
                        content.ContentVersion,
                        expectedContentVersion,
                        StringComparison.Ordinal))
                {
                    // Permit exactly the previously released U04:L21 V1 marker
                    // to become its V2 marker ONLY for JSON-backed metadata
                    // with no saved prose.  Other unknown predecessors still
                    // fail closed; never relax CanUpgradeExisting.
                    var guardedU04L21JsonMetadata =
                        metadataOnly &&
                        IsGuardedG1U04L21JsonMetadataPredecessor(
                            document, sourceLesson, content.ContentVersion,
                            expectedContentVersion,
                            verifiedNoPersistedTranslations: true) &&
                        !await _db.CurriculumLessonContentTranslations
                            .AsNoTracking()
                            .AnyAsync(row =>
                                row.CurriculumLessonContentId == content.Id, ct);

                    // Exact-source G2 predecessors from the observed production
                    // ReadFromJson metadata inventory can be reconciled ONLY
                    // on the metadata-only path and ONLY when no persisted
                    // translation exists. The outer full-grade preflight has
                    // already verified all content identities/statuses and
                    // checked zero persisted translations across 294 targets.
                    // This targeted path cannot rewrite student-visible prose.
                    // Exact production G4 U01:L01 metadata-only predecessor,
                    // backed by the original source hash, reviewed A-B-C body,
                    // and a fresh stored-translation emptiness check.
                    var guardedG4JsonMetadata =
                        metadataOnly &&
                        IsGuardedG4KnownProductionJsonMetadataPredecessor(
                            document, sourceLesson, content.ContentVersion,
                            expectedContentVersion,
                            verifiedNoPersistedTranslations: true) &&
                        !await _db.CurriculumLessonContentTranslations
                            .AsNoTracking()
                            .AnyAsync(row =>
                                row.CurriculumLessonContentId == content.Id, ct);

                    if (guardedG4JsonMetadata)
                    {
                        if (content.Status != document.Status)
                            throw new InvalidOperationException(
                                "G4 metadata-only publication status drift: " +
                                sourceLesson.LessonCode);
                        content.ContentVersion = expectedContentVersion;
                        content.UpdatedAtUtc = now;
                        ApplyStatusMetadata(content, document.Status, now);
                        continue;
                    }

                    var guardedG2JsonMetadata =
                        metadataOnly &&
                        IsGuardedG2KnownProductionJsonMetadataPredecessor(
                            document, sourceLesson, content.ContentVersion,
                            expectedContentVersion,
                            verifiedNoPersistedTranslations: true) &&
                        !await _db.CurriculumLessonContentTranslations
                            .AsNoTracking()
                            .AnyAsync(row =>
                                row.CurriculumLessonContentId == content.Id, ct);

                    if (guardedG2JsonMetadata)
                    {
                        if (content.Status != document.Status)
                            throw new InvalidOperationException(
                                "G2 metadata-only publication status drift: " +
                                sourceLesson.LessonCode);
                        content.ContentVersion = expectedContentVersion;
                        content.UpdatedAtUtc = now;
                        ApplyStatusMetadata(content, document.Status, now);
                        continue;
                    }

                    // Only the TWO production-observed Grade 3 B001 markers
                    // can advance to the source-SHA-guarded G3 version.
                    // Recheck the absence of stored translations at mutation.
                    var guardedG3JsonMetadata =
                        metadataOnly &&
                        IsGuardedG3KnownProductionJsonMetadataPredecessor(
                            document, sourceLesson, content.ContentVersion,
                            expectedContentVersion,
                            verifiedNoPersistedTranslations: true) &&
                        !await _db.CurriculumLessonContentTranslations
                            .AsNoTracking()
                            .AnyAsync(row =>
                                row.CurriculumLessonContentId == content.Id, ct);

                    if (guardedG3JsonMetadata)
                    {
                        if (content.Status != document.Status)
                            throw new InvalidOperationException(
                                "G3 metadata-only publication status drift: " +
                                sourceLesson.LessonCode);
                        content.ContentVersion = expectedContentVersion;
                        content.UpdatedAtUtc = now;
                        ApplyStatusMetadata(content, document.Status, now);
                        continue;
                    }

                    if (CambridgeReviewedExampleContentCorrections
                        .IsTarget(document, sourceLesson))
                    {
                        CambridgeReviewedExampleContentCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document,
                                sourceLesson,
                                content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var reviewedExistingRows)
                                    ? reviewedExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG4MultiplesSourceCorrection
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG4MultiplesSourceCorrection
                            .ValidateExistingBodyBeforeUpgrade(
                                document,
                                sourceLesson,
                                content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g4ExistingRows)
                                    ? g4ExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2StoryProblemsSourceCorrection
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2StoryProblemsSourceCorrection
                            .ValidateExistingBodyBeforeUpgrade(
                                document,
                                sourceLesson,
                                content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g2ExistingRows)
                                    ? g2ExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2TapeDiagramAndStoryResolutionCorrection
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2TapeDiagramAndStoryResolutionCorrection
                            .ValidateExistingBodyBeforeUpgrade(
                                document,
                                sourceLesson,
                                content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g2U09ExistingRows)
                                    ? g2U09ExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreSourceBoundHtmlEntityCorrections
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreSourceBoundHtmlEntityCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document,
                                sourceLesson,
                                content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var sourceHtmlEntityRows)
                                    ? sourceHtmlEntityRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2SourceGroundedFoundationsCorrections
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2SourceGroundedFoundationsCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document,
                                sourceLesson,
                                content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g2FoundationsExistingRows)
                                    ? g2FoundationsExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit1TenTwentySourceTeachingCorrections
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit1TenTwentySourceTeachingCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g2TenTwentyExistingRows)
                                    ? g2TenTwentyExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit1HowCloseSourceTeachingCorrection
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit1HowCloseSourceTeachingCorrection
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g2HowCloseExistingRows)
                                    ? g2HowCloseExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit1AddWithin50SourceTeachingCorrection
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit1AddWithin50SourceTeachingCorrection
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g2FiftyExistingRows)
                                    ? g2FiftyExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit1CentersAndRealDataCorrections
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit1CentersAndRealDataCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g2CentersExistingRows)
                                    ? g2CentersExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit1PictureGraphSourceTeachingCorrection
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit1PictureGraphSourceTeachingCorrection
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g2PictureExistingRows)
                                    ? g2PictureExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit1BarGraphsSourceTeachingCorrection
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit1BarGraphsSourceTeachingCorrection
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g2BarExistingRows)
                                    ? g2BarExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit1ConstructGraphsTeachingCorrection
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit1ConstructGraphsTeachingCorrection
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g2ConstructExistingRows)
                                    ? g2ConstructExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit1QuestionsAboutDataTeachingCorrection
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit1QuestionsAboutDataTeachingCorrection
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g2QuestionExistingRows)
                                    ? g2QuestionExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit1OptionalSortAndDisplaySourceCorrection
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit1OptionalSortAndDisplaySourceCorrection
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g2OptionalExistingRows)
                                    ? g2OptionalExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit1BarCompareTeachingCorrection
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit1BarCompareTeachingCorrection
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g2BarComparePrevious)
                                    ? g2BarComparePrevious
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit1TapeDiagramTeachingCorrection
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit1TapeDiagramTeachingCorrection
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g2TapeExistingRows)
                                    ? g2TapeExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit1ThreeUnknownPositionsTeachingCorrection
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit1ThreeUnknownPositionsTeachingCorrection
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g2ThreeUnknownExistingRows)
                                    ? g2ThreeUnknownExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit1AllCompareProblemsTeachingCorrection
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit1AllCompareProblemsTeachingCorrection
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g2AllCompareOldRows)
                                    ? g2AllCompareOldRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit1CountingCollectionsCenterTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit1CountingCollectionsCenterTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document, sourceLesson, content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id, out var g2CollectionPreviousRows)
                                ? g2CollectionPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit1ClassSurveysTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit1ClassSurveysTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document, sourceLesson, content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id, out var g2SurveyPreviousRows)
                                ? g2SurveyPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit2AddSubtractCompareTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit2AddSubtractCompareTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var g2Unit2FirstPreviousRows)
                                ? g2Unit2FirstPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit2UnknownAddendTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit2UnknownAddendTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var g2U02MissingOldRows)
                                ? g2U02MissingOldRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit2ZooStoryProblemsTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit2ZooStoryProblemsTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var g2ZooPreviousRows)
                                ? g2ZooPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit2CaptureAndFiveCentersTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit2CaptureAndFiveCentersTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var g2CenterPreviousRows)
                                ? g2CenterPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit2DecomposeTenTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit2DecomposeTenTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var grade2FifthPreviousRows)
                                ? grade2FifthPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit2TargetNumbersStage4TeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit2TargetNumbersStage4TeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var g2TargetPreviousRows)
                                ? g2TargetPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit2TwoDigitTradeTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit2TwoDigitTradeTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var grade2TwoDigitPrevious)
                                ? grade2TwoDigitPrevious
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit2DifferentWaysDecomposeTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit2DifferentWaysDecomposeTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var grade2DifferentPrevious)
                                ? grade2DifferentPrevious
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit2MixedAddSubtractTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit2MixedAddSubtractTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var grade2MixedPreviousRows)
                                ? grade2MixedPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit2CenterDay2TargetStage5TeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit2CenterDay2TargetStage5TeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var grade2CenterDay2Previous)
                                ? grade2CenterDay2Previous
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit2SeedsStoryTypesTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit2SeedsStoryTypesTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var grade2SeedsPreviousRows)
                                ? grade2SeedsPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit2MancalaTapeDiagramsTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit2MancalaTapeDiagramsTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var grade2MancalaPreviousRows)
                                ? grade2MancalaPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit2StoryEquationsTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit2StoryEquationsTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var grade2EquationsPreviousRows)
                                ? grade2EquationsPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit2TwoStepSeedStoriesTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit2TwoStepSeedStoriesTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var grade2TwoStepPreviousRows)
                                ? grade2TwoStepPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit2TapeStoriesCenterTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit2TapeStoriesCenterTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var grade2TapeCenterPreviousRows)
                                ? grade2TapeCenterPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit2MarketInventoryTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit2MarketInventoryTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2MarketPreviousRows)
                                ? g2MarketPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit3StandardUnitsTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit3StandardUnitsTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var g2StandardUnitsPreviousRows)
                                ? g2StandardUnitsPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit3BeardedDragonCmTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit3BeardedDragonCmTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2DragonPreviousRows)
                                ? g2DragonPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit3BuildRulerTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit3BuildRulerTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2RulerPrevious)
                                ? g2RulerPrevious
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit3EstimateMeasureCmTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit3EstimateMeasureCmTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var g2EstimateCmPrevious)
                                ? g2EstimateCmPrevious
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit3MetersReptileTapeTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit3MetersReptileTapeTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var g2MetersPreviousRows)
                                ? g2MetersPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit3ReptileCompareStoryTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit3ReptileCompareStoryTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var g2ReptileStoryPreviousRows)
                                ? g2ReptileStoryPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit3EstimateMeasureStage2TeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit3EstimateMeasureStage2TeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var g2Stage2PreviousRows)
                                ? g2Stage2PreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit3WhatIsAnInchTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit3WhatIsAnInchTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var g2InchPreviousRows)
                                ? g2InchPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit3FeetFishTapeTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit3FeetFishTapeTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var g2FeetPreviousRows)
                                ? g2FeetPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit3TornTapeTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit3TornTapeTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var g2TornPreviousRows)
                                ? g2TornPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit3SareeRibbonStoriesTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit3SareeRibbonStoriesTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var g2RibbonPreviousRows)
                                ? g2RibbonPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit3PencilLinePlotTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit3PencilLinePlotTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var g2PencilPlotRows)
                                ? g2PencilPlotRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit3HandSpanLinePlotTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit3HandSpanLinePlotTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var g2HandSpanPriorRows)
                                ? g2HandSpanPriorRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit3TargetMeasurementsTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit3TargetMeasurementsTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var g2TargetMeasurementPrevious)
                                ? g2TargetMeasurementPrevious
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit3FriendshipBraceletsTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit3FriendshipBraceletsTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var g2BraceletsPreviousRows)
                                ? g2BraceletsPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit3PlantHeightsTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit3PlantHeightsTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var grade2PlantPreviousRows)
                                ? grade2PlantPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit3CenterDay3LinePlotsTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit3CenterDay3LinePlotsTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var optionalLinePlotPreviousRows)
                                ? optionalLinePlotPreviousRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit4CompareNumberLinesCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit4CompareNumberLinesCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2NewSourceRows0)
                                ? g2NewSourceRows0
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit4EstimateNumberLineCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit4EstimateNumberLineCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2NewSourceRows1)
                                ? g2NewSourceRows1
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit4DirectionalJumpCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit4DirectionalJumpCorrection.ValidateExistingBodyBeforeUpgrade(document,sourceLesson,
                            content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2NextContent0)
                                ? g2NextContent0
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit4EquationArrowCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit4EquationArrowCorrection.ValidateExistingBodyBeforeUpgrade(document,sourceLesson,
                            content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2NextContent1)
                                ? g2NextContent1
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit4PlaceValueLineCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit4PlaceValueLineCorrection.ValidateExistingBodyBeforeUpgrade(document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2NextSourceRows0)?
                                g2NextSourceRows0:Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit4MakeTenStrategiesCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit4MakeTenStrategiesCorrection.ValidateExistingBodyBeforeUpgrade(document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2NextSourceRows1)?
                                g2NextSourceRows1:Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit4UnknownArrowRiddlesCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit4UnknownArrowRiddlesCorrection.ValidateExistingBodyBeforeUpgrade(document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2NextSourceRows2)?
                                g2NextSourceRows2:Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit4StoryRepresentationCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit4StoryRepresentationCorrection.ValidateExistingBodyBeforeUpgrade(document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2NextSourceRows3)?
                                g2NextSourceRows3:Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit4JumpTheLineCenterCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit4JumpTheLineCenterCorrection.ValidateExistingBodyBeforeUpgrade(document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2NextSourceRows4)?
                                g2NextSourceRows4:Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit9SourceCorrections.IsTarget(document, sourceLesson))
                    {
                       CommonCoreG2Unit9SourceCorrections.ValidateExistingBodyBeforeUpgrade(document,sourceLesson,content.ContentVersion,
                         translationsByContentId.TryGetValue(content.Id,out var g2U09Stored)
                         ?g2U09Stored:Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit8SourceCorrections.IsTarget(document, sourceLesson))
                    {
                       CommonCoreG2Unit8SourceCorrections.ValidateExistingBodyBeforeUpgrade(document,sourceLesson,content.ContentVersion,
                         translationsByContentId.TryGetValue(content.Id,out var g2Unit8Saved)
                            ?g2Unit8Saved:Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit7LastTenCorrections.IsTarget(document, sourceLesson))
                    {
                      CommonCoreG2Unit7LastTenCorrections.ValidateExistingBodyBeforeUpgrade(
                        document,sourceLesson,content.ContentVersion,
                        translationsByContentId.TryGetValue(content.Id,out var g2U7LastStoredRows)
                          ?g2U7LastStoredRows:Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit7NextFourCorrections.IsTarget(document, sourceLesson))
                    {
                      CommonCoreG2Unit7NextFourCorrections.ValidateExistingBodyBeforeUpgrade(
                        document,sourceLesson,content.ContentVersion,
                        translationsByContentId.TryGetValue(content.Id,out var g2U7NextOriginalSaved)
                          ?g2U7NextOriginalSaved:Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit7FirstFourCorrections.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit7FirstFourCorrections.ValidateExistingBodyBeforeUpgrade(
                          document,sourceLesson,content.ContentVersion,
                          translationsByContentId.TryGetValue(content.Id,out var g2U7OriginalStoredRows)
                            ?g2U7OriginalStoredRows:Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit6SourceCorrections.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit6SourceCorrections.ValidateExistingBodyBeforeUpgrade(document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2U6OriginalStoredRows)
                              ?g2U6OriginalStoredRows:Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit5ComparisonSourceCorrections.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit5ComparisonSourceCorrections.ValidateExistingBodyBeforeUpgrade(document,sourceLesson,
                            content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2U5ComparisonOriginalRows)
                            ?g2U5ComparisonOriginalRows:Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit5OriginalRichContentCorrections.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit5OriginalRichContentCorrections.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2U5RichPriorRows)
                               ?g2U5RichPriorRows:Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit4AgeRiddlesCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit4AgeRiddlesCorrection.ValidateExistingBodyBeforeUpgrade(document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2NextSourceRows5)?
                                g2NextSourceRows5:Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit4DifferenceStrategiesCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit4DifferenceStrategiesCorrection.ValidateExistingBodyBeforeUpgrade(document,sourceLesson,
                            content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2NextContent2)
                                ? g2NextContent2
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit4CenterDay1NumberLineCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit4CenterDay1NumberLineCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2NewSourceRows2)
                                ? g2NewSourceRows2
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit4UnlabeledTicksTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit4UnlabeledTicksTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var g2UnlabeledRows)
                                ? g2UnlabeledRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit4NumberLineFeaturesTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit4NumberLineFeaturesTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var numberLineFeaturesRows)
                                ? numberLineFeaturesRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit4NumberLineTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit4NumberLineTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var numberLineOriginalRows)
                                ? numberLineOriginalRows
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2Unit3YardstickTeachingCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2Unit3YardstickTeachingCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(
                                content.Id,out var grade2YardPrevious)
                                ? grade2YardPrevious
                                : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG2HundredsPlaceValueSourceCorrections
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG2HundredsPlaceValueSourceCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document,
                                sourceLesson,
                                content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g2HundredsExistingRows)
                                    ? g2HundredsExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreB001SourceGoalMismatchCorrections
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreB001SourceGoalMismatchCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var b001SourceGoalsExistingRows)
                                    ? b001SourceGoalsExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG1FullGradeFinalUnit4Corrections
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG1FullGradeFinalUnit4Corrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document,sourceLesson,content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id,out var g1FinalU04Rows)
                                    ? g1FinalU04Rows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }
                    if (CommonCoreG1FullGradeUnit6To8ContentCorrections
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG1FullGradeUnit6To8ContentCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document,sourceLesson,content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id,out var fullGradeU06ToU08Rows)
                                    ? fullGradeU06ToU08Rows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }
                    if (CommonCoreG1FullGradeUnit5ContentCorrections
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG1FullGradeUnit5ContentCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document,sourceLesson,content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id,out var fullGradeUnit5Rows)
                                    ? fullGradeUnit5Rows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }
                    if (CommonCoreG1FullGradeStartingContentCorrections
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG1FullGradeStartingContentCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g1FullGradeRows)
                                    ? g1FullGradeRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }
                    if (CommonCoreG1TenAndTeenSourceFiveCorrections
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG1TenAndTeenSourceFiveCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g1010Rows)
                                    ? g1010Rows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }
                    if (CommonCoreG1SourceFiveNextCorrections
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG1SourceFiveNextCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var next009Rows)
                                    ? next009Rows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }
                    if (CommonCoreG1NextSourceFiveCorrections
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG1NextSourceFiveCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g1008ExistingRows)
                                    ? g1008ExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }
                    if (CommonCoreG1Unit2EquationsAndStoriesSourceCorrections
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG1Unit2EquationsAndStoriesSourceCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var newFiveExistingRows)
                                    ? newFiveExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG1Unit2CompareFiveSourceTeachingCorrections
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG1Unit2CompareFiveSourceTeachingCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g1CompareExisting)
                                    ? g1CompareExisting
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG1Unit2NextFiveSourceTeachingCorrections
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG1Unit2NextFiveSourceTeachingCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g1Unit2NewFiveExisting)
                                    ? g1Unit2NewFiveExisting
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG1Unit2Part2SourceTeachingCorrections
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG1Unit2Part2SourceTeachingCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g1Unit2B001L04L05Existing)
                                    ? g1Unit2B001L04L05Existing
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG1Unit2StorySourceCorrections
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG1Unit2StorySourceCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document, sourceLesson, content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var b001G1U02ExistingRows)
                                    ? b001G1U02ExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG1SourceGroundedInstructionalCorrections
                        .IsTarget(document, sourceLesson) &&
                        !guardedU04L21JsonMetadata)
                    {
                        CommonCoreG1SourceGroundedInstructionalCorrections
                            .ValidateExistingBodyBeforeUpgrade(
                                document,
                                sourceLesson,
                                content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var g1ExistingRows)
                                    ? g1ExistingRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    if (CommonCoreG1CountAndAddPilotCorrection
                        .IsTarget(document, sourceLesson))
                    {
                        CommonCoreG1CountAndAddPilotCorrection
                            .ValidateExistingBodyBeforeUpgrade(
                                document,
                                sourceLesson,
                                content.ContentVersion,
                                translationsByContentId.TryGetValue(
                                    content.Id, out var countAndAddRows)
                                    ? countAndAddRows
                                    : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    // Only two reviewed T001 source-bound content edits in this
                    // main-based release branch: no other B01 lesson upgrade.
                    if (CommonCoreT001MixedAddSubtractContentCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreT001MixedAddSubtractContentCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var mixedExisting)
                                ? mixedExisting : Array.Empty<CurriculumLessonContentTranslation>());
                    }
                    if (CommonCoreT001FindThePairContentCorrection.IsTarget(document, sourceLesson))
                    {
                        CommonCoreT001FindThePairContentCorrection.ValidateExistingBodyBeforeUpgrade(
                            document,sourceLesson,content.ContentVersion,
                            translationsByContentId.TryGetValue(content.Id,out var pairExisting)
                                ? pairExisting : Array.Empty<CurriculumLessonContentTranslation>());
                    }

                    var canUpgradeReviewedCorrection =
                        CanonicalLessonContentMaterializer
                            .CanUpgradeExisting(
                                document,
                                sourceLesson,
                                content.ContentVersion);

                    if (!isApprovedCommonCoreReplacement &&
                        !isApprovedUaeOfficialRebuildReplacement &&
                        !canUpgradeReviewedCorrection &&
                        !guardedU04L21JsonMetadata)
                    {
                        throw new InvalidOperationException(
                            $"Refusing silent canonical content-version replacement for " +
                            $"{sourceLesson.LessonCode}. " +
                            $"Existing={content.ContentVersion}, " +
                            $"incoming={expectedContentVersion}.");
                    }

                    content.ContentVersion =
                        expectedContentVersion;
                    content.UpdatedAtUtc = now;
                    didUpgradeReviewedCorrection =
                        canUpgradeReviewedCorrection;
                }

                if ((int)content.Status >
                    (int)document.Status)
                {
                    throw new InvalidOperationException(
                        $"Refusing canonical content status downgrade for " +
                        $"{sourceLesson.LessonCode}: " +
                        $"{content.Status} -> {document.Status}.");
                }

                if (content.Status != document.Status)
                {
                    content.Status =
                        document.Status;

                    content.UpdatedAtUtc =
                        now;
                }

                ApplyStatusMetadata(
                    content,
                    document.Status,
                    now);
            }

            if (metadataOnly)
            {
                // Version/status/outcome identity is already verified above.
                // Full localized prose is resolved by the embedded JSON index.
                continue;
            }

            var currentTranslations =
                translationsByContentId
                    .TryGetValue(
                        content.Id,
                        out var currentRows)
                    ? currentRows
                    : Array.Empty<
                        CurriculumLessonContentTranslation>();

            var incomingCultures =
                sourceLesson.Translations
                    .Select(x => x.CultureCode)
                    .ToHashSet(
                        StringComparer.Ordinal);

            var unexpected =
                currentTranslations
                    .Where(
                        x =>
                            !incomingCultures.Contains(
                                x.CultureCode))
                    .Select(x => x.CultureCode)
                    .ToArray();

            if (unexpected.Length != 0)
            {
                if (!isApprovedCommonCoreReplacement &&
                    !isApprovedUaeOfficialRebuildReplacement)
                {
                    throw new InvalidOperationException(
                        $"Canonical translation drift for " +
                        $"{sourceLesson.LessonCode}. " +
                        $"Existing unexpected culture(s): " +
                        $"{string.Join(", ", unexpected)}.");
                }

                _db.CurriculumLessonContentTranslations.RemoveRange(
                    currentTranslations.Where(
                        x => unexpected.Contains(x.CultureCode)));
            }

            var existingByCulture =
                currentTranslations.ToDictionary(
                    x => x.CultureCode,
                    StringComparer.Ordinal);

            foreach (var incoming in
                     sourceLesson.Translations)
            {
                if (existingByCulture.TryGetValue(
                        incoming.CultureCode,
                        out var current))
                {
                    if (isApprovedCommonCoreReplacement ||
                        isApprovedUaeOfficialRebuildReplacement ||
                        didUpgradeReviewedCorrection)
                    {
                        current.Title = incoming.Title;
                        current.Explanation = incoming.Explanation;
                        current.KeyConceptsAndRules = incoming.KeyConceptsAndRules;
                        current.WorkedExamples = incoming.WorkedExamples;
                        current.StepByStepSolutions = incoming.StepByStepSolutions;
                        current.CommonMistakes = incoming.CommonMistakes;
                        current.QuickSummary = incoming.QuickSummary;
                        current.UpdatedAtUtc = now;
                    }
                    else
                    {
                        EnsureTranslationMatches(
                            sourceLesson.LessonCode,
                            current,
                            incoming);
                    }

                    continue;
                }

                _db
                    .CurriculumLessonContentTranslations
                    .Add(
                        new CurriculumLessonContentTranslation
                        {
                            Id =
                                G(
                                    $"canonical-translation|" +
                                    $"{content.Id}|" +
                                    $"{incoming.CultureCode}"),
                            CurriculumLessonContentId =
                                content.Id,
                            CultureCode =
                                incoming.CultureCode,
                            Title =
                                incoming.Title,
                            Explanation =
                                incoming.Explanation,
                            KeyConceptsAndRules =
                                incoming.KeyConceptsAndRules,
                            WorkedExamples =
                                incoming.WorkedExamples,
                            StepByStepSolutions =
                                incoming.StepByStepSolutions,
                            CommonMistakes =
                                incoming.CommonMistakes,
                            QuickSummary =
                                incoming.QuickSummary,
                            CreatedAtUtc = now,
                            UpdatedAtUtc = now,
                            RowVersion = []
                        });
            }
        }

        // One persistence boundary per canonical content pack,
        // instead of one SaveChanges round-trip per lesson.
        await _db.SaveChangesAsync(ct);
    }

    private static void ApplyStatusMetadata(
        CurriculumLessonContent content,
        CanonicalLessonContentStatus status,
        DateTime now)
    {
        if (status is
            CanonicalLessonContentStatus.Verified or
            CanonicalLessonContentStatus.Published)
        {
            content.VerifiedAtUtc ??= now;
        }

        if (status ==
            CanonicalLessonContentStatus.Published)
        {
            content.PublishedAtUtc ??= now;
        }
    }

    private static void EnsureTranslationMatches(
        string lessonCode,
        CurriculumLessonContentTranslation current,
        CanonicalLessonContentPackTranslation incoming)
    {
        if (!string.Equals(
                current.Title,
                incoming.Title,
                StringComparison.Ordinal) ||
            !string.Equals(
                current.Explanation,
                incoming.Explanation,
                StringComparison.Ordinal) ||
            !string.Equals(
                current.KeyConceptsAndRules,
                incoming.KeyConceptsAndRules,
                StringComparison.Ordinal) ||
            !string.Equals(
                current.WorkedExamples,
                incoming.WorkedExamples,
                StringComparison.Ordinal) ||
            !string.Equals(
                current.StepByStepSolutions,
                incoming.StepByStepSolutions,
                StringComparison.Ordinal) ||
            !string.Equals(
                current.CommonMistakes,
                incoming.CommonMistakes,
                StringComparison.Ordinal) ||
            !string.Equals(
                current.QuickSummary,
                incoming.QuickSummary,
                StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Canonical lesson body drift for " +
                $"{lessonCode}:{incoming.CultureCode}. " +
                "Refusing silent rewrite of reviewed content.");
        }
    }

    private static void ValidateDistinctTargets(
        IEnumerable<CanonicalLessonContentPackDocument> documents)
    {
        var keys =
            documents
                .SelectMany(
                    document =>
                        document.Lessons.Select(
                            lesson =>
                                $"{document.PackCode}\u001f" +
                                $"{document.VersionCode}\u001f" +
                                lesson.LessonCode))
                .ToArray();

        if (keys.Length !=
            keys.Distinct(StringComparer.Ordinal).Count())
        {
            throw new InvalidOperationException(
                "A pedagogical lesson appears in more than one canonical content pack document.");
        }
    }

    private static Guid G(string value)
    {
        var hash =
            SHA256.HashData(
                Encoding.UTF8.GetBytes(value));

        Span<byte> bytes =
            stackalloc byte[16];

        hash.AsSpan(0, 16)
            .CopyTo(bytes);

        bytes[6] =
            (byte)((bytes[6] & 0x0f) | 0x50);

        bytes[8] =
            (byte)((bytes[8] & 0x3f) | 0x80);

        return new Guid(bytes);
    }
}
