namespace Edulytics.Services.Analytics;

public interface IAnalyticsService
{
    Task<AnalyticsQueryResult<AnalyticsDashboard>>
        GetDashboardAsync(
            Guid actorUserId,
            Guid? academicYearId = null,
            Guid? classGroupId = null,
            Guid? subjectId = null,
            CancellationToken cancellationToken = default);

    Task<AnalyticsQueryResult<AnalyticsStudentEvaluationPage>>
        GetStudentEvaluationAsync(
            Guid actorUserId,
            Guid studentProfileId,
            Guid academicYearId,
            Guid classGroupId,
            Guid subjectId,
            CancellationToken cancellationToken = default);

    Task<AnalyticsQueryResult<AnalyticsStudentsEvaluationPage>>
        GetStudentsEvaluationAsync(
            Guid actorUserId,
            Guid academicYearId,
            Guid classGroupId,
            Guid subjectId,
            CancellationToken cancellationToken = default);

    Task<AnalyticsQueryResult<AnalyticsTopicSkillEvaluationPage>>
        GetTopicSkillEvaluationAsync(
            Guid actorUserId,
            Guid academicYearId,
            Guid classGroupId,
            Guid subjectId,
            CancellationToken cancellationToken = default);

    Task<AnalyticsQueryResult<AnalyticsStudentReport>>
        GetStudentReportAsync(
            Guid actorUserId,
            Guid studentProfileId,
            Guid academicYearId,
            Guid classGroupId,
            Guid subjectId,
            CancellationToken cancellationToken = default);

    Task<AnalyticsCommandResult> RecalculateAsync(
        Guid actorUserId,
        CancellationToken cancellationToken = default);
}
