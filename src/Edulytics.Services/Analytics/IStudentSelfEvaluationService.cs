namespace Edulytics.Services.Analytics;

public interface IStudentSelfEvaluationService
{
    Task<StudentSelfEvaluationResult<StudentSelfEvaluationPage>> GetAsync(
        Guid actorUserId,
        Guid academicYearId,
        Guid classGroupId,
        Guid subjectId,
        CancellationToken cancellationToken = default);
}
