using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Resilience;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Repositories;

public sealed class IdempotencyRepository
    : IIdempotencyRepository
{
    private static readonly TimeSpan Lifetime =
        TimeSpan.FromHours(24);

    private readonly EdulyticsDbContext _db;

    public IdempotencyRepository(
        EdulyticsDbContext db)
    {
        _db = db;
    }

    public async Task<IdempotencyReservation> ReserveAsync(
        Guid actorUserId,
        Guid? schoolId,
        string operation,
        string key,
        string requestHash,
        DateTime nowUtc,
        CancellationToken cancellationToken)
    {
        var record = new IdempotencyRecord
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            ActorUserId = actorUserId,
            Operation = operation,
            IdempotencyKey = key,
            RequestHash = requestHash,
            Status = IdempotencyStatus.Processing,
            CreatedAtUtc = nowUtc,
            ExpiresAtUtc = nowUtc.Add(Lifetime)
        };

        // The unique actor/operation/key index is the concurrency boundary.
        // PostgreSQL ON CONFLICT waits for a competing insert to resolve and
        // returns without surfacing an expected unique-violation exception.
        var inserted =
            await _db.Database.ExecuteSqlInterpolatedAsync(
                $"""
                INSERT INTO "IdempotencyRecords"
                    ("Id",
                     "SchoolId",
                     "ActorUserId",
                     "Operation",
                     "IdempotencyKey",
                     "RequestHash",
                     "Status",
                     "CreatedAtUtc",
                     "ExpiresAtUtc",
                     "RowVersion")
                VALUES
                    ({record.Id},
                     {record.SchoolId},
                     {record.ActorUserId},
                     {record.Operation},
                     {record.IdempotencyKey},
                     {record.RequestHash},
                     {(int)record.Status},
                     {record.CreatedAtUtc},
                     {record.ExpiresAtUtc},
                     {record.RowVersion})
                ON CONFLICT
                    ("ActorUserId",
                     "Operation",
                     "IdempotencyKey")
                DO NOTHING;
                """,
                cancellationToken);

        if (inserted == 1)
        {
            return new IdempotencyReservation(
                IdempotencyReservationOutcome.Acquired,
                record.Id,
                record.Status,
                null);
        }

        var existing =
            await _db.IdempotencyRecords
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    x =>
                        x.ActorUserId == actorUserId &&
                        x.Operation == operation &&
                        x.IdempotencyKey == key,
                    cancellationToken);

        if (existing is null)
        {
            throw new InvalidOperationException(
                "The idempotency reservation conflicted but the durable record could not be loaded.");
        }

        var outcome =
            string.Equals(
                existing.RequestHash,
                requestHash,
                StringComparison.Ordinal)
                ? IdempotencyReservationOutcome
                    .DuplicateSameRequest
                : IdempotencyReservationOutcome
                    .KeyReusedForDifferentRequest;

        return new IdempotencyReservation(
            outcome,
            existing.Id,
            existing.Status,
            existing.ResultStatusCode);
    }

    public async Task MarkCompletedAsync(
        Guid recordId,
        int statusCode,
        DateTime nowUtc,
        CancellationToken cancellationToken)
    {
        var record =
            await _db.IdempotencyRecords
                .SingleOrDefaultAsync(
                    x => x.Id == recordId,
                    cancellationToken);

        if (record is null)
            return;

        record.Status =
            IdempotencyStatus.Completed;

        record.ResultStatusCode =
            statusCode;

        record.CompletedAtUtc =
            nowUtc;

        await _db.SaveChangesAsync(
            cancellationToken);
    }

    public async Task MarkIndeterminateAsync(
        Guid recordId,
        DateTime nowUtc,
        CancellationToken cancellationToken)
    {
        var record =
            await _db.IdempotencyRecords
                .SingleOrDefaultAsync(
                    x => x.Id == recordId,
                    cancellationToken);

        if (record is null)
            return;

        record.Status =
            IdempotencyStatus.Indeterminate;

        record.CompletedAtUtc =
            nowUtc;

        await _db.SaveChangesAsync(
            cancellationToken);
    }
}
