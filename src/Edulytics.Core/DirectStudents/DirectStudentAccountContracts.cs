namespace Edulytics.Core.DirectStudents;

public sealed record DirectStudentRegistrationRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName);

public enum DirectStudentRegistrationError
{
    None = 0,
    DuplicateEmail = 1,
    InvalidEmail = 2,
    PasswordPolicy = 3,
    PersistenceFailure = 4
}

public sealed record DirectStudentRegistrationResult(
    bool Succeeded,
    Guid? UserId,
    DirectStudentRegistrationError Error,
    IReadOnlyList<string> IdentityErrors)
{
    public static DirectStudentRegistrationResult Success(Guid userId) =>
        new(true, userId, DirectStudentRegistrationError.None, []);

    public static DirectStudentRegistrationResult Failure(
        DirectStudentRegistrationError error,
        IReadOnlyList<string>? identityErrors = null) =>
        new(false, null, error, identityErrors ?? []);
}
