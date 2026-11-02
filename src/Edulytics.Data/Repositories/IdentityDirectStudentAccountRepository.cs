using System.Net.Mail;
using Edulytics.Core.Constants;
using Edulytics.Core.DirectStudents;
using Edulytics.Core.Entities;
using Edulytics.Core.Interfaces;
using Edulytics.Data.Contexts;
using Edulytics.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Repositories;

public sealed class IdentityDirectStudentAccountRepository
    : IDirectStudentAccountRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly EdulyticsDbContext _db;

    public IdentityDirectStudentAccountRepository(
        UserManager<ApplicationUser> userManager,
        EdulyticsDbContext db)
    {
        _userManager = userManager;
        _db = db;
    }

    public Task<bool> ExistsAsync(
        Guid userId,
        CancellationToken cancellationToken = default) =>
        _db.DirectStudentProfiles
            .AsNoTracking()
            .AnyAsync(
                x => x.UserId == userId && x.IsActive,
                cancellationToken);

    public Task<DirectStudentProfile?> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default) =>
        _db.DirectStudentProfiles
            .AsNoTracking()
            .SingleOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);

    public async Task<DirectStudentRegistrationResult> RegisterAsync(
        DirectStudentRegistrationRequest request,
        CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim();
        var firstName = request.FirstName.Trim();
        var lastName = request.LastName.Trim();

        if (!IsValidEmail(email))
        {
            return DirectStudentRegistrationResult.Failure(
                DirectStudentRegistrationError.InvalidEmail);
        }

        if (await _userManager.FindByEmailAsync(email) is not null)
        {
            return DirectStudentRegistrationResult.Failure(
                DirectStudentRegistrationError.DuplicateEmail);
        }

        await using var transaction =
            await _db.Database.BeginTransactionAsync(cancellationToken);

        var now = DateTime.UtcNow;
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = email,
            Email = email,
            EmailConfirmed = false,
            SchoolId = null,
            IsActive = true,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        var create = await _userManager.CreateAsync(
            user,
            request.Password);

        if (!create.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);

            var passwordFailure = create.Errors.Any(error =>
                error.Code.Contains("Password", StringComparison.OrdinalIgnoreCase));

            return DirectStudentRegistrationResult.Failure(
                passwordFailure
                    ? DirectStudentRegistrationError.PasswordPolicy
                    : DirectStudentRegistrationError.PersistenceFailure,
                create.Errors.Select(x => x.Description).ToArray());
        }

        var role = await _userManager.AddToRoleAsync(
            user,
            RoleNames.Student);

        if (!role.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);

            return DirectStudentRegistrationResult.Failure(
                DirectStudentRegistrationError.PersistenceFailure,
                role.Errors.Select(x => x.Description).ToArray());
        }

        _db.DirectStudentProfiles.Add(
            new DirectStudentProfile
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FirstName = firstName,
                LastName = lastName,
                DisplayName = $"{firstName} {lastName}".Trim(),
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            });

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return DirectStudentRegistrationResult.Success(user.Id);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            return DirectStudentRegistrationResult.Failure(
                DirectStudentRegistrationError.PersistenceFailure);
        }
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            return new MailAddress(email).Address == email;
        }
        catch
        {
            return false;
        }
    }
}
