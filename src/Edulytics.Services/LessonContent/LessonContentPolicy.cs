using Edulytics.Core.Constants;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Services.LessonContent;

public static class LessonContentPolicy
{
    public static bool CanReadStaff(
        IReadOnlyList<string> roles) =>
        roles.Count == 1 &&
        (
            roles[0] == RoleNames.SchoolAdmin ||
            roles[0] == RoleNames.SubjectSupervisor ||
            roles[0] == RoleNames.Teacher
        );

    public static bool IsStudent(
        IReadOnlyList<string> roles) =>
        roles.Count == 1 &&
        roles[0] == RoleNames.Student;

    public static bool CanExposeCanonicalBody(
        CanonicalLessonContentStatus status) =>
        status ==
        CanonicalLessonContentStatus.Published;

    /// <summary>
    /// Exact accepted mappings distinguish aligned lessons from Supporting
    /// lessons. Both are valid canonical-content targets; Supporting lessons
    /// deliberately have no independent official OutcomeCodes.
    /// </summary>
    public static bool IsStandaloneCanonicalTarget(
        int officialOutcomeCount) =>
        officialOutcomeCount > 0;

    public static bool IsSupporting(int officialOutcomeCount) =>
        officialOutcomeCount == 0;

    public static bool IsCanonicalTarget(int officialOutcomeCount) =>
        officialOutcomeCount >= 0;


    // The canonical 78 Arabic UAE G7 General lessons have explicitly
    // registered exact Practice contracts. Never whitelist the old English
    // G7 General curriculum using its broad prefix alone.
    public static bool IsUaeGrade7GeneralScope(
        string frameworkCode, string? lessonCode) =>
        string.Equals(frameworkCode,
            MathematicsCurriculumPackRegistry.UaeCode, StringComparison.Ordinal) &&
        lessonCode is not null &&
        lessonCode.StartsWith(
            "PED:UAE-MOE-MATH:L7:GENERAL:", StringComparison.Ordinal);

    public static bool IsUaeG7ArabicLesson(
        string frameworkCode, string? lessonCode) =>
        IsUaeGrade7GeneralScope(frameworkCode, lessonCode) &&
        LessonPracticeContractRegistry.TryResolve(lessonCode, out var contract) &&
        contract is not null &&
        string.Equals(contract.Readiness, "READY_VERIFIED", StringComparison.Ordinal);

    // Stage D Grade 9 General Arabic must be an explicitly verified identity.
    // The UAE framework default language is English, so a broad prefix alone
    // must never publish a legacy English or draft Grade 9 lesson.
    public static bool IsUaeGrade9GeneralScope(
        string frameworkCode, string? lessonCode) =>
        string.Equals(frameworkCode,
            MathematicsCurriculumPackRegistry.UaeCode, StringComparison.Ordinal) &&
        lessonCode is not null &&
        lessonCode.StartsWith(
            "PED:UAE-MOE-MATH:L9:GENERAL:", StringComparison.Ordinal);

    // Two first-party Arabic lessons in the approved 82-item G9 General
    // index require teacher-reviewed geometry, not automated Practice. Their
    // educational pages still belong in the 82-page student course when the
    // explicit G9 release token is present. Never use a broad G9 prefix:
    // unlisted legacy English or draft identities remain hidden.
    public static bool IsUaeG9TeacherOnlyArabicLesson(
        string frameworkCode, string? lessonCode, string? practiceReleaseToken) =>
        IsUaeGrade9GeneralScope(frameworkCode, lessonCode) &&
        string.Equals(practiceReleaseToken,
            "CONFIRM_RELEASE_UAE_GRADE9_GENERAL_80_DIGITAL_PRACTICE",
            StringComparison.Ordinal) &&
        (string.Equals(lessonCode,
            "PED:UAE-MOE-MATH:L9:GENERAL:13:07:CONGRUENCE-TRANSFORMATIONS",
            StringComparison.Ordinal) ||
         string.Equals(lessonCode,
            "PED:UAE-MOE-MATH:L9:GENERAL:13:08:TRIANGLES-AND-COORDINATE-PROOF",
            StringComparison.Ordinal));

