using Edulytics.Data.Contexts;
using Edulytics.Data.Repositories;
using Edulytics.Data.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Npgsql;

// This executable only accepts a separately configured, EMPTY target database.
// It is never called against the suspended legacy Neon account by this plan.
if (Environment.GetEnvironmentVariable("EDULYTICS_CLEAN_SEED_CONFIRM") !=
    "I_CONFIRM_NEW_EMPTY_DATABASE")
{
    throw new InvalidOperationException(
        "Clean seed requires EDULYTICS_CLEAN_SEED_CONFIRM=I_CONFIRM_NEW_EMPTY_DATABASE.");
}

var connection = Environment.GetEnvironmentVariable(
    "EDULYTICS_CLEAN_DATABASE_CONNECTION");

if (string.IsNullOrWhiteSpace(connection))
    throw new InvalidOperationException(
        "EDULYTICS_CLEAN_DATABASE_CONNECTION must be provided through a secret.");

var target = new NpgsqlConnectionStringBuilder(connection);
if (string.IsNullOrWhiteSpace(target.Host) ||
    string.IsNullOrWhiteSpace(target.Database))
{
    throw new InvalidOperationException("Invalid target host or database.");
}

// Fail closed if even the host is not an independently approved target.
// A database name alone is not sufficient to distinguish old and new Neon.
var expectedHost = Environment.GetEnvironmentVariable(
    "EDULYTICS_CLEAN_DATABASE_HOST");
var isLocalCi =
    string.Equals(target.Host, "127.0.0.1", StringComparison.Ordinal) &&
    string.Equals(target.Database, "edulytics_clean_ci", StringComparison.Ordinal);
if (!isLocalCi &&
    (string.IsNullOrWhiteSpace(expectedHost) ||
     !string.Equals(target.Host, expectedHost, StringComparison.OrdinalIgnoreCase)))
{
    throw new InvalidOperationException(
        "Remote target PostgreSQL host must match independently approved host.");
}

// The operator must bind the independently verified target database name.
// This prevents accidental use of a default/unintended local database.
var expectedDatabase = Environment.GetEnvironmentVariable(
    "EDULYTICS_CLEAN_DATABASE_NAME");
if (string.IsNullOrWhiteSpace(expectedDatabase) ||
    !string.Equals(expectedDatabase, target.Database, StringComparison.Ordinal))
{
    throw new InvalidOperationException(
        "Target database name was not explicitly approved.");
}

var queryMeter = new CleanReadQueryMeter();
var options = new DbContextOptionsBuilder<EdulyticsDbContext>()
    .UseNpgsql(connection)
    .AddInterceptors(queryMeter)
    .Options;

await using var db = new EdulyticsDbContext(options);
if (!await db.Database.CanConnectAsync())
    throw new InvalidOperationException("New PostgreSQL database is unavailable.");

var migrations = await db.Database.GetPendingMigrationsAsync();
if (migrations.Any())
{
    throw new InvalidOperationException(
        "Apply all reviewed EF Core migrations before running clean seed.");
}

// Database-wide fail-closed preflight: a fresh target may contain only
// migration history or platform role definitions. Repeated seed executions may
// contain the exact set of shared curriculum-reference tables shown below.
// Any other populated table (including legacy jobs, payments or audit logs)
// means this is NOT an empty/clean target and must not be modified.
var permittedPopulatedTables = new HashSet<string>(StringComparer.Ordinal)
{
    "__EFMigrationsHistory",
    "AspNetRoles",
    "CurriculumFrameworks",
    "CurriculumFrameworkVersions",
    "CurriculumPackContentNodes",
    "CurriculumPackNodeLinks",
    "CurriculumPackImportStates",
    "CurriculumPedagogicalLessons",
    "CurriculumPedagogicalLessonOutcomes",
    "CurriculumLessonContents"
};
var dbConnection = db.Database.GetDbConnection();
await db.Database.OpenConnectionAsync();
try
{
    var tableNames = new List<string>();
    await using (var tableQuery = dbConnection.CreateCommand())
    {
        tableQuery.CommandText =
            "SELECT tablename FROM pg_catalog.pg_tables " +
            "WHERE schemaname = current_schema() ORDER BY tablename";
        await using var rows = await tableQuery.ExecuteReaderAsync();
        while (await rows.ReadAsync())
            tableNames.Add(rows.GetString(0));
    }

    var unexpectedTables = new List<string>();
    foreach (var tableName in tableNames)
    {
        if (permittedPopulatedTables.Contains(tableName))
            continue;

        // Identifiers originate from PostgreSQL's own table catalog.
        var safeName = tableName.Replace("\"", "\"\"", StringComparison.Ordinal);
        await using var existenceQuery = dbConnection.CreateCommand();
        existenceQuery.CommandText =
            $"SELECT EXISTS (SELECT 1 FROM \"{safeName}\" LIMIT 1)";
        if ((bool)(await existenceQuery.ExecuteScalarAsync() ?? false))
            unexpectedTables.Add(tableName);
    }

    if (unexpectedTables.Count > 0)
    {
        throw new InvalidOperationException(
            "Refusing to modify a database containing non-reference records: " +
            string.Join(", ", unexpectedTables));
    }

    Console.WriteLine(
        $"EDULYTICS_CLEAN_PREFLIGHT_PASS checkedTables={tableNames.Count} " +
        $"unexpectedPopulatedTables=0");
}
finally
{
    await db.Database.CloseConnectionAsync();
}

