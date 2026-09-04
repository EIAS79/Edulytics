using Edulytics.Core.Constants;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;

namespace Edulytics.Services.StudentPortal;

public sealed class StudentPortalService : IStudentPortalService
{
    private readonly IStudentPortalRepository _portal;
    private readonly ISchoolUserRepository _users;
    private readonly ISchoolRepository _schools;

    public StudentPortalService(
        IStudentPortalRepository portal,
        ISchoolUserRepository users,
        ISchoolRepository schools)
    {
        _portal = portal;
        _users = users;
        _schools = schools;
    }

    public async Task<StudentPortalQueryResult<StudentPortalWorkspace>>
        GetWorkspaceAsync(
            Guid actorUserId,
            CancellationToken cancellationToken = default)
    {
        var actor = await _users.GetActorAsync(actorUserId, cancellationToken);

        if (actor is null ||
            !actor.IsActive ||
            actor.IsLocked ||
            !actor.SchoolId.HasValue ||
            actor.Roles.Count != 1 ||
            actor.Roles[0] != RoleNames.Student)
        {
            return StudentPortalQueryResult<StudentPortalWorkspace>
                .Failure(StudentPortalErrorCode.AccessDenied);
        }

        var school = await _schools.GetByIdAsync(
            actor.SchoolId.Value,
            cancellationToken);

        if (school is null || school.Status != SchoolStatus.Active)
        {
            return StudentPortalQueryResult<StudentPortalWorkspace>
                .Failure(StudentPortalErrorCode.SchoolNotActive);
        }

        var snapshot = await _portal.GetSnapshotAsync(
            school.Id,
            actorUserId,
            cancellationToken);

        if (snapshot.Profile is null)
        {
            return StudentPortalQueryResult<StudentPortalWorkspace>
                .Failure(StudentPortalErrorCode.ProfileNotLinked);
        }

        var classMap = snapshot.ClassGroups.ToDictionary(x => x.Id);
        var yearMap = snapshot.AcademicYears.ToDictionary(x => x.Id);
        var gradeMap = snapshot.GradeLevels.ToDictionary(x => x.Id);
        var subjectMap = snapshot.Subjects.ToDictionary(x => x.Id);
        var versionMap = snapshot.FrameworkVersions.ToDictionary(x => x.Id);
        var frameworkMap = snapshot.Frameworks.ToDictionary(x => x.Id);

        var enrollmentItems = snapshot.Enrollments
            .Select(enrollment =>
            {
                if (!classMap.TryGetValue(enrollment.ClassGroupId, out var classGroup) ||
                    !yearMap.TryGetValue(enrollment.AcademicYearId, out var year) ||
                    !gradeMap.TryGetValue(classGroup.GradeLevelId, out var grade))
                {
                    return null;
                }

                var levelLabel = grade.Name;
                if (TryResolveAdoption(
                        classGroup,
                        enrollment.AcademicYearId,
                        snapshot.CurriculumAdoptions,
                        out var adoption) &&
                    !string.IsNullOrWhiteSpace(adoption.CurriculumLevelLabel))
                {
                    levelLabel = adoption.CurriculumLevelLabel;
                }

                return new StudentEnrollmentItem(
                    classGroup.Id,
                    year.Id,
                    grade.Id,
                    classGroup.Name,
                    classGroup.Code,
                    year.Name,
                    levelLabel);
            })
            .Where(x => x is not null)
            .Select(x => x!)
            .ToArray();

        var activeYearIds = snapshot.AcademicYears
            .Where(x => x.Status == AcademicStructureStatus.Active)
            .Select(x => x.Id)
            .ToHashSet();

        var activeEnrollments = snapshot.Enrollments
            .Where(x => activeYearIds.Contains(x.AcademicYearId))
            .Where(x =>
                classMap.TryGetValue(x.ClassGroupId, out var classGroup) &&
                classGroup.Status == AcademicStructureStatus.Active)
            .ToArray();

        var learning = new List<StudentLearningSubjectItem>();

        foreach (var enrollment in activeEnrollments)
        {
            if (!classMap.TryGetValue(enrollment.ClassGroupId, out var classGroup) ||
                !yearMap.TryGetValue(enrollment.AcademicYearId, out var year) ||
                !gradeMap.TryGetValue(classGroup.GradeLevelId, out var grade) ||
                !TryResolveAdoption(
                    classGroup,
                    enrollment.AcademicYearId,
                    snapshot.CurriculumAdoptions,
                    out var adoption) ||
                !subjectMap.TryGetValue(adoption.SubjectId, out var subject) ||
                !versionMap.TryGetValue(adoption.FrameworkVersionId, out var version) ||
                !frameworkMap.TryGetValue(version.FrameworkId, out var framework))
            {
                continue;
            }

            if (!TryResolveCurriculumIdentity(
                    adoption,
                    framework.Code,
                    grade,
                    out var logicalLevel,
                    out var pathway,
                    out var levelLabel))
            {
                continue;
            }

            var nodes = snapshot.CurriculumNodes
                .Where(x =>
                    x.FrameworkVersionId == adoption.FrameworkVersionId &&
                    x.LogicalLevelFrom <= logicalLevel &&
                    logicalLevel <= x.LogicalLevelTo &&
                    PathwayMatches(x.Pathway, pathway))
                .OrderBy(x => x.SortOrder)
                .Select(x => new StudentLearningNodeItem(
                    x.Id,
                    x.ParentId,
                    x.NodeKind,
                    x.Code,
                    x.Title,
                    x.Pathway,
                    x.OfficialText,
                    x.SortOrder))
                .ToArray();

            learning.Add(new StudentLearningSubjectItem(
                subject.Id,
                subject.Name,
                subject.Code,
                version.Id,
                framework.Name,
                version.Name,
                year.Name,
                levelLabel,
                nodes));
        }

        learning = learning
            .GroupBy(x =>
                (
                    x.SubjectId,
                    x.FrameworkVersionId,
                    x.AcademicYearName,
                    x.GradeName))
            .Select(x => x.First())
            .OrderBy(x => x.SubjectName)
            .ThenByDescending(x => x.AcademicYearName)
            .ToList();

        var enrollmentKeys = snapshot.Enrollments
            .Select(x => (x.ClassGroupId, x.AcademicYearId))
            .ToHashSet();

        var openAssessments = snapshot.Assessments
            .Where(x =>
                x.Status == AssessmentStatus.Open &&
                enrollmentKeys.Contains((x.ClassGroupId, x.AcademicYearId)) &&
                (x.TargetType == AssessmentTargetType.Class ||
                 x.TargetStudentProfileId == snapshot.Profile.Id))
            .OrderBy(x => x.AssessmentDate)
            .ThenBy(x => x.Title)
            .Select(x =>
            {
                classMap.TryGetValue(x.ClassGroupId, out var classGroup);
                subjectMap.TryGetValue(x.SubjectId, out var subject);

                return new StudentAssessmentItem(
                    x.Id,
                    x.Title,
                    subject?.Name ?? string.Empty,
                    classGroup?.Name ?? string.Empty,
                    x.AssessmentDate,
                    x.MaxScore,
                    x.DeliveryMode,
                    x.DifficultyBand,
                    x.TargetType,
                    snapshot.Results.Any(result =>
                        result.AssessmentId == x.Id &&
                        result.StudentProfileId == snapshot.Profile.Id));
            })
            .ToArray();

        var assessmentMap = snapshot.Assessments.ToDictionary(x => x.Id);

        var resultItems = snapshot.Results
            .Where(x => x.StudentProfileId == snapshot.Profile.Id)
            .Select(result =>
            {
                if (!assessmentMap.TryGetValue(result.AssessmentId, out var assessment))
                    return null;

                subjectMap.TryGetValue(assessment.SubjectId, out var subject);

                return new StudentResultItem(
                    assessment.Id,
                    assessment.Title,
                    subject?.Name ?? string.Empty,
                    assessment.AssessmentDate,
                    result.Score,
                    assessment.MaxScore,
                    result.Percentage);
            })
            .Where(x => x is not null)
            .Select(x => x!)
            .OrderByDescending(x => x.AssessmentDate)
            .ThenBy(x => x.AssessmentTitle)
            .ToArray();

        return StudentPortalQueryResult<StudentPortalWorkspace>.Success(
            new StudentPortalWorkspace(
                school.Id,
                school.Name,
                snapshot.Profile.Id,
                snapshot.Profile.StudentNumber,
                snapshot.Profile.DisplayName,
                enrollmentItems,
                learning,
                openAssessments,
                resultItems));
    }

