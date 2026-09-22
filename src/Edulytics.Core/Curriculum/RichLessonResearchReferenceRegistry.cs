using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Core.Curriculum;

public sealed record RichLessonResearchReference(
    string Id,
    string Title,
    string Publisher,
    string Edition,
    string Url,
    string Licence,
    string ScopeNote);

/// <summary>
/// Reviewed external research references used only to cross-check independently
/// authored Rich Lesson Content V2. This registry never changes curriculum
/// authority, lesson identity or official Outcome mappings.
///
/// The Polish R8 rollout deliberately separates:
/// - Polish ZPE/ELI curriculum authority: WHAT the learner must learn;
/// - open/reference mathematics sources: method/content cross-checking;
/// - Edulytics-authored Polish lesson prose: HOW the topic is explained.
///
/// Runtime lesson text does not copy source prose or illustrations.
/// </summary>
public static class RichLessonResearchReferenceRegistry
{
    private static readonly RichLessonResearchReference ImK12FirstEdition = new(
        "IM-K12-FIRST-EDITION-CC-BY-4",
        "IM K–12 Math — first edition",
        "Illustrative Mathematics",
        "© 2019–2021 first edition",
        "https://illustrativemathematics.org/terms-of-use/",
        "CC BY 4.0",
        "K–12 number, fractions, ratio, algebra, geometry, statistics and probability where topic coverage matches.");

    private static readonly RichLessonResearchReference MhccPrecalculus = new(
        "MHCC-PRECALCULUS-CC-BY-4",
        "Precalculus: An Active Reading Approach and MHCC precalculus readings",
        "Mt. Hood Community College",
        "Archived/open precalculus resource set",
        "https://www.mhcc.edu/student-resources/textbook-affordability/course-materials/mathematics-open-educational-resources/index",
        "CC BY 4.0",
        "Functions, transformations, polynomial/precalculus, exponential, logarithmic and trigonometric cross-checking.");

    private static readonly RichLessonResearchReference AppliedCalculus = new(
        "APPLIED-CALCULUS-CC-BY-4",
        "Applied Calculus",
        "Shana Calaway, Dale Hoffman and David Lippman / LibreTexts",
        "Open textbook",
        "https://commons.libretexts.org/book/math-71036",
        "CC BY 4.0",
        "Derivative, integral and introductory calculus cross-checking.");

    private static readonly RichLessonResearchReference KuttlerLinearAlgebra = new(
        "KUTTLER-LINEAR-ALGEBRA-2023-B-D-CC-BY-4",
        "A First Course in Linear Algebra",
        "Ken Kuttler / Vretta-Lyryx Inc.",
        "2023-B-D",
        "https://collection.bccampus.ca/textbook/ELnyC3cw/",
        "CC BY 4.0 except where otherwise noted",
        "Vectors, matrices, systems and related linear-algebra cross-checking.");

    public static RichLessonResearchReference? ResolvePolish(
        CanonicalLessonContentPackLesson lesson)
    {
        ArgumentNullException.ThrowIfNull(lesson);

        var skills = lesson.OutcomeCodes
            .Select(code =>
                PolishOutcomePracticeMapRegistry.TryResolve(
                    code,
                    out var mapping)
                    ? mapping
                    : null)
            .Where(mapping => mapping is not null)
            .SelectMany(mapping => mapping!.TargetRules)
            .Select(rule => rule.SkillId)
            .Where(skill => !string.IsNullOrWhiteSpace(skill))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        if (skills.Length == 0)
            return null;

        if (skills.Any(skill =>
                skill.StartsWith("calculus.", StringComparison.Ordinal)))
        {
            return AppliedCalculus;
        }

        if (skills.Any(skill =>
                skill.StartsWith("vectors.", StringComparison.Ordinal) ||
                skill.StartsWith("matrices.", StringComparison.Ordinal) ||
                skill.StartsWith("complex.", StringComparison.Ordinal)))
        {
            return KuttlerLinearAlgebra;
        }

        if (skills.Any(skill =>
                skill.StartsWith("functions.", StringComparison.Ordinal) ||
                skill.StartsWith("trigonometry.", StringComparison.Ordinal) ||
                skill.StartsWith("exponentials.", StringComparison.Ordinal) ||
                skill.StartsWith("logarithms.", StringComparison.Ordinal) ||
                skill.StartsWith("indices.", StringComparison.Ordinal) ||
                skill.StartsWith("sequences.", StringComparison.Ordinal)))
        {
            return MhccPrecalculus;
        }

        return ImK12FirstEdition;
    }
}
