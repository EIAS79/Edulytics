using System.Text.Json;
using Edulytics.Core.Curriculum;

namespace Edulytics.Data.Seeding;

/// <summary>
/// First-party Grade 3 U03 L07 2021 source-specific English teaching content.
/// Applied only after the existing canonical edition/body-SHA guards in
/// CommonCoreG3SourceTeachingCorrections. This is an authoring candidate;
/// no source publisher art or teacher-only material is embedded.
/// </summary>
internal static class CommonCoreG3Unit3Lesson7RichV2
{
    internal const string LessonCode = "PED:US-CCSS-MATH:G3:U03:L07";
    private const string ResourceName = "Edulytics.Data.Grade3.Unit3Lesson7RichV2";
    private const string ExpectedContentVersion =
        "g3-u03-l07-2021-source-abc-firstparty-20261024-v1";

    private static readonly Lazy<JsonElement> Source = new(Load);

    internal static void Apply(CanonicalLessonContentPackLesson lesson)
    {
        if (!string.Equals(lesson.LessonCode, LessonCode, StringComparison.Ordinal))
            throw new InvalidOperationException("Grade 3 U03 L07 guard failed.");

        var en = lesson.Translations.Single(x =>
            string.Equals(x.CultureCode, "en", StringComparison.Ordinal));
        var source = Source.Value;

        en.Title = Text(source, "Title");
        en.Explanation = string.Join("\n\n",
            source.GetProperty("ExplanationParagraphs").EnumerateArray()
                .Select(p => p.GetString()!));
        en.KeyConceptsAndRules = string.Join("\n\n",
            source.GetProperty("KeyConcepts").EnumerateArray().Select(c =>
                Text(c, "Title") + "\nDefinition: " + Text(c, "Definition") +
                "\nRule: " + Text(c, "Rule") +
                "\nExample: " + Text(c, "Example")));
        en.WorkedExamples = string.Join("\n\n",
            source.GetProperty("WorkedExamples").EnumerateArray().Select(ex =>
                Text(ex, "Title") + "\nProblem: " + Text(ex, "Question") +
                "\nMethod: " + Text(ex, "Method") +
                "\nSteps: " + string.Join("; ",
                    ex.GetProperty("Steps").EnumerateArray().Select(p => p.GetString()!)) +
                "\nAnswer: " + Text(ex, "Answer") +
                "\nCheck: " + Text(ex, "Check")));
        en.StepByStepSolutions = string.Join("\n\n",
            source.GetProperty("WorkedExamples").EnumerateArray().Select(ex =>
                Text(ex, "Title") + "\n" +
                string.Join("\n", ex.GetProperty("Steps").EnumerateArray()
                    .Select((p, i) => $"Step {i + 1}: {p.GetString()}")) +
                "\nCheck: " + Text(ex, "Check")));
        en.CommonMistakes = string.Join("\n\n",
            source.GetProperty("CommonMistakes").EnumerateArray().Select(m =>
                "Mistake: " + Text(m, "Mistake") +
                "\nWhy incorrect: " + Text(m, "WhyWrong") +
                "\nCorrection: " + Text(m, "Correction")));
        en.QuickSummary = string.Join("\n",
            source.GetProperty("SummaryPoints").EnumerateArray()
                .Select(s => s.GetString()!));
    }

    private static JsonElement Load()
    {
        using var stream = typeof(CommonCoreG3Unit3Lesson7RichV2).Assembly
            .GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException(
                "First-party Grade 3 U03 L07 Rich V2 authoring source was not embedded.");
        using var document = JsonDocument.Parse(stream);
        var root = document.RootElement;
        if (root.GetProperty("SchemaVersion").GetInt32() != 1 ||
            !string.Equals(Text(root, "ContentVersion"), ExpectedContentVersion,
                StringComparison.Ordinal) ||
            !string.Equals(Text(root, "PackCode"), "US-CCSS-MATH",
                StringComparison.Ordinal))
            throw new InvalidOperationException(
                "Grade 3 U03 L07 source-specific Rich V2 version or pack changed.");
        var lessons = root.GetProperty("Lessons");
        if (lessons.GetArrayLength() != 1 ||
            !string.Equals(Text(lessons[0], "LessonCode"), LessonCode,
                StringComparison.Ordinal) ||
            !string.Equals(Text(lessons[0], "CultureCode"), "en",
                StringComparison.Ordinal))
            throw new InvalidOperationException("Grade 3 U03 L07 Rich V2 identity changed.");
        return lessons[0].Clone();
    }

    private static string Text(JsonElement source, string key) =>
        source.GetProperty(key).GetString() ??
        throw new InvalidOperationException($"Grade 3 U03 L07 missing {key}.");
}

