using System.Data;
using System.Data.Common;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Edulytics.Web.Bootstrap;

internal static class Phase38CleanBaselineV3
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

    private static readonly HashSet<string> PreservedIdentityTables =
        new(StringComparer.Ordinal)
        {
            "AspNetUsers",
            "AspNetUserRoles",
            "AspNetUserClaims",
            "AspNetUserLogins",
            "AspNetUserTokens"
        };

    private static readonly string[] SuperAdminDependentTables =
    [
        "AspNetUserRoles",
        "AspNetUserClaims",
        "AspNetUserLogins",
        "AspNetUserTokens"
    ];

    private sealed record PreservedState(long Rows, string Hash);

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

        var superAdminEmail = configuredSuperAdminEmail.Trim();

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
                await VerifyCleanBaselineAsync(connection, superAdminEmail, cancellationToken);
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

                var preservedBaseline = await CapturePreservedBaselineAsync(
                    connection,
                    cancellationToken,
                    nativeTransaction);

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
                    superAdminEmail);

                if (superAdminIds.Count != 1)
                {
                    throw new InvalidOperationException(
                        $"Phase38 cleanup refused: configured real SuperAdmin match count is {superAdminIds.Count}, expected exactly 1.");
                }

                var superAdminId = superAdminIds[0];

                await PreserveSuperAdminAsync(
                    connection,
                    superAdminId,
                    cancellationToken,
                    nativeTransaction);

                var operationalTables = tables
                    .Where(table => !PreservedTables.Contains(table))
                    .OrderBy(table => table, StringComparer.Ordinal)
                    .ToArray();

                if (operationalTables.Length == 0)
                {
                    throw new InvalidOperationException(
                        "Phase38 cleanup refused: no operational tables were selected for cleanup.");
                }

                var deleteOrder = await BuildDeleteOrderAsync(
                    connection,
                    operationalTables,
                    cancellationToken,
                    nativeTransaction);

                Write(
                    "PHASE38_CLEANUP_BEGIN operational_tables={0} preserved_tables={1} strategy=delete_fk_order_preserved_snapshot",
                    operationalTables.Length,
                    PreservedTables.Count);

                foreach (var table in deleteOrder)
                {
                    await ExecuteAsync(
                        connection,
                        $"DELETE FROM \"public\".{QuoteIdentifier(table)}",
                        cancellationToken,
                        nativeTransaction);
                }

                await RestoreSuperAdminAsync(
                    connection,
                    cancellationToken,
                    nativeTransaction);

                await VerifyPreservedUnchangedAsync(
                    connection,
                    preservedBaseline,
                    cancellationToken,
                    nativeTransaction);

                await VerifyOperationalTablesAsync(
                    connection,
                    operationalTables,
                    superAdminEmail,
                    cancellationToken,
                    nativeTransaction);

                await ExecuteAsync(
                    connection,
                    $"CREATE TABLE {QuoteIdentifier(BackupSchema)}.\"_cleanup\" (" +
                    "\"CompletedAtUtc\" timestamptz NOT NULL, " +
                    "\"SuperAdminId\" uuid NOT NULL, " +
                    "\"OperationalTablesCleared\" integer NOT NULL, " +
                    "\"PreservedTablesVerified\" integer NOT NULL, " +
                    "\"Strategy\" text NOT NULL)",
                    cancellationToken,
                    nativeTransaction);

                await ExecuteAsync(
                    connection,
                    $"INSERT INTO {QuoteIdentifier(BackupSchema)}.\"_cleanup\" " +
                    "(\"CompletedAtUtc\", \"SuperAdminId\", \"OperationalTablesCleared\", \"PreservedTablesVerified\", \"Strategy\") " +
                    "VALUES (NOW(), @id::uuid, @count, @preserved, 'delete_fk_order_preserved_snapshot')",
                    cancellationToken,
                    nativeTransaction,
                    "id",
                    superAdminId,
                    "count",
                    operationalTables.Length,
                    "preserved",
                    preservedBaseline.Count);

                await transaction.CommitAsync(cancellationToken);
                Write(
                    "PHASE38_CLEANUP_COMMITTED operational_tables={0} preserved_tables_verified={1} real_superadmin_preserved=1 strategy=delete_fk_order_preserved_snapshot",
                    operationalTables.Length,
                    preservedBaseline.Count);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                Write("PHASE38_CLEANUP_ROLLED_BACK");
                throw;
            }

            await VerifyCleanBaselineAsync(
                connection,
                superAdminEmail,
                cancellationToken);
            Write("PHASE38_CLEAN_BASELINE_VERIFIED");
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }

    private static async Task<IReadOnlyDictionary<string, PreservedState>> CapturePreservedBaselineAsync(
        DbConnection connection,
        CancellationToken cancellationToken,
        DbTransaction transaction)
    {
        var baseline = new Dictionary<string, PreservedState>(StringComparer.Ordinal);
        var driftCount = 0;

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
                    $"Phase38 preserved-table row-count gate failed for '{table}': current={currentRows}, backup={backupRows}.");
            }

            var currentHash = await TableContentHashAsync(
                connection,
                "public",
                table,
                cancellationToken,
                transaction);
            var backupHash = await TableContentHashAsync(
                connection,
                BackupSchema,
                table,
                cancellationToken,
                transaction);

            if (!string.Equals(currentHash, backupHash, StringComparison.Ordinal))
            {
                driftCount++;
                Write("PHASE38_PRESERVED_PRE_CLEANUP_DRIFT table={0}", table);
            }

            baseline.Add(table, new PreservedState(currentRows, currentHash));
        }

        Write(
            "PHASE38_PRESERVED_BASELINE_CAPTURED tables={0} pre_cleanup_backup_drift={1}",
            baseline.Count,
            driftCount);

        return baseline;
    }

    private static async Task VerifyPreservedUnchangedAsync(
        DbConnection connection,
        IReadOnlyDictionary<string, PreservedState> baseline,
        CancellationToken cancellationToken,
        DbTransaction transaction)
    {
        foreach (var pair in baseline.OrderBy(x => x.Key, StringComparer.Ordinal))
        {
            var currentRows = await ScalarLongAsync(
                connection,
                $"SELECT COUNT(*) FROM \"public\".{QuoteIdentifier(pair.Key)}",
                cancellationToken,
                transaction);
            var currentHash = await TableContentHashAsync(
                connection,
                "public",
                pair.Key,
                cancellationToken,
                transaction);

            if (currentRows != pair.Value.Rows ||
                !string.Equals(currentHash, pair.Value.Hash, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Phase38 cleanup mutated preserved table '{pair.Key}'; transaction will roll back.");
            }
        }

        Write("PHASE38_PRESERVED_UNCHANGED_VERIFIED tables={0}", baseline.Count);
    }

    private static async Task VerifyPreservedCountsAgainstBackupAsync(
        DbConnection connection,
        CancellationToken cancellationToken,
        DbTransaction? transaction = null)
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
                    $"Phase38 preserved-table row-count verification failed for '{table}': current={currentRows}, backup={backupRows}.");
            }
        }
    }

    private static async Task PreserveSuperAdminAsync(
        DbConnection connection,
        string superAdminId,
        CancellationToken cancellationToken,
        DbTransaction transaction)
    {
        await ExecuteAsync(
            connection,
            "CREATE TEMP TABLE \"_phase38_keep_AspNetUsers\" ON COMMIT DROP AS " +
            "SELECT * FROM \"AspNetUsers\" WHERE \"Id\" = @id::uuid",
            cancellationToken,
            transaction,
            "id",
            superAdminId);

        await ExecuteAsync(
            connection,
            "CREATE TEMP TABLE \"_phase38_keep_AspNetUserRoles\" ON COMMIT DROP AS " +
            "SELECT ur.* FROM \"AspNetUserRoles\" ur " +
            "JOIN \"AspNetRoles\" r ON r.\"Id\" = ur.\"RoleId\" " +
            "WHERE ur.\"UserId\" = @id::uuid AND r.\"Name\" = 'SuperAdmin'",
            cancellationToken,
            transaction,
            "id",
            superAdminId);

        foreach (var table in SuperAdminDependentTables.Skip(1))
        {
            await ExecuteAsync(
                connection,
                $"CREATE TEMP TABLE {QuoteIdentifier("_phase38_keep_" + table)} ON COMMIT DROP AS " +
                $"SELECT * FROM {QuoteIdentifier(table)} WHERE \"UserId\" = @id::uuid",
                cancellationToken,
                transaction,
                "id",
                superAdminId);
        }
    }

    private static async Task RestoreSuperAdminAsync(
        DbConnection connection,
        CancellationToken cancellationToken,
        DbTransaction transaction)
    {
        await ExecuteAsync(
            connection,
            "INSERT INTO \"AspNetUsers\" SELECT * FROM \"_phase38_keep_AspNetUsers\"",
            cancellationToken,
            transaction);

        await ExecuteAsync(
            connection,
            "INSERT INTO \"AspNetUserRoles\" SELECT * FROM \"_phase38_keep_AspNetUserRoles\"",
            cancellationToken,
            transaction);

        foreach (var table in SuperAdminDependentTables.Skip(1))
        {
            await ExecuteAsync(
                connection,
                $"INSERT INTO {QuoteIdentifier(table)} SELECT * FROM {QuoteIdentifier("_phase38_keep_" + table)}",
                cancellationToken,
                transaction);
        }
    }

    private static async Task<IReadOnlyList<string>> BuildDeleteOrderAsync(
        DbConnection connection,
        IReadOnlyCollection<string> operationalTables,
        CancellationToken cancellationToken,
        DbTransaction transaction)
    {
        var operational = new HashSet<string>(operationalTables, StringComparer.Ordinal);
        var outgoing = operational.ToDictionary(
            table => table,
            _ => new HashSet<string>(StringComparer.Ordinal),
            StringComparer.Ordinal);
        var incomingCount = operational.ToDictionary(
            table => table,
            _ => 0,
            StringComparer.Ordinal);

        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            "SELECT child.relname, parent.relname " +
            "FROM pg_constraint c " +
            "JOIN pg_class child ON child.oid = c.conrelid " +
            "JOIN pg_namespace child_ns ON child_ns.oid = child.relnamespace " +
            "JOIN pg_class parent ON parent.oid = c.confrelid " +
            "JOIN pg_namespace parent_ns ON parent_ns.oid = parent.relnamespace " +
            "WHERE c.contype = 'f' " +
            "AND child_ns.nspname = 'public' " +
            "AND parent_ns.nspname = 'public'";

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            var child = reader.GetString(0);
            var parent = reader.GetString(1);

            if (child == parent ||
                !operational.Contains(child) ||
                !operational.Contains(parent))
            {
                continue;
            }

            if (outgoing[child].Add(parent))
                incomingCount[parent]++;
        }

        var ready = new SortedSet<string>(
            incomingCount
                .Where(pair => pair.Value == 0)
                .Select(pair => pair.Key),
            StringComparer.Ordinal);
        var order = new List<string>(operational.Count);

        while (ready.Count > 0)
        {
            var child = ready.Min!;
            ready.Remove(child);
            order.Add(child);

            foreach (var parent in outgoing[child].OrderBy(x => x, StringComparer.Ordinal))
            {
                incomingCount[parent]--;
                if (incomingCount[parent] == 0)
                    ready.Add(parent);
            }
        }

        if (order.Count != operational.Count)
        {
            var cycleTables = incomingCount
                .Where(pair => pair.Value > 0)
                .Select(pair => pair.Key)
                .OrderBy(x => x, StringComparer.Ordinal)
                .ToArray();

            throw new InvalidOperationException(
                $"Phase38 cleanup refused: cyclic operational foreign keys prevent safe ordered DELETE: {string.Join(',', cycleTables)}.");
        }

        return order;
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

    private static async Task VerifyOperationalTablesAsync(
        DbConnection connection,
        IReadOnlyCollection<string> operationalTables,
        string superAdminEmail,
        CancellationToken cancellationToken,
        DbTransaction? transaction = null)
    {
        foreach (var table in operationalTables.OrderBy(x => x, StringComparer.Ordinal))
        {
            if (PreservedIdentityTables.Contains(table))
                continue;

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
        var schoolScopedUsers = await ScalarLongAsync(
            connection,
            "SELECT COUNT(*) FROM \"AspNetUsers\" WHERE \"SchoolId\" IS NOT NULL",
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

        if (users != 1 || schoolScopedUsers != 0 || matchingSuperAdmin != 1 || userRoles != 1)
        {
            throw new InvalidOperationException(
                $"Phase38 real SuperAdmin verification failed: users={users}, schoolScopedUsers={schoolScopedUsers}, matchingSuperAdmin={matchingSuperAdmin}, userRoles={userRoles}.");
        }
    }

    private static async Task VerifyCleanBaselineAsync(
        DbConnection connection,
        string superAdminEmail,
        CancellationToken cancellationToken)
    {
        var tables = await ReadFirstColumnAsync(
            connection,
            "SELECT tablename FROM pg_tables WHERE schemaname = 'public' ORDER BY tablename",
            cancellationToken);
        var operationalTables = tables
            .Where(table => !PreservedTables.Contains(table))
            .ToArray();

        var schools = await ScalarLongAsync(
            connection,
            "SELECT COUNT(*) FROM \"Schools\"",
            cancellationToken);

        if (schools != 0)
        {
            throw new InvalidOperationException(
                $"Phase38 clean baseline no longer holds: schools={schools}.");
        }

        await VerifyOperationalTablesAsync(
            connection,
            operationalTables,
            superAdminEmail,
            cancellationToken);
        await VerifyPreservedCountsAgainstBackupAsync(
            connection,
            cancellationToken);
    }

    private static async Task<string> TableContentHashAsync(
        DbConnection connection,
        string schema,
        string table,
        CancellationToken cancellationToken,
        DbTransaction? transaction = null)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            "SELECT md5(COALESCE(string_agg(payload, E'\\n' ORDER BY payload), '')) " +
            "FROM (SELECT to_jsonb(t)::text AS payload FROM " +
            $"{QuoteIdentifier(schema)}.{QuoteIdentifier(table)} t) q";
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return Convert.ToString(result, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty;
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
            AddParameter(command, (string)parameters[index]!, parameters[index + 1]);
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