// No old or existing tenant data is allowed: avoid modifying an operational DB.
var hasTenantRecords =
    await db.Users.AnyAsync() ||
    await db.Schools.AnyAsync() ||
    await db.StudentProfiles.AnyAsync() ||
    await db.DirectStudentProfiles.AnyAsync() ||
    await db.SchoolCurriculumAdoptions.AnyAsync() ||
    await db.AssessmentAttempts.AnyAsync() ||
    await db.PracticeAttempts.AnyAsync() ||
    await db.PersonalPaymentTransactions.AnyAsync() ||
    await db.BillingInvoices.AnyAsync();

if (hasTenantRecords)
{
    throw new InvalidOperationException(
        "Refusing to seed: target database contains users, schools or tenant data.");
}

// No destructive operations, truncation, demo users or school scaffolding.
await new MathematicsCurriculumPackSeeder(db).SeedAsync();
await new MathematicsPedagogicalLessonSeeder(db).SeedAsync();
await new MathematicsCanonicalLessonContentSeeder(db)
    .SeedMetadataOnlyAsync();

var versions = await db.CurriculumFrameworkVersions.AsNoTracking().CountAsync();
var pedagogical = await db.CurriculumPedagogicalLessons.AsNoTracking().CountAsync();
var metadata = await db.CurriculumLessonContents.AsNoTracking().CountAsync();
var proseRows = await db.CurriculumLessonContentTranslations.AsNoTracking().CountAsync();
var jsonBodies = new EmbeddedCanonicalLessonContentIndex().Count;

if (proseRows != 0)
    throw new InvalidOperationException(
        "Clean JSON mode unexpectedly wrote canonical lesson prose to PostgreSQL.");
if (metadata < 1 || pedagogical < 1 || versions < 4)
    throw new InvalidOperationException(
        "Missing expected curriculum, lesson or content metadata.");
if (metadata != jsonBodies)
    throw new InvalidOperationException(
        $"Canonical metadata count {metadata} does not match approved JSON identities {jsonBodies}.");

if (await db.Users.AnyAsync() || await db.Schools.AnyAsync() ||
    await db.StudentProfiles.AnyAsync() || await db.BillingInvoices.AnyAsync())
{
    throw new InvalidOperationException(
        "Unexpected user, school or billing records after seed.");
}

// Real PostgreSQL read path: retain identity and publication metadata in
// PostgreSQL, but resolve the published learner body from bundled JSON.
const string smokeCode = "PED:UAE:G9:ADV:T1:L1-2";
var sample = await db.CurriculumPedagogicalLessons.AsNoTracking()
    .SingleAsync(x => x.Code == smokeCode);
var jsonReadConfiguration = new ConfigurationBuilder()
    .AddInMemoryCollection(new Dictionary<string, string?>
    {
        ["Edulytics:LessonContent:ReadFromJson"] = "true"
    })
    .Build();
var commandCountBefore = queryMeter.ReaderCommands.Count;
var sampleContent = await new LessonContentRepository(db, jsonReadConfiguration)
    .ListCanonicalContentsAsync([sample.Id]);
var readCommands = queryMeter.ReaderCommands
    .Skip(commandCountBefore)
    .ToArray();
if (readCommands.Length != 1)
{
    throw new InvalidOperationException(
        $"Expected one JSON-canonical metadata query, got {readCommands.Length}.");
}
if (readCommands.Any(sql =>
        sql.Contains("CurriculumLessonContentTranslations", StringComparison.Ordinal)))
{
    throw new InvalidOperationException(
        "JSON-backed reader unexpectedly fetched lesson prose from PostgreSQL.");
}
Console.WriteLine("CLEAN_JSON_READ_QUERY_BUDGET_PASS sqlQueries=1 proseSqlQueries=0");

