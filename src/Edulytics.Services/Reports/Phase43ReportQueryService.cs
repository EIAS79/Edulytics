using Edulytics.Core.Constants;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Reports;

namespace Edulytics.Services.Reports;

public sealed class Phase43ReportQueryService
    : IReportQueryService
{
    private readonly ReportQueryService _inner;
    private readonly IAnalyticsRepository _analytics;

    public Phase43ReportQueryService(
        ReportQueryService inner,
        IAnalyticsRepository analytics)
    {
        _inner = inner;
        _analytics = analytics;
    }

    public Task<ReportQueryResult<ReportCatalog>>
        GetCatalogAsync(
            Guid actorUserId,
            CancellationToken cancellationToken = default) =>
        _inner.GetCatalogAsync(
            actorUserId,
            cancellationToken);

    public async Task<ReportQueryResult<ReportCatalog>>
        GetCatalogAsync(
            Guid actorUserId,
            ReportRequest request,
            CancellationToken cancellationToken = default)
    {
        request =
            ReportRequestPolicy.Normalize(request);

        var catalogResult =
            await _inner.GetCatalogAsync(
                actorUserId,
                cancellationToken);

        if (catalogResult.Value is null)
        {
            return catalogResult;
        }

        var catalog =
            await BuildScopedCatalogAsync(
                actorUserId,
                catalogResult.Value,
                request,
                cancellationToken);

        return ReportQueryResult<ReportCatalog>
            .Success(catalog);
    }

    public async Task<ReportQueryResult<ReportCatalog>>
        ValidateAsync(
            Guid actorUserId,
            ReportRequest request,
            CancellationToken cancellationToken = default)
    {
        request =
            ReportRequestPolicy.Normalize(request);

        if (!ReportRequestPolicy
            .HasRequiredSelection(request))
        {
            return ReportQueryResult<ReportCatalog>
                .Failure(
                    ReportErrorCode.InvalidFilter);
        }

        if (request.Kind == ReportKind.Student)
        {
            return await ValidateStudentAsync(
                actorUserId,
                request,
                cancellationToken);
        }

        var validation =
            await _inner.ValidateAsync(
                actorUserId,
                request,
                cancellationToken);

        if (validation.Value is null)
        {
            return validation;
        }

        var hierarchyError =
            await ValidateHierarchyAsync(
                validation.Value.SchoolId,
                request,
                cancellationToken);

        return hierarchyError.HasValue
            ? ReportQueryResult<ReportCatalog>
                .Failure(hierarchyError.Value)
            : validation;
    }

    public async Task<ReportQueryResult<ReportDocument>>
        BuildAsync(
            Guid actorUserId,
            ReportRequest request,
            int maxRows,
            CancellationToken cancellationToken = default)
    {
        request =
            ReportRequestPolicy.Normalize(request);

        var validation =
            await ValidateAsync(
                actorUserId,
                request,
                cancellationToken);

        if (validation.Value is null)
        {
            return ReportQueryResult<ReportDocument>
                .Failure(validation.Error!.Value);
        }

        if (request.Kind == ReportKind.Student)
        {
            var projection =
                await _analytics
                    .GetProjectionSnapshotAsync(
                        validation.Value.SchoolId,
                        cancellationToken);

            var hasMasteryEvidence =
                projection.StudentOutcomeMasteries
                    .Any(
                        x =>
                            x.StudentProfileId ==
                                request.StudentProfileId!.Value &&
                            x.AcademicYearId ==
                                request.AcademicYearId!.Value &&
                            x.ClassGroupId ==
                                request.ClassGroupId!.Value);

            if (!hasMasteryEvidence)
            {
                return ReportQueryResult<ReportDocument>
                    .Success(
                        EmptyStudentDocument());
            }
        }

        return await _inner.BuildAsync(
            actorUserId,
            request,
            maxRows,
            cancellationToken);
    }

    private async Task<ReportQueryResult<ReportCatalog>>
        ValidateStudentAsync(
            Guid actorUserId,
            ReportRequest request,
            CancellationToken cancellationToken)
    {
        var catalogResult =
            await _inner.GetCatalogAsync(
                actorUserId,
                cancellationToken);

        if (catalogResult.Value is null)
        {
            return catalogResult;
        }

        var catalog = catalogResult.Value;

        if (!catalog.AllowedKinds.Contains(
                ReportKind.Student))
        {
            return ReportQueryResult<ReportCatalog>
                .Failure(
                    ReportErrorCode.AccessDenied);
        }

        if (!catalog.AcademicYears.Any(
                x =>
                    x.Id ==
                    request.AcademicYearId!.Value) ||
            !catalog.ClassGroups.Any(
                x =>
                    x.Id ==
                    request.ClassGroupId!.Value))
        {
            return ReportQueryResult<ReportCatalog>
                .Failure(
                    ReportErrorCode.AccessDenied);
        }

        var source =
            await _analytics.GetSourceSnapshotAsync(
                catalog.SchoolId,
                cancellationToken);

        var projection =
            await _analytics
                .GetProjectionSnapshotAsync(
                    catalog.SchoolId,
                    cancellationToken);

        var selectedClass =
            projection.ClassGroups
                .SingleOrDefault(
                    x =>
                        x.Id ==
                        request.ClassGroupId.Value);

        if (selectedClass is null)
        {
            return ReportQueryResult<ReportCatalog>
                .Failure(
                    ReportErrorCode.AccessDenied);
        }

        if (selectedClass.AcademicYearId !=
            request.AcademicYearId.Value)
        {
            return ReportQueryResult<ReportCatalog>
                .Failure(
                    ReportErrorCode.InvalidFilter);
        }

        var studentExists =
            source.StudentProfiles
                .Concat(projection.StudentProfiles)
                .Any(
                    x =>
                        x.Id ==
                            request.StudentProfileId!.Value &&
                        x.Status ==
                            AcademicStructureStatus.Active);

        if (!studentExists)
        {
            return ReportQueryResult<ReportCatalog>
                .Failure(
                    ReportErrorCode.AccessDenied);
        }

        var baseSubjectIds =
            catalog.Subjects
                .Select(x => x.Id)
                .ToHashSet();

        if (!PairVisible(
                actorUserId,
                catalog.Role,
                projection,
                baseSubjectIds,
                request.AcademicYearId.Value,
                request.ClassGroupId.Value))
        {
            return ReportQueryResult<ReportCatalog>
                .Failure(
                    ReportErrorCode.AccessDenied);
        }

        var enrolled =
            source.StudentEnrollments
                .Any(
                    x =>
                        x.StudentProfileId ==
                            request.StudentProfileId.Value &&
                        x.AcademicYearId ==
                            request.AcademicYearId.Value &&
                        x.ClassGroupId ==
                            request.ClassGroupId.Value);

        var masteryFallback =
            projection.StudentOutcomeMasteries
                .Any(
                    x =>
                        x.StudentProfileId ==
                            request.StudentProfileId.Value &&
                        x.AcademicYearId ==
                            request.AcademicYearId.Value &&
                        x.ClassGroupId ==
                            request.ClassGroupId.Value &&
                        (catalog.Role !=
                            RoleNames.SubjectSupervisor ||
                         baseSubjectIds.Contains(
                            x.SubjectId)));

        if (!enrolled && !masteryFallback)
        {
            return ReportQueryResult<ReportCatalog>
                .Failure(
                    ReportErrorCode.InvalidFilter);
        }

        var scopedCatalog =
            await BuildScopedCatalogAsync(
                actorUserId,
                catalog,
                request,
                cancellationToken);

        return ReportQueryResult<ReportCatalog>
            .Success(scopedCatalog);
    }

    private async Task<ReportCatalog>
        BuildScopedCatalogAsync(
            Guid actorUserId,
            ReportCatalog catalog,
            ReportRequest request,
            CancellationToken cancellationToken)
    {
        var source =
            await _analytics.GetSourceSnapshotAsync(
                catalog.SchoolId,
                cancellationToken);

        var projection =
            await _analytics
                .GetProjectionSnapshotAsync(
                    catalog.SchoolId,
                    cancellationToken);

        var baseClassIds =
            catalog.ClassGroups
                .Select(x => x.Id)
                .ToHashSet();

        var baseSubjectIds =
            catalog.Subjects
                .Select(x => x.Id)
                .ToHashSet();

        var baseOutcomeIds =
            catalog.LearningOutcomes
                .Select(x => x.Id)
                .ToHashSet();

        var classes =
            projection.ClassGroups
                .Where(
                    x =>
                        baseClassIds.Contains(x.Id) &&
                        x.Status ==
                            AcademicStructureStatus.Active)
                .Where(
                    x =>
                        !request.AcademicYearId.HasValue ||
                        x.AcademicYearId ==
                            request.AcademicYearId.Value)
                .OrderBy(x => x.Name)
                .Select(
                    x =>
                        new ReportFilterItem(
                            x.Id,
                            x.Name))
                .ToArray();

        bool SubjectMatchesSelection(
            Guid subjectId)
        {
            if (!request.AcademicYearId.HasValue &&
                !request.ClassGroupId.HasValue)
            {
                return true;
            }

            bool AssignmentMatches(
                Core.Entities.TeacherAssignment x) =>
                x.SubjectId == subjectId &&
                (!request.AcademicYearId.HasValue ||
                 x.AcademicYearId ==
                    request.AcademicYearId.Value) &&
                (!request.ClassGroupId.HasValue ||
                 x.ClassGroupId ==
                    request.ClassGroupId.Value);

            if (catalog.Role ==
                RoleNames.SchoolAdmin)
            {
                return true;
            }

            if (catalog.Role == RoleNames.Teacher)
            {
                return projection.TeacherAssignments
                    .Any(
                        x =>
                            x.TeacherUserId ==
                                actorUserId &&
                            AssignmentMatches(x));
            }

            return projection.TeacherAssignments
                       .Any(AssignmentMatches) ||
                   projection.ClassOutcomeSummaries
                       .Any(
                           x =>
                               x.SubjectId == subjectId &&
                               (!request.AcademicYearId.HasValue ||
                                x.AcademicYearId ==
                                    request.AcademicYearId.Value) &&
                               (!request.ClassGroupId.HasValue ||
                                x.ClassGroupId ==
                                    request.ClassGroupId.Value));
        }

        var subjects =
            projection.Subjects
                .Where(
                    x =>
                        baseSubjectIds.Contains(x.Id) &&
                        x.Status ==
                            AcademicStructureStatus.Active &&
                        SubjectMatchesSelection(x.Id))
                .OrderBy(x => x.Name)
                .Select(
                    x =>
                        new ReportFilterItem(
                            x.Id,
                            x.Name))
                .ToArray();

        bool EnrollmentMatchesSelection(
            Guid yearId,
            Guid classId) =>
            (!request.AcademicYearId.HasValue ||
             request.AcademicYearId.Value == yearId) &&
            (!request.ClassGroupId.HasValue ||
             request.ClassGroupId.Value == classId) &&
            PairVisible(
                actorUserId,
                catalog.Role,
                projection,
                baseSubjectIds,
                yearId,
                classId);

        var visibleStudentIds =
            source.StudentEnrollments
                .Where(
                    x =>
                        EnrollmentMatchesSelection(
                            x.AcademicYearId,
                            x.ClassGroupId))
                .Select(x => x.StudentProfileId)
                .ToHashSet();

        visibleStudentIds.UnionWith(
            projection.StudentOutcomeMasteries
                .Where(
                    x =>
                        EnrollmentMatchesSelection(
                            x.AcademicYearId,
                            x.ClassGroupId) &&
                        (catalog.Role !=
                            RoleNames.SubjectSupervisor ||
                         baseSubjectIds.Contains(
                            x.SubjectId)))
                .Select(x => x.StudentProfileId));

        var profiles =
            source.StudentProfiles
                .Concat(projection.StudentProfiles)
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x => x.First());

        var students =
            visibleStudentIds
                .Where(profiles.ContainsKey)
                .Select(id => profiles[id])
                .Where(
                    x =>
                        x.Status ==
                            AcademicStructureStatus.Active)
                .OrderBy(x => x.DisplayName)
                .Select(
                    x =>
                        new ReportFilterItem(
                            x.Id,
                            $"{x.DisplayName} " +
                            $"({x.StudentNumber})"))
                .ToArray();

        var visibleOutcomeIds =
            projection.ClassOutcomeSummaries
                .Where(
                    x =>
                        baseOutcomeIds.Contains(
                            x.LearningOutcomeId) &&
                        (!request.AcademicYearId.HasValue ||
                         x.AcademicYearId ==
                            request.AcademicYearId.Value) &&
                        (!request.ClassGroupId.HasValue ||
                         x.ClassGroupId ==
                            request.ClassGroupId.Value))
                .Select(x => x.LearningOutcomeId)
                .ToHashSet();

        var outcomes =
            request.AcademicYearId.HasValue ||
            request.ClassGroupId.HasValue
                ? catalog.LearningOutcomes
                    .Where(
                        x =>
                            visibleOutcomeIds.Contains(
                                x.Id))
                    .ToArray()
                : catalog.LearningOutcomes;

        return catalog with
        {
            ClassGroups = classes,
            Subjects = subjects,
            Students = students,
            LearningOutcomes = outcomes
        };
    }

    private static bool PairVisible(
        Guid actorUserId,
        string role,
        Core.Analytics.AnalyticsProjectionSnapshot projection,
        IReadOnlySet<Guid> visibleSubjectIds,
        Guid academicYearId,
        Guid classGroupId)
    {
        if (role == RoleNames.SchoolAdmin)
        {
            return true;
        }

        if (role == RoleNames.Teacher)
        {
            return projection.TeacherAssignments
                .Any(
                    x =>
                        x.TeacherUserId == actorUserId &&
                        x.AcademicYearId == academicYearId &&
                        x.ClassGroupId == classGroupId);
        }

        return projection.TeacherAssignments
                   .Any(
                       x =>
                           x.AcademicYearId ==
                               academicYearId &&
                           x.ClassGroupId == classGroupId &&
                           visibleSubjectIds.Contains(
                               x.SubjectId)) ||
               projection.ClassOutcomeSummaries
                   .Any(
                       x =>
                           x.AcademicYearId ==
                               academicYearId &&
                           x.ClassGroupId == classGroupId &&
                           visibleSubjectIds.Contains(
                               x.SubjectId)) ||
               projection.StudentOutcomeMasteries
                   .Any(
                       x =>
                           x.AcademicYearId ==
                               academicYearId &&
                           x.ClassGroupId == classGroupId &&
                           visibleSubjectIds.Contains(
                               x.SubjectId));
    }

    private async Task<ReportErrorCode?>
        ValidateHierarchyAsync(
            Guid schoolId,
            ReportRequest request,
            CancellationToken cancellationToken)
    {
        if (!request.ClassGroupId.HasValue)
        {
            return null;
        }

        var projection =
            await _analytics
                .GetProjectionSnapshotAsync(
                    schoolId,
                    cancellationToken);

        var selectedClass =
            projection.ClassGroups
                .SingleOrDefault(
                    x =>
                        x.Id ==
                        request.ClassGroupId.Value);

        if (selectedClass is null)
        {
            return ReportErrorCode.AccessDenied;
        }

        if (request.AcademicYearId.HasValue &&
            selectedClass.AcademicYearId !=
                request.AcademicYearId.Value)
        {
            return ReportErrorCode.InvalidFilter;
        }

        if (request.Kind ==
            ReportKind.LearningOutcome)
        {
            var outcomeMatchesClass =
                projection.ClassOutcomeSummaries
                    .Any(
                        x =>
                            x.LearningOutcomeId ==
                                request.LearningOutcomeId!.Value &&
                            x.AcademicYearId ==
                                request.AcademicYearId!.Value &&
                            x.ClassGroupId ==
                                request.ClassGroupId.Value);

            if (!outcomeMatchesClass)
            {
                return ReportErrorCode.InvalidFilter;
            }
        }

        return null;
    }

    private static ReportDocument
        EmptyStudentDocument() =>
        new(
            ReportKind.Student,
            "ReportTitleStudent",
            DateTime.UtcNow,
            [
                new(
                    "ColumnStudentNumber",
                    ReportCellKind.Text),
                new(
                    "ColumnStudentName",
                    ReportCellKind.Text),
                new(
                    "ColumnAcademicYear",
                    ReportCellKind.Text),
                new(
                    "ColumnClass",
                    ReportCellKind.Text),
                new(
                    "ColumnSubject",
                    ReportCellKind.Text),
                new(
                    "ColumnOutcomeCode",
                    ReportCellKind.Text),
                new(
                    "ColumnOutcomeDescription",
                    ReportCellKind.Text),
                new(
                    "ColumnEarned",
                    ReportCellKind.Decimal),
                new(
                    "ColumnPossible",
                    ReportCellKind.Decimal),
                new(
                    "ColumnMastery",
                    ReportCellKind.Percentage),
                new(
                    "ColumnEvidence",
                    ReportCellKind.Integer)
            ],
            [],
            0,
            false);
}
