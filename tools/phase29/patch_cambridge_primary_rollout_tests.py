#!/usr/bin/env python3
from pathlib import Path
import re

ROOT = Path(__file__).resolve().parents[2]


def replace_method(path: Path, method_name: str, new_method: str) -> None:
    text = path.read_text(encoding="utf-8")
    pattern = re.compile(
        rf"    \[Fact\]\n    public async Task(?:\n        )?{re.escape(method_name)}\(\)\n    \{{.*?\n    \}}\n\n    \[Fact\]",
        re.S,
    )
    match = pattern.search(text)
    if not match:
        # Handle methods whose signature wraps differently.
        pattern = re.compile(
            rf"    \[Fact\]\n    public async Task\s+{re.escape(method_name)}\(\)\n    \{{.*?\n    \}}\n\n    \[Fact\]",
            re.S,
        )
        match = pattern.search(text)
    if not match:
        raise SystemExit(f"FAIL: method not found: {path}:{method_name}")
    replacement = new_method.rstrip() + "\n\n    [Fact]"
    path.write_text(text[:match.start()] + replacement + text[match.end():], encoding="utf-8")
    print(f"PASS: patched {path.name}:{method_name}")


architecture = ROOT / "tests/Edulytics.Tests/Phase29/Phase29PedagogicalLessonArchitectureTests.cs"
replace_method(
    architecture,
    "CambridgeCreatesOnlyExplicitStageOneBlueprintAndNoSyntheticOutcomeBackedFallback",
    r'''    [Fact]
    public async Task CambridgeCreatesReviewedPrimaryBlueprintsWithoutSyntheticOutcomeFallback()
    {
        await using var db = CreateDb();

        await new MathematicsCurriculumPackSeeder(db).SeedAsync();
        await new MathematicsPedagogicalLessonSeeder(db).SeedAsync();

        var versionId = await db.CurriculumPackImportStates
            .Where(x => x.FrameworkCode == MathematicsCurriculumPackRegistry.CambridgeCode)
            .Select(x => x.FrameworkVersionId)
            .SingleAsync();

        var lessons = await db.CurriculumPedagogicalLessons
            .Where(x => x.FrameworkVersionId == versionId)
            .OrderBy(x => x.SortOrder)
            .ToArrayAsync();

        Assert.Equal(169, lessons.Length);

        var stageOne = lessons.Where(x => x.LogicalLevelFrom == 1 && x.LogicalLevelTo == 1).ToArray();
        var supportingPrimary = lessons.Where(x => x.LogicalLevelFrom >= 2 && x.LogicalLevelFrom <= 6).ToArray();

        Assert.Equal(27, stageOne.Length);
        Assert.Equal(142, supportingPrimary.Length);
        Assert.All(stageOne, x => Assert.Equal("Cambridge Primary Stage 1", x.NativeLevel));
        Assert.All(stageOne, x => Assert.StartsWith("PED:CAMBRIDGE-INTL-MATH:S1:", x.Code, StringComparison.Ordinal));

        foreach (var stage in Enumerable.Range(2, 5))
        {
            var scoped = supportingPrimary.Where(x => x.LogicalLevelFrom == stage && x.LogicalLevelTo == stage).ToArray();
            Assert.NotEmpty(scoped);
            Assert.All(scoped, x => Assert.Equal($"Cambridge Primary Stage {stage}", x.NativeLevel));
            Assert.All(scoped, x => Assert.StartsWith($"PED:CAMBRIDGE-INTL-MATH:S{stage}:", x.Code, StringComparison.Ordinal));
        }

        var stageOneIds = stageOne.Select(x => x.Id).ToArray();
        var supportingIds = supportingPrimary.Select(x => x.Id).ToArray();

        Assert.Equal(
            36,
            await db.CurriculumPedagogicalLessonOutcomes.CountAsync(
                x => x.FrameworkVersionId == versionId && stageOneIds.Contains(x.PedagogicalLessonId)));

        Assert.False(
            await db.CurriculumPedagogicalLessonOutcomes.AnyAsync(
                x => x.FrameworkVersionId == versionId && supportingIds.Contains(x.PedagogicalLessonId)));

        Assert.False(
            await db.CurriculumPedagogicalLessons.AnyAsync(
                x => x.FrameworkVersionId == versionId &&
                     (x.LogicalLevelFrom < 1 || x.LogicalLevelTo > 6)));
    }''',
)

