using System.Data;
using System.Data.Common;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Edulytics.Web.Bootstrap;

internal static class Phase38BackupSnapshot
{
    private const string ExpectedRenderServiceId = "srv-da1o4url550s73aecsn0";
    private const string BackupSchema = "phase38_backup_20260901";

    public static async Task RunAsync(
        EdulyticsDbContext db,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(db);

        if (!string.Equals(
                Environment.GetEnvironmentVariable("RENDER_SERVICE_ID"),
                ExpectedRenderServiceId,
                StringComparison.Ordinal))
        {
            return;
        }

        await db.Database.OpenConnectionAsync(cancellationToken);
        try
        {
            var connection = db.Database.GetDbConnection();
            if (await SchemaExistsAsync(connection, cancellationToken))
            {
                await VerifyExistingSnapshotAsync(connection, cancellationToken);
                return;
            }

            await using var transaction = await db.Database.BeginTransactionAsync(
                IsolationLevel.RepeatableRead,
                cancellationToken);

            try
            {
                Write("PHASE38_BACKUP_BEGIN schema={0} database={1}", BackupSchema, connection.Database);

                await ExecuteAsync(
                    connection,
                    $"CREATE SCHEMA {QuoteIdentifier(BackupSchema)}",
                    cancellationToken,
                    transaction.GetDbTransaction());

                await ExecuteAsync(
                    connection,
                    $"CREATE TABLE {QuoteIdentifier(BackupSchema)}.\"_manifest\" (" +
                    "\"TableName\" text PRIMARY KEY, " +
                    "\"SourceRows\" bigint NOT NULL, " +
                    "\"BackupRows\" bigint NOT NULL, " +
                    "\"SourceDatabase\" text NOT NULL, " +
                    "\"SourceCommit\" text NULL, " +
                    "\"CapturedAtUtc\" timestamptz NOT NULL)",
                    cancellationToken,
                    transaction.GetDbTransaction());

                var tables = await ReadFirstColumnAsync(
                    connection,
                    "SELECT tablename FROM pg_tables WHERE schemaname = 'public' ORDER BY tablename",
                    cancellationToken,
                    transaction.GetDbTransaction());

                long totalRows = 0;
                var sourceCommit = Environment.GetEnvironmentVariable("RENDER_GIT_COMMIT");

                foreach (var table in tables)
                {
                    var source = $"\"public\".{QuoteIdentifier(table)}";
                    var target = $"{QuoteIdentifier(BackupSchema)}.{QuoteIdentifier(table)}";

                    await ExecuteAsync(
                        connection,
                        $"CREATE TABLE {target} AS TABLE {source} WITH DATA",
                        cancellationToken,
                        transaction.GetDbTransaction());

                    var sourceRows = await ScalarLongAsync(
                        connection,
                        $"SELECT COUNT(*) FROM {source}",
                        cancellationToken,
                        transaction.GetDbTransaction());
                    var backupRows = await ScalarLongAsync(
                        connection,
                        $"SELECT COUNT(*) FROM {target}",
                        cancellationToken,
                        transaction.GetDbTransaction());

                    if (sourceRows != backupRows)
                    {
                        throw new InvalidOperationException(
                            $"Phase38 backup verification failed for table '{table}': source={sourceRows}, backup={backupRows}.");
                    }

                    await InsertManifestAsync(
                        connection,
                        table,
                        sourceRows,
                        backupRows,
                        sourceCommit,
                        cancellationToken,
                        transaction.GetDbTransaction());

                    totalRows += sourceRows;
                    Write("PHASE38_BACKUP_TABLE table={0} rows={1}", table, sourceRows);
                }

                await transaction.CommitAsync(cancellationToken);
                Write(
                    "PHASE38_BACKUP_COMMITTED schema={0} tables={1} rows={2}",
                    BackupSchema,
                    tables.Count,
                    totalRows);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                Write("PHASE38_BACKUP_ROLLED_BACK schema={0}", BackupSchema);
                throw;
            }

            await VerifyExistingSnapshotAsync(connection, cancellationToken);
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }

    private static async Task VerifyExistingSnapshotAsync(
        DbConnection connection,
        CancellationToken cancellationToken)
    {
        var sourceTables = await ReadFirstColumnAsync(
            connection,
            "SELECT tablename FROM pg_tables WHERE schemaname = 'public' ORDER BY tablename",
            cancellationToken);
        var manifestRows = await ScalarLongAsync(
            connection,
            $"SELECT COUNT(*) FROM {QuoteIdentifier(BackupSchema)}.\"_manifest\"",
            cancellationToken);

        if (manifestRows != sourceTables.Count)
        {
            throw new InvalidOperationException(
                $"Phase38 backup manifest mismatch: public tables={sourceTables.Count}, manifest rows={manifestRows}.");
        }

        long totalRows = 0;
        foreach (var table in sourceTables)
        {
            var target = $"{QuoteIdentifier(BackupSchema)}.{QuoteIdentifier(table)}";
            var backupRows = await ScalarLongAsync(
                connection,
                $"SELECT COUNT(*) FROM {target}",
                cancellationToken);
            var manifestBackupRows = await ScalarLongAsync(
                connection,
                $"SELECT \"BackupRows\" FROM {QuoteIdentifier(BackupSchema)}.\"_manifest\" WHERE \"TableName\" = @table",
                cancellationToken,
                parameterName: "table",
                parameterValue: table);

            if (backupRows != manifestBackupRows)
            {
                throw new InvalidOperationException(
                    $"Phase38 backup integrity mismatch for table '{table}': actual={backupRows}, manifest={manifestBackupRows}.");
            }

            totalRows += backupRows;
        }

        Write(
            "PHASE38_BACKUP_VERIFIED schema={0} tables={1} rows={2}",
            BackupSchema,
            sourceTables.Count,
            totalRows);
    }

    private static async Task<bool> SchemaExistsAsync(
        DbConnection connection,
        CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText =
            "SELECT EXISTS (SELECT 1 FROM pg_namespace WHERE nspname = @schema)";
        AddParameter(command, "schema", BackupSchema);
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return result is true;
    }

    private static async Task InsertManifestAsync(
        DbConnection connection,
        string table,
        long sourceRows,
        long backupRows,
        string? sourceCommit,
        CancellationToken cancellationToken,
        DbTransaction transaction)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            $"INSERT INTO {QuoteIdentifier(BackupSchema)}.\"_manifest\" " +
            "(\"TableName\", \"SourceRows\", \"BackupRows\", \"SourceDatabase\", \"SourceCommit\", \"CapturedAtUtc\") " +
            "VALUES (@table, @sourceRows, @backupRows, @database, @commit, NOW())";
        AddParameter(command, "table", table);
        AddParameter(command, "sourceRows", sourceRows);
        AddParameter(command, "backupRows", backupRows);
        AddParameter(command, "database", connection.Database);
        AddParameter(command, "commit", sourceCommit);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task ExecuteAsync(
        DbConnection connection,
        string sql,
        CancellationToken cancellationToken,
        DbTransaction transaction)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = sql;
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task<IReadOnlyList<string>> ReadFirstColumnAsync(
        DbConnection connection,
        string sql,
        CancellationToken cancellationToken,
        DbTransaction? transaction = null)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = sql;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var result = new List<string>();
        while (await reader.ReadAsync(cancellationToken))
            result.Add(reader.GetString(0));
        return result;
    }

    private static async Task<long> ScalarLongAsync(
        DbConnection connection,
        string sql,
        CancellationToken cancellationToken,
        DbTransaction? transaction = null,
        string? parameterName = null,
        object? parameterValue = null)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = sql;
        if (parameterName is not null)
            AddParameter(command, parameterName, parameterValue);
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return Convert.ToInt64(result, System.Globalization.CultureInfo.InvariantCulture);
    }

    private static void AddParameter(
        DbCommand command,
        string name,
        object? value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value ?? DBNull.Value;
        command.Parameters.Add(parameter);
    }

    private static string QuoteIdentifier(string value) =>
        $"\"{value.Replace("\"", "\"\"", StringComparison.Ordinal)}\"";

    private static void Write(string format, params object?[] values) =>
        Console.WriteLine(format, values);
}
