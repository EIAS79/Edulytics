using System.Text.Json;

namespace Edulytics.Web.Presentation;

/// <summary>
/// Source-specific teacher-review tasks authored in the A–B–C lesson packs.
/// This is a human-scored activity bank, not an automatic answer key or
/// third-party image reproduction. Original U01:L01 keeps its richer 92-task
/// manual rubric and specialist source-part/diagram checks.
/// </summary>
public static class Grade12PrecalculusTeacherTaskBank
{
    public const string LessonPrefix =
        "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:";
    public const string FirstLesson = LessonPrefix + "U01:L01";

    public sealed record Task(int Number, string Prompt);
    private sealed record LessonRow(string LessonCode, string OriginalSourceField,
        Task[] Tasks);
    private sealed record Document(int SchemaVersion, string AcademicOwnerApproval,
        string ReviewMode, bool SourceExactParity, LessonRow[] Lessons);

    private static readonly Lazy<IReadOnlyDictionary<string, IReadOnlyList<Task>>> Entries =
        new(Load, LazyThreadSafetyMode.ExecutionAndPublication);

    public static IReadOnlyDictionary<string, IReadOnlyList<Task>> All => Entries.Value;

    public static Task? Find(string? lessonCode, int exerciseNumber)
    {
        if (lessonCode == FirstLesson)
        {
            var first = Grade12PrecalculusSection11Exercises.Find(exerciseNumber);
            return first is null ? null : new Task(first.Number, first.Prompt);
        }
        if (string.IsNullOrWhiteSpace(lessonCode) ||
            !Entries.Value.TryGetValue(lessonCode, out var tasks))
            return null;
        return tasks.FirstOrDefault(t => t.Number == exerciseNumber);
    }

    public static string? ComposeTeacherResponse(string lessonCode,
        int exerciseNumber, string? explanation, string? graphEvidence)
    {
        if (Find(lessonCode, exerciseNumber) is null ||
            string.IsNullOrWhiteSpace(explanation))
            return null;
        var reasoning = explanation.Trim();
        if (reasoning.Length is < 20 or > 8000) return null;
        var graph = graphEvidence?.Trim() ?? string.Empty;
        if (graph.Length > 3000) return null;
        if (graph.Length == 0) return reasoning;
        var combined = "Written mathematical reasoning:\n" + reasoning +
            "\n\nOptional plotted graph or coordinate evidence:\n" + graph;
        return combined.Length <= 8000 ? combined : null;
    }

    public static string DisplayLabel(string? lessonCode, int exerciseNumber) =>
        lessonCode == FirstLesson
            ? Grade12PrecalculusSection11Exercises.DisplayLabel(exerciseNumber)
            : $"Exercise {exerciseNumber}";

    private static IReadOnlyDictionary<string, IReadOnlyList<Task>> Load()
    {
        const string resource = "Edulytics.Web.G12.FirstPartyTeacherTasks";
        using var stream = typeof(Grade12PrecalculusTeacherTaskBank)
            .Assembly.GetManifestResourceStream(resource)
            ?? throw new InvalidOperationException(
                "Grade12 student source task bank was not included in the web artifact.");
        var doc = JsonSerializer.Deserialize<Document>(stream)
            ?? throw new InvalidOperationException("Grade12 teacher task JSON invalid.");
        if (doc.SchemaVersion != 1 || doc.Lessons.Length != 72 ||
            doc.ReviewMode != "MANUAL_TEACHER_ONLY" || doc.SourceExactParity)
            throw new InvalidOperationException("Grade12 teacher task provenance mismatch.");
        var result = new Dictionary<string, IReadOnlyList<Task>>(
            StringComparer.Ordinal);
        foreach (var lesson in doc.Lessons)
        {
            if (lesson.LessonCode == FirstLesson ||
                !lesson.LessonCode.StartsWith(LessonPrefix, StringComparison.Ordinal) ||
                lesson.Tasks.Length == 0 ||
                lesson.Tasks.Any(t => t.Number < 1 ||
                    string.IsNullOrWhiteSpace(t.Prompt) || t.Prompt.Length > 7900) ||
                lesson.Tasks.Select(t => t.Number).Distinct().Count() !=
                    lesson.Tasks.Length ||
                !result.TryAdd(lesson.LessonCode, lesson.Tasks))
                throw new InvalidOperationException(
                    "Grade12 teacher task bank has missing or invalid lesson entry.");
        }
        return result;
    }
}
