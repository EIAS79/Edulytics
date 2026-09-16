#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
from collections import Counter, defaultdict
from pathlib import Path
from typing import Any

from lesson_semantic_content_audit import audit as semantic_audit
from lesson_skill_resolution_audit import audit as skill_resolution_audit

ROOT = Path(__file__).resolve().parents[2]
SKILL_REGISTRY = ROOT / "src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json"
LESSON_MAPPINGS = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json"
REPORT = ROOT / "artifacts/math-intelligence/lesson-generation-readiness-audit.json"

READY = {"READY_VERIFIED", "READY_CONTEXTUAL"}


def read_json(path: Path) -> Any:
    return json.loads(path.read_text(encoding="utf-8-sig"))


def clean_list(value: Any) -> list[str]:
    if not isinstance(value, list):
        return []
    return [str(item).strip() for item in value if str(item).strip()]


def load_registry() -> dict[str, dict[str, Any]]:
    doc = read_json(SKILL_REGISTRY)
    result: dict[str, dict[str, Any]] = {}
    for row in doc.get("skills") or []:
        if not isinstance(row, dict):
            continue
        skill_id = str(row.get("id") or "").strip()
        if skill_id:
            result[skill_id] = row
    return result


def load_approved_mappings() -> dict[str, dict[str, Any]]:
    doc = read_json(LESSON_MAPPINGS)
    result: dict[str, dict[str, Any]] = {}
    for row in doc.get("mappings") or []:
        if not isinstance(row, dict):
            continue
        code = str(row.get("lessonCode") or "").strip()
        if code:
            result[code] = row
    return result


def capability_for_mapping(
    mapping: dict[str, Any] | None,
    registry: dict[str, dict[str, Any]],
) -> tuple[bool, bool, bool, list[str]]:
    if not mapping:
        return False, False, False, ["No approved lesson-skill mapping exists."]

    skills = clean_list(mapping.get("primarySkills"))
    if not skills:
        return False, False, False, ["Approved mapping has no primary skills."]

    missing = [skill for skill in skills if skill not in registry]
    if missing:
        return False, False, False, [
            "Mapped SkillIds are absent from the registry: " + ", ".join(sorted(missing))
        ]

    rows = [registry[skill] for skill in skills]
    has_verified = all(bool(row.get("legacyGameMechanic")) for row in rows)
    has_contextual = all(
        bool(row.get("legacyGameMechanic")) or bool(row.get("legacyCanonicalSkill"))
        for row in rows
    )
    has_question_family = has_contextual

    reasons: list[str] = []
    if has_verified:
        reasons.append("Every primary skill has an exact production game mechanic already verified against the lesson target.")
    elif has_contextual:
        reasons.append("Every primary skill is represented by an existing native/contextual generation capability.")
    else:
        reasons.append("At least one primary skill has no current generation capability binding.")

    return has_question_family, has_verified, has_contextual, reasons


def decide(
    skill_status: str,
    semantic_status: str,
    has_approved_mapping: bool,
    has_question_family: bool,
    has_verified: bool,
    has_contextual: bool,
) -> tuple[str, list[str]]:
    if semantic_status == "BLOCKED":
        return "BLOCKED", ["Semantic content audit blocked the lesson."]
    if skill_status == "CONFLICT" or semantic_status == "MAPPING_CONFLICT":
        return "MAPPING_CONFLICT", ["Mapping evidence contains a conflict."]
    if semantic_status == "CONTENT_WEAK":
        return "CONTENT_WEAK", ["Worked examples do not demonstrate the recognized mathematical target strongly enough."]
    if semantic_status in {"REVIEW_REQUIRED", "UNCLASSIFIED"}:
        return "REQUIRES_ACADEMIC_REVIEW", ["Semantic content evidence is not strong enough for generation readiness."]
    if skill_status == "AMBIGUOUS":
        return "SKILL_AMBIGUOUS", ["Multiple SkillIds remain plausible."]
    if skill_status == "REVIEW_REQUIRED":
        return "REQUIRES_ACADEMIC_REVIEW", ["Skill evidence requires review before promotion."]
    if skill_status == "HIGH_CONFIDENCE_CANDIDATE":
        return "REQUIRES_ACADEMIC_REVIEW", ["High-confidence candidate is not an approved mapping."]
    if skill_status in {"UNRESOLVED", "ONTOLOGY_GAP"}:
        return "SOLVER_CAPABILITY_MISSING", ["Current Skill/Capability ontology cannot represent the lesson precisely enough."]
    if not has_approved_mapping:
        return "REQUIRES_ACADEMIC_REVIEW", ["No approved lesson-skill profile exists."]
    if not has_question_family:
        return "QUESTION_FAMILY_MISSING", ["Approved skill has no eligible question family."]
    if has_verified:
        return "READY_VERIFIED", ["Approved mapping, semantic content, question family and verified capability are available."]
    if has_contextual:
        return "READY_CONTEXTUAL", ["Approved mapping and contextual generation capability are available."]
    return "SOLVER_CAPABILITY_MISSING", ["Approved skill lacks sufficient generation/solver capability."]