    private static bool TryResolveAdoption(
        ClassGroup classGroup,
        Guid academicYearId,
        IReadOnlyList<SchoolCurriculumAdoption> adoptions,
        out SchoolCurriculumAdoption adoption)
    {
        if (classGroup.CurriculumAdoptionId.HasValue)
        {
            var explicitAdoption = adoptions.SingleOrDefault(x =>
                x.Id == classGroup.CurriculumAdoptionId.Value &&
                x.IsActive &&
                x.IsPrimary &&
                x.AcademicYearId == academicYearId);

            if (explicitAdoption is not null)
            {
                adoption = explicitAdoption;
                return true;
            }

            adoption = null!;
            return false;
        }

        var candidates = adoptions
            .Where(x =>
                x.IsActive &&
                x.IsPrimary &&
                x.AcademicProgramId == classGroup.AcademicProgramId &&
                x.GradeLevelId == classGroup.GradeLevelId)
            .ToArray();

        var yearSpecific = candidates
            .Where(x => x.AcademicYearId == academicYearId)
            .ToArray();
        if (yearSpecific.Length == 1)
        {
            adoption = yearSpecific[0];
            return true;
        }
        if (yearSpecific.Length > 1)
        {
            adoption = null!;
            return false;
        }

        var defaults = candidates
            .Where(x => !x.AcademicYearId.HasValue)
            .ToArray();
        if (defaults.Length == 1)
        {
            adoption = defaults[0];
            return true;
        }

        adoption = null!;
        return false;
    }

