#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
from collections import Counter, defaultdict
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[2]
CONTENT_DIR = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs"
SKILL_REGISTRY = ROOT / "src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json"
LESSON_MAPPINGS = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json"
REPORT = ROOT / "artifacts/math-intelligence/catalogue-skill-baseline.json"

REQUIRED_TRANSLATION_FIELDS = (
    "Title",
    "Explanation",
    "WorkedExamples",
    "StepByStepSolutions",
    "CommonMistakes",
    "QuickSummary",
)


def read_json(path: Path) -> Any:
    return json.loads(path.read_text(encoding="utf-8-sig"))


def get_case(row: dict[str, Any], *names: str, default: Any = None) -> Any:
    for name in names:
        if name in row:
            return row[name]
    return default


def clean_list(value: Any) -> list[str]:
    if not isinstance(value, list):
        return []
    return [str(item).strip() for item in value if str(item).strip()]


def lesson_structural_quality(lesson: dict[str, Any]) -> tuple[str, list[str]]:
    translations = get_case(lesson, "Translations", "translations", default=[])
    if not isinstance(translations, list) or not translations:
        return "FAIL", ["No lesson translation/content body exists."]

    best = translations[0] if isinstance(translations[0], dict) else {}
    missing = [
        field
        for field in REQUIRED_TRANSLATION_FIELDS
        if not str(get_case(best, field, field[0].lower() + field[1:], default="") or "").strip()
    ]
    if not missing:
        return "PASS", []
    if len(missing) <= 2:
        return "PASS_WITH_WARNINGS", [f"Missing structural field: {field}" for field in missing]
    return "FAIL", [f"Missing structural field: {field}" for field in missing]


def load_skill_registry() -> tuple[set[str], list[dict[str, Any]]]:
    if not SKILL_REGISTRY.exists():
        return set(), []
    doc = read_json(SKILL_REGISTRY)
    skills = doc.get("skills") or []
    ids = {
        str(row.get("id") or "").strip()
        for row in skills
        if isinstance(row, dict) and str(row.get("id") or "").strip()
    }
    return ids, skills


def load_mappings(skill_ids: set[str]) -> tuple[dict[str, dict[str, Any]], list[str]]:
    if not LESSON_MAPPINGS.exists():
        return {}, []
    doc = read_json(LESSON_MAPPINGS)
    result: dict[str, dict[str, Any]] = {}
    errors: list[str] = []
    for row in doc.get("mappings") or []:
        if not isinstance(row, dict):
            continue
        lesson_code = str(row.get("lessonCode") or "").strip()
        if not lesson_code:
            errors.append("A lesson-skill mapping has no lessonCode.")
            continue
        if lesson_code in result:
            errors.append(f"Duplicate lesson-skill mapping: {lesson_code}")
            continue

        mapped_skills = clean_list(row.get("primarySkills")) + clean_list(row.get("secondarySkills"))
        unknown = sorted(set(mapped_skills) - skill_ids)
        if unknown:
            errors.append(
                f"{lesson_code} references unknown SkillIds: {', '.join(unknown)}"
            )
        result[lesson_code] = row
    return result, errors


