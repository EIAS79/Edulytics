using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Data.Contexts;
using Edulytics.Data.Seeding;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Edulytics.Tests.Phase29;

public sealed class Phase29CommonCoreElementaryBatchTests
{
    private sealed record GradeSpec(
        int Grade,
        int LogicalLevel,
        int UnitCount,
        int LessonCount,
        int FormalMappingCount,
        int OfficialStandardCount,
        int ZeroFormalCount,
        string SourceGraphSha,
        string BlueprintGraphSha,
        int[] UnitCounts);

    private static readonly GradeSpec[] Specs =
    [
        new(
            1,
            2,
            8,
            146,
            414,
            23,
            0,
            "1e3d264fc9d6b2da2d0b494c3379370d3216b967f497242c912e9a3fc3934eb8",
            "4c7aec07ca974daf1f6a2c7ad122f3495213163e898a38114e3e40b3f09492ca",
            [15, 22, 28, 23, 14, 17, 17, 10]),
        new(
            2,
            3,
            9,
            146,
            334,
            26,
            0,
            "5566fcd85ca099f10cd62021c8d0cd778093b3789fe7f72d2a1b1a1cc6594c74",
            "480c9b7996ebfcef0ea97a24a1f3f8305a00d8dcb9fb8ea46a314910c3956281",
            [18, 16, 18, 15, 14, 21, 18, 13, 13]),
        new(
            3,
            4,
            8,
            143,
            230,
            25,
            0,
            "4ef968a9f5024e97199617425c7b18244886b12285498160e46284a5141648e0",
            "1c6351085790000dea34c4687acb125ce2119ea4b7818a87442ebd5130ce1a4f",
            [21, 15, 21, 22, 18, 16, 15, 15]),
        new(
            4,
            5,
            9,
            149,
            263,
            30,
            0,
            "fe06ae8a0daabaa0ad36c8c6f956b2348705bd732873f52c60730731d61c03f1",
            "caccb7beeb9380e6e4b3fbab451bbf512f19187e10e20babf6c55ffd3294e1f6",
            [8, 17, 20, 23, 18, 25, 16, 10, 12]),
        new(
            5,
            6,
            8,
            148,
            215,
            27,
            0,
            "7d99645b607df03449c2ceb9a64114f51634c1206632f0396ccf3dd1da19d848",
            "90db8f4d444f4bd790dcbd7a73f29819d5f48cbd2d8d9f3303815446c0a1bf54",
            [12, 17, 20, 21, 26, 21, 13, 18])
    ];

