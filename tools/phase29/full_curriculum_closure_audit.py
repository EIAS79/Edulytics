#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
from dataclasses import dataclass
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[2]
CURRICULUM_DIR = ROOT / "src/Edulytics.Core/Curriculum/Packs"
BLUEPRINT_DIR = ROOT / "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs"
CONTENT_DIR = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs"
REPORT = ROOT / "artifacts/phase29/full-curriculum-closure-audit.json"
COMMON_CORE_AUDIT = ROOT / "docs/PHASE_29_COMMON_CORE_CONTENT_ROLLOUT_AUDIT.json"


@dataclass(frozen=True, order=True)
class Scope:
    pack: str
    level: int
    pathway: str = ""

    @property
    def key(self) -> str:
        return f"{self.pack}:L{self.level:02d}:{self.pathway or 'SHARED'}"


def load_json(path: Path) -> Any:
    return json.loads(path.read_text(encoding="utf-8"))


def norm_pathway(value: Any) -> str:
    return str(value or "").strip()


def expected_scopes() -> list[Scope]:
    scopes: list[Scope] = []

    # Common Core Kindergarten remains a verified framework node but is not a
    # Phase 29 product lesson scope. Grade 1..12 map to logical levels 2..13.
    scopes.extend(Scope("US-CCSS-MATH", level) for level in range(2, 14))

    scopes.extend(Scope("CAMBRIDGE-INTL-MATH", level) for level in range(1, 10))
    for level in (10, 11):
        scopes.append(Scope("CAMBRIDGE-INTL-MATH", level, "Core"))
        scopes.append(Scope("CAMBRIDGE-INTL-MATH", level, "Extended"))
    scopes.extend(
        [
            Scope("CAMBRIDGE-INTL-MATH", 12),
            Scope("CAMBRIDGE-INTL-MATH", 13),
        ]
    )

    scopes.extend(Scope("UAE-MOE-MATH", level, "Common") for level in range(1, 5))
    for level in range(5, 13):
        scopes.append(Scope("UAE-MOE-MATH", level, "General"))
        scopes.append(Scope("UAE-MOE-MATH", level, "Advanced"))

    scopes.extend(Scope("PL-NATIONAL-MATH", level, "Edukacja wczesnoszkolna") for level in range(1, 4))
    scopes.extend(Scope("PL-NATIONAL-MATH", level) for level in range(4, 9))
    scopes.extend(Scope("PL-NATIONAL-MATH", level, "Liceum ogólnokształcące") for level in range(9, 13))
    scopes.extend(Scope("PL-NATIONAL-MATH", level, "Technikum") for level in range(9, 14))

    return sorted(scopes)


def blueprint_scope_rows() -> dict[Scope, dict[str, Any]]:
    result: dict[Scope, dict[str, Any]] = {}
    if not BLUEPRINT_DIR.exists():
        return result

    for path in sorted(BLUEPRINT_DIR.glob("*.lesson-blueprint.json")):
        doc = load_json(path)
        pack = str(doc.get("PackCode") or doc.get("packCode") or "").strip()
        if not pack:
            continue

        schema = int(doc.get("SchemaVersion") or doc.get("schemaVersion") or 1)
        pathway = norm_pathway(doc.get("Pathway") if "Pathway" in doc else doc.get("pathway"))
        lessons = doc.get("Lessons") or doc.get("lessons") or []
        lesson_codes = {
            str(x.get("LessonCode") or x.get("lessonCode") or "").strip()
            for x in lessons
            if str(x.get("LessonCode") or x.get("lessonCode") or "").strip()
        }

        if schema == 1:
            levels = [int(doc.get("LogicalLevel") or doc.get("logicalLevel") or 0)]
        else:
            start = int(doc.get("LogicalLevelFrom") or doc.get("logicalLevelFrom") or 0)
            end = int(doc.get("LogicalLevelTo") or doc.get("logicalLevelTo") or start)
            levels = list(range(start, end + 1)) if start > 0 and end >= start else []

        for level in levels:
            scope = Scope(pack, level, pathway)
            row = result.setdefault(scope, {"files": [], "lessonCodes": set()})
            row["files"].append(str(path.relative_to(ROOT)))
            row["lessonCodes"].update(lesson_codes)

    return result


def canonical_rows() -> tuple[dict[str, set[str]], list[dict[str, Any]]]:
    by_pack: dict[str, set[str]] = {}
    docs: list[dict[str, Any]] = []
    if not CONTENT_DIR.exists():
        return by_pack, docs

    for path in sorted(CONTENT_DIR.glob("*.lesson-content-pack.json")):
        doc = load_json(path)
        pack = str(doc.get("packCode") or doc.get("PackCode") or "").strip()
        lessons = doc.get("lessons") or doc.get("Lessons") or []
        codes = {
            str(x.get("lessonCode") or x.get("LessonCode") or "").strip()
            for x in lessons
            if str(x.get("lessonCode") or x.get("LessonCode") or "").strip()
        }
        by_pack.setdefault(pack, set()).update(codes)
        docs.append(
            {
                "file": str(path.relative_to(ROOT)),
                "pack": pack,
                "status": doc.get("status") or doc.get("Status"),
                "academicLanguage": doc.get("academicLanguage") or doc.get("AcademicLanguage"),
                "lessonCount": len(codes),
            }
        )

    return by_pack, docs