    public static bool IsUaeG9ArabicLesson(
        string frameworkCode, string? lessonCode) =>
        IsUaeGrade9GeneralScope(frameworkCode, lessonCode) &&
        ((LessonPracticeContractRegistry.TryResolve(lessonCode, out var contract) &&
          contract is not null &&
          string.Equals(contract.Readiness, "READY_VERIFIED", StringComparison.Ordinal)) ||
         IsUaeG9TeacherOnlyArabicLesson(frameworkCode, lessonCode,
             Environment.GetEnvironmentVariable(
                 "EDULYTICS_G9_DIGITAL_PRACTICE_RELEASE")));

    // G10 uses the owner-approved exact 148-item unified General blueprint.
    // Supporting lessons belong in the catalogue even without invented
    // official alignment. No old Advanced/Applied or unknown General IDs.
    private static readonly Lazy<HashSet<string>> UaeG10CanonicalCodes = new(() =>
    {
        var documents = PedagogicalLessonBlueprintRegistry.LoadEmbeddedDocuments()
            .Where(x => x.PackCode == MathematicsCurriculumPackRegistry.UaeCode &&
                x.LogicalLevel == 10 && x.Pathway == "General" &&
                x.BlueprintCode == "UAE-G10-GENERAL-T1-OGL-V1:OGL-V1").ToArray();
        if (documents.Length != 1 || documents[0].Units.Count != 18 ||
            documents[0].Lessons.Count != 148)
            throw new InvalidOperationException("G10 exact unified catalogue unavailable.");
        var codes = documents[0].Lessons.Select(x => x.LessonCode)
            .ToHashSet(StringComparer.Ordinal);
        if (codes.Count != 148 || codes.Any(x => !x.StartsWith(
            "PED:UAE-MOE-MATH:L10:GENERAL:", StringComparison.Ordinal)))
            throw new InvalidOperationException("G10 canonical identity scope drift.");
        return codes;
    });

    public static bool IsUaeGrade10Scope(string frameworkCode, string? lessonCode) =>
        string.Equals(frameworkCode, MathematicsCurriculumPackRegistry.UaeCode,
            StringComparison.Ordinal) &&
        lessonCode is not null && lessonCode.StartsWith(
            "PED:UAE-MOE-MATH:L10:", StringComparison.Ordinal);

    public static bool IsUaeG10ArabicLesson(string frameworkCode, string? lessonCode) =>
        IsUaeGrade10Scope(frameworkCode, lessonCode) &&
        UaeG10CanonicalCodes.Value.Contains(lessonCode!);

    // Owner-approved UAE G6 General Arabic (128 lessons) is independently
    // authored and published without invented formal outcome mappings.
    // The general UAE/Cambridge Supporting-lesson exclusion stays in force.
    // A caller must still enforce student enrollment, adoption and Published
    // canonical content; this only classifies a *known* curriculum identity.
    public static bool CanExposeStudentLesson(
        string frameworkCode,
        bool isSupporting,
        string? lessonCode = null) =>
        (IsUaeGrade10Scope(frameworkCode, lessonCode)
            ? IsUaeG10ArabicLesson(frameworkCode, lessonCode)
            : IsUaeGrade7GeneralScope(frameworkCode, lessonCode)
            ? IsUaeG7ArabicLesson(frameworkCode, lessonCode)
            : IsUaeGrade9GeneralScope(frameworkCode, lessonCode)
                ? IsUaeG9ArabicLesson(frameworkCode, lessonCode)
                : !isSupporting ||
        (string.Equals(frameworkCode,
             MathematicsCurriculumPackRegistry.UaeCode,
             StringComparison.Ordinal) &&
         lessonCode is not null &&
         lessonCode.StartsWith(
             "PED:UAE-MOE-MATH:L6:GENERAL:",
             StringComparison.Ordinal) &&
         (lessonCode.EndsWith(
              ":STUDENT-AR-128", StringComparison.Ordinal) ||
          string.Equals(lessonCode,
              "PED:UAE-MOE-MATH:L6:GENERAL:01:01:FACTORS-AND-MULTIPLES",
              StringComparison.Ordinal))) ||
        (
            !string.Equals(
                frameworkCode,
                MathematicsCurriculumPackRegistry.UaeCode,
                StringComparison.Ordinal) &&
            !string.Equals(
                frameworkCode,
                MathematicsCurriculumPackRegistry.CambridgeCode,
                StringComparison.Ordinal)
        ));

    public static bool IsProductionReady(
        CanonicalLessonContentStatus? status,
        bool hasOfficialAlignment) =>
        status == CanonicalLessonContentStatus.Published;
}
