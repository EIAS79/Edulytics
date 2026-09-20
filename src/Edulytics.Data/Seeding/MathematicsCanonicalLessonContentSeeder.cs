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

        await SeedDocumentsAsync(targeted, ct);

        var parityMismatches =
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
        CancellationToken ct = default)
    {
        if (documents.Count == 0)
            return;

        foreach (var document in documents)
        {
            CanonicalLessonContentMaterializer
                .Materialize(document);
        }

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
                await SeedOneAsync(document, ct);
            }

            await transaction.CommitAsync(ct);
            return;
        }

        foreach (var document in documents
                     .OrderBy(x => x.PackCode, StringComparer.Ordinal)
                     .ThenBy(x => x.VersionCode, StringComparer.Ordinal))
        {
            await SeedOneAsync(document, ct);
        }
    }

    private async Task SeedOneAsync(
        CanonicalLessonContentPackDocument document,
        CancellationToken ct)
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

        var existingTranslations =
            await _db
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
                    var canUpgradeReviewedCorrection =
                        CanonicalLessonContentMaterializer
                            .CanUpgradeExisting(
                                document,
                                sourceLesson,
                                content.ContentVersion);

                    if (!isApprovedCommonCoreReplacement &&
                        !canUpgradeReviewedCorrection)
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
                if (!isApprovedCommonCoreReplacement)
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
