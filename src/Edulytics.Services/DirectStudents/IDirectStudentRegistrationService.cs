namespace Edulytics.Services.DirectStudents;

public interface IDirectStudentRegistrationService
{
    Task<DirectStudentRegistrationServiceResult> RegisterAsync(
        RegisterDirectStudentRequest request,
        CancellationToken cancellationToken = default);

    Task<bool> IsDirectStudentAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
