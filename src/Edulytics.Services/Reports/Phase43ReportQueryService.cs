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

        return await _inner.BuildAsync(
            actorUserId,
            request,
            maxRows,
            cancellationToken);
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

        if (request.Kind == ReportKind.Student)
        {
            var studentMatchesClass =
                projection.StudentOutcomeMasteries
                    .Any(
                        x =>
                            x.StudentProfileId ==
                                request.StudentProfileId!.Value &&
                            x.AcademicYearId ==
                                request.AcademicYearId!.Value &&
                            x.ClassGroupId ==
                                request.ClassGroupId.Value);

            if (!studentMatchesClass)
            {
                return ReportErrorCode.InvalidFilter;
            }
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
}