def audit() -> dict[str, Any]:
    skill_ids, skill_rows = load_skill_registry()
    mappings, mapping_errors = load_mappings(skill_ids)

    seen_codes: dict[str, str] = {}
    duplicate_codes: list[dict[str, str]] = []
    packs: list[dict[str, Any]] = []
    lessons: list[dict[str, Any]] = []
    totals = Counter()
    structural = Counter()
    source_breakdown: dict[str, Counter] = defaultdict(Counter)

    if not CONTENT_DIR.exists():
        raise RuntimeError(f"Lesson content directory does not exist: {CONTENT_DIR}")

    for path in sorted(CONTENT_DIR.glob("*.lesson-content-pack.json")):
        doc = read_json(path)
        pack_code = str(get_case(doc, "PackCode", "packCode", default="") or "").strip()
        content_version = str(
            get_case(doc, "ContentVersion", "contentVersion", default="") or ""
        ).strip()
        pack_lessons = get_case(doc, "Lessons", "lessons", default=[])
        if not isinstance(pack_lessons, list):
            pack_lessons = []

        pack_counter = Counter()
        for lesson in pack_lessons:
            if not isinstance(lesson, dict):
                continue

            code = str(get_case(lesson, "LessonCode", "lessonCode", default="") or "").strip()
            if not code:
                totals["missingLessonCode"] += 1
                continue

            if code in seen_codes:
                duplicate_codes.append({
                    "lessonCode": code,
                    "firstFile": seen_codes[code],
                    "secondFile": str(path.relative_to(ROOT)),
                })
            else:
                seen_codes[code] = str(path.relative_to(ROOT))

            outcomes = clean_list(get_case(lesson, "OutcomeCodes", "outcomeCodes", default=[]))
            is_supporting = bool(get_case(lesson, "IsSupporting", "isSupporting", default=False))
            mapping = mappings.get(code)
            quality, quality_issues = lesson_structural_quality(lesson)

            if outcomes:
                source_type = "OfficialMapped"
                totals["officialMapped"] += 1
            else:
                source_type = "PedagogicalUnmapped"
                totals["pedagogicalUnmapped"] += 1

            if is_supporting:
                totals["isSupportingFlagTrue"] += 1

            if mapping:
                totals["skillMapped"] += 1
            else:
                totals["skillUnmapped"] += 1

            structural[quality] += 1
            pack_counter["lessonCount"] += 1
            pack_counter[source_type] += 1
            pack_counter[f"structural{quality.title().replace('_', '')}"] += 1
            source_breakdown[pack_code][source_type] += 1

            lessons.append({
                "lessonCode": code,
                "packCode": pack_code,
                "contentVersion": content_version,
                "sourceType": source_type,
                "outcomeCount": len(outcomes),
                "isSupportingFlag": is_supporting,
                "structuralContentStatus": quality,
                "structuralIssues": quality_issues,
                "skillMappingStatus": "MAPPED_FOUNDATION" if mapping else "UNMAPPED",
                "primarySkills": [] if not mapping else clean_list(mapping.get("primarySkills")),
                "secondarySkills": [] if not mapping else clean_list(mapping.get("secondarySkills")),
                "mappingConfidence": None if not mapping else mapping.get("mappingConfidence"),
            })

        packs.append({
            "file": str(path.relative_to(ROOT)),
            "packCode": pack_code,
            "contentVersion": content_version,
            **dict(pack_counter),
        })

    totals["lessonCount"] = len(lessons)
    totals["packCount"] = len(packs)
    totals["skillRegistryCount"] = len(skill_ids)
    totals["foundationLessonMappingCount"] = len(mappings)

    missing_mapping_targets = sorted(set(mappings) - set(seen_codes))
    for code in missing_mapping_targets:
        mapping_errors.append(f"Lesson-skill mapping target not found in canonical content packs: {code}")

    blockers = []
    if duplicate_codes:
        blockers.append(f"{len(duplicate_codes)} duplicate lesson codes detected.")
    if mapping_errors:
        blockers.append(f"{len(mapping_errors)} lesson-skill mapping integrity errors detected.")
    if totals["missingLessonCode"]:
        blockers.append(f"{totals['missingLessonCode']} lesson rows have no LessonCode.")

    return {
        "schemaVersion": 1,
        "audit": "Edulytics Mathematics catalogue/skill foundation baseline",
        "scope": "Structural baseline only. It does not claim academic approval of every lesson.",
        "summary": {
            **dict(totals),
            "structuralContent": dict(structural),
            "duplicateLessonCodeCount": len(duplicate_codes),
            "mappingIntegrityErrorCount": len(mapping_errors),
            "blockerCount": len(blockers),
        },
        "blockers": blockers,
        "mappingErrors": mapping_errors,
        "duplicateLessonCodes": duplicate_codes,
        "skills": skill_rows,
        "sourceBreakdown": {
            pack: dict(counter)
            for pack, counter in sorted(source_breakdown.items())
        },
        "packs": packs,
        "lessons": lessons,
    }


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--write-report", action="store_true")
    parser.add_argument("--strict", action="store_true")
    args = parser.parse_args()

    report = audit()
    if args.write_report:
        REPORT.parent.mkdir(parents=True, exist_ok=True)
        REPORT.write_text(
            json.dumps(report, ensure_ascii=False, indent=2) + "\n",
            encoding="utf-8",
        )

    print(json.dumps(report["summary"], ensure_ascii=False, indent=2))
    if args.strict and report["summary"]["blockerCount"]:
        return 2
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
