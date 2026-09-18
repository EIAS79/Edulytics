#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
import re
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
TARGET_RULES = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/supporting-target-classification-rules.v1.json"
REPORT = ROOT / "artifacts/math-intelligence/supporting-practice-readiness-audit.json"

READY = "READY_VERIFIED"
BLOCKED = "EXPLICITLY_BLOCKED"


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


def normalize_title(value: Any) -> str:
    text = re.sub(r"\s+", " ", str(value or "")).strip()
    text = re.sub(r"\s+—\s+advanced reasoning\s*$", "", text, flags=re.IGNORECASE)
    text = re.sub(r":\s*Build the Idea\s*$", "", text, flags=re.IGNORECASE)
    text = re.sub(r":\s*Reason and Apply\s*$", "", text, flags=re.IGNORECASE)
    text = re.sub(r"^Consolidating\s+", "", text, flags=re.IGNORECASE)
    return re.sub(r"\s+", " ", text).strip()


def slug(value: str) -> str:
    text = normalize_title(value).lower().replace("’", "").replace("'", "")
    text = re.sub(r"[^a-z0-9]+", "_", text)
    return re.sub(r"_+", "_", text).strip("_")[:90] or "target"


def choose_translation(lesson: dict[str, Any]) -> dict[str, Any]:
    translations = get_case(lesson, "Translations", "translations", default=[])
    if not isinstance(translations, list):
        return {}
    rows = [row for row in translations if isinstance(row, dict)]
    for row in rows:
        culture = str(get_case(row, "CultureCode", "cultureCode", default="") or "").lower()
        if culture.startswith("en"):
            return row
    return rows[0] if rows else {}


def load_target_rules() -> dict[str, Any]:
    doc = read_json(TARGET_RULES)
    reviewed = {
        str(row.get("normalizedTitle") or "").strip().lower(): str(row.get("skillId") or "").strip()
        for row in doc.get("reviewedTitleMappings") or []
        if isinstance(row, dict)
    }
    reusable = {
        str(row.get("normalizedTitle") or "").strip().lower(): str(row.get("skillId") or "").strip()
        for row in doc.get("reusableTitleMappings") or []
        if isinstance(row, dict)
    }
    blocked = {
        str(row.get("lessonCode") or "").strip(): str(row.get("reason") or "ExplicitBlock").strip()
        for row in doc.get("blockedLessonCodes") or []
        if isinstance(row, dict)
    }
    vague = [re.compile(str(pattern), re.IGNORECASE) for pattern in doc.get("vagueTitlePatterns") or []]
    domains = {
        str(domain): [str(term).lower() for term in terms]
        for domain, terms in (doc.get("domainKeywords") or {}).items()
        if isinstance(terms, list)
    }
    return {"reviewed": reviewed, "reusable": reusable, "blocked": blocked, "vague": vague, "domains": domains}


def infer_domain(title: str, domains: dict[str, list[str]]) -> str:
    lowered = title.lower()
    for domain, terms in domains.items():
        if domain == "number":
            continue
        if any(term in lowered for term in terms):
            return domain
    return "number"


def classify_target(code: str, title: str, rules: dict[str, Any]) -> dict[str, Any]:
    normalized = normalize_title(title)
    key = normalized.lower()
    if code in rules["blocked"]:
        return {"classification": "ExplicitlyBlocked", "targetId": None, "normalizedTarget": normalized,
                "blockReason": rules["blocked"][code]}
    if any(pattern.search(normalized) for pattern in rules["vague"]):
        return {"classification": "ExplicitlyBlocked", "targetId": None, "normalizedTarget": normalized,
                "blockReason": "TargetTooBroadForExactPractice"}
    if key in rules["reviewed"]:
        return {"classification": "Reviewed", "targetId": rules["reviewed"][key],
                "normalizedTarget": normalized, "blockReason": None}
    if key in rules["reusable"]:
        return {"classification": "CatalogueTarget", "targetId": rules["reusable"][key],
                "normalizedTarget": normalized, "blockReason": None}
    domain = infer_domain(normalized, rules["domains"])
    return {"classification": "CatalogueTarget", "targetId": f"{domain}.{slug(normalized)}",
            "normalizedTarget": normalized, "blockReason": None}


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
            translation = choose_translation(lesson)
            title = str(
                get_case(translation, "Title", "title", default="")
                or get_case(lesson, "Title", "title", default="")
            ).strip()
            result[code] = {"lessonCode": code, "packCode": pack_code, "title": title}
    return result


