using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edulytics.Data.Migrations;

[DbContext(typeof(EdulyticsDbContext))]
[Migration("20260929014500_UnifiedPracticeGuidedRemediation")]
public sealed class UnifiedPracticeGuidedRemediation : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "IncorrectAttemptCount",
            table: "AdaptivePracticeTurns",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<string>(
            name: "LastIncorrectAnswer",
            table: "AdaptivePracticeTurns",
            type: "character varying(2000)",
            maxLength: 2000,
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "LastIncorrectAtUtc",
            table: "AdaptivePracticeTurns",
            type: "timestamp with time zone",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IncorrectAttemptCount",
            table: "AdaptivePracticeTurns");

        migrationBuilder.DropColumn(
            name: "LastIncorrectAnswer",
            table: "AdaptivePracticeTurns");

        migrationBuilder.DropColumn(
            name: "LastIncorrectAtUtc",
            table: "AdaptivePracticeTurns");
    }
}
