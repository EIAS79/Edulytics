using Edulytics.Core.Enums;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;

namespace Edulytics.Data.Repositories;

/// <summary>
/// Immutable, in-process index of the checked-in, materialized canonical JSON
/// lesson corpus. No school data, PostgreSQL connection or public file endpoint.
/// Authorization and curriculum-adoption checks remain in LessonContentService.
/// </summary>
public sealed class EmbeddedCanonicalLessonContentIndex
{
    private static readonly Lazy<IReadOnlyDictionary<string, EmbeddedLessonBody>> Cache =
        new(BuildIndex, LazyThreadSafetyMode.ExecutionAndPublication);

    public int Count => Cache.Value.Count;

    public bool TryGet(string lessonCode, out EmbeddedLessonBody body) =>
        Cache.Value.TryGetValue(lessonCode, out body!);

    private static IReadOnlyDictionary<string, EmbeddedLessonBody> BuildIndex()
    {
        var result = new Dictionary<string, EmbeddedLessonBody>(
            StringComparer.Ordinal);

        // The existing loader applies approved, source-reviewed corrections
        // before validating every embedded JSON document.
        foreach (var document in MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments())
        {
            foreach (var lesson in document.Lessons)
            {
                var translations = lesson.Translations
                    .Select(t => new CanonicalLessonTranslationRecord(
                        t.CultureCode,
                        t.Title,
                        t.Explanation,
                        t.KeyConceptsAndRules,
                        t.WorkedExamples,
                        t.StepByStepSolutions,
                        t.CommonMistakes,
                        t.QuickSummary))
                    .ToArray();

                var body = new EmbeddedLessonBody(
                    document.Status,
                    CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                        document, lesson),
                    Array.AsReadOnly(translations));

                if (!result.TryAdd(lesson.LessonCode, body))
                {
                    throw new InvalidOperationException(
                        "Duplicate canonical JSON lesson identity: " +
                        lesson.LessonCode);
                }
            }
        }

        return result;
    }
}

public sealed record EmbeddedLessonBody(
    CanonicalLessonContentStatus Status,
    string ContentVersion,
    IReadOnlyList<CanonicalLessonTranslationRecord> Translations);
