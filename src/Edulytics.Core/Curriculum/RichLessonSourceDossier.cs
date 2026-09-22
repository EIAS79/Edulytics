namespace Edulytics.Core.Curriculum;

public enum RichLessonSourceUseMode
{
    ResearchRequired = 0,
    ReferenceOnlyOriginalAuthoring = 1,
    AdaptationApproved = 2
}

public sealed record RichLessonSourceDossier(
    string LessonCode,
    string PackCode,
    string CurriculumAuthority,
    string CurriculumSourceUrl,
    string PedagogicalSourceType,
    string PedagogicalSourceTitle,
    string PedagogicalSourcePublisher,
    string PedagogicalSourceEdition,
    string PedagogicalSourceUrl,
    string PedagogicalSourceSelectionReason,
    string PedagogicalSourceSelectionEvidence,
    string PedagogicalSourceRightsNote,
    string LessonSourceUrl,
    string LessonSourceLocator,
    string LessonSourceRights,
    string SourceVerifiedAtUtc,
    RichLessonSourceUseMode UseMode,
    string UseReason);

/// <summary>
/// Resolves the legal/pedagogical use mode of the currently recorded lesson source.
/// This is deliberately fail-closed: a useful reference is not automatically an
/// adaptation source. The resolver never changes curriculum authority or mappings.
/// </summary>
public static class RichLessonSourceDossierResolver
{
    private static readonly string[] ApprovedLicenseMarkers =
    [
        "Public Domain",
        "CC0 1.0",
        "CC BY 4.0",
        "Open Government Licence v3.0"
    ];

    public static RichLessonSourceDossier Resolve(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson)
    {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(lesson);

        var useMode = ResolveUseMode(
            document,
            lesson,
            out var useReason);

        return new(
            lesson.LessonCode,
            document.PackCode,
            document.SourceAuthority,
            document.SourceUrl,
            document.PedagogicalSourceType.ToString(),
            document.PedagogicalSourceTitle,
            document.PedagogicalSourcePublisher,
            document.PedagogicalSourceEdition,
            document.PedagogicalSourceUrl,
            document.PedagogicalSourceSelectionReason,
            document.PedagogicalSourceSelectionEvidence,
            document.PedagogicalSourceRightsNote,
            lesson.SourceUrl,
            lesson.SourceLocator,
            lesson.SourceRights,
            lesson.SourceVerifiedAtUtc,
            useMode,
            useReason);
    }

    private static RichLessonSourceUseMode ResolveUseMode(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        out string reason)
    {
        if (document.SourcePolicyVersion < 2 ||
            document.PedagogicalSourceType is
                PedagogicalSourceType.LegacyUnspecified or
                PedagogicalSourceType.OfficialFrameworkOnly)
        {
            reason =
                "No approved pedagogical adaptation source is recorded for this lesson. " +
                "Research is required before source-driven Rich Content V2 enrichment.";
            return RichLessonSourceUseMode.ResearchRequired;
        }

        var rightsEvidence = string.Join(
            " ",
            document.PedagogicalSourceRightsNote,
            lesson.SourceRights);

        var approved = ApprovedLicenseMarkers
            .FirstOrDefault(marker =>
                rightsEvidence.Contains(
                    marker,
                    StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(approved))
        {
            reason =
                $"Recorded rights evidence contains approved commercial adaptation licence marker: {approved}.";
            return RichLessonSourceUseMode.AdaptationApproved;
        }

        reason =
            "The recorded source can be used as curriculum/pedagogical reference, " +
            "but no approved adaptation licence marker is recorded. " +
            "Rich learner-facing prose/examples must therefore be independently authored.";
        return RichLessonSourceUseMode.ReferenceOnlyOriginalAuthoring;
    }
}
