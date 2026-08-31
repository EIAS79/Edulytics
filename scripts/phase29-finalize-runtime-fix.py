from __future__ import annotations

import json
from pathlib import Path


def replace_once(path: Path, old: str, new: str) -> None:
    text = path.read_text(encoding="utf-8")
    count = text.count(old)
    if count != 1:
        raise SystemExit(f"Expected exactly one match in {path}, found {count}")
    path.write_text(text.replace(old, new, 1), encoding="utf-8")


# 1) AcademicStructureSnapshot must actually carry the explicit curriculum
# adoptions consumed by the Phase 29 lesson selector/scoping service.
repo = Path("src/Edulytics.Data/Repositories/AcademicStructureRepository.cs")
replace_once(
    repo,
    """        var classes = await _db.ClassGroups.AsNoTracking()\n""",
    """        var curriculumAdoptions = await _db.SchoolCurriculumAdoptions\n            .AsNoTracking()\n            .Where(x => x.SchoolId == schoolId)\n            .OrderBy(x => x.AcademicYearId)\n            .ThenBy(x => x.AcademicProgramId)\n            .ThenBy(x => x.CurriculumLogicalLevel)\n            .ThenBy(x => x.CurriculumPathway)\n            .ToArrayAsync(cancellationToken);\n\n        var classes = await _db.ClassGroups.AsNoTracking()\n""",
)
replace_once(
    repo,
    """            AcademicPrograms = programs,\n            AcademicYearProgramOfferings = programOfferings\n""",
    """            AcademicPrograms = programs,\n            AcademicYearProgramOfferings = programOfferings,\n            CurriculumAdoptions = curriculumAdoptions\n""",
)


# 2) UAE explicit curriculum identities must match the verified current
# SourceCatalog topology: Grades 1-4 Common; Grades 5-12 General/Advanced.
pack_path = Path("src/Edulytics.Core/Curriculum/Packs/uae-moe-math.curriculum-pack.json")
pack = json.loads(pack_path.read_text(encoding="utf-8"))
catalog = []
for node in pack["Nodes"]:
    if node.get("Kind") != "SourceCatalog":
        continue
    item = (node["LogicalLevelFrom"], node["NativeLevel"], node.get("Pathway"))
    if item not in catalog:
        catalog.append(item)

expected = (
    [(grade, f"Grade {grade}", "Common") for grade in range(1, 5)]
    + [
        (grade, f"Grade {grade}", pathway)
        for grade in range(5, 13)
        for pathway in ("General", "Advanced")
    ]
)
if catalog != expected:
    raise SystemExit(
        "UAE SourceCatalog topology drift.\n"
        f"Expected: {expected}\n"
        f"Actual:   {catalog}"
    )

registry = Path("src/Edulytics.Core/Curriculum/MathematicsCurriculumPackRegistry.cs")
old_levels = """            [
                new(1,\"Grade 1\",\"Cycle 1\",null,true),
                new(2,\"Grade 2\",\"Cycle 1\",null,true),
                new(3,\"Grade 3\",\"Cycle 1\",null,true),
                new(4,\"Grade 4\",\"Cycle 1\",null,true),
                new(5,\"Grade 5\",\"Cycle 2\",null,true),
                new(6,\"Grade 6\",\"Cycle 2\",null,true),
                new(7,\"Grade 7\",\"Cycle 2\",null,true),
                new(8,\"Grade 8\",\"Cycle 2\",null,true),
                new(9,\"Grade 9\",\"Secondary\",\"Preserve current pathway metadata\",true),
                new(10,\"Grade 10\",\"Secondary\",\"Preserve current pathway metadata\",true),
                new(11,\"Grade 11\",\"Secondary\",\"Preserve current pathway metadata\",true),
                new(12,\"Grade 12\",\"Secondary\",\"Preserve current pathway metadata\",true)
            ]),"""
new_levels = """            [
                new(1,\"Grade 1\",\"Cycle 1\",\"Common\",true),
                new(2,\"Grade 2\",\"Cycle 1\",\"Common\",true),
                new(3,\"Grade 3\",\"Cycle 1\",\"Common\",true),
                new(4,\"Grade 4\",\"Cycle 1\",\"Common\",true),
                new(5,\"Grade 5\",\"Cycle 2\",\"General\",true),
                new(5,\"Grade 5\",\"Cycle 2\",\"Advanced\",true),
                new(6,\"Grade 6\",\"Cycle 2\",\"General\",true),
                new(6,\"Grade 6\",\"Cycle 2\",\"Advanced\",true),
                new(7,\"Grade 7\",\"Cycle 2\",\"General\",true),
                new(7,\"Grade 7\",\"Cycle 2\",\"Advanced\",true),
                new(8,\"Grade 8\",\"Cycle 2\",\"General\",true),
                new(8,\"Grade 8\",\"Cycle 2\",\"Advanced\",true),
                new(9,\"Grade 9\",\"Secondary\",\"General\",true),
                new(9,\"Grade 9\",\"Secondary\",\"Advanced\",true),
                new(10,\"Grade 10\",\"Secondary\",\"General\",true),
                new(10,\"Grade 10\",\"Secondary\",\"Advanced\",true),
                new(11,\"Grade 11\",\"Secondary\",\"General\",true),
                new(11,\"Grade 11\",\"Secondary\",\"Advanced\",true),
                new(12,\"Grade 12\",\"Secondary\",\"General\",true),
                new(12,\"Grade 12\",\"Secondary\",\"Advanced\",true)
            ]),"""
replace_once(registry, old_levels, new_levels)

old_validation = """        var uae = All.Single(x => x.Code == UaeCode);
        if (uae.Levels.Max(x => x.LogicalLevel) != 12 ||
            uae.Levels.Any(x => x.LogicalLevel == 13))
        {
            throw new InvalidOperationException(\"UAE must stop at Grade 12.\");
        }"""
new_validation = """        var uae = All.Single(x => x.Code == UaeCode);
        if (uae.Levels.Max(x => x.LogicalLevel) != 12 ||
            uae.Levels.Any(x => x.LogicalLevel == 13) ||
            uae.Levels.Count != 20 ||
            Enumerable.Range(1, 4).Any(level =>
                !uae.Levels.Any(x =>
                    x.LogicalLevel == level &&
                    x.Pathway == \"Common\")) ||
            Enumerable.Range(5, 8).Any(level =>
                !uae.Levels.Any(x =>
                    x.LogicalLevel == level &&
                    x.Pathway == \"General\") ||
                !uae.Levels.Any(x =>
                    x.LogicalLevel == level &&
                    x.Pathway == \"Advanced\")))
        {
            throw new InvalidOperationException(
                \"UAE level/pathway topology must be Grade 1-4 Common and Grade 5-12 General/Advanced.\");
        }"""
replace_once(registry, old_validation, new_validation)

print("Phase 29 runtime acceptance patch applied.")