baseline = ROOT / "tests/Edulytics.Tests/Phase29/Phase29CambridgeCurriculumBaselineTests.cs"
replace_method(
    baseline,
    "CambridgeSeederIsIdempotentAndCreatesOnlyReviewedStageOneLessons",
    r'''    [Fact]
    public async Task CambridgeSeederIsIdempotentAndCreatesReviewedPrimaryStageOneToSixLessons()
    {
        await using var db = Db("cambridge-v2-" + Guid.NewGuid().ToString("N"));

        var packSeeder = new MathematicsCurriculumPackSeeder(db);
        await packSeeder.SeedAsync();
        await packSeeder.SeedAsync();

        var state = await db.CurriculumPackImportStates.SingleAsync(
            x => x.FrameworkCode == MathematicsCurriculumPackRegistry.CambridgeCode);

        Assert.Equal(779, state.OfficialNodeCount);
        Assert.Equal(888, state.NodeCount);
        Assert.Equal(
            888,
            await db.CurriculumPackContentNodes.CountAsync(
                x => x.FrameworkVersionId == state.FrameworkVersionId));
        Assert.False(
            await db.CurriculumPackContentNodes.AnyAsync(
                x => x.FrameworkVersionId == state.FrameworkVersionId && x.OfficialText != null));

        var pedagogical = new MathematicsPedagogicalLessonSeeder(db);
        await pedagogical.SeedAsync();
        await pedagogical.SeedAsync();

        var lessons = await db.CurriculumPedagogicalLessons
            .Where(x => x.FrameworkVersionId == state.FrameworkVersionId)
            .OrderBy(x => x.SortOrder)
            .ToArrayAsync();

        Assert.Equal(169, lessons.Length);
        var stageOne = lessons.Where(x => x.LogicalLevelFrom == 1 && x.LogicalLevelTo == 1).ToArray();
        var stagesTwoToSix = lessons.Where(x => x.LogicalLevelFrom >= 2 && x.LogicalLevelFrom <= 6).ToArray();
        Assert.Equal(27, stageOne.Length);
        Assert.Equal(142, stagesTwoToSix.Length);

        var stageOneIds = stageOne.Select(x => x.Id).ToArray();
        var supportingIds = stagesTwoToSix.Select(x => x.Id).ToArray();

        var mappings = await (
            from mapping in db.CurriculumPedagogicalLessonOutcomes
            join node in db.CurriculumPackContentNodes on mapping.OutcomeNodeId equals node.Id
            where mapping.FrameworkVersionId == state.FrameworkVersionId &&
                  stageOneIds.Contains(mapping.PedagogicalLessonId)
            select node.Code).ToArrayAsync();

        Assert.Equal(36, mappings.Length);
        Assert.Equal(36, mappings.Distinct(StringComparer.Ordinal).Count());
        Assert.All(mappings, code => Assert.StartsWith("CAM:OUT:0096:1", code, StringComparison.Ordinal));
        Assert.False(
            await db.CurriculumPedagogicalLessonOutcomes.AnyAsync(
                x => x.FrameworkVersionId == state.FrameworkVersionId &&
                     supportingIds.Contains(x.PedagogicalLessonId)));

        Assert.False(
            await db.CurriculumPedagogicalLessons.AnyAsync(
                x => x.FrameworkVersionId == state.FrameworkVersionId &&
                     (x.LogicalLevelFrom < 1 || x.LogicalLevelTo > 6)));
    }''',
)