    [Fact]
    public void
        GradeOneToFiveBlueprintsLockExactSourceGraphs()
    {
        var documents =
            PedagogicalLessonBlueprintRegistry
                .LoadEmbeddedDocuments();

        Assert.Equal(
            8,
            documents.Count(
                x =>
                    x.PackCode ==
                    MathematicsCurriculumPackRegistry
                        .CommonCoreCode &&
                    x.SchemaVersion == 1));

        foreach (var spec in Specs)
        {
            var blueprint =
                Assert.Single(
                    documents,
                    x =>
                        x.PackCode ==
                            MathematicsCurriculumPackRegistry
                                .CommonCoreCode &&
                        x.NativeLevel ==
                            $"Grade {spec.Grade}");

            Assert.Equal(
                $"US-CCSS-MATH:G{spec.Grade}:IM-K5-2021",
                blueprint.BlueprintCode);

            Assert.Equal(
                spec.LogicalLevel,
                blueprint.LogicalLevel);

            Assert.Equal(
                "CC BY 4.0",
                blueprint.SourceLicense);

            Assert.Equal(
                "1st Edition (2021)",
                blueprint.SourceEdition);

            Assert.Equal(
                spec.BlueprintGraphSha,
                blueprint.SemanticGraphSha256);

            Assert.Contains(
                spec.SourceGraphSha,
                blueprint.SourceSelectionEvidence);

            Assert.Equal(
                spec.UnitCount,
                blueprint.Units.Count);

            Assert.Equal(
                spec.LessonCount,
                blueprint.Lessons.Count);

            Assert.Equal(
                spec.UnitCounts,
                blueprint.Units
                    .OrderBy(x => x.Number)
                    .Select(x => x.LessonCount)
                    .ToArray());

            Assert.Equal(
                Enumerable.Range(
                    1,
                    spec.LessonCount),
                blueprint.Lessons
                    .OrderBy(x => x.SortOrder)
                    .Select(x => x.SortOrder));

            Assert.Equal(
                spec.FormalMappingCount,
                blueprint.Lessons.Sum(
                    x =>
                        x.OutcomeCodes.Count));

            Assert.Equal(
                spec.ZeroFormalCount,
                blueprint.Lessons.Count(
                    x =>
                        x.OutcomeCodes.Count ==
                        0));

            Assert.Equal(
                spec.OfficialStandardCount,
                blueprint.Lessons
                    .SelectMany(
                        x =>
                            x.OutcomeCodes)
                    .Distinct(
                        StringComparer.Ordinal)
                    .Count());

            Assert.Equal(
                spec.UnitCount,
                blueprint
                    .AcquisitionDiagnostics
                    .UnitCount);

            Assert.Equal(
                spec.LessonCount,
                blueprint
                    .AcquisitionDiagnostics
                    .LessonCount);

            Assert.Equal(
                spec.OfficialStandardCount,
                blueprint
                    .AcquisitionDiagnostics
                    .EffectiveOfficialStandardCount);

            Assert.Equal(
                spec.OfficialStandardCount,
                blueprint
                    .AcquisitionDiagnostics
                    .AddressingCoverageCount);

            Assert.Equal(
                spec.FormalMappingCount,
                blueprint
                    .AcquisitionDiagnostics
                    .FormalMappingCount);

            Assert.Equal(
                spec.ZeroFormalCount,
                blueprint
                    .AcquisitionDiagnostics
                    .LessonsWithoutNumberedAddressingStandard);

            Assert.All(
                blueprint.Lessons,
                lesson =>
                {
                    Assert.Matches(
                        $@"^PED:US-CCSS-MATH:G{spec.Grade}:U\d{{2}}:L\d{{2}}$",
                        lesson.LessonCode);

                    Assert.False(
                        string.IsNullOrWhiteSpace(
                            lesson.Title));

                    Assert.Matches(
                        "^[0-9a-f]{64}$",
                        lesson.SemanticSha256);

                    Assert.DoesNotMatch(
                        @"(?:^|\s[—-]\s)Lesson\s+\d+\s*$",
                        lesson.Title);

                    Assert.Equal(
                        lesson.OutcomeCodes
                            .Distinct(
                                StringComparer.Ordinal)
                            .Count(),
                        lesson.OutcomeCodes.Count);

                    Assert.All(
                        lesson.OutcomeCodes,
                        code =>
                        {
                            Assert.True(
                                code.StartsWith(
                                    $"CCSS:{spec.Grade}.",
                                    StringComparison.Ordinal) ||
                                Regex.IsMatch(
                                    code,
                                    @"^CCSS:MP\.[1-8]$",
                                    RegexOptions.CultureInvariant));

                            Assert.Contains(
                                lesson.Alignments,
                                alignment =>
                                    alignment.Role ==
                                        "Addressing" &&
                                    alignment.OutcomeCode ==
                                        code &&
                                    alignment.ResolutionKind is
                                        "ExactAcceptedStandard" or
                                        "SubpartToAcceptedParent");
                        });

                    Assert.DoesNotContain(
                        lesson.Alignments,
                        alignment =>
                            alignment.Role !=
                                "Addressing" &&
                            !string.IsNullOrWhiteSpace(
                                alignment.OutcomeCode));

                    Assert.DoesNotContain(
                        lesson.Alignments,
                        alignment =>
                            alignment.ReferenceKind is
                                "Cluster" or "Domain" &&
                            !string.IsNullOrWhiteSpace(
                                alignment.OutcomeCode));
                });
        }

        Assert.Equal(
            42,
            Specs.Sum(x => x.UnitCount));

        Assert.Equal(
            732,
            Specs.Sum(x => x.LessonCount));

        Assert.Equal(
            1456,
            Specs.Sum(
                x =>
                    x.FormalMappingCount));

        Assert.Equal(
            131,
            Specs.Sum(
                x =>
                    x.OfficialStandardCount));
    }