def audit() -> dict[str, Any]:
    support = supporting_lessons()
    mapping_doc = read_json(MAPPINGS)
    family_doc = read_json(FAMILIES)
    capability_doc = read_json(CAPABILITIES)
    rules = load_target_rules()
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
        str(row.get("lessonCode") or ""): row for row in semantic.get("lessons") or [] if isinstance(row, dict)
    }
    skill_by_code = {
        str(row.get("lessonCode") or ""): row for row in skill.get("lessons") or [] if isinstance(row, dict)
    }

    rows: list[dict[str, Any]] = []
    blockers: list[str] = []
    summary = Counter()
    unique_targets: set[str] = set()

    for code, lesson in sorted(support.items()):
        mapping = mappings.get(code)
        target = classify_target(code, lesson["title"], rules)
        if target.get("targetId"):
            unique_targets.add(str(target["targetId"]))

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
            if primary_skills and str(family.get("skillId") or "") not in primary_skills:
                family_errors.append(
                    f"Family {family_id} targets {family.get('skillId')} instead of approved Primary skill"
                )
            for capability_id in clean_list(family.get("requiredCapabilities")):
                capability = capabilities.get(capability_id)
                if capability is None:
                    family_errors.append(f"Family {family_id} requires unknown capability {capability_id}")
                    continue
                if str(capability.get("status") or "") not in {"ShadowVerified", "ProductionVerified", "Verified"}:
                    family_errors.append(f"Family {family_id} capability {capability_id} is not verified")

        if declared_readiness == READY:
            if semantic_status in {"CONTENT_WEAK", "REVIEW_REQUIRED"}:
                terminal, reason = BLOCKED, "CONTENT_WEAK"
                blockers.append(
                    f"READY_VERIFIED Practice contract has insufficient semantic-content evidence: {code} ({semantic_status})"
                )
            elif len(primary_skills) != 1:
                terminal, reason = BLOCKED, "PRIMARY_SKILL_AMBIGUOUS"
                blockers.append(f"READY_VERIFIED Practice contract must have exactly one Primary skill: {code}")
            elif not allowed_families:
                terminal, reason = BLOCKED, "QUESTION_FAMILY_MISSING"
                blockers.append(f"READY_VERIFIED Practice contract has no allowed families: {code}")
            elif family_errors:
                terminal, reason = BLOCKED, "CAPABILITY_INVALID"
                blockers.extend(f"{code}: {error}" for error in family_errors)
            else:
                terminal, reason = READY, "EXACT_CONTRACT_VERIFIED"
        else:
            terminal = BLOCKED
            if target["classification"] == "ExplicitlyBlocked":
                reason = str(target["blockReason"] or "TARGET_EXPLICITLY_BLOCKED")
            elif semantic_status in {"CONTENT_WEAK", "REVIEW_REQUIRED"}:
                reason = "CONTENT_WEAK"
            elif skill_status == "AMBIGUOUS":
                reason = "PRIMARY_SKILL_AMBIGUOUS"
                if mapping is None:
                    blockers.append(f"R4 ambiguity remains unresolved: {code}")
            elif mapping is None:
                # R5 closes the former ONTOLOGY_GAP by assigning a deterministic
                # reusable catalogue target identity. It does NOT authorize
                # generation. The next blocker is exact mapping/family/capability.
                reason = "EXACT_SKILL_MAPPING_OR_FAMILY_MISSING"
            elif len(primary_skills) != 1:
                reason = "PRIMARY_SKILL_AMBIGUOUS"
            elif not allowed_families:
                reason = "QUESTION_FAMILY_MISSING"
            elif family_errors:
                reason = "SOLVER_OR_VERIFIER_CAPABILITY_MISSING"
            else:
                reason = "PRACTICE_CONTRACT_NOT_APPROVED"

        summary["supportingLessonCount"] += 1
        summary[terminal] += 1
        summary[reason] += 1
        summary[target["classification"]] += 1
        if target.get("targetId"):
            summary["ontologyClassifiedCount"] += 1
        if semantic_status == "CONTENT_WEAK":
            summary["contentWeakCount"] += 1

        rows.append({
            **lesson,
            "normalizedTarget": target["normalizedTarget"],
            "catalogueTargetId": target.get("targetId"),
            "targetClassification": target["classification"],
            "semanticContentStatus": semantic_status,
            "skillResolutionStatus": skill_status,
            "approvedPrimarySkills": primary_skills,
            "allowedQuestionFamilies": allowed_families,
            "declaredPracticeReadiness": declared_readiness or None,
            "terminalState": terminal,
            "terminalReason": reason,
            "familyErrors": family_errors,
        })

    non_terminal = [
        row for row in rows if row["terminalState"] not in {READY, BLOCKED}
    ]
    missing_target = [
        row for row in rows
        if row["targetClassification"] != "ExplicitlyBlocked" and not row.get("catalogueTargetId")
    ]
    if non_terminal:
        blockers.append(f"{len(non_terminal)} Supporting lessons have no terminal readiness state.")
    if missing_target:
        blockers.append(f"{len(missing_target)} non-blocked Supporting lessons have no catalogue target identity.")

    summary["uniqueCatalogueTargetCount"] = len(unique_targets)
    summary["terminalCoverageCount"] = len(rows) - len(non_terminal)
    summary["terminalCoveragePercent"] = 100 if not rows else round(
        100 * (len(rows) - len(non_terminal)) / len(rows), 2
    )
    summary["blockerCount"] = len(blockers)

    return {
        "schemaVersion": 2,
        "audit": "Supporting Lesson exact Practice terminal-readiness and ontology audit",
        "authority": (
            "Every Supporting/PedagogicalUnmapped lesson must have a deterministic catalogue target or explicit target block, "
            "and a terminal Practice state. Catalogue classification never authorizes Practice. READY_VERIFIED requires an "
            "approved exact mapping, allowed family, verified capability, sufficient content, solver and independent verifier."
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