    private static bool TryResolveCurriculumIdentity(
        SchoolCurriculumAdoption adoption,
        string frameworkCode,
        GradeLevel grade,
        out int logicalLevel,
        out string? pathway,
        out string levelLabel)
    {
        if (adoption.CurriculumLogicalLevel.HasValue &&
            !string.IsNullOrWhiteSpace(adoption.CurriculumLevelKey))
        {
            logicalLevel = adoption.CurriculumLogicalLevel.Value;
            pathway = adoption.CurriculumPathway;
            levelLabel = !string.IsNullOrWhiteSpace(adoption.CurriculumLevelLabel)
                ? adoption.CurriculumLevelLabel
                : grade.Name;
            return logicalLevel is >= 1 and <= 13;
        }

        var legacy = CurriculumLevelIdentityRegistry.ResolveLegacy(
            frameworkCode,
            grade.Name,
            grade.Order);

        if (legacy is not null)
        {
            logicalLevel = legacy.LogicalLevel;
            pathway = legacy.Pathway;
            levelLabel = legacy.Label;
            return true;
        }

        var approvedPack = MathematicsCurriculumPackRegistry.All.Any(x =>
            string.Equals(x.Code, frameworkCode, StringComparison.Ordinal));
        if (approvedPack || grade.Order is < 1 or > 13)
        {
            logicalLevel = 0;
            pathway = null;
            levelLabel = grade.Name;
            return false;
        }

        // Compatibility only for pre-pack/custom legacy frameworks. The four
        // approved Mathematics packs above must resolve through their registry.
        logicalLevel = grade.Order;
        pathway = null;
        levelLabel = grade.Name;
        return true;
    }

    private static bool PathwayMatches(
        string? nodePathway,
        string? contextPathway)
    {
        var nodeIsShared = string.IsNullOrWhiteSpace(nodePathway);
        var contextIsShared = string.IsNullOrWhiteSpace(contextPathway);

        if (contextIsShared)
            return nodeIsShared;

        return !nodeIsShared &&
               string.Equals(
                   nodePathway!.Trim(),
                   contextPathway!.Trim(),
                   StringComparison.OrdinalIgnoreCase);
    }
}