// Measure the old DB-based metadata lookup on the SAME disposable database.
// No translation prose exists in this clean database, so this comparison
// measures SQL round trips only; it does NOT establish body parity or egress.
var baselineConfiguration = new ConfigurationBuilder()
    .AddInMemoryCollection(new Dictionary<string, string?>
    {
        ["Edulytics:LessonContent:ReadFromJson"] = "false"
    })
    .Build();
var baselineStart = queryMeter.ReaderCommands.Count;
var baselineMetadata = await new LessonContentRepository(db, baselineConfiguration)
    .ListCanonicalContentsAsync([sample.Id]);
var baselineSqlCount = queryMeter.ReaderCommands.Count - baselineStart;
if (baselineMetadata.Count != 1 || baselineSqlCount != 2)
{
    throw new InvalidOperationException(
        $"Unexpected legacy DB lookup count: {baselineSqlCount}.");
}
Console.WriteLine(
    "CLEAN_ARCHITECTURE_QUERY_COMPARISON_PASS " +
    "legacySqlQueries=2 jsonHybridSqlQueries=1 " +
    "bodyParity=NOT_MEASURED(no_prose_rows)");

// Distinct 25-lesson batch: query count must remain constant as catalogue
// sizes grow. This is not a p95 latency, throughput or egress benchmark.
var batchIds = await db.CurriculumPedagogicalLessons
    .AsNoTracking()
    .OrderBy(x => x.Code)
    .Select(x => x.Id)
    .Take(25)
    .ToArrayAsync();
var batchStart = queryMeter.ReaderCommands.Count;
var batchBodies = await new LessonContentRepository(db, jsonReadConfiguration)
    .ListCanonicalContentsAsync(batchIds);
var batchSqlCount = queryMeter.ReaderCommands.Count - batchStart;
if (batchIds.Length != 25 || batchBodies.Count != batchIds.Length ||
    batchSqlCount != 1)
{
    throw new InvalidOperationException(
        $"JSON batch query budget failed: ids={batchIds.Length}, " +
        $"bodies={batchBodies.Count}, queries={batchSqlCount}.");
}
Console.WriteLine(
    "CLEAN_ARCHITECTURE_BATCH_QUERY_PASS " +
    "lessons=25 jsonHybridSqlQueries=1 proseSqlQueries=0");


// Stage 2: bounded, reproducible warm-cache lookup timing on a local or approved
// clean PostgreSQL database. These are operational samples, NOT production
// egress, cold-start p95, or identical-payload proof for the legacy path.
var latencySamplesMs = new List<double>(20);
for (var iteration = 0; iteration < 20; iteration++)
{
    var started = System.Diagnostics.Stopwatch.GetTimestamp();
    var repeated = await new LessonContentRepository(db, jsonReadConfiguration)
        .ListCanonicalContentsAsync(batchIds);
    if (repeated.Count != 25)
        throw new InvalidOperationException("JSON benchmark returned an incomplete lesson batch.");
    latencySamplesMs.Add(System.Diagnostics.Stopwatch.GetElapsedTime(started).TotalMilliseconds);
}
latencySamplesMs.Sort();
var warmP50 = latencySamplesMs[9];
var warmP95 = latencySamplesMs[18];
Console.WriteLine(
    $"CLEAN_JSON_WARM_BATCH_LATENCY lessons=25 runs=20 " +
    $"p50Ms={warmP50:F3} p95Ms={warmP95:F3} " +
    "comparisonScope=hybrid-only noColdStart=true " +
    "neonEgress=NOT_MEASURED legacyBodyParity=NOT_MEASURED");

if (sampleContent.Count != 1 ||
    sampleContent[0].Translations.Count == 0 ||
    string.IsNullOrWhiteSpace(sampleContent[0].Translations[0].Explanation))
{
    throw new InvalidOperationException(
        "Clean PostgreSQL JSON-backed lesson read smoke test failed.");
}

Console.WriteLine("CLEAN_JSON_READ_GATE_PASS code=" + smokeCode);

Console.WriteLine(
    $"EDULYTICS_CLEAN_SEED_PASS frameworks={versions} " +
    $"pedagogicalLessons={pedagogical} metadata={metadata} " +
    $"jsonBodies={jsonBodies} postgresProseRows={proseRows} users=0 schools=0");


/// <summary>
/// Test-only SQL command meter for the PostgreSQL 18 clean-architecture gate.
/// Query counts describe the database boundary, not real Neon egress costs.
/// </summary>
internal sealed class CleanReadQueryMeter : DbCommandInterceptor
{
    private readonly List<string> _readerCommands = [];

    public IReadOnlyList<string> ReaderCommands => _readerCommands;

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        _readerCommands.Add(command.CommandText);
        return new ValueTask<InterceptionResult<DbDataReader>>(result);
    }
}
