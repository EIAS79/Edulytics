#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
import re
from collections import Counter, defaultdict
from pathlib import Path
from typing import Any

from lesson_semantic_content_audit import audit as semantic_audit
from lesson_skill_resolution_audit import audit as skill_resolution_audit

ROOT = Path(__file__).resolve().parents[2]
CONTENT_DIR = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs"
RULES_FILE = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/supporting-target-classification-rules.v1.json"
SKILL_REGISTRY = ROOT / "src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json"
CONTRACT_SOURCE = ROOT / "src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs"
REPORT = ROOT / "artifacts/math-intelligence/supporting-practice-remediation-audit.json"

TERMINAL_READY = "READY_VERIFIED"
TERMINAL_BLOCKED = "EXPLICITLY_BLOCKED"


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


def normalize_title(value: Any) -> str:
    text = re.sub(r"\s+", " ", str(value or "")).strip()
    text = re.sub(r"\s+—\s+advanced reasoning\s*$", "", text, flags=re.IGNORECASE)
    text = re.sub(r":\s*Build the Idea\s*$", "", text, flags=re.IGNORECASE)
    text = re.sub(r":\s*Reason and Apply\s*$", "", text, flags=re.IGNORECASE)
    text = re.sub(r"^Consolidating\s+", "", text, flags=re.IGNORECASE)
    return re.sub(r"\s+", " ", text).strip()


def slug(value: str) -> str:
    text = normalize_title(value).lower()
    text = text.replace("’", "").replace("'", "")
    text = re.sub(r"[^a-z0-9]+", "_", text)
    text = re.sub(r"_+", "_", text).strip("_")
    return (text[:70].rstrip("_") or "target")


def load_rules() -> dict[str, Any]:
    doc = read_json(RULES_FILE)
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
    vague = [
        re.compile(str(pattern), re.IGNORECASE)
        for pattern in doc.get("vagueTitlePatterns") or []
    ]
    domains = {
        str(domain): [str(term).lower() for term in terms]
        for domain, terms in (doc.get("domainKeywords") or {}).items()
        if isinstance(terms, list)
    }
    return {
        "reviewed": reviewed,
        "reusable": reusable,
        "blocked": blocked,
        "vague": vague,
        "domains": domains,
    }


def infer_domain(title: str, domains: dict[str, list[str]]) -> str:
    lowered = title.lower()
    for domain, terms in domains.items():
        if domain == "number":
            continue
        if any(term in lowered for term in terms):
            return domain
    return "number"


def load_runtime_contracts() -> set[str]:
    text = CONTRACT_SOURCE.read_text(encoding="utf-8")
    # LessonPracticeContracts.cs uses Ready("lesson-code", ...) entries. Keep
    # the parser deliberately narrow so malformed source fails closed.
    return set(re.findall(r'Ready\(\s*"([^"]+)"\s*,', text, flags=re.MULTILINE))


def load_skill_ids() -> set[str]:
    doc = read_json(SKILL_REGISTRY)
    return {
        str(row.get("id") or "").strip()
        for row in doc.get("skills") or []
        if isinstance(row, dict) and str(row.get("id") or "").strip()
    }


def classify_target(
    code: str,
    title: str,
    rules: dict[str, Any],
    skill_ids: set[str],
) -> dict[str, Any]:
    normalized = normalize_title(title)
    key = normalized.lower()

    if code in rules["blocked"]:
        return {
            "classification": "ExplicitlyBlocked",
            "normalizedTarget": normalized,
            "targetId": None,
            "skillId": None,
            "reviewStatus": "Blocked",
            "blockReason": rules["blocked"][code],
        }

    if any(pattern.search(normalized) for pattern in rules["vague"]):
        return {
            "classification": "ExplicitlyBlocked",
            "normalizedTarget": normalized,
            "targetId": None,
            "skillId": None,
            "reviewStatus": "Blocked",
            "blockReason": "TargetTooBroadForExactPractice",
        }

    if key in rules["reviewed"]:
        skill_id = rules["reviewed"][key]
        return {
            "classification": "Reviewed",
            "normalizedTarget": normalized,
            "targetId": skill_id,
            "skillId": skill_id,
            "reviewStatus": "Approved",
            "blockReason": None,
            "registeredSkill": skill_id in skill_ids,
        }

    if key in rules["reusable"]:
        skill_id = rules["reusable"][key]
        return {
            "classification": "CatalogueTarget",
            "normalizedTarget": normalized,
            "targetId": skill_id,
            "skillId": skill_id,
            "reviewStatus": "CatalogueTarget",
            "blockReason": None,
            "registeredSkill": skill_id in skill_ids,
        }

    domain = infer_domain(normalized, rules["domains"])
    target_id = f"{domain}.{slug(normalized)}"
    return {
        "classification": "CatalogueTarget",
        "normalizedTarget": normalized,
        "targetId": target_id,
        "skillId": target_id if target_id in skill_ids else None,
        "reviewStatus": "DeterministicCatalogueTarget",
        "blockReason": None,
        "registeredSkill": target_id in skill_ids,
    }


