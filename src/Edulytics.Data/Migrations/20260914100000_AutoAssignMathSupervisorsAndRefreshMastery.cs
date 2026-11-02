using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edulytics.Data.Migrations;

[DbContext(typeof(EdulyticsDbContext))]
[Migration("20260914100000_AutoAssignMathSupervisorsAndRefreshMastery")]
public sealed class AutoAssignMathSupervisorsAndRefreshMastery : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        if (!migrationBuilder.ActiveProvider.Contains("Npgsql", StringComparison.OrdinalIgnoreCase))
            return;

        migrationBuilder.Sql(
            """
            CREATE OR REPLACE FUNCTION edulytics_auto_assign_math_supervisor()
            RETURNS trigger
            LANGUAGE plpgsql
            AS $function$
            DECLARE
                school_id uuid;
            BEGIN
                IF NOT EXISTS (
                    SELECT 1
                    FROM "AspNetRoles" r
                    WHERE r."Id" = NEW."RoleId"
                      AND r."Name" = 'SubjectSupervisor'
                ) THEN
                    RETURN NEW;
                END IF;

                SELECT u."SchoolId"
                INTO school_id
                FROM "AspNetUsers" u
                WHERE u."Id" = NEW."UserId";

                IF school_id IS NULL THEN
                    RETURN NEW;
                END IF;

                INSERT INTO "SubjectSupervisorAssignments"
                (
                    "Id",
                    "SchoolId",
                    "SupervisorUserId",
                    "SubjectId",
                    "CreatedAtUtc"
                )
                SELECT
                    md5(NEW."UserId"::text || ':' || s."Id"::text)::uuid,
                    s."SchoolId",
                    NEW."UserId",
                    s."Id",
                    CURRENT_TIMESTAMP
                FROM "Subjects" s
                WHERE s."SchoolId" = school_id
                  AND s."Status" = 1
                  AND (s."Code" = 'MATH' OR s."NormalizedCode" = 'MATH')
                ON CONFLICT ("SchoolId", "SupervisorUserId", "SubjectId")
                DO NOTHING;

                RETURN NEW;
            END;
            $function$;

            DROP TRIGGER IF EXISTS trg_edulytics_auto_assign_math_supervisor
                ON "AspNetUserRoles";

            CREATE TRIGGER trg_edulytics_auto_assign_math_supervisor
            AFTER INSERT ON "AspNetUserRoles"
            FOR EACH ROW
            EXECUTE FUNCTION edulytics_auto_assign_math_supervisor();

            CREATE OR REPLACE FUNCTION edulytics_remove_math_supervisor_assignment()
            RETURNS trigger
            LANGUAGE plpgsql
            AS $function$
            BEGIN
                IF EXISTS (
                    SELECT 1
                    FROM "AspNetRoles" r
                    WHERE r."Id" = OLD."RoleId"
                      AND r."Name" = 'SubjectSupervisor'
                ) THEN
                    DELETE FROM "SubjectSupervisorAssignments" a
                    USING "Subjects" s
                    WHERE a."SupervisorUserId" = OLD."UserId"
                      AND a."SubjectId" = s."Id"
                      AND a."SchoolId" = s."SchoolId"
                      AND (s."Code" = 'MATH' OR s."NormalizedCode" = 'MATH');
                END IF;

                RETURN OLD;
            END;
            $function$;

            DROP TRIGGER IF EXISTS trg_edulytics_remove_math_supervisor_assignment
                ON "AspNetUserRoles";

            CREATE TRIGGER trg_edulytics_remove_math_supervisor_assignment
            AFTER DELETE ON "AspNetUserRoles"
            FOR EACH ROW
            EXECUTE FUNCTION edulytics_remove_math_supervisor_assignment();

            INSERT INTO "SubjectSupervisorAssignments"
            (
                "Id",
                "SchoolId",
                "SupervisorUserId",
                "SubjectId",
                "CreatedAtUtc"
            )
            SELECT
                md5(u."Id"::text || ':' || s."Id"::text)::uuid,
                u."SchoolId",
                u."Id",
                s."Id",
                CURRENT_TIMESTAMP
            FROM "AspNetUsers" u
            JOIN "AspNetUserRoles" ur
              ON ur."UserId" = u."Id"
            JOIN "AspNetRoles" r
              ON r."Id" = ur."RoleId"
            JOIN "Subjects" s
              ON s."SchoolId" = u."SchoolId"
            WHERE r."Name" = 'SubjectSupervisor'
              AND u."SchoolId" IS NOT NULL
              AND s."Status" = 1
              AND (s."Code" = 'MATH' OR s."NormalizedCode" = 'MATH')
            ON CONFLICT ("SchoolId", "SupervisorUserId", "SubjectId")
            DO NOTHING;

            INSERT INTO "AnalyticsRefreshStates" AS current
            (
                "SchoolId",
                "RequestedVersion",
                "CompletedVersion",
                "FirstRequestedAtUtc",
                "LastRequestedAtUtc",
                "CoalesceDeadlineUtc",
                "AvailableAtUtc",
                "LeaseOwner",
                "LeaseToken",
                "LeaseUntilUtc",
                "ProcessingAttempts",
                "LastError",
                "RowVersion"
            )
            SELECT
                s."Id",
                1,
                0,
                CURRENT_TIMESTAMP,
                CURRENT_TIMESTAMP,
                CURRENT_TIMESTAMP,
                CURRENT_TIMESTAMP,
                NULL,
                NULL,
                NULL,
                0,
                NULL,
                decode(md5(s."Id"::text || clock_timestamp()::text), 'hex')
            FROM "Schools" s
            WHERE EXISTS (
                SELECT 1
                FROM "Assessments" a
                JOIN "AssessmentResults" ar
                  ON ar."AssessmentId" = a."Id"
                WHERE a."SchoolId" = s."Id"
                  AND a."Status" <> 0
            )
            ON CONFLICT ("SchoolId")
            DO UPDATE SET
                "RequestedVersion" = current."RequestedVersion" + 1,
                "LastRequestedAtUtc" = CURRENT_TIMESTAMP,
                "AvailableAtUtc" = LEAST(
                    current."AvailableAtUtc",
                    CURRENT_TIMESTAMP),
                "LastError" = NULL,
                "RowVersion" = decode(
                    md5(current."SchoolId"::text || clock_timestamp()::text),
                    'hex');
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        if (!migrationBuilder.ActiveProvider.Contains("Npgsql", StringComparison.OrdinalIgnoreCase))
            return;

        migrationBuilder.Sql(
            """
            DROP TRIGGER IF EXISTS trg_edulytics_auto_assign_math_supervisor
                ON "AspNetUserRoles";
            DROP TRIGGER IF EXISTS trg_edulytics_remove_math_supervisor_assignment
                ON "AspNetUserRoles";
            DROP FUNCTION IF EXISTS edulytics_auto_assign_math_supervisor();
            DROP FUNCTION IF EXISTS edulytics_remove_math_supervisor_assignment();
            """);
    }
}
