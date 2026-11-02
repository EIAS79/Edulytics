using Edulytics.Core.DirectStudents;
using Edulytics.Core.Interfaces;

namespace Edulytics.Services.DirectStudents;

public sealed class DirectStudentRegistrationService
    : IDirectStudentRegistrationService
{
    private readonly IDirectStudentAccountRepository _accounts;

    public DirectStudentRegistrationService(
        IDirectStudentAccountRepository accounts)
    {
        _accounts = accounts;
    }

    public async Task<DirectStudentRegistrationServiceResult> RegisterAsync(
        RegisterDirectStudentRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.FirstName) ||
            string.IsNullOrWhiteSpace(request.LastName))
        {
            return DirectStudentRegistrationServiceResult.Failure(
                DirectStudentRegistrationErrorCode.Required);
        }

        var result = await _accounts.RegisterAsync(
            new DirectStudentRegistrationRequest(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName),
            cancellationToken);

        if (result.Succeeded && result.UserId.HasValue)
        {
            return DirectStudentRegistrationServiceResult.Success(
                result.UserId.Value);
        }

        var mapped = result.Error switch
        {
            DirectStudentRegistrationError.InvalidEmail =>
                DirectStudentRegistrationErrorCode.InvalidEmail,
            DirectStudentRegistrationError.DuplicateEmail =>
                DirectStudentRegistrationErrorCode.DuplicateEmail,
            DirectStudentRegistrationError.PasswordPolicy =>
                DirectStudentRegistrationErrorCode.PasswordPolicy,
            _ =>
                DirectStudentRegistrationErrorCode.PersistenceFailure
        };

        return DirectStudentRegistrationServiceResult.Failure(
            mapped,
            result.IdentityErrors);
    }

    public Task<bool> IsDirectStudentAsync(
        Guid userId,
        CancellationToken cancellationToken = default) =>
        _accounts.ExistsAsync(userId, cancellationToken);
}