    [Fact]
    public async Task
        SeederCreatesExactElementaryGraphsAndIsIdempotent()
    {
        await using var db =
            CreateDb(
                "phase29-elementary-clean-");

        await new MathematicsCurriculumPackSeeder(
                db)
            .SeedAsync();

        var seeder =
            new MathematicsPedagogicalLessonSeeder(
                db);

        await seeder.SeedAsync();

        var documents =
            PedagogicalLessonBlueprintRegistry
                .LoadEmbeddedDocuments();

        var versionId =
            await db
                .CurriculumPackImportStates
                .Where(
                    x =>
                        x.FrameworkCode ==
                        MathematicsCurriculumPackRegistry
                            .CommonCoreCode)
                .Select(
                    x =>
                        x.FrameworkVersionId)
                .SingleAsync();

        foreach (var spec in Specs)
        {
            var blueprint =
                Assert.Single(
                    documents,
                    x =>
                        x.PackCode ==
                            MathematicsCurriculumPackRegistry
                                .CommonCoreCode &&
                        x.NativeLevel ==
                            $"Grade {spec.Grade}");

            var lessons =
                await db
                    .CurriculumPedagogicalLessons
                    .Where(
                        x =>
                            x.FrameworkVersionId ==
                                versionId &&
                            x.LogicalLevelFrom ==
                                spec.LogicalLevel &&
                            x.LogicalLevelTo ==
                                spec.LogicalLevel &&
                            x.NativeLevel ==
                                $"Grade {spec.Grade}")
                    .OrderBy(
                        x =>
                            x.SortOrder)
                    .ToArrayAsync();

            Assert.Equal(
                spec.LessonCount,
                lessons.Length);

            var byCode =
                lessons.ToDictionary(
                    x => x.Code,
                    StringComparer.Ordinal);

            var lessonIds =
                lessons
                    .Select(x => x.Id)
                    .ToArray();

            var mappings =
                await db
                    .CurriculumPedagogicalLessonOutcomes
                    .Where(
                        x =>
                            lessonIds.Contains(
                                x.PedagogicalLessonId))
                    .ToArrayAsync();

            Assert.Equal(
                spec.FormalMappingCount,
                mappings.Length);

            Assert.Equal(
                spec.ZeroFormalCount,
                lessons.Count(
                    lesson =>
                        !mappings.Any(
                            x =>
                                x.PedagogicalLessonId ==
                                lesson.Id)));

            var nodeIds =
                mappings
                    .Select(
                        x =>
                            x.OutcomeNodeId)
                    .Distinct()
                    .ToArray();

            var nodeCodes =
                await db
                    .CurriculumPackContentNodes
                    .Where(
                        x =>
                            nodeIds.Contains(
                                x.Id))
                    .ToDictionaryAsync(
                        x => x.Id,
                        x => x.Code);

            Assert.Equal(
                spec.OfficialStandardCount,
                nodeCodes.Values
                    .Distinct(
                        StringComparer.Ordinal)
                    .Count());

            foreach (var expected in
                     blueprint.Lessons)
            {
                Assert.True(
                    byCode.TryGetValue(
                        expected.LessonCode,
                        out var actual));

                Assert.Equal(
                    expected.Title,
                    actual!.Title);

                Assert.Equal(
                    expected.UnitTitle,
                    actual.UnitTitle);

                Assert.Equal(
                    expected.SortOrder,
                    actual.SortOrder);

                Assert.Null(
                    actual.OfficialLessonNodeId);

                var actualMappings =
                    mappings
                        .Where(
                            x =>
                                x.PedagogicalLessonId ==
                                actual.Id)
                        .OrderBy(
                            x =>
                                x.SortOrder)
                        .Select(
                            x =>
                                nodeCodes[
                                    x.OutcomeNodeId])
                        .ToArray();

                Assert.Equal(
                    expected.OutcomeCodes,
                    actualMappings);
            }
        }

        await AssertMiddleSchoolPreservedAsync(
            db,
            versionId);

        var lessonCount =
            await db
                .CurriculumPedagogicalLessons
                .CountAsync();

        var mappingCount =
            await db
                .CurriculumPedagogicalLessonOutcomes
                .CountAsync();

        await seeder.SeedAsync();

        Assert.Equal(
            lessonCount,
            await db
                .CurriculumPedagogicalLessons
                .CountAsync());

        Assert.Equal(
            mappingCount,
            await db
                .CurriculumPedagogicalLessonOutcomes
                .CountAsync());
    }

