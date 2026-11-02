using System.Data;
using System.Data.Common;
using Edulytics.Core.Interfaces;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Edulytics.Data.Repositories;

public sealed class SchoolTrialRepository : ISchoolTrialRepository
{
    private readonly EdulyticsDbContext _db;

    public SchoolTrialRepository(EdulyticsDbContext db)
    {
        _db = db;
    }

    public async Task<SchoolTrialWindow?> GetAsync(
        Guid schoolId,
        CancellationToken cancellationToken = default)
    {
        var connection = await OpenConnectionAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        AttachTransaction(command);
        command.CommandText =
            """
            SELECT "SchoolId", "StartsAtUtc", "EndsAtUtc", "EndedAtUtc", "CreatedAtUtc", "UpdatedAtUtc"
            FROM "SchoolTrials"
            WHERE "SchoolId" = @schoolId
            LIMIT 1;
            """;
        AddParameter(command, "@schoolId", schoolId);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
            return null;

        return new SchoolTrialWindow(
            reader.GetGuid(0),
            reader.GetDateTime(1),
            reader.GetDateTime(2),
            reader.IsDBNull(3) ? null : reader.GetDateTime(3),
            reader.GetDateTime(4),
            reader.GetDateTime(5));
    }

    public async Task<bool> CreateAsync(
        SchoolTrialWindow trial,
        CancellationToken cancellationToken = default)
    {
        var connection = await OpenConnectionAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        AttachTransaction(command);
        command.CommandText =
            """
            INSERT INTO "SchoolTrials"
                ("SchoolId", "StartsAtUtc", "EndsAtUtc", "EndedAtUtc", "CreatedAtUtc", "UpdatedAtUtc")
            VALUES
                (@schoolId, @startsAtUtc, @endsAtUtc, @endedAtUtc, @createdAtUtc, @updatedAtUtc)
            ON CONFLICT ("SchoolId") DO NOTHING;
            """;
        AddParameter(command, "@schoolId", trial.SchoolId);
        AddParameter(command, "@startsAtUtc", trial.StartsAtUtc);
        AddParameter(command, "@endsAtUtc", trial.EndsAtUtc);
        AddParameter(command, "@endedAtUtc", trial.EndedAtUtc);
        AddParameter(command, "@createdAtUtc", trial.CreatedAtUtc);
        AddParameter(command, "@updatedAtUtc", trial.UpdatedAtUtc);

        return await command.ExecuteNonQueryAsync(cancellationToken) == 1;
    }

    public async Task EndAsync(
        Guid schoolId,
        DateTime endedAtUtc,
        CancellationToken cancellationToken = default)
    {
        var connection = await OpenConnectionAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        AttachTransaction(command);
        command.CommandText =
            """
            UPDATE "SchoolTrials"
            SET "EndedAtUtc" = COALESCE("EndedAtUtc", @endedAtUtc),
                "UpdatedAtUtc" = @endedAtUtc
            WHERE "SchoolId" = @schoolId;
            """;
        AddParameter(command, "@schoolId", schoolId);
        AddParameter(command, "@endedAtUtc", endedAtUtc);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task<DbConnection> OpenConnectionAsync(CancellationToken cancellationToken)
    {
        var connection = _db.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync(cancellationToken);
        return connection;
    }

    private void AttachTransaction(DbCommand command)
    {
        if (_db.Database.CurrentTransaction is not null)
            command.Transaction = _db.Database.CurrentTransaction.GetDbTransaction();
    }

    private static void AddParameter(DbCommand command, string name, object? value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value ?? DBNull.Value;
        command.Parameters.Add(parameter);
    }
}
