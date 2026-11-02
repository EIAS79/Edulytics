namespace Edulytics.Services.DirectStudents;

public sealed record RegisterDirectStudentRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName);

public enum DirectStudentRegistrationErrorCode
{
    InvalidEmail = 1,
    DuplicateEmail = 2,
    PasswordPolicy = 3,
    Required = 4,
    PersistenceFailure = 5
}

public sealed record DirectStudentRegistrationServiceResult(
    bool Succeeded,
    Guid? UserId,
    DirectStudentRegistrationErrorCode? Error,
    IReadOnlyList<string> Details)
{
    public static DirectStudentRegistrationServiceResult Success(Guid userId) =>
        new(true, userId, null, []);

    public static DirectStudentRegistrationServiceResult Failure(
        DirectStudentRegistrationErrorCode error,
        IReadOnlyList<string>? details = null) =>
        new(false, null, error, details ?? []);
}
