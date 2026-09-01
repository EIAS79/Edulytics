using System.Data.Common;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Web.Bootstrap;

internal static class Phase38LaunchGateInventory
{
    private const string ExpectedRenderServiceId = "srv-da1o4url550s73aecsn0";

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

            Write("PHASE38_INVENTORY_BEGIN database={0}", connection.Database);

            var databaseSize = await ScalarAsync<string>(
                connection,
                "SELECT pg_size_pretty(pg_database_size(current_database()))",
                cancellationToken);
            Write("PHASE38_DATABASE_SIZE size={0}", databaseSize ?? "unknown");

            var tables = await ReadFirstColumnAsync(
                connection,
                "SELECT tablename FROM pg_tables WHERE schemaname = 'public' ORDER BY tablename",
                cancellationToken);

            foreach (var table in tables)
            {
                if (!table.All(ch => char.IsLetterOrDigit(ch) || ch == '_'))
                    continue;

                var count = await ScalarAsync<long>(
                    connection,
                    $"SELECT COUNT(*) FROM \"{table}\"",
                    cancellationToken);
                Write("PHASE38_TABLE_COUNT table={0} rows={1}", table, count);
            }

            await LogSchoolsAsync(connection, cancellationToken);
            await LogIdentitySummaryAsync(connection, cancellationToken);
            await LogCurriculumOwnershipAsync(connection, cancellationToken);

            Write("PHASE38_INVENTORY_END");
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }

    private static async Task LogSchoolsAsync(
        DbConnection connection,
        CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText =
            "SELECT \"Id\", \"Name\", \"SchoolCode\", \"Status\", \"CountryCode\", \"City\", \"ArchivedAtUtc\" " +
            "FROM \"Schools\" ORDER BY \"CreatedAtUtc\", \"Id\"";

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            Write(
                "PHASE38_SCHOOL id={0} name={1} code={2} status={3} country={4} city={5} archived={6}",
                reader.GetGuid(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetValue(3),
                reader.GetString(4),
                reader.GetString(5),
                !reader.IsDBNull(6));
        }
    }

    private static async Task LogIdentitySummaryAsync(
        DbConnection connection,
        CancellationToken cancellationToken)
    {
        await using (var command = connection.CreateCommand())
        {
            command.CommandText =
                "SELECT COUNT(*) FILTER (WHERE \"SchoolId\" IS NULL), COUNT(*) FILTER (WHERE \"SchoolId\" IS NOT NULL) FROM \"AspNetUsers\"";
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if (await reader.ReadAsync(cancellationToken))
            {
                Write(
                    "PHASE38_USERS global={0} school_scoped={1}",
                    reader.GetInt64(0),
                    reader.GetInt64(1));
            }
        }

        await using (var command = connection.CreateCommand())
        {
            command.CommandText =
                "SELECT r.\"Name\", COUNT(DISTINCT ur.\"UserId\") " +
                "FROM \"AspNetRoles\" r LEFT JOIN \"AspNetUserRoles\" ur ON ur.\"RoleId\" = r.\"Id\" " +
                "GROUP BY r.\"Name\" ORDER BY r.\"Name\"";
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                Write(
                    "PHASE38_ROLE role={0} users={1}",
                    reader.IsDBNull(0) ? "<null>" : reader.GetString(0),
                    reader.GetInt64(1));
            }
        }
    }

    private static async Task LogCurriculumOwnershipAsync(
        DbConnection connection,
        CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText =
            "SELECT COUNT(*) FILTER (WHERE \"OwnerSchoolId\" IS NULL), COUNT(*) FILTER (WHERE \"OwnerSchoolId\" IS NOT NULL) FROM \"CurriculumFrameworks\"";
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (await reader.ReadAsync(cancellationToken))
        {
            Write(
                "PHASE38_CURRICULUM_FRAMEWORKS global={0} school_owned={1}",
                reader.GetInt64(0),
                reader.GetInt64(1));
        }
    }

    private static async Task<IReadOnlyList<string>> ReadFirstColumnAsync(
        DbConnection connection,
        string sql,
        CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var result = new List<string>();
        while (await reader.ReadAsync(cancellationToken))
            result.Add(reader.GetString(0));
        return result;
    }

    private static async Task<T?> ScalarAsync<T>(
        DbConnection connection,
        string sql,
        CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        var result = await command.ExecuteScalarAsync(cancellationToken);
        if (result is null || result is DBNull)
            return default;
        return (T)Convert.ChangeType(result, typeof(T), System.Globalization.CultureInfo.InvariantCulture);
    }

    private static void Write(string format, params object?[] values) =>
        Console.WriteLine(format, values);
}