def audit() -> dict[str, Any]:
    rules = load_rules()
    skill_ids = load_skill_ids()
    runtime_contracts = load_runtime_contracts()
    semantic_report = semantic_audit()
    resolution_report = skill_resolution_audit()
    semantic_by_code = {row["lessonCode"]: row for row in semantic_report["lessons"]}
    resolution_by_code = {row["lessonCode"]: row for row in resolution_report["lessons"]}

    rows: list[dict[str, Any]] = []
    summary = Counter()
    by_pack: dict[str, Counter] = defaultdict(Counter)
    blockers: list[str] = []
    seen: set[str] = set()

    for path in sorted(CONTENT_DIR.glob("*.lesson-content-pack.json")):
        doc = read_json(path)
        pack_code = str(get_case(doc, "PackCode", "packCode", default="") or "").strip()
        lessons = get_case(doc, "Lessons", "lessons", default=[])
        if not isinstance(lessons, list):
            continue

        for lesson in lessons:
            if not isinstance(lesson, dict):
                continue
            code = str(get_case(lesson, "LessonCode", "lessonCode", default="") or "").strip()
            if not code or code in seen:
                continue
            seen.add(code)
            outcomes = clean_list(get_case(lesson, "OutcomeCodes", "outcomeCodes", default=[]))
            if outcomes:
                continue

            translation = choose_translation(lesson)
            title = str(
                get_case(translation, "Title", "title", default="")
                or get_case(lesson, "Title", "title", default="")
            ).strip()
            target = classify_target(code, title, rules, skill_ids)
            semantic = semantic_by_code.get(code) or {}
            resolution = resolution_by_code.get(code) or {}
            semantic_status = str(semantic.get("status") or "UNCLASSIFIED")
            original_resolution = str(resolution.get("status") or "UNRESOLVED")

            runtime_ready = code in runtime_contracts
            if runtime_ready:
                terminal = TERMINAL_READY
                reason = "Exact LessonPracticeContract with tested solver/verifier path."
            elif target["classification"] == "ExplicitlyBlocked":
                terminal = TERMINAL_BLOCKED
                reason = str(target.get("blockReason") or "Explicit block.")
            elif semantic_status in {"CONTENT_WEAK", "BLOCKED", "MAPPING_CONFLICT"}:
                terminal = TERMINAL_BLOCKED
                reason = semantic_status
            elif target["classification"] == "Reviewed":
                terminal = TERMINAL_BLOCKED
                reason = "QuestionFamilyOrSolverCapabilityMissing"
            else:
                terminal = TERMINAL_BLOCKED
                reason = "QuestionFamilyMissing"

            summary["supportingLessonCount"] += 1
            summary[terminal] += 1
            summary[target["classification"]] += 1
            summary[f"original_{original_resolution}"] += 1
            if target.get("targetId"):
                summary["ontologyClassifiedCount"] += 1
            if target.get("registeredSkill"):
                summary["registeredSkillTargetCount"] += 1
            if semantic_status == "CONTENT_WEAK":
                summary["contentWeakCount"] += 1
                if terminal == TERMINAL_BLOCKED:
                    summary["contentWeakExplicitlyBlockedCount"] += 1

            by_pack[pack_code]["supportingLessonCount"] += 1
            by_pack[pack_code][terminal] += 1

            rows.append({
                "lessonCode": code,
                "packCode": pack_code,
                "title": title,
                "normalizedTarget": target.get("normalizedTarget"),
                "targetId": target.get("targetId"),
                "skillId": target.get("skillId"),
                "targetClassification": target.get("classification"),
                "reviewStatus": target.get("reviewStatus"),
                "originalSkillResolutionStatus": original_resolution,
                "semanticContentStatus": semantic_status,
                "hasExactRuntimeContract": runtime_ready,
                "terminalStatus": terminal,
                "terminalReason": reason,
                "officialOutcomeMapped": False,
            })

    # The audited Supporting population must be fully terminal. No lesson is
    # allowed to disappear into an unclassified/implicit fallback state.
    unresolved = [
        row for row in rows
        if row["terminalStatus"] not in {TERMINAL_READY, TERMINAL_BLOCKED}
    ]
    missing_target = [
        row for row in rows
        if row["targetClassification"] != "ExplicitlyBlocked"
        and not row.get("targetId")
    ]
    if unresolved:
        blockers.append(f"{len(unresolved)} Supporting lessons have no terminal readiness state.")
    if missing_target:
        blockers.append(f"{len(missing_target)} non-blocked Supporting lessons have no target identity.")

    # R3/R4 closure: every lesson that was REVIEW_REQUIRED or AMBIGUOUS in the
    # baseline candidate audit must now be reviewed or explicitly blocked.
    review_open = [
        row for row in rows
        if row["originalSkillResolutionStatus"] in {"REVIEW_REQUIRED", "AMBIGUOUS"}
        and row["targetClassification"] not in {"Reviewed", "ExplicitlyBlocked"}
    ]
    if review_open:
        blockers.append(
            f"{len(review_open)} baseline review/ambiguity lessons still lack an explicit review decision."
        )

    summary["runtimeContractCount"] = len(runtime_contracts)
    summary["terminalCount"] = summary[TERMINAL_READY] + summary[TERMINAL_BLOCKED]
    summary["blockerCount"] = len(blockers)

    return {
        "schemaVersion": 1,
        "audit": "Edulytics Supporting Lesson Practice remediation terminal-state audit",
        "authority": (
            "Supporting-only execution gate. Catalogue target classification resolves ontology "
            "without granting production generation. Only exact runtime contracts are READY_VERIFIED; "
            "all other lessons are explicitly blocked with a reason and cannot silently fall back."
        ),
        "summary": dict(summary),
        "blockers": blockers,
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
