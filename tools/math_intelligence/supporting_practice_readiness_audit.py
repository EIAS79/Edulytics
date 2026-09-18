#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
from collections import Counter
from pathlib import Path
from typing import Any

from lesson_semantic_content_audit import audit as semantic_audit
from lesson_skill_resolution_audit import audit as skill_audit

ROOT = Path(__file__).resolve().parents[2]
CONTENT_DIR = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs"
MAPPINGS = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json"
FAMILIES = ROOT / "src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json"
CAPABILITIES = ROOT / "src/Edulytics.Core/Mathematics/Skills/capability-registry.v1.json"
REPORT = ROOT / "artifacts/math-intelligence/supporting-practice-readiness-audit.json"


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


def supporting_lessons() -> dict[str, dict[str, Any]]:
    result: dict[str, dict[str, Any]] = {}
    for path in sorted(CONTENT_DIR.glob("*.lesson-content-pack.json")):
        doc = read_json(path)
        pack_code = str(get_case(doc, "PackCode", "packCode", default="") or "")
        lessons = get_case(doc, "Lessons", "lessons", default=[])
        if not isinstance(lessons, list):
            continue

        for lesson in lessons:
            if not isinstance(lesson, dict):
                continue
            code = str(get_case(lesson, "LessonCode", "lessonCode", default="") or "").strip()
            if not code or code in result:
                continue
            outcomes = clean_list(get_case(lesson, "OutcomeCodes", "outcomeCodes", default=[]))
            if outcomes:
                continue
            translations = get_case(lesson, "Translations", "translations", default=[])
            title = ""
            if isinstance(translations, list):
                candidates = [row for row in translations if isinstance(row, dict)]
                selected = next(
                    (row for row in candidates
                     if str(get_case(row, "CultureCode", "cultureCode", default="") or "").lower().startswith("en")),
                    candidates[0] if candidates else {},
                )
                title = str(get_case(selected, "Title", "title", default="") or "")
            result[code] = {"lessonCode": code, "packCode": pack_code, "title": title}
    return result


