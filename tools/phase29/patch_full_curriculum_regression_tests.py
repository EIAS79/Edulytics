#!/usr/bin/env python3
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]


def patch(path: str, replacements: list[tuple[str, str]]) -> None:
    p = ROOT / path
    text = p.read_text(encoding="utf-8")
    for old, new in replacements:
        if new in text:
            continue
        if old not in text:
            raise SystemExit(f"FAIL: expected test baseline not found in {path}: {old[:80]!r}")
        text = text.replace(old, new, 1)
    p.write_text(text, encoding="utf-8")
    print(f"PASS: patched {path}")


patch(
    "tests/Edulytics.Tests/Phase29/Phase29CanonicalContentPackPipelineTests.cs",
    [
        (
            '''        var uae =\n            Assert.Single(documents, x =>\n                        x.PackCode ==\n                        MathematicsCurriculumPackRegistry.UaeCode);\n''',
            '''        var uae =\n            Assert.Single(documents, x =>\n                        x.PackCode ==\n                            MathematicsCurriculumPackRegistry.UaeCode &&\n                        x.ContentVersion ==\n                            "uae-g9-adv-t1-pilot-v1");\n''',
        ),
    ],
)

patch(
    "tests/Edulytics.Tests/Phase29/Phase29PedagogicalLessonArchitectureTests.cs",
    [
        ("        Assert.Equal(169, lessons.Length);\n", "        Assert.Equal(566, lessons.Length);\n"),
        (
            '''        var supportingPrimary = lessons.Where(x => x.LogicalLevelFrom >= 2 && x.LogicalLevelFrom <= 6).ToArray();\n\n        Assert.Equal(27, stageOne.Length);\n        Assert.Equal(142, supportingPrimary.Length);\n''',
            '''        var supportingPrimary = lessons.Where(x => x.LogicalLevelFrom >= 2 && x.LogicalLevelFrom <= 6).ToArray();\n        var supportingLater = lessons.Where(x => x.LogicalLevelFrom >= 7 && x.LogicalLevelFrom <= 13).ToArray();\n\n        Assert.Equal(27, stageOne.Length);\n        Assert.Equal(142, supportingPrimary.Length);\n        Assert.Equal(397, supportingLater.Length);\n''',
        ),
        (
            '''        var stageOneIds = stageOne.Select(x => x.Id).ToArray();\n        var supportingIds = supportingPrimary.Select(x => x.Id).ToArray();\n''',
            '''        var stageOneIds = stageOne.Select(x => x.Id).ToArray();\n        var supportingIds = supportingPrimary.Concat(supportingLater).Select(x => x.Id).ToArray();\n\n        Assert.Contains(supportingLater, x => x.LogicalLevelFrom == 7);\n        Assert.Contains(supportingLater, x => x.LogicalLevelFrom == 8);\n        Assert.Contains(supportingLater, x => x.LogicalLevelFrom == 9);\n        Assert.Contains(supportingLater, x => x.LogicalLevelFrom == 10 && x.Pathway == "Core");\n        Assert.Contains(supportingLater, x => x.LogicalLevelFrom == 10 && x.Pathway == "Extended");\n        Assert.Contains(supportingLater, x => x.LogicalLevelFrom == 11 && x.Pathway == "Core");\n        Assert.Contains(supportingLater, x => x.LogicalLevelFrom == 11 && x.Pathway == "Extended");\n        Assert.Contains(supportingLater, x => x.LogicalLevelFrom == 12 && x.Pathway == "Component/route structure preserved in reference graph");\n        Assert.Contains(supportingLater, x => x.LogicalLevelFrom == 13 && x.Pathway == "Component/route structure preserved in reference graph");\n''',
        ),
        (
            '''                     (x.LogicalLevelFrom < 1 || x.LogicalLevelTo > 6)));\n''',
            '''                     (x.LogicalLevelFrom < 1 || x.LogicalLevelTo > 13)));\n''',
        ),
    ],
)

patch(
    "tests/Edulytics.Tests/Phase29/Phase29CambridgeCurriculumBaselineTests.cs",
    [
        ("        Assert.Equal(169, lessons.Length);\n", "        Assert.Equal(566, lessons.Length);\n"),
        (
            '''        var stagesTwoToSix = lessons.Where(x => x.LogicalLevelFrom >= 2 && x.LogicalLevelFrom <= 6).ToArray();\n        Assert.Equal(27, stageOne.Length);\n        Assert.Equal(142, stagesTwoToSix.Length);\n\n        var stageOneIds = stageOne.Select(x => x.Id).ToArray();\n        var supportingIds = stagesTwoToSix.Select(x => x.Id).ToArray();\n''',
            '''        var stagesTwoToSix = lessons.Where(x => x.LogicalLevelFrom >= 2 && x.LogicalLevelFrom <= 6).ToArray();\n        var laterScopes = lessons.Where(x => x.LogicalLevelFrom >= 7 && x.LogicalLevelFrom <= 13).ToArray();\n        Assert.Equal(27, stageOne.Length);\n        Assert.Equal(142, stagesTwoToSix.Length);\n        Assert.Equal(397, laterScopes.Length);\n\n        var stageOneIds = stageOne.Select(x => x.Id).ToArray();\n        var supportingIds = stagesTwoToSix.Concat(laterScopes).Select(x => x.Id).ToArray();\n''',
        ),
        (
            '''                     (x.LogicalLevelFrom < 1 || x.LogicalLevelTo > 6)));\n''',
            '''                     (x.LogicalLevelFrom < 1 || x.LogicalLevelTo > 13)));\n''',
        ),
    ],
)

patch(
    "tests/Edulytics.Tests/Phase29/Phase29CambridgePrimaryStage1ContentTests.cs",
    [
        ("        Assert.Equal(169, lessons.Length);\n", "        Assert.Equal(566, lessons.Length);\n"),
        (
            '''        var supporting = lessons.Where(x => x.LogicalLevelFrom >= 2 && x.LogicalLevelFrom <= 6).ToArray();\n        Assert.Equal(27, stageOne.Length);\n        Assert.Equal(142, supporting.Length);\n''',
            '''        var supporting = lessons.Where(x => x.LogicalLevelFrom >= 2 && x.LogicalLevelFrom <= 13).ToArray();\n        Assert.Equal(27, stageOne.Length);\n        Assert.Equal(539, supporting.Length);\n''',
        ),
        (
            '''            169,\n            await db.CurriculumLessonContents.CountAsync(\n''',
            '''            566,\n            await db.CurriculumLessonContents.CountAsync(\n''',
        ),
        (
            '''            169,\n            await db.CurriculumLessonContentTranslations.CountAsync(\n''',
            '''            566,\n            await db.CurriculumLessonContentTranslations.CountAsync(\n''',
        ),
    ],
)
