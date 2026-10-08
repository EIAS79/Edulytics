using Edulytics.Data.Contexts;
using Edulytics.Data.Repositories;
using Edulytics.Data.Seeding;
using Microsoft.EntityFrameworkCore;
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

var options = new DbContextOptionsBuilder<EdulyticsDbContext>()
    .UseNpgsql(connection)
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

Console.WriteLine(
    $"EDULYTICS_CLEAN_SEED_PASS frameworks={versions} " +
    $"pedagogicalLessons={pedagogical} metadata={metadata} " +
    $"jsonBodies={jsonBodies} postgresProseRows={proseRows} users=0 schools=0");