    [Fact]
    public async Task
        SeederMigratesExactExistingGradeOneToFiveFallbackGraph()
    {
        await using var db =
            CreateDb(
                "phase29-elementary-migrate-");

        await new MathematicsCurriculumPackSeeder(
                db)
            .SeedAsync();

        var versionId =
            await db
                .CurriculumPackImportStates
                .Where(
                    x =>
                        x.FrameworkCode ==
                        MathematicsCurriculumPackRegistry
                            .CommonCoreCode)
                .Select(
                    x =>
                        x.FrameworkVersionId)
                .SingleAsync();

        var legacyIds =
            await SeedExistingFallbackGradeOneToFiveAsync(
                db,
                versionId);

        Assert.Equal(
            166,
            legacyIds.Count);

        Assert.Equal(
            166,
            await db
                .CurriculumPedagogicalLessonOutcomes
                .CountAsync(
                    x =>
                        legacyIds.Contains(
                            x.PedagogicalLessonId)));

        var seeder =
            new MathematicsPedagogicalLessonSeeder(
                db);

        await seeder.SeedAsync();

        Assert.False(
            await db
                .CurriculumPedagogicalLessons
                .AnyAsync(
                    x =>
                        legacyIds.Contains(
                            x.Id)));

        Assert.False(
            await db
                .CurriculumPedagogicalLessonOutcomes
                .AnyAsync(
                    x =>
                        legacyIds.Contains(
                            x.PedagogicalLessonId)));

        var elementaryLessons =
            await db
                .CurriculumPedagogicalLessons
                .Where(
                    x =>
                        x.FrameworkVersionId ==
                            versionId &&
                        x.LogicalLevelFrom >= 2 &&
                        x.LogicalLevelFrom <= 6 &&
                        x.LogicalLevelTo ==
                            x.LogicalLevelFrom &&
                        x.NativeLevel.StartsWith(
                            "Grade "))
                .ToArrayAsync();

        Assert.Equal(
            732,
            elementaryLessons.Length);

        var elementaryIds =
            elementaryLessons
                .Select(
                    x => x.Id)
                .ToArray();

        Assert.Equal(
            1456,
            await db
                .CurriculumPedagogicalLessonOutcomes
                .CountAsync(
                    x =>
                        elementaryIds.Contains(
                            x.PedagogicalLessonId)));

        await AssertMiddleSchoolPreservedAsync(
            db,
            versionId);

        var lessonCount =
            await db
                .CurriculumPedagogicalLessons
                .CountAsync();

        var mappingCount =
            await db
                .CurriculumPedagogicalLessonOutcomes
                .CountAsync();

        await seeder.SeedAsync();

        Assert.Equal(
            lessonCount,
            await db
                .CurriculumPedagogicalLessons
                .CountAsync());

        Assert.Equal(
            mappingCount,
            await db
                .CurriculumPedagogicalLessonOutcomes
                .CountAsync());
    }

    private static async Task<HashSet<Guid>>
        SeedExistingFallbackGradeOneToFiveAsync(
            EdulyticsDbContext db,
            Guid versionId)
    {
        var nodes =
            await db
                .CurriculumPackContentNodes
                .Where(
                    x =>
                        x.FrameworkVersionId ==
                            versionId &&
                        x.FrameworkCode ==
                            MathematicsCurriculumPackRegistry
                                .CommonCoreCode &&
                        x.IsActive)
                .OrderBy(
                    x =>
                        x.SortOrder)
                .ThenBy(
                    x =>
                        x.Code)
                .ToArrayAsync();

        var nodeById =
            nodes.ToDictionary(
                x => x.Id);

        var now =
            DateTime.UtcNow;

        var ids =
            new HashSet<Guid>();

        for (var grade = 1;
             grade <= 5;
             grade++)
        {
            var logicalLevel =
                grade + 1;

            var native =
                $"Grade {grade}";

            var nativeKey =
                NormalizeKey(
                    native);

            var applicable =
                nodes
                    .Where(
                        x =>
                            x.IsOfficial &&
                            x.NodeKind is
                                "Standard" or "Outcome" &&
                            x.LogicalLevelFrom <=
                                logicalLevel &&
                            x.LogicalLevelTo >=
                                logicalLevel &&
                            string.IsNullOrWhiteSpace(
                                x.Pathway))
                    .OrderBy(
                        x =>
                            x.SortOrder)
                    .ThenBy(
                        x =>
                            x.Code)
                    .ToArray();

            // Fallback source nodes are the complete grade registry,
            // independent of the closure's distinct lesson mappings.
            var officialRegistryNodeCounts =
                new Dictionary<int, int>
                {
                    [1] = 29,
                    [2] = 34,
                    [3] = 33,
                    [4] = 36,
                    [5] = 34
                };

            Assert.Equal(
                officialRegistryNodeCounts[grade],
                applicable.Length);

            var unitCounters =
                new Dictionary<string, int>(
                    StringComparer.Ordinal);

            var lessonSort =
                0;

            foreach (var outcome in
                     applicable)
            {
                var unit =
                    ResolveTeachingUnit(
                        outcome,
                        nodeById);

                Assert.NotNull(
                    unit);

                var unitKey =
                    $"{unit!.Code}:" +
                    $"L{logicalLevel}:" +
                    $"{nativeKey}:CORE";

                unitCounters.TryGetValue(
                    unitKey,
                    out var withinUnit);

                withinUnit++;

                unitCounters[
                    unitKey] =
                    withinUnit;

                lessonSort++;

                var id =
                    G(
                        $"pedagogical|" +
                        $"{versionId}|" +
                        $"L{logicalLevel}|" +
                        $"{nativeKey}|" +
                        $"CORE|" +
                        $"{outcome.Id}");

                Assert.True(
                    ids.Add(
                        id));

                db.CurriculumPedagogicalLessons.Add(
                    new CurriculumPedagogicalLesson
                    {
                        Id =
                            id,
                        FrameworkVersionId =
                            versionId,
                        OfficialLessonNodeId =
                            null,
                        Code =
                            $"PED:US-CCSS-MATH:" +
                            $"L{logicalLevel}:" +
                            $"{nativeKey}:" +
                            $"CORE:" +
                            $"{NormalizeKey(outcome.Code)}",
                        UnitKey =
                            unitKey,
                        UnitTitle =
                            unit.Title,
                        Title =
                            $"{unit.Title} — " +
                            $"Lesson {withinUnit:D2}",
                        LogicalLevelFrom =
                            logicalLevel,
                        LogicalLevelTo =
                            logicalLevel,
                        NativeLevel =
                            native,
                        Pathway =
                            null,
                        SortOrder =
                            lessonSort,
                        CreatedAtUtc =
                            now,
                        UpdatedAtUtc =
                            now
                    });

                db.CurriculumPedagogicalLessonOutcomes.Add(
                    new CurriculumPedagogicalLessonOutcome
                    {
                        PedagogicalLessonId =
                            id,
                        FrameworkVersionId =
                            versionId,
                        OutcomeNodeId =
                            outcome.Id,
                        SortOrder =
                            1
                    });
            }
        }

        await db.SaveChangesAsync();

        return ids;
    }

