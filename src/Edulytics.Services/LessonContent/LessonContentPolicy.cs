using Edulytics.Core.Constants;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Enums;

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

    // Owner-approved UAE G6 General Arabic (128 lessons) is independently
    // authored and published without invented formal outcome mappings.
    // The general UAE/Cambridge Supporting-lesson exclusion stays in force.
    // A caller must still enforce student enrollment, adoption and Published
    // canonical content; this only classifies a *known* curriculum identity.
    public static bool CanExposeStudentLesson(
        string frameworkCode,
        bool isSupporting,
        string? lessonCode = null) =>
        !isSupporting ||
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
        );

    public static bool IsProductionReady(
        CanonicalLessonContentStatus? status,
        bool hasOfficialAlignment) =>
        status == CanonicalLessonContentStatus.Published;
}