def uae_verified_official_lessons() -> dict[Scope, set[str]]:
    path = CURRICULUM_DIR / "uae-moe-math.curriculum-pack.json"
    if not path.exists():
        return {}
    doc = load_json(path)
    result: dict[Scope, set[str]] = {}
    for node in doc.get("Nodes", []):
        if node.get("Kind") != "Lesson" or not node.get("IsActive", False):
            continue
        level = int(node.get("LogicalLevelFrom") or 0)
        pathway = norm_pathway(node.get("Pathway"))
        code = str(node.get("Code") or "").strip()
        if level and code:
            result.setdefault(Scope("UAE-MOE-MATH", level, pathway), set()).add(f"PED:{code}")
    return result


def common_core_baseline_ok() -> tuple[bool, dict[str, Any]]:
    if not COMMON_CORE_AUDIT.exists():
        return False, {"reason": "Common Core final rollout audit is missing."}
    audit = load_json(COMMON_CORE_AUDIT)
    expected = {
        "sourcePedagogicalLessons": 1560,
        "standaloneCanonicalTargets": 1466,
        "supportingOnlyLessons": 94,
    }
    mismatches = {
        key: {"expected": value, "actual": audit.get(key)}
        for key, value in expected.items()
        if audit.get(key) != value
    }
    return not mismatches, {"audit": str(COMMON_CORE_AUDIT.relative_to(ROOT)), "mismatches": mismatches}


def audit() -> dict[str, Any]:
    blueprints = blueprint_scope_rows()
    canonical_by_pack, canonical_docs = canonical_rows()
    uae_official = uae_verified_official_lessons()
    common_core_ok, common_core_details = common_core_baseline_ok()

    rows: list[dict[str, Any]] = []
    missing: list[str] = []

    for scope in expected_scopes():
        if scope.pack == "US-CCSS-MATH":
            rows.append(
                {
                    "scope": scope.key,
                    "status": "COMPLETE" if common_core_ok else "INCOMPLETE",
                    "mode": "accepted-common-core-rollout-baseline",
                    **common_core_details,
                }
            )
            if not common_core_ok:
                missing.append(scope.key)
            continue

        bp = blueprints.get(scope)
        expected_codes: set[str] = set(bp["lessonCodes"]) if bp else set()
        mode = "blueprint"

        if scope.pack == "UAE-MOE-MATH" and scope in uae_official:
            expected_codes |= uae_official[scope]
            mode = "verified-official-lesson-graph" if not bp else "blueprint+verified-official-lesson-graph"

        actual = canonical_by_pack.get(scope.pack, set())
        covered = expected_codes & actual
        missing_codes = sorted(expected_codes - actual)

        complete = bool(expected_codes) and not missing_codes
        status = "COMPLETE" if complete else "INCOMPLETE"
        if not complete:
            missing.append(scope.key)

        rows.append(
            {
                "scope": scope.key,
                "status": status,
                "mode": mode if expected_codes else "missing-lesson-sequence",
                "blueprintFiles": [] if not bp else bp["files"],
                "expectedLessonCount": len(expected_codes),
                "canonicalLessonCount": len(covered),
                "missingCanonicalLessonCount": len(missing_codes),
                "missingCanonicalLessonCodesSample": missing_codes[:20],
            }
        )

    summary = {
        "expectedScopeCount": len(rows),
        "completeScopeCount": sum(1 for row in rows if row["status"] == "COMPLETE"),
        "missingScopeCount": len(missing),
        "missingScopes": missing,
    }

    return {
        "schemaVersion": 1,
        "phase": 29,
        "closureRule": "All supported curriculum scopes require a source-backed lesson sequence and canonical lesson bodies before Phase 29 can close.",
        "summary": summary,
        "scopes": rows,
        "canonicalDocuments": canonical_docs,
    }


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--strict", action="store_true", help="Exit non-zero while any required scope is incomplete.")
    parser.add_argument("--write-report", action="store_true")
    args = parser.parse_args()

    report = audit()
    if args.write_report:
        REPORT.parent.mkdir(parents=True, exist_ok=True)
        REPORT.write_text(json.dumps(report, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")

    print(json.dumps(report["summary"], ensure_ascii=False, indent=2))
    if args.strict and report["summary"]["missingScopeCount"]:
        return 2
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