    private static CurriculumPackContentNode?
        ResolveTeachingUnit(
            CurriculumPackContentNode outcome,
            IReadOnlyDictionary<
                Guid,
                CurriculumPackContentNode> nodeById)
    {
        var parentId =
            outcome.ParentId;

        while (
            parentId.HasValue &&
            nodeById.TryGetValue(
                parentId.Value,
                out var parent))
        {
            if (parent.NodeKind is
                "Domain" or "Strand" or "Unit")
            {
                return parent;
            }

            parentId =
                parent.ParentId;
        }

        return null;
    }

    private static async Task
        AssertMiddleSchoolPreservedAsync(
            EdulyticsDbContext db,
            Guid versionId)
    {
        var expected =
            new[]
            {
                (
                    Grade: "Grade 6",
                    Lessons: 147,
                    Mappings: 237
                ),
                (
                    Grade: "Grade 7",
                    Lessons: 145,
                    Mappings: 208
                ),
                (
                    Grade: "Grade 8",
                    Lessons: 131,
                    Mappings: 179
                )
            };

        foreach (var spec in expected)
        {
            var lessonIds =
                await db
                    .CurriculumPedagogicalLessons
                    .Where(
                        x =>
                            x.FrameworkVersionId ==
                                versionId &&
                            x.NativeLevel ==
                                spec.Grade)
                    .Select(
                        x =>
                            x.Id)
                    .ToArrayAsync();

            Assert.Equal(
                spec.Lessons,
                lessonIds.Length);

            Assert.Equal(
                spec.Mappings,
                await db
                    .CurriculumPedagogicalLessonOutcomes
                    .CountAsync(
                        x =>
                            lessonIds.Contains(
                                x.PedagogicalLessonId)));
        }
    }

    private static EdulyticsDbContext
        CreateDb(
            string prefix)
    {
        var options =
            new DbContextOptionsBuilder<
                    EdulyticsDbContext>()
                .UseInMemoryDatabase(
                    prefix +
                    Guid.NewGuid()
                        .ToString("N"))
                .Options;

        return new EdulyticsDbContext(
            options);
    }

    private static string NormalizeKey(
        string value)
    {
        var normalized =
            Regex.Replace(
                value
                    .Trim()
                    .ToUpperInvariant(),
                @"[^A-Z0-9]+",
                "-");

        return normalized.Trim(
            '-');
    }

    private static Guid G(
        string value)
    {
        var hash =
            SHA256.HashData(
                Encoding.UTF8.GetBytes(
                    value));

        Span<byte> bytes =
            stackalloc byte[16];

        hash
            .AsSpan(
                0,
                16)
            .CopyTo(
                bytes);

        bytes[6] =
            (byte)(
                (bytes[6] & 0x0f)
                | 0x50);

        bytes[8] =
            (byte)(
                (bytes[8] & 0x3f)
                | 0x80);

        return new Guid(
            bytes);
    }
}