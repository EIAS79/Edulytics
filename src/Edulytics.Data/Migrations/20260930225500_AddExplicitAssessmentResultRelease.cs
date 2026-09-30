using System;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edulytics.Data.Migrations;

[DbContext(typeof(EdulyticsDbContext))]
[Migration("20260930225500_AddExplicitAssessmentResultRelease")]
public partial class AddExplicitAssessmentResultRelease : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "ResultReleaseStatus",
            table: "Assessments",
            type: "integer",
            nullable: false,
            defaultValue: 1);

        migrationBuilder.AddColumn<DateTime>(
            name: "ResultsPublishedAtUtc",
            table: "Assessments",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "ResultsPublishedByUserId",
            table: "Assessments",
            type: "uuid",
            nullable: true);

        migrationBuilder.Sql(
            "UPDATE \"Assessments\" SET \"ResultReleaseStatus\" = 2 WHERE \"Status\" = 3;");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ResultReleaseStatus",
            table: "Assessments");

        migrationBuilder.DropColumn(
            name: "ResultsPublishedAtUtc",
            table: "Assessments");

        migrationBuilder.DropColumn(
            name: "ResultsPublishedByUserId",
            table: "Assessments");
    }
}
