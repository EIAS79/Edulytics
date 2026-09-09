namespace Edulytics.Services.StudentSetup;

public interface IStudentRoleProvisioningService
{
    Task<StudentRoleProvisioningContext?> GetContextAsync(
        Guid actorUserId,
        Guid schoolId,
        Guid targetUserId,
        CancellationToken cancellationToken = default);

    Task<StudentRoleProvisioningResult> ConvertToStudentAsync(
        Guid actorUserId,
        Guid schoolId,
        Guid targetUserId,
        StudentRoleProvisioningRequest request,
        CancellationToken cancellationToken = default);
}

public interface IStudentRoleProvisioningOperations
{
    Task<StudentRoleProvisioningContext?> ReadContextAsync(
        Guid actorUserId,
        Guid schoolId,
        Guid targetUserId,
        CancellationToken cancellationToken = default);

    Task<StudentRoleProvisioningOperationResult> ChangeRoleAsync(
        Guid actorUserId,
        Guid schoolId,
        Guid targetUserId,
        string role,
        CancellationToken cancellationToken = default);

    // Legacy overloads remain available so existing operation fakes and
    // non-platform implementations keep their original contract. Platform
    // student provisioning uses the explicit-school overloads below.
    Task<StudentRoleProvisioningOperationResult> CreateProfileAsync(
        Guid actorUserId,
        Guid targetUserId,
        string studentNumber,
        string firstName,
        string lastName,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(
            StudentRoleProvisioningOperationResult.Failure(
                "Explicit school scope is required."));

    Task<StudentRoleProvisioningOperationResult> ArchiveProfileAsync(
        Guid actorUserId,
        Guid studentProfileId,
        byte[] expectedRowVersion,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(
            StudentRoleProvisioningOperationResult.Failure(
                "Explicit school scope is required."));

    Task<StudentRoleProvisioningOperationResult> RestoreProfileAsync(
        Guid actorUserId,
        Guid studentProfileId,
        byte[] expectedRowVersion,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(
            StudentRoleProvisioningOperationResult.Failure(
                "Explicit school scope is required."));

    Task<StudentRoleProvisioningOperationResult> CreateEnrollmentAsync(
        Guid actorUserId,
        Guid studentProfileId,
        Guid classGroupId,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(
            StudentRoleProvisioningOperationResult.Failure(
                "Explicit school scope is required."));

    Task<StudentRoleProvisioningOperationResult> CreateProfileAsync(
        Guid actorUserId,
        Guid schoolId,
        Guid targetUserId,
        string studentNumber,
        string firstName,
        string lastName,
        CancellationToken cancellationToken = default) =>
        CreateProfileAsync(
            actorUserId,
            targetUserId,
            studentNumber,
            firstName,
            lastName,
            cancellationToken);

    Task<StudentRoleProvisioningOperationResult> ArchiveProfileAsync(
        Guid actorUserId,
        Guid schoolId,
        Guid studentProfileId,
        byte[] expectedRowVersion,
        CancellationToken cancellationToken = default) =>
        ArchiveProfileAsync(
            actorUserId,
            studentProfileId,
            expectedRowVersion,
            cancellationToken);

    Task<StudentRoleProvisioningOperationResult> RestoreProfileAsync(
        Guid actorUserId,
        Guid schoolId,
        Guid studentProfileId,
        byte[] expectedRowVersion,
        CancellationToken cancellationToken = default) =>
        RestoreProfileAsync(
            actorUserId,
            studentProfileId,
            expectedRowVersion,
            cancellationToken);

    Task<StudentRoleProvisioningOperationResult> CreateEnrollmentAsync(
        Guid actorUserId,
        Guid schoolId,
        Guid studentProfileId,
        Guid classGroupId,
        CancellationToken cancellationToken = default) =>
        CreateEnrollmentAsync(
            actorUserId,
            studentProfileId,
            classGroupId,
            cancellationToken);
}