def audit() -> dict[str, Any]:
    skill_report = skill_resolution_audit()
    semantic_report = semantic_audit()
    registry = load_registry()
    mappings = load_approved_mappings()

    skill_by_code = {row["lessonCode"]: row for row in skill_report["lessons"]}
    semantic_by_code = {row["lessonCode"]: row for row in semantic_report["lessons"]}

    all_codes = sorted(set(skill_by_code) | set(semantic_by_code))
    blockers: list[str] = []
    if set(skill_by_code) != set(semantic_by_code):
        missing_skill = sorted(set(semantic_by_code) - set(skill_by_code))
        missing_semantic = sorted(set(skill_by_code) - set(semantic_by_code))
        if missing_skill:
            blockers.append(f"{len(missing_skill)} lessons are missing from skill resolution audit.")
        if missing_semantic:
            blockers.append(f"{len(missing_semantic)} lessons are missing from semantic content audit.")

    rows: list[dict[str, Any]] = []
    summary = Counter()
    by_pack: dict[str, Counter] = defaultdict(Counter)
    by_source: dict[str, Counter] = defaultdict(Counter)

    for code in all_codes:
        skill = skill_by_code.get(code)
        semantic = semantic_by_code.get(code)
        if not skill or not semantic:
            continue

        skill_status = str(skill.get("status") or "UNRESOLVED")
        semantic_status = str(semantic.get("status") or "UNCLASSIFIED")
        mapping = mappings.get(code)
        has_family, has_verified, has_contextual, capability_reasons = capability_for_mapping(mapping, registry)
        readiness, reasons = decide(
            skill_status,
            semantic_status,
            mapping is not None,
            has_family,
            has_verified,
            has_contextual,
        )

        pack_code = str(skill.get("packCode") or semantic.get("packCode") or "")
        source_type = str(skill.get("sourceType") or semantic.get("sourceType") or "Unknown")
        summary["lessonCount"] += 1
        summary[readiness] += 1
        if readiness in READY:
            summary["generationReadyCount"] += 1
        else:
            summary["generationBlockedCount"] += 1
        by_pack[pack_code]["lessonCount"] += 1
        by_pack[pack_code][readiness] += 1
        by_source[source_type]["lessonCount"] += 1
        by_source[source_type][readiness] += 1

        rows.append({
            "lessonCode": code,
            "packCode": pack_code,
            "sourceType": source_type,
            "title": semantic.get("title"),
            "skillResolutionStatus": skill_status,
            "semanticContentStatus": semantic_status,
            "approvedMapping": mapping is not None,
            "approvedPrimarySkills": [] if mapping is None else clean_list(mapping.get("primarySkills")),
            "hasQuestionFamily": has_family,
            "hasVerifiedSolverCapability": has_verified,
            "hasContextualGenerationCapability": has_contextual,
            "generationReadiness": readiness,
            "reasons": reasons + capability_reasons,
        })

    summary["skillRegistryCount"] = len(registry)
    summary["approvedLessonSkillMappingCount"] = len(mappings)
    summary["blockerCount"] = len(blockers)

    return {
        "schemaVersion": 1,
        "audit": "Edulytics Mathematics V2 generation readiness gate",
        "authority": (
            "Fail-closed shadow readiness only. This report does not alter legacy production routing. "
            "Only READY_VERIFIED or READY_CONTEXTUAL may be considered for a future feature-flagged V2 cutover."
        ),
        "summary": dict(summary),
        "blockers": blockers,
        "bySourceType": {key: dict(value) for key, value in sorted(by_source.items())},
        "byPack": {key: dict(value) for key, value in sorted(by_pack.items())},
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
