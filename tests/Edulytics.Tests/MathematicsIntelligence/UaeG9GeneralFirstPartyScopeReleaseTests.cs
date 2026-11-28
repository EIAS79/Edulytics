using System.Text.Json;
using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG9GeneralFirstPartyScopeReleaseTests
{
    private const string Prefix = "PED:UAE-MOE-MATH:L9:GENERAL:";
    private const string Blueprint =
        "UAE-G9-GENERAL-AR82-OWNER-FIRST-PARTY-V1:COMPOSITE-SOURCE-CANDIDATE";

    [Fact]
    public void Retired_UAE_Grade9_Advanced_is_not_a_selectable_pathway()
    {
        MathematicsCurriculumPackRegistry.Validate();
        var uae = Assert.Single(MathematicsCurriculumPackRegistry.All,
            x => x.Code == MathematicsCurriculumPackRegistry.UaeCode);

        Assert.Equal(15, uae.Levels.Count);
        Assert.DoesNotContain(uae.Levels, x => x.LogicalLevel == 11);
        Assert.Contains(uae.Levels, x =>
            x.LogicalLevel == 9 && x.Pathway == "General");
        Assert.DoesNotContain(uae.Levels, x =>
            x.LogicalLevel == 9 && x.Pathway == "Advanced");
        Assert.DoesNotContain(uae.Levels, x =>
            x.LogicalLevel == 8 && x.Pathway == "Advanced");
        Assert.Contains(uae.Levels, x => x.LogicalLevel == 10 && x.Pathway == "General");
        Assert.DoesNotContain(uae.Levels, x => x.LogicalLevel == 10 && x.Pathway == "Advanced");
        foreach (var grade in new[] { 5, 6, 7, 12 })
        {
            Assert.Contains(uae.Levels, x =>
                x.LogicalLevel == grade && x.Pathway == "General");
            Assert.Contains(uae.Levels, x =>
                x.LogicalLevel == grade && x.Pathway == "Advanced");
        }
    }

    [Fact]
    public void One_Grade9_General_Arabic_blueprint_owns_exactly_82_ordered_first_party_identities()
    {
        var document = Assert.Single(PedagogicalLessonBlueprintRegistry
            .LoadEmbeddedDocuments(), x => x.BlueprintCode == Blueprint);
        PedagogicalLessonBlueprintContract.Validate(document);
        Assert.Equal(1, document.SchemaVersion);
        Assert.Equal("UAE-MOE-MATH", document.PackCode);
        Assert.Equal("MOE-2026-2027-T1", document.VersionCode);
        Assert.Equal(9, document.LogicalLevel);
        Assert.Equal("General", document.Pathway);
        Assert.Equal(13, document.Units.Count);
        Assert.Equal(82, document.Lessons.Count);
        Assert.Equal(82, document.Lessons.Select(x => x.LessonCode)
            .Distinct(StringComparer.Ordinal).Count());
        Assert.Equal(Enumerable.Range(1, 82),
            document.Lessons.Select(x => x.SortOrder));
        Assert.All(document.Lessons, x =>
        {
            Assert.StartsWith(Prefix, x.LessonCode);
            Assert.StartsWith("EDU:UAE-MOE-MATH:L9:GENERAL:STUDENT-AR82:", x.SourceLessonCode);
            Assert.Empty(x.OutcomeCodes);
            Assert.Empty(x.Alignments);
            Assert.Null(x.OfficialReferenceCode);
            Assert.Matches("[\\u0600-\\u06FF]", x.Title);
        });
        Assert.Equal(
            new [] {8,9,6,4,6,6,9,6,5,4,4,6,9},
            document.Units.Select(u => u.LessonCount));
    }

    [Fact]
    public void Grade9_reference_only_source_rights_exception_is_exact_scoped()
    {
        var source = Assert.Single(PedagogicalLessonBlueprintRegistry
            .LoadEmbeddedDocuments(), x => x.BlueprintCode == Blueprint);
        PedagogicalLessonBlueprintContract.Validate(source);
        Assert.Contains("no reuse license presumed", source.SourceLicense);
        Assert.Contains("publisher illustration", source.SourceRightsNote);

        var changedScope = JsonSerializer.Deserialize<PedagogicalLessonBlueprintDocument>(
            JsonSerializer.Serialize(source))!;
        changedScope.Pathway = "Advanced";
        Assert.Throws<InvalidOperationException>(() =>
            PedagogicalLessonBlueprintContract.Validate(changedScope));

        var changedLicense = JsonSerializer.Deserialize<PedagogicalLessonBlueprintDocument>(
            JsonSerializer.Serialize(source))!;
        changedLicense.SourceLicense = "Unknown license from external publisher";
        Assert.Throws<InvalidOperationException>(() =>
            PedagogicalLessonBlueprintContract.Validate(changedLicense));

        var changedSize = JsonSerializer.Deserialize<PedagogicalLessonBlueprintDocument>(
            JsonSerializer.Serialize(source))!;
        changedSize.Lessons.RemoveAt(changedSize.Lessons.Count - 1);
        Assert.Throws<InvalidOperationException>(() =>
            PedagogicalLessonBlueprintContract.Validate(changedSize));
    }

    [Fact]
    public void Approved_Grade9_canonical_content_is_Published_but_no_database_import_is_implied()
    {
        var docs = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var pack = Assert.Single(docs, d => d.Lessons.Any(x =>
            x.LessonCode.StartsWith(Prefix, StringComparison.Ordinal)));
        Assert.Equal("ar", pack.AcademicLanguage);
        Assert.Equal("Published", pack.Status.ToString());
        Assert.Equal("MOE-2026-2027-T1", pack.VersionCode);
        Assert.Equal(82, pack.Lessons.Count);
        Assert.Equal(
            PedagogicalLessonBlueprintRegistry.LoadEmbeddedDocuments()
                .Single(x => x.BlueprintCode == Blueprint)
                .Lessons.Select(x => x.LessonCode),
            pack.Lessons.Select(x => x.LessonCode));
    }
}
