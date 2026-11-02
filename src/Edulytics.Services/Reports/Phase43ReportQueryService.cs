using Edulytics.Core.Analytics;
using Edulytics.Core.Constants;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Reports;

namespace Edulytics.Services.Reports;

public sealed class Phase43ReportQueryService
    : IReportQueryService
{
    private readonly IAnalyticsRepository _analytics;
    private readonly ISchoolRepository _schools;
    private readonly ISchoolUserRepository _users;
    private readonly ISubjectSupervisorAssignmentRepository _subjectSupervisors;

    public Phase43ReportQueryService(
        ReportQueryService inner,
        IAnalyticsRepository analytics,
        ISchoolRepository schools,
        ISchoolUserRepository users,
        ISubjectSupervisorAssignmentRepository subjectSupervisors)
    {
        _ = inner;
        _analytics = analytics;
        _schools = schools;
        _users = users;
        _subjectSupervisors = subjectSupervisors;
    }

    public async Task<ReportQueryResult<ReportCatalog>>
        GetCatalogAsync(
            Guid actorUserId,
            CancellationToken cancellationToken = default)
    {
        var contextResult =
            await ResolveAsync(actorUserId, cancellationToken);

        if (contextResult.Value is null)
        {
            return ReportQueryResult<ReportCatalog>.Failure(
                contextResult.Error!.Value);
        }

        return ReportQueryResult<ReportCatalog>.Success(
            BuildBaseCatalog(contextResult.Value));
    }

    public async Task<ReportQueryResult<ReportCatalog>>
        GetCatalogAsync(
            Guid actorUserId,
            ReportRequest request,
            CancellationToken cancellationToken = default)
    {
        request = ReportRequestPolicy.Normalize(request);

        var contextResult =
            await ResolveAsync(actorUserId, cancellationToken);

        if (contextResult.Value is null)
        {
            return ReportQueryResult<ReportCatalog>.Failure(
                contextResult.Error!.Value);
        }

        var baseCatalog = BuildBaseCatalog(contextResult.Value);
        var scopedCatalog = BuildScopedCatalog(
            contextResult.Value,
            baseCatalog,
            request);

        return ReportQueryResult<ReportCatalog>.Success(scopedCatalog);
    }

    public async Task<ReportQueryResult<ReportCatalog>>
        ValidateAsync(
            Guid actorUserId,
            ReportRequest request,
            CancellationToken cancellationToken = default)
    {
        request = ReportRequestPolicy.Normalize(request);

        if (!ReportRequestPolicy.HasRequiredSelection(request))
        {
            return ReportQueryResult<ReportCatalog>.Failure(
                ReportErrorCode.InvalidFilter);
        }

        var contextResult =
            await ResolveAsync(actorUserId, cancellationToken);

        if (contextResult.Value is null)
        {
            return ReportQueryResult<ReportCatalog>.Failure(
                contextResult.Error!.Value);
        }

        var context = contextResult.Value;
        var baseCatalog = BuildBaseCatalog(context);

        if (!baseCatalog.AllowedKinds.Contains(request.Kind))
        {
            return ReportQueryResult<ReportCatalog>.Failure(
                ReportErrorCode.AccessDenied);
        }

        if (request.AcademicYearId.HasValue &&
            !baseCatalog.AcademicYears.Any(
                x => x.Id == request.AcademicYearId.Value))
        {
            return ReportQueryResult<ReportCatalog>.Failure(
                ReportErrorCode.AccessDenied);
        }

        var scopedCatalog = BuildScopedCatalog(
            context,
            baseCatalog,
            request);

        if (request.ClassGroupId.HasValue &&
            !scopedCatalog.ClassGroups.Any(
                x => x.Id == request.ClassGroupId.Value))
        {
            return ReportQueryResult<ReportCatalog>.Failure(
                ReportErrorCode.AccessDenied);
        }

        if (request.SubjectId.HasValue &&
            !scopedCatalog.Subjects.Any(
                x => x.Id == request.SubjectId.Value))
        {
            return ReportQueryResult<ReportCatalog>.Failure(
                ReportErrorCode.AccessDenied);
        }

        if (request.StudentProfileId.HasValue &&
            !scopedCatalog.Students.Any(
                x => x.Id == request.StudentProfileId.Value))
        {
            return ReportQueryResult<ReportCatalog>.Failure(
                ReportErrorCode.AccessDenied);
        }

        if (request.LearningOutcomeId.HasValue &&
            !scopedCatalog.LearningOutcomes.Any(
                x => x.Id == request.LearningOutcomeId.Value))
        {
            return ReportQueryResult<ReportCatalog>.Failure(
                ReportErrorCode.AccessDenied);
        }

        return ReportQueryResult<ReportCatalog>.Success(scopedCatalog);
    }

    public async Task<ReportQueryResult<ReportDocument>>
        BuildAsync(
            Guid actorUserId,
            ReportRequest request,
            int maxRows,
            CancellationToken cancellationToken = default)
    {
        if (maxRows <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxRows));
        }

        request = ReportRequestPolicy.Normalize(request);

        var validation =
            await ValidateAsync(
                actorUserId,
                request,
                cancellationToken);

        if (validation.Value is null)
        {
            return ReportQueryResult<ReportDocument>.Failure(
                validation.Error!.Value);
        }

        var contextResult =
            await ResolveAsync(actorUserId, cancellationToken);

        if (contextResult.Value is null)
        {
            return ReportQueryResult<ReportDocument>.Failure(
                contextResult.Error!.Value);
        }

        return ReportQueryResult<ReportDocument>.Success(
            BuildDocument(contextResult.Value, request, maxRows));
    }

    private async Task<ReportQueryResult<ReportContext>>
        ResolveAsync(
            Guid actorUserId,
            CancellationToken cancellationToken)
    {
        var actor = await _users.GetActorAsync(
            actorUserId,
            cancellationToken);

        if (actor is null ||
            !actor.IsActive ||
            actor.IsLocked ||
            !actor.SchoolId.HasValue)
        {
            return ReportQueryResult<ReportContext>.Failure(
                ReportErrorCode.AccessDenied);
        }

        var role =
            actor.Roles.Contains(RoleNames.SchoolAdmin)
                ? RoleNames.SchoolAdmin
                : actor.Roles.Contains(RoleNames.SubjectSupervisor)
                    ? RoleNames.SubjectSupervisor
                    : actor.Roles.Contains(RoleNames.Teacher)
                        ? RoleNames.Teacher
                        : null;

        if (role is null)
        {
            return ReportQueryResult<ReportContext>.Failure(
                ReportErrorCode.AccessDenied);
        }

        var school = await _schools.GetByIdAsync(
            actor.SchoolId.Value,
            cancellationToken);

        if (school is null || school.Status != SchoolStatus.Active)
        {
            return ReportQueryResult<ReportContext>.Failure(
                ReportErrorCode.SchoolNotActive);
        }

        var projection =
            await _analytics.GetProjectionSnapshotAsync(
                school.Id,
                cancellationToken);

        var source =
            await _analytics.GetSourceSnapshotAsync(
                school.Id,
                cancellationToken);

        var supervisedSubjectIds = new HashSet<Guid>();

        if (role == RoleNames.SubjectSupervisor)
        {
            var assignments =
                await _subjectSupervisors.ListActiveBySupervisorAsync(
                    school.Id,
                    actorUserId,
                    cancellationToken);

            supervisedSubjectIds = assignments
                .Select(x => x.SubjectId)
                .ToHashSet();
        }

        IReadOnlySet<(Guid AcademicYearId, Guid ClassGroupId, Guid SubjectId)>
            pairs = role switch
            {
                RoleNames.Teacher =>
                    projection.TeacherAssignments
                        .Where(x => x.TeacherUserId == actorUserId)
                        .Select(x =>
                            (x.AcademicYearId, x.ClassGroupId, x.SubjectId))
                        .ToHashSet(),

                RoleNames.SubjectSupervisor =>
                    projection.TeacherAssignments
                        .Where(x => supervisedSubjectIds.Contains(x.SubjectId))
                        .Select(x =>
                            (x.AcademicYearId, x.ClassGroupId, x.SubjectId))
                        .ToHashSet(),

                _ =>
                    new HashSet<(Guid, Guid, Guid)>()
            };

        return ReportQueryResult<ReportContext>.Success(
            new ReportContext(
                school.Id,
                school.Name,
                role,
                projection,
                source,
                pairs,
                supervisedSubjectIds));
    }

    private static bool PairAllowed(
        ReportContext context,
        Guid academicYearId,
        Guid classGroupId,
        Guid subjectId)
    {
        if (context.Role == RoleNames.SchoolAdmin)
        {
            return true;
        }

        return context.Pairs.Contains(
            (academicYearId, classGroupId, subjectId));
    }

    private static bool ClassAllowed(
        ReportContext context,
        Guid academicYearId,
        Guid classGroupId) =>
        context.Role == RoleNames.SchoolAdmin ||
        context.Pairs.Any(
            x =>
                x.AcademicYearId == academicYearId &&
                x.ClassGroupId == classGroupId);

    private static ReportCatalog BuildBaseCatalog(
        ReportContext context)
    {
        var projection = context.Projection;

        var years = projection.AcademicYears
            .Where(x => x.Status == AcademicStructureStatus.Active)
            .Where(x =>
                context.Role == RoleNames.SchoolAdmin ||
                context.Pairs.Any(pair => pair.AcademicYearId == x.Id))
            .OrderByDescending(x => x.StartsOn)
            .Select(x => new ReportFilterItem(x.Id, x.Name))
            .ToArray();

        var classes = projection.ClassGroups
            .Where(x => x.Status == AcademicStructureStatus.Active)
            .Where(x => ClassAllowed(context, x.AcademicYearId, x.Id))
            .OrderBy(x => x.Name)
            .Select(x => new ReportFilterItem(x.Id, x.Name))
            .ToArray();

        var subjects = projection.Subjects
            .Where(x => x.Status == AcademicStructureStatus.Active)
            .Where(x =>
                context.Role == RoleNames.SchoolAdmin ||
                (context.Role == RoleNames.SubjectSupervisor &&
                 context.SupervisedSubjectIds.Contains(x.Id)) ||
                (context.Role == RoleNames.Teacher &&
                 context.Pairs.Any(pair => pair.SubjectId == x.Id)))
            .OrderBy(x => x.Name)
            .Select(x => new ReportFilterItem(x.Id, x.Name))
            .ToArray();

        var visibleStudentIds = context.Source.StudentEnrollments
            .Where(x => ClassAllowed(
                context,
                x.AcademicYearId,
                x.ClassGroupId))
            .Select(x => x.StudentProfileId)
            .ToHashSet();

        var profiles = context.Source.StudentProfiles
            .Concat(projection.StudentProfiles)
            .GroupBy(x => x.Id)
            .ToDictionary(x => x.Key, x => x.First());

        var students = visibleStudentIds
            .Where(profiles.ContainsKey)
            .Select(id => profiles[id])
            .Where(x => x.Status == AcademicStructureStatus.Active)
            .OrderBy(x => x.DisplayName)
            .Select(x => new ReportFilterItem(
                x.Id,
                $"{x.DisplayName} ({x.StudentNumber})"))
            .ToArray();

        var visibleOutcomeIds = projection.ClassOutcomeSummaries
            .Where(x => PairAllowed(
                context,
                x.AcademicYearId,
                x.ClassGroupId,
                x.SubjectId))
            .Select(x => x.LearningOutcomeId)
            .Concat(
                projection.StudentOutcomeMasteries
                    .Where(x => PairAllowed(
                        context,
                        x.AcademicYearId,
                        x.ClassGroupId,
                        x.SubjectId))
                    .Select(x => x.LearningOutcomeId))
            .ToHashSet();

        var outcomes = projection.LearningOutcomes
            .Where(x => visibleOutcomeIds.Contains(x.Id))
            .OrderBy(x => x.Code)
            .Select(x => new ReportFilterItem(
                x.Id,
                $"{x.Code} — {x.Description}"))
            .ToArray();

        IReadOnlyList<ReportKind> allowedKinds =
        [
            ReportKind.School,
            ReportKind.Class,
            ReportKind.Subject,
            ReportKind.Student,
            ReportKind.LearningOutcome
        ];

        return new ReportCatalog(
            context.SchoolId,
            context.SchoolName,
            context.Role,
            allowedKinds,
            years,
            classes,
            subjects,
            students,
            outcomes);
    }

    private static ReportCatalog BuildScopedCatalog(
        ReportContext context,
        ReportCatalog catalog,
        ReportRequest request)
    {
        var projection = context.Projection;

        var classes = projection.ClassGroups
            .Where(x => x.Status == AcademicStructureStatus.Active)
            .Where(x => ClassAllowed(context, x.AcademicYearId, x.Id))
            .Where(x =>
                !request.AcademicYearId.HasValue ||
                x.AcademicYearId == request.AcademicYearId.Value)
            .OrderBy(x => x.Name)
            .Select(x => new ReportFilterItem(x.Id, x.Name))
            .ToArray();

        bool SubjectAvailable(Guid subjectId)
        {
            if (!request.AcademicYearId.HasValue &&
                !request.ClassGroupId.HasValue)
            {
                return true;
            }

            if (context.Role != RoleNames.SchoolAdmin)
            {
                return context.Pairs.Any(pair =>
                    pair.SubjectId == subjectId &&
                    (!request.AcademicYearId.HasValue ||
                     pair.AcademicYearId == request.AcademicYearId.Value) &&
                    (!request.ClassGroupId.HasValue ||
                     pair.ClassGroupId == request.ClassGroupId.Value));
            }

            var assignmentMatch = projection.TeacherAssignments.Any(x =>
                x.SubjectId == subjectId &&
                (!request.AcademicYearId.HasValue ||
                 x.AcademicYearId == request.AcademicYearId.Value) &&
                (!request.ClassGroupId.HasValue ||
                 x.ClassGroupId == request.ClassGroupId.Value));

            if (assignmentMatch)
            {
                return true;
            }

            return projection.ClassOutcomeSummaries.Any(x =>
                       x.SubjectId == subjectId &&
                       (!request.AcademicYearId.HasValue ||
                        x.AcademicYearId == request.AcademicYearId.Value) &&
                       (!request.ClassGroupId.HasValue ||
                        x.ClassGroupId == request.ClassGroupId.Value)) ||
                   projection.StudentOutcomeMasteries.Any(x =>
                       x.SubjectId == subjectId &&
                       (!request.AcademicYearId.HasValue ||
                        x.AcademicYearId == request.AcademicYearId.Value) &&
                       (!request.ClassGroupId.HasValue ||
                        x.ClassGroupId == request.ClassGroupId.Value));
        }

        var baseSubjectIds = catalog.Subjects.Select(x => x.Id).ToHashSet();

        var subjects = projection.Subjects
            .Where(x =>
                x.Status == AcademicStructureStatus.Active &&
                baseSubjectIds.Contains(x.Id) &&
                SubjectAvailable(x.Id))
            .OrderBy(x => x.Name)
            .Select(x => new ReportFilterItem(x.Id, x.Name))
            .ToArray();

        bool EnrollmentMatches(
            Guid academicYearId,
            Guid classGroupId) =>
            (!request.AcademicYearId.HasValue ||
             academicYearId == request.AcademicYearId.Value) &&
            (!request.ClassGroupId.HasValue ||
             classGroupId == request.ClassGroupId.Value) &&
            ClassAllowed(context, academicYearId, classGroupId);

        var visibleStudentIds = context.Source.StudentEnrollments
            .Where(x => EnrollmentMatches(
                x.AcademicYearId,
                x.ClassGroupId))
            .Select(x => x.StudentProfileId)
            .ToHashSet();

        var profiles = context.Source.StudentProfiles
            .Concat(projection.StudentProfiles)
            .GroupBy(x => x.Id)
            .ToDictionary(x => x.Key, x => x.First());

        var students = visibleStudentIds
            .Where(profiles.ContainsKey)
            .Select(id => profiles[id])
            .Where(x => x.Status == AcademicStructureStatus.Active)
            .OrderBy(x => x.DisplayName)
            .Select(x => new ReportFilterItem(
                x.Id,
                $"{x.DisplayName} ({x.StudentNumber})"))
            .ToArray();

        var visibleOutcomeIds = projection.ClassOutcomeSummaries
            .Where(x => PairAllowed(
                context,
                x.AcademicYearId,
                x.ClassGroupId,
                x.SubjectId))
            .Where(x =>
                !request.AcademicYearId.HasValue ||
                x.AcademicYearId == request.AcademicYearId.Value)
            .Where(x =>
                !request.ClassGroupId.HasValue ||
                x.ClassGroupId == request.ClassGroupId.Value)
            .Where(x =>
                !request.SubjectId.HasValue ||
                x.SubjectId == request.SubjectId.Value)
            .Select(x => x.LearningOutcomeId)
            .Concat(
                projection.StudentOutcomeMasteries
                    .Where(x => PairAllowed(
                        context,
                        x.AcademicYearId,
                        x.ClassGroupId,
                        x.SubjectId))
                    .Where(x =>
                        !request.AcademicYearId.HasValue ||
                        x.AcademicYearId == request.AcademicYearId.Value)
                    .Where(x =>
                        !request.ClassGroupId.HasValue ||
                        x.ClassGroupId == request.ClassGroupId.Value)
                    .Where(x =>
                        !request.SubjectId.HasValue ||
                        x.SubjectId == request.SubjectId.Value)
                    .Select(x => x.LearningOutcomeId))
            .ToHashSet();

        var outcomes = projection.LearningOutcomes
            .Where(x => visibleOutcomeIds.Contains(x.Id))
            .OrderBy(x => x.Code)
            .Select(x => new ReportFilterItem(
                x.Id,
                $"{x.Code} — {x.Description}"))
            .ToArray();

        return catalog with
        {
            ClassGroups = classes,
            Subjects = subjects,
            Students = students,
            LearningOutcomes = outcomes
        };
    }

    private static ReportDocument BuildDocument(
        ReportContext context,
        ReportRequest request,
        int maxRows)
    {
        var projection = context.Projection;
        var years = projection.AcademicYears.ToDictionary(x => x.Id);
        var subjects = projection.Subjects.ToDictionary(x => x.Id);
        var outcomes = projection.LearningOutcomes.ToDictionary(x => x.Id);

        var profiles = context.Source.StudentProfiles
            .Concat(projection.StudentProfiles)
            .GroupBy(x => x.Id)
            .ToDictionary(x => x.Key, x => x.First());

        string YearName(Guid id) =>
            years.TryGetValue(id, out var year) ? year.Name : string.Empty;

        string SubjectName(Guid id) =>
            subjects.TryGetValue(id, out var subject)
                ? subject.Name
                : string.Empty;

        bool Matches(
            Guid academicYearId,
            Guid classGroupId,
            Guid subjectId) =>
            PairAllowed(context, academicYearId, classGroupId, subjectId) &&
            (!request.AcademicYearId.HasValue ||
             academicYearId == request.AcademicYearId.Value) &&
            (!request.ClassGroupId.HasValue ||
             classGroupId == request.ClassGroupId.Value) &&
            (!request.SubjectId.HasValue ||
             subjectId == request.SubjectId.Value);

        return request.Kind switch
        {
            ReportKind.School => BuildPerformanceOverview(),
            ReportKind.Class => BuildClassReport(),
            ReportKind.Subject => BuildSubjectReport(),
            ReportKind.Student => BuildStudentReport(),
            ReportKind.LearningOutcome => BuildLearningOutcomeReport(),
            _ => throw new InvalidOperationException("Unsupported report kind.")
        };

        ReportDocument BuildPerformanceOverview()
        {
            var summaryRows = projection.ClassOutcomeSummaries
                .Where(x => Matches(
                    x.AcademicYearId,
                    x.ClassGroupId,
                    x.SubjectId))
                .GroupBy(x => x.AcademicYearId)
                .OrderByDescending(group =>
                    years.TryGetValue(group.Key, out var year)
                        ? year.StartsOn
                        : DateOnly.MinValue)
                .Select(group =>
                {
                    var mastery = group.Any()
                        ? group.Average(x => x.AverageMasteryPercentage)
                        : 0m;

                    var studentsWithEvidence =
                        projection.StudentOutcomeMasteries
                            .Where(x =>
                                x.AcademicYearId == group.Key &&
                                Matches(
                                    x.AcademicYearId,
                                    x.ClassGroupId,
                                    x.SubjectId))
                            .Select(x => x.StudentProfileId)
                            .Distinct()
                            .Count();

                    return new ReportRow(
                    [
                        ReportCell.Text(YearName(group.Key)),
                        ReportCell.Percentage(mastery),
                        ReportCell.Integer(studentsWithEvidence),
                        ReportCell.Integer(group.Sum(x => x.EvidenceCount)),
                        ReportCell.DateTime(group.Max(x => x.CalculatedAtUtc))
                    ]);
                })
                .ToArray();

            return CreateDocument(
                ReportKind.School,
                "ReportTitleSchool",
                [
                    new("ColumnAcademicYear", ReportCellKind.Text),
                    new("ColumnOverallMastery", ReportCellKind.Percentage),
                    new("ColumnStudentsWithEvidence", ReportCellKind.Integer),
                    new("ColumnEvidence", ReportCellKind.Integer),
                    new("ColumnCalculatedAt", ReportCellKind.DateTime)
                ],
                summaryRows);
        }

        ReportDocument BuildClassReport()
        {
            var rows = projection.ClassOutcomeSummaries
                .Where(x => Matches(
                    x.AcademicYearId,
                    x.ClassGroupId,
                    x.SubjectId))
                .GroupBy(x => x.SubjectId)
                .OrderBy(group => SubjectName(group.Key))
                .Select(group => new ReportRow(
                [
                    ReportCell.Text(SubjectName(group.Key)),
                    ReportCell.Percentage(
                        group.Average(x => x.AverageMasteryPercentage)),
                    ReportCell.Integer(group.Max(x => x.StudentCount)),
                    ReportCell.Integer(group.Sum(x => x.AtRiskStudentCount)),
                    ReportCell.Integer(group.Sum(x => x.EvidenceCount)),
                    ReportCell.DateTime(group.Max(x => x.CalculatedAtUtc))
                ]))
                .ToArray();

            return CreateDocument(
                ReportKind.Class,
                "ReportTitleClass",
                [
                    new("ColumnSubject", ReportCellKind.Text),
                    new("ColumnOverallMastery", ReportCellKind.Percentage),
                    new("ColumnStudentsWithEvidence", ReportCellKind.Integer),
                    new("ColumnAtRisk", ReportCellKind.Integer),
                    new("ColumnEvidence", ReportCellKind.Integer),
                    new("ColumnCalculatedAt", ReportCellKind.DateTime)
                ],
                rows);
        }

        ReportDocument BuildSubjectReport()
        {
            var rows = projection.ClassOutcomeSummaries
                .Where(x => Matches(
                    x.AcademicYearId,
                    x.ClassGroupId,
                    x.SubjectId))
                .Where(x => outcomes.ContainsKey(x.LearningOutcomeId))
                .OrderBy(x => outcomes[x.LearningOutcomeId].Code)
                .Select(x =>
                {
                    var outcome = outcomes[x.LearningOutcomeId];

                    return new ReportRow(
                    [
                        ReportCell.Text(outcome.Code),
                        ReportCell.Text(outcome.Description),
                        ReportCell.Percentage(x.AverageMasteryPercentage),
                        ReportCell.Integer(x.StudentCount),
                        ReportCell.Integer(x.AtRiskStudentCount),
                        ReportCell.Integer(x.EvidenceCount),
                        ReportCell.DateTime(x.CalculatedAtUtc)
                    ]);
                })
                .ToArray();

            return CreateDocument(
                ReportKind.Subject,
                "ReportTitleSubject",
                [
                    new("ColumnOutcomeCode", ReportCellKind.Text),
                    new("ColumnOutcomeDescription", ReportCellKind.Text),
                    new("ColumnMastery", ReportCellKind.Percentage),
                    new("ColumnStudents", ReportCellKind.Integer),
                    new("ColumnAtRisk", ReportCellKind.Integer),
                    new("ColumnEvidence", ReportCellKind.Integer),
                    new("ColumnCalculatedAt", ReportCellKind.DateTime)
                ],
                rows);
        }

        ReportDocument BuildStudentReport()
        {
            var rows = projection.StudentOutcomeMasteries
                .Where(x =>
                    x.StudentProfileId == request.StudentProfileId!.Value &&
                    Matches(
                        x.AcademicYearId,
                        x.ClassGroupId,
                        x.SubjectId))
                .Where(x => outcomes.ContainsKey(x.LearningOutcomeId))
                .OrderBy(x => SubjectName(x.SubjectId))
                .ThenBy(x => outcomes[x.LearningOutcomeId].Code)
                .Select(x =>
                {
                    var outcome = outcomes[x.LearningOutcomeId];

                    return new ReportRow(
                    [
                        ReportCell.Text(SubjectName(x.SubjectId)),
                        ReportCell.Text(outcome.Code),
                        ReportCell.Text(outcome.Description),
                        ReportCell.Decimal(x.EarnedScore),
                        ReportCell.Decimal(x.PossibleScore),
                        ReportCell.Percentage(x.MasteryPercentage),
                        ReportCell.Integer(x.EvidenceCount)
                    ]);
                })
                .ToArray();

            return CreateDocument(
                ReportKind.Student,
                "ReportTitleStudent",
                [
                    new("ColumnSubject", ReportCellKind.Text),
                    new("ColumnOutcomeCode", ReportCellKind.Text),
                    new("ColumnOutcomeDescription", ReportCellKind.Text),
                    new("ColumnEarned", ReportCellKind.Decimal),
                    new("ColumnPossible", ReportCellKind.Decimal),
                    new("ColumnMastery", ReportCellKind.Percentage),
                    new("ColumnEvidence", ReportCellKind.Integer)
                ],
                rows);
        }

        ReportDocument BuildLearningOutcomeReport()
        {
            var rows = projection.StudentOutcomeMasteries
                .Where(x =>
                    x.LearningOutcomeId == request.LearningOutcomeId!.Value &&
                    Matches(
                        x.AcademicYearId,
                        x.ClassGroupId,
                        x.SubjectId))
                .Where(x => profiles.ContainsKey(x.StudentProfileId))
                .OrderBy(x => profiles[x.StudentProfileId].DisplayName)
                .Select(x =>
                {
                    var student = profiles[x.StudentProfileId];

                    return new ReportRow(
                    [
                        ReportCell.Text(student.StudentNumber),
                        ReportCell.Text(student.DisplayName),
                        ReportCell.Decimal(x.EarnedScore),
                        ReportCell.Decimal(x.PossibleScore),
                        ReportCell.Percentage(x.MasteryPercentage),
                        ReportCell.Integer(x.EvidenceCount)
                    ]);
                })
                .ToArray();

            return CreateDocument(
                ReportKind.LearningOutcome,
                "ReportTitleLearningOutcome",
                [
                    new("ColumnStudentNumber", ReportCellKind.Text),
                    new("ColumnStudentName", ReportCellKind.Text),
                    new("ColumnEarned", ReportCellKind.Decimal),
                    new("ColumnPossible", ReportCellKind.Decimal),
                    new("ColumnMastery", ReportCellKind.Percentage),
                    new("ColumnEvidence", ReportCellKind.Integer)
                ],
                rows);
        }

        ReportDocument CreateDocument(
            ReportKind kind,
            string titleKey,
            IReadOnlyList<ReportColumn> columns,
            IReadOnlyList<ReportRow> allRows)
        {
            var truncated = allRows.Count > maxRows;
            var rows = allRows.Take(maxRows).ToArray();

            return new ReportDocument(
                kind,
                titleKey,
                DateTime.UtcNow,
                columns,
                rows,
                allRows.Count,
                truncated);
        }
    }

    private sealed record ReportContext(
        Guid SchoolId,
        string SchoolName,
        string Role,
        AnalyticsProjectionSnapshot Projection,
        AnalyticsSourceSnapshot Source,
        IReadOnlySet<(
            Guid AcademicYearId,
            Guid ClassGroupId,
            Guid SubjectId)> Pairs,
        IReadOnlySet<Guid> SupervisedSubjectIds);
}
