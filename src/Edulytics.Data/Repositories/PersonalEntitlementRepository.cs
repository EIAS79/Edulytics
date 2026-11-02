using Edulytics.Core.Entities;
using Edulytics.Core.Interfaces;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Repositories;

public sealed class PersonalEntitlementRepository
    : IPersonalEntitlementRepository
{
    private readonly EdulyticsDbContext _db;

    public PersonalEntitlementRepository(EdulyticsDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<PersonalEntitlement>>
        ListActiveByStudentAsync(
            Guid studentUserId,
            DateTime asOfUtc,
            CancellationToken cancellationToken = default) =>
        await _db.PersonalEntitlements
            .AsNoTracking()
            .Where(x =>
                x.StudentUserId == studentUserId &&
                x.IsActive &&
                x.StartsAtUtc <= asOfUtc &&
                x.EndsAtUtc > asOfUtc)
            .OrderBy(x => x.EndsAtUtc)
            .ToArrayAsync(cancellationToken);
}