stage_one = ROOT / "tests/Edulytics.Tests/Phase29/Phase29CambridgePrimaryStage1ContentTests.cs"
replace_method(
    stage_one,
    "StageOneSeedsExactlyAndDoesNotCreateCambridgeFallbackBeyondStageOne",
    r'''    [Fact]
    public async Task StageOneRemainsExactWhileStagesTwoToSixSeedAsReviewedSupportingContent()
    {
        await using var db = CreateDb();

        await new MathematicsCurriculumPackSeeder(db).SeedAsync();

        var pedagogy = new MathematicsPedagogicalLessonSeeder(db);
        await pedagogy.SeedAsync();

        var canonical = new MathematicsCanonicalLessonContentSeeder(db);
        await canonical.SeedAsync();

        var versionId = await db.CurriculumPackImportStates
            .Where(x => x.FrameworkCode == MathematicsCurriculumPackRegistry.CambridgeCode)
            .Select(x => x.FrameworkVersionId)
            .SingleAsync();

        var lessons = await db.CurriculumPedagogicalLessons
            .Where(x => x.FrameworkVersionId == versionId)
            .OrderBy(x => x.SortOrder)
            .ToArrayAsync();

        Assert.Equal(169, lessons.Length);
        var stageOne = lessons.Where(x => x.LogicalLevelFrom == 1 && x.LogicalLevelTo == 1).ToArray();
        var supporting = lessons.Where(x => x.LogicalLevelFrom >= 2 && x.LogicalLevelFrom <= 6).ToArray();
        Assert.Equal(27, stageOne.Length);
        Assert.Equal(142, supporting.Length);
        Assert.All(stageOne, x => Assert.Equal("Cambridge Primary Stage 1", x.NativeLevel));

        var stageOneIds = stageOne.Select(x => x.Id).ToArray();
        var supportingIds = supporting.Select(x => x.Id).ToArray();
        var allLessonIds = lessons.Select(x => x.Id).ToArray();

        var mappings = await (
            from mapping in db.CurriculumPedagogicalLessonOutcomes
            join node in db.CurriculumPackContentNodes on mapping.OutcomeNodeId equals node.Id
            where mapping.FrameworkVersionId == versionId &&
                  stageOneIds.Contains(mapping.PedagogicalLessonId)
            select node.Code).ToArrayAsync();

        Assert.Equal(36, mappings.Length);
        Assert.True(ExpectedStageOneCodes.SetEquals(mappings));
        Assert.DoesNotContain(mappings, x => x.StartsWith("TWM.", StringComparison.Ordinal));
        Assert.False(
            await db.CurriculumPedagogicalLessonOutcomes.AnyAsync(
                x => x.FrameworkVersionId == versionId && supportingIds.Contains(x.PedagogicalLessonId)));

        Assert.Equal(
            169,
            await db.CurriculumLessonContents.CountAsync(
                x => allLessonIds.Contains(x.PedagogicalLessonId)));

        var contentIds = await db.CurriculumLessonContents
            .Where(x => allLessonIds.Contains(x.PedagogicalLessonId))
            .Select(x => x.Id)
            .ToArrayAsync();

        Assert.Equal(
            169,
            await db.CurriculumLessonContentTranslations.CountAsync(
                x => contentIds.Contains(x.CurriculumLessonContentId) && x.CultureCode == "en"));

        var lessonCount = await db.CurriculumPedagogicalLessons.CountAsync(
            x => x.FrameworkVersionId == versionId);
        var mappingCount = await db.CurriculumPedagogicalLessonOutcomes.CountAsync(
            x => x.FrameworkVersionId == versionId);
        var contentCount = await db.CurriculumLessonContents.CountAsync(
            x => allLessonIds.Contains(x.PedagogicalLessonId));

        await pedagogy.SeedAsync();
        await canonical.SeedAsync();

        Assert.Equal(lessonCount, await db.CurriculumPedagogicalLessons.CountAsync(
            x => x.FrameworkVersionId == versionId));
        Assert.Equal(mappingCount, await db.CurriculumPedagogicalLessonOutcomes.CountAsync(
            x => x.FrameworkVersionId == versionId));
        Assert.Equal(contentCount, await db.CurriculumLessonContents.CountAsync(
            x => allLessonIds.Contains(x.PedagogicalLessonId)));
    }''',
)