def audit() -> dict[str, Any]:
    support = supporting_lessons()
    mapping_doc = read_json(MAPPINGS)
    family_doc = read_json(FAMILIES)
    capability_doc = read_json(CAPABILITIES)
    semantic = semantic_audit()
    skill = skill_audit()

    mappings = {
        str(row.get("lessonCode") or ""): row
        for row in mapping_doc.get("mappings") or []
        if isinstance(row, dict) and str(row.get("lessonCode") or "")
    }
    families = {
        str(row.get("id") or ""): row
        for row in family_doc.get("families") or []
        if isinstance(row, dict) and str(row.get("id") or "")
    }
    capabilities = {
        str(row.get("id") or ""): row
        for row in capability_doc.get("capabilities") or []
        if isinstance(row, dict) and str(row.get("id") or "")
    }
    semantic_by_code = {
        str(row.get("lessonCode") or ""): row
        for row in semantic.get("lessons") or []
        if isinstance(row, dict)
    }
    skill_by_code = {
        str(row.get("lessonCode") or ""): row
        for row in skill.get("lessons") or []
        if isinstance(row, dict)
    }

    rows: list[dict[str, Any]] = []
    blockers: list[str] = []
    summary = Counter()

    for code, lesson in sorted(support.items()):
        mapping = mappings.get(code)
        semantic_status = str((semantic_by_code.get(code) or {}).get("status") or "UNCLASSIFIED")
        skill_status = str((skill_by_code.get(code) or {}).get("status") or "UNCLASSIFIED")
        primary_skills = clean_list(mapping.get("primarySkills") if mapping else [])
        allowed_families = clean_list(mapping.get("allowedQuestionFamilies") if mapping else [])
        declared_readiness = str(mapping.get("practiceReadiness") if mapping else "")

        family_errors: list[str] = []
        for family_id in allowed_families:
            family = families.get(family_id)
            if family is None:
                family_errors.append(f"Unknown family {family_id}")
                continue
            for capability_id in clean_list(family.get("requiredCapabilities")):
                capability = capabilities.get(capability_id)
                if capability is None:
                    family_errors.append(f"Family {family_id} requires unknown capability {capability_id}")
                    continue
                if str(capability.get("status") or "") not in {"ShadowVerified", "ProductionVerified", "Verified"}:
                    family_errors.append(
                        f"Family {family_id} capability {capability_id} is not verified"
                    )

        if declared_readiness == "READY_VERIFIED":
            if semantic_status in {"CONTENT_WEAK", "REVIEW_REQUIRED"}:
                terminal = "EXPLICITLY_BLOCKED"
                reason = "CONTENT_WEAK"
                blockers.append(
                    f"READY_VERIFIED Practice contract has insufficient semantic-content evidence: {code} ({semantic_status})"
                )
            elif len(primary_skills) != 1:
                terminal = "EXPLICITLY_BLOCKED"
                reason = "PRIMARY_SKILL_AMBIGUOUS"
                blockers.append(f"READY_VERIFIED Practice contract must have exactly one Primary skill: {code}")
            elif not allowed_families:
                terminal = "EXPLICITLY_BLOCKED"
                reason = "QUESTION_FAMILY_MISSING"
                blockers.append(f"READY_VERIFIED Practice contract has no allowed families: {code}")
            elif family_errors:
                terminal = "EXPLICITLY_BLOCKED"
                reason = "CAPABILITY_INVALID"
                blockers.extend(f"{code}: {error}" for error in family_errors)
            else:
                terminal = "READY_VERIFIED"
                reason = "EXACT_CONTRACT_VERIFIED"
        elif mapping is not None:
            terminal = "EXPLICITLY_BLOCKED"
            if semantic_status in {"CONTENT_WEAK", "REVIEW_REQUIRED"}:
                reason = "CONTENT_WEAK"
            elif len(primary_skills) != 1:
                reason = "PRIMARY_SKILL_AMBIGUOUS"
            elif not allowed_families:
                reason = "QUESTION_FAMILY_MISSING"
            elif family_errors:
                reason = "SOLVER_OR_VERIFIER_CAPABILITY_MISSING"
            else:
                reason = "PRACTICE_CONTRACT_NOT_APPROVED"
        else:
            terminal = "EXPLICITLY_BLOCKED"
            if semantic_status in {"CONTENT_WEAK", "REVIEW_REQUIRED"}:
                reason = "CONTENT_WEAK"
            elif skill_status == "AMBIGUOUS":
                reason = "PRIMARY_SKILL_AMBIGUOUS"
                blockers.append(f"R4 ambiguity remains unresolved: {code}")
            else:
                reason = "EXACT_SKILL_MAPPING_MISSING"

        summary["supportingLessonCount"] += 1
        summary[terminal] += 1
        summary[reason] += 1
        rows.append({
            **lesson,
            "semanticContentStatus": semantic_status,
            "skillResolutionStatus": skill_status,
            "approvedPrimarySkills": primary_skills,
            "allowedQuestionFamilies": allowed_families,
            "declaredPracticeReadiness": declared_readiness or None,
            "terminalState": terminal,
            "terminalReason": reason,
            "familyErrors": family_errors,
        })

    summary["blockerCount"] = len(blockers)
    summary["terminalCoverageCount"] = len(rows)
    summary["terminalCoveragePercent"] = 100 if not rows else round(
        100 * sum(1 for row in rows if row["terminalState"] in {"READY_VERIFIED", "EXPLICITLY_BLOCKED"}) / len(rows),
        2,
    )

    return {
        "schemaVersion": 1,
        "audit": "Supporting Lesson exact Practice terminal-readiness audit",
        "authority": (
            "Every Supporting/PedagogicalUnmapped lesson must be terminal: READY_VERIFIED exact Practice or "
            "EXPLICITLY_BLOCKED with a machine-readable reason. Broad contextual fallback is never counted as exact Practice."
        ),
        "summary": dict(summary),
        "blockers": blockers,
        "lessons": rows,
    }


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--write-report", action="store_true")
    parser.add_argument("--strict", action="store_true")
    args = parser.parse_args()

    report = audit()
    if args.write_report:
        REPORT.parent.mkdir(parents=True, exist_ok=True)
        REPORT.write_text(json.dumps(report, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")

    print(json.dumps(report["summary"], ensure_ascii=False, indent=2))
    if report["blockers"]:
        print(json.dumps(report["blockers"], ensure_ascii=False, indent=2))
    return 2 if args.strict and report["blockers"] else 0


if __name__ == "__main__":
    raise SystemExit(main())
