using System.Data;
using System.Data.Common;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Edulytics.Web.Bootstrap;

internal static class Phase38CleanBaseline
{
    private const string ExpectedRenderServiceId = "srv-da1o4url550s73aecsn0";
    private const string BackupSchema = "phase38_backup_20260901";

    private static readonly HashSet<string> PreservedTables =
        new(StringComparer.Ordinal)
        {
            "AspNetRoles",
            "AspNetRoleClaims",
            "DataProtectionKeys",
            "__EFMigrationsHistory",
            "CurriculumFrameworks",
            "CurriculumFrameworkVersions",
            "CurriculumPackContentNodes",
            "CurriculumPackNodeLinks",
            "CurriculumPackImportStates",
            "CurriculumPedagogicalLessons",
            "CurriculumPedagogicalLessonOutcomes",
            "CurriculumLessonContents",
            "CurriculumLessonContentTranslations"
        };

    private static readonly string[] SuperAdminDependentTables =
    [
        "AspNetUserRoles",
        "AspNetUserClaims",
        "AspNetUserLogins",
        "AspNetUserTokens"
    ];

    public static async Task RunAsync(
        EdulyticsDbContext db,
        string? configuredSuperAdminEmail,
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

        if (string.IsNullOrWhiteSpace(configuredSuperAdminEmail))
        {
            throw new InvalidOperationException(
                "Phase38 cleanup requires Edulytics:SuperAdmin:Email so the real global SuperAdmin can be preserved exactly.");
        }

        await db.Database.OpenConnectionAsync(cancellationToken);
        try
        {
            var connection = db.Database.GetDbConnection();

            if (!await SchemaExistsAsync(connection, cancellationToken))
            {
                throw new InvalidOperationException(
                    $"Phase38 cleanup refused: verified backup schema '{BackupSchema}' does not exist.");
            }

            if (await CleanupCompletedAsync(connection, cancellationToken))
            {
                await VerifyCleanBaselineAsync(
                    connection,
                    configuredSuperAdminEmail.Trim(),
                    cancellationToken);
                Write("PHASE38_CLEANUP_ALREADY_VERIFIED schema={0}", BackupSchema);
                return;
            }

            await using var transaction = await db.Database.BeginTransactionAsync(
                IsolationLevel.Serializable,
                cancellationToken);

            try
            {
                var nativeTransaction = transaction.GetDbTransaction();
                var tables = await ReadFirstColumnAsync(
                    connection,
                    "SELECT tablename FROM pg_tables WHERE schemaname = 'public' ORDER BY tablename",
                    cancellationToken,
                    nativeTransaction);

                await VerifyBackupSnapshotAsync(
                    connection,
                    tables,
                    cancellationToken,
                    nativeTransaction);

                var missingPreserved = PreservedTables
                    .Where(table => !tables.Contains(table, StringComparer.Ordinal))
                    .OrderBy(table => table, StringComparer.Ordinal)
                    .ToArray();

                if (missingPreserved.Length > 0)
                {
                    throw new InvalidOperationException(
                        $"Phase38 cleanup refused: preserved tables are missing: {string.Join(',', missingPreserved)}.");
                }

                var superAdminIds = await ReadFirstColumnAsync(
                    connection,
                    "SELECT u.\"Id\"::text " +
                    "FROM \"AspNetUsers\" u " +
                    "JOIN \"AspNetUserRoles\" ur ON ur.\"UserId\" = u.\"Id\" " +
                    "JOIN \"AspNetRoles\" r ON r.\"Id\" = ur.\"RoleId\" " +
                    "WHERE lower(u.\"Email\") = lower(@email) " +
                    "AND u.\"SchoolId\" IS NULL " +
                    "AND r.\"Name\" = 'SuperAdmin'",
                    cancellationToken,
                    nativeTransaction,
                    "email",
                    configuredSuperAdminEmail.Trim());

                if (superAdminIds.Count != 1)
                {
                    throw new InvalidOperationException(
                        $"Phase38 cleanup refused: configured real SuperAdmin match count is {superAdminIds.Count}, expected exactly 1.");
                }

                var superAdminId = superAdminIds[0];

                await ExecuteAsync(
                    connection,
                    "CREATE TEMP TABLE \"_phase38_keep_AspNetUsers\" ON COMMIT DROP AS " +
                    "SELECT * FROM \"AspNetUsers\" WHERE \"Id\" = @id::uuid",
                    cancellationToken,
                    nativeTransaction,
                    "id",
                    superAdminId);

                await ExecuteAsync(
                    connection,
                    "CREATE TEMP TABLE \"_phase38_keep_AspNetUserRoles\" ON COMMIT DROP AS " +
                    "SELECT ur.* FROM \"AspNetUserRoles\" ur " +
                    "JOIN \"AspNetRoles\" r ON r.\"Id\" = ur.\"RoleId\" " +
                    "WHERE ur.\"UserId\" = @id::uuid AND r.\"Name\" = 'SuperAdmin'",
                    cancellationToken,
                    nativeTransaction,
                    "id",
                    superAdminId);

                foreach (var table in SuperAdminDependentTables.Skip(1))
                {
                    await ExecuteAsync(
                        connection,
                        $"CREATE TEMP TABLE {QuoteIdentifier("_phase38_keep_" + table)} ON COMMIT DROP AS " +
                        $"SELECT * FROM {QuoteIdentifier(table)} WHERE \"UserId\" = @id::uuid",
                        cancellationToken,
                        nativeTransaction,
                        "id",
                        superAdminId);
                }

                var truncateTables = tables
                    .Where(table => !PreservedTables.Contains(table))
                    .OrderBy(table => table, StringComparer.Ordinal)
                    .ToArray();

                if (truncateTables.Length == 0)
                {
                    throw new InvalidOperationException(
                        "Phase38 cleanup refused: no operational tables were selected for cleanup.");
                }

                Write(
                    "PHASE38_CLEANUP_BEGIN operational_tables={0} preserved_tables={1}",
                    truncateTables.Length,
                    PreservedTables.Count);

                var truncateSql =
                    "TRUNCATE TABLE " +
                    string.Join(
                        ", ",
                        truncateTables.Select(table => $"\"public\".{QuoteIdentifier(table)}")) +
                    " RESTART IDENTITY";

                await ExecuteAsync(
                    connection,
                    truncateSql,
                    cancellationToken,
                    nativeTransaction);

                await ExecuteAsync(
                    connection,
                    "INSERT INTO \"AspNetUsers\" SELECT * FROM \"_phase38_keep_AspNetUsers\"",
                    cancellationToken,
                    nativeTransaction);
                await ExecuteAsync(
                    connection,
                    "INSERT INTO \"AspNetUserRoles\" SELECT * FROM \"_phase38_keep_AspNetUserRoles\"",
                    cancellationToken,
                    nativeTransaction);

                foreach (var table in SuperAdminDependentTables.Skip(1))
                {
                    await ExecuteAsync(
                        connection,
                        $"INSERT INTO {QuoteIdentifier(table)} SELECT * FROM {QuoteIdentifier("_phase38_keep_" + table)}",
                        cancellationToken,
                        nativeTransaction);
                }

                await VerifyPreservedTablesAsync(
                    connection,
                    cancellationToken,
                    nativeTransaction);

                await VerifyOperationalTablesAsync(
                    connection,
                    truncateTables,
                    configuredSuperAdminEmail.Trim(),
                    cancellationToken,
                    nativeTransaction);

                await ExecuteAsync(
                    connection,
                    $"CREATE TABLE {QuoteIdentifier(BackupSchema)}.\"_cleanup\" (" +
                    "\"CompletedAtUtc\" timestamptz NOT NULL, " +
                    "\"SuperAdminId\" uuid NOT NULL, " +
                    "\"OperationalTablesCleared\" integer NOT NULL)",
                    cancellationToken,
                    nativeTransaction);
                await ExecuteAsync(
                    connection,
                    $"INSERT INTO {QuoteIdentifier(BackupSchema)}.\"_cleanup\" " +
                    "(\"CompletedAtUtc\", \"SuperAdminId\", \"OperationalTablesCleared\") " +
                    "VALUES (NOW(), @id::uuid, @count)",
                    cancellationToken,
                    nativeTransaction,
                    "id",
                    superAdminId,
                    "count",
                    truncateTables.Length);

                await transaction.CommitAsync(cancellationToken);
                Write(
                    "PHASE38_CLEANUP_COMMITTED operational_tables={0} real_superadmin_preserved=1",
                    truncateTables.Length);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                Write("PHASE38_CLEANUP_ROLLED_BACK");
                throw;
            }

            await VerifyCleanBaselineAsync(
                connection,
                configuredSuperAdminEmail.Trim(),
                cancellationToken);
            Write("PHASE38_CLEAN_BASELINE_VERIFIED");
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }

    private static async Task VerifyBackupSnapshotAsync(
        DbConnection connection,
        IReadOnlyList<string> publicTables,
        CancellationToken cancellationToken,
        DbTransaction transaction)
    {
        var manifestRows = await ScalarLongAsync(
            connection,
            $"SELECT COUNT(*) FROM {QuoteIdentifier(BackupSchema)}.\"_manifest\"",
            cancellationToken,
            transaction);

        if (manifestRows != publicTables.Count)
        {
            throw new InvalidOperationException(
                $"Phase38 cleanup refused: backup manifest rows={manifestRows}, public tables={publicTables.Count}.");
        }

        foreach (var table in publicTables)
        {
            var backupRows = await ScalarLongAsync(
                connection,
                $"SELECT COUNT(*) FROM {QuoteIdentifier(BackupSchema)}.{QuoteIdentifier(table)}",
                cancellationToken,
                transaction);
            var manifestRowsForTable = await ScalarLongAsync(
                connection,
                $"SELECT \"BackupRows\" FROM {QuoteIdentifier(BackupSchema)}.\"_manifest\" WHERE \"TableName\" = @table",
                cancellationToken,
                transaction,
                "table",
                table);

            if (backupRows != manifestRowsForTable)
            {
                throw new InvalidOperationException(
                    $"Phase38 cleanup refused: backup integrity mismatch for '{table}'.");
            }
        }

        Write("PHASE38_CLEANUP_BACKUP_VERIFIED tables={0}", publicTables.Count);
    }

    private static async Task VerifyPreservedTablesAsync(
        DbConnection connection,
        CancellationToken cancellationToken,
        DbTransaction transaction)
    {
        foreach (var table in PreservedTables.OrderBy(x => x, StringComparer.Ordinal))
        {
            var currentRows = await ScalarLongAsync(
                connection,
                $"SELECT COUNT(*) FROM \"public\".{QuoteIdentifier(table)}",
                cancellationToken,
                transaction);
            var backupRows = await ScalarLongAsync(
                connection,
                $"SELECT \"BackupRows\" FROM {QuoteIdentifier(BackupSchema)}.\"_manifest\" WHERE \"TableName\" = @table",
                cancellationToken,
                transaction,
                "table",
                table);

            if (currentRows != backupRows)
            {
                throw new InvalidOperationException(
                    $"Phase38 preserved-table verification failed for '{table}': current={currentRows}, backup={backupRows}.");
            }
        }
    }

    private static async Task VerifyOperationalTablesAsync(
        DbConnection connection,
        IReadOnlyList<string> operationalTables,
        string superAdminEmail,
        CancellationToken cancellationToken,
        DbTransaction transaction)
    {
        foreach (var table in operationalTables)
        {
            if (table is "AspNetUsers" or "AspNetUserRoles" or "AspNetUserClaims" or "AspNetUserLogins" or "AspNetUserTokens")
            {
                continue;
            }

            var rows = await ScalarLongAsync(
                connection,
                $"SELECT COUNT(*) FROM \"public\".{QuoteIdentifier(table)}",
                cancellationToken,
                transaction);

            if (rows != 0)
            {
                throw new InvalidOperationException(
                    $"Phase38 clean-baseline verification failed: operational table '{table}' still has {rows} rows.");
            }
        }

        var users = await ScalarLongAsync(
            connection,
            "SELECT COUNT(*) FROM \"AspNetUsers\"",
            cancellationToken,
            transaction);
        var matchingSuperAdmin = await ScalarLongAsync(
            connection,
            "SELECT COUNT(*) FROM \"AspNetUsers\" u " +
            "JOIN \"AspNetUserRoles\" ur ON ur.\"UserId\" = u.\"Id\" " +
            "JOIN \"AspNetRoles\" r ON r.\"Id\" = ur.\"RoleId\" " +
            "WHERE lower(u.\"Email\") = lower(@email) AND u.\"SchoolId\" IS NULL AND r.\"Name\" = 'SuperAdmin'",
            cancellationToken,
            transaction,
            "email",
            superAdminEmail);
        var userRoles = await ScalarLongAsync(
            connection,
            "SELECT COUNT(*) FROM \"AspNetUserRoles\"",
            cancellationToken,
            transaction);

        if (users != 1 || matchingSuperAdmin != 1 || userRoles != 1)
        {
            throw new InvalidOperationException(
                $"Phase38 real SuperAdmin verification failed: users={users}, matchingSuperAdmin={matchingSuperAdmin}, userRoles={userRoles}.");
        }
    }

    private static async Task VerifyCleanBaselineAsync(
        DbConnection connection,
        string superAdminEmail,
        CancellationToken cancellationToken)
    {
        var schools = await ScalarLongAsync(
            connection,
            "SELECT COUNT(*) FROM \"Schools\"",
            cancellationToken);
        var users = await ScalarLongAsync(
            connection,
            "SELECT COUNT(*) FROM \"AspNetUsers\"",
            cancellationToken);
        var matchingSuperAdmin = await ScalarLongAsync(
            connection,
            "SELECT COUNT(*) FROM \"AspNetUsers\" u " +
            "JOIN \"AspNetUserRoles\" ur ON ur.\"UserId\" = u.\"Id\" " +
            "JOIN \"AspNetRoles\" r ON r.\"Id\" = ur.\"RoleId\" " +
            "WHERE lower(u.\"Email\") = lower(@email) AND u.\"SchoolId\" IS NULL AND r.\"Name\" = 'SuperAdmin'",
            cancellationToken,
            parameterName: "email",
            parameterValue: superAdminEmail);

        if (schools != 0 || users != 1 || matchingSuperAdmin != 1)
        {
            throw new InvalidOperationException(
                $"Phase38 clean baseline no longer holds: schools={schools}, users={users}, matchingSuperAdmin={matchingSuperAdmin}.");
        }

        foreach (var table in PreservedTables.OrderBy(x => x, StringComparer.Ordinal))
        {
            var currentRows = await ScalarLongAsync(
                connection,
                $"SELECT COUNT(*) FROM \"public\".{QuoteIdentifier(table)}",
                cancellationToken);
            var backupRows = await ScalarLongAsync(
                connection,
                $"SELECT \"BackupRows\" FROM {QuoteIdentifier(BackupSchema)}.\"_manifest\" WHERE \"TableName\" = @table",
                cancellationToken,
                parameterName: "table",
                parameterValue: table);

            if (currentRows != backupRows)
            {
                throw new InvalidOperationException(
                    $"Phase38 preserved-table verification no longer holds for '{table}'.");
            }
        }
    }

    private static async Task<bool> CleanupCompletedAsync(
        DbConnection connection,
        CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText =
            "SELECT EXISTS (" +
            "SELECT 1 FROM information_schema.tables " +
            "WHERE table_schema = @schema AND table_name = '_cleanup')";
        AddParameter(command, "schema", BackupSchema);
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return result is true;
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

    private static async Task ExecuteAsync(
        DbConnection connection,
        string sql,
        CancellationToken cancellationToken,
        DbTransaction transaction,
        params object?[] parameters)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = sql;
        for (var index = 0; index + 1 < parameters.Length; index += 2)
        {
            AddParameter(command, (string)parameters[index]!, parameters[index + 1]);
        }
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task<IReadOnlyList<string>> ReadFirstColumnAsync(
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
