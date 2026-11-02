#!/usr/bin/env python3
from __future__ import annotations

import argparse
import hashlib
import json
import re
from collections import Counter
from pathlib import Path

import lesson_generation_readiness_audit
import lesson_semantic_content_audit
import lesson_skill_resolution_audit

ROOT = Path(__file__).resolve().parents[2]
REPORT = ROOT / "artifacts/math-intelligence/supporting-practice-remediation-closure-audit.json"
AMBIGUITY = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/supporting-ambiguity-decisions.r4.v1.json"


def normalize_target(title: str) -> str:
    value = re.sub(
        r"\s*[:—-]\s*(Build the Idea|Reason and Apply|advanced reasoning)\s*$",
        "",
        title or "",
        flags=re.IGNORECASE,
    )
    value = re.sub(r"^\s*Consolidating\s+", "", value, flags=re.IGNORECASE)
    return re.sub(r"\s+", " ", value).strip()


def cluster_id(target: str) -> str:
    digest = hashlib.sha256(target.lower().encode("utf-8")).hexdigest()[:16]
    return f"supporting.target.{digest}"


def blocker_for(readiness: str, semantic: str, skill_status: str) -> str:
    if semantic == "CONTENT_WEAK":
        return "CONTENT_WEAK"
    if readiness == "SOLVER_CAPABILITY_MISSING":
        return "SOLVER_CAPABILITY_MISSING"
    if readiness == "QUESTION_FAMILY_MISSING":
        return "QUESTION_FAMILY_MISSING"
    if readiness == "REPRESENTATION_MISSING":
        return "REPRESENTATION_MISSING"
    if readiness == "SKILL_AMBIGUOUS" or skill_status == "AMBIGUOUS":
        return "MAPPING_CONFLICT"
    if readiness == "REQUIRES_ACADEMIC_REVIEW" or skill_status == "REVIEW_REQUIRED":
        return "ACADEMIC_REVIEW_REQUIRED"
    if skill_status == "ONTOLOGY_GAP":
        return "ACADEMIC_SKILL_PROMOTION_REQUIRED"
    return readiness or skill_status or "UNRESOLVED_CAPABILITY"


def audit() -> dict:
    skill_report = lesson_skill_resolution_audit.audit()
    semantic_report = lesson_semantic_content_audit.audit()
    readiness_report = lesson_generation_readiness_audit.audit()

    skill_by_code = {
        row["lessonCode"]: row
        for row in skill_report["lessons"]
        if row.get("sourceType") == "PedagogicalUnmapped"
    }
    semantic_by_code = {
        row["lessonCode"]: row
        for row in semantic_report["lessons"]
        if row.get("sourceType") == "PedagogicalUnmapped"
    }
    readiness_by_code = {
        row["lessonCode"]: row
        for row in readiness_report["lessons"]
        if row.get("sourceType") == "PedagogicalUnmapped"
    }

    ambiguity = {}
    if AMBIGUITY.exists():
        doc = json.loads(AMBIGUITY.read_text(encoding="utf-8-sig"))
        ambiguity = {
            row["lessonCode"]: row
            for row in doc.get("decisions", [])
            if isinstance(row, dict) and row.get("lessonCode")
        }

    rows = []
    blockers = []
    summary = Counter()
    clusters: dict[str, set[str]] = {}

    for code, skill_row in sorted(skill_by_code.items()):
        semantic_row = semantic_by_code.get(code, {})
        readiness_row = readiness_by_code.get(code, {})
        title = str(skill_row.get("title") or "")
        target = normalize_target(title)
        target_id = cluster_id(target)
        clusters.setdefault(target_id, set()).add(code)

        readiness = str(readiness_row.get("generationReadiness") or "")
        semantic = str(semantic_row.get("status") or "")
        skill_status = str(skill_row.get("status") or "")
        approved_mapping = bool(readiness_row.get("approvedMapping"))

        if readiness == "READY_VERIFIED" and approved_mapping:
            terminal = "READY_VERIFIED"
            blocker = None
        else:
            terminal = "EXPLICITLY_BLOCKED"
            blocker = blocker_for(readiness, semantic, skill_status)

        # R4 can explicitly block a lesson even when a broad audit would otherwise
        # appear permissive. Explicit human-reviewed blocking takes precedence.
        ambiguity_decision = ambiguity.get(code)
        if ambiguity_decision and ambiguity_decision.get("terminalState") == "EXPLICITLY_BLOCKED":
            terminal = "EXPLICITLY_BLOCKED"
            blocker = str(ambiguity_decision.get("blocker") or "MAPPING_CONFLICT")

        summary["supportingLessonCount"] += 1
        summary[terminal] += 1
        summary[f"blocker:{blocker}"] += 1 if blocker else 0
        if skill_status == "ONTOLOGY_GAP":
            summary["baselineOntologyGap"] += 1
        if semantic == "CONTENT_WEAK":
            summary["contentWeak"] += 1

        if not target:
            blockers.append(f"Supporting lesson has no canonical target text: {code}")
        if terminal == "READY_VERIFIED" and not approved_mapping:
            blockers.append(f"READY_VERIFIED Supporting lesson has no approved mapping: {code}")
        if terminal == "EXPLICITLY_BLOCKED" and not blocker:
            blockers.append(f"Blocked Supporting lesson has no explicit blocker: {code}")

        rows.append({
            "lessonCode": code,
            "title": title,
            "canonicalTarget": target,
            "catalogueTargetId": target_id,
            "skillResolutionStatus": skill_status,
            "semanticContentStatus": semantic,
            "generationReadiness": readiness,
            "approvedMapping": approved_mapping,
            "terminalState": terminal,
            "blocker": blocker,
        })

    summary["catalogueTargetClusterCount"] = len(clusters)
    summary["uncataloguedTargetCount"] = sum(1 for row in rows if not row["canonicalTarget"])
    summary["terminalStateCoveragePercent"] = (
        100 if not rows else round(100 * sum(1 for row in rows if row["terminalState"] in {"READY_VERIFIED", "EXPLICITLY_BLOCKED"}) / len(rows), 2)
    )
    summary["blockerCount"] = len(blockers)

    return {
        "schemaVersion": 1,
        "audit": "Supporting Lesson Practice remediation closure",
        "authority": (
            "Every Supporting lesson is assigned a stable catalogue target and a terminal Practice state. "
            "READY_VERIFIED requires approved exact mapping plus generation readiness. All other lessons are "
            "explicitly blocked with a machine-readable reason; no broad fallback is authorized by this report."
        ),
        "summary": dict(summary),
        "blockers": blockers,
        "targetClusters": [
            {
                "catalogueTargetId": target_id,
                "lessonCount": len(codes),
            }
            for target_id, codes in sorted(clusters.items())
        ],
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
    if args.strict and report["summary"]["blockerCount"]:
        return 2
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
