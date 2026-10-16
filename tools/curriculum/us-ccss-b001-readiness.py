#!/usr/bin/env python3
"""Scope the existing Math Intelligence readiness audit to all 500 US-CCSS B001 lessons.

This is a static, fail-closed capability triage report, NOT acceptance of any
source, worked example, generated item, runtime Practice interaction, or lesson.
"""
from __future__ import annotations

import argparse
import json
import sys
from collections import Counter
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
sys.path.insert(0, str(ROOT / "tools" / "math_intelligence"))
from lesson_generation_readiness_audit import audit as global_readiness_audit  # noqa: E402

CATALOGUE = ROOT / "docs/curriculum/batches/US-CCSS-B001-CATALOGUE.json"
OUTPUT = ROOT / "artifacts/curriculum-delivery/US-CCSS-MATH/B001/skill-capability-triage.json"
WARNING = (
    "STATIC_PRELIMINARY_TRIAGE_ONLY: registry family readiness does not prove "
    "runtime grading, independent mathematical verification of generated questions, "
    "student-visible correctness, licensing or production release."
)


def build_report() -> dict:
    catalogue = json.loads(CATALOGUE.read_text(encoding="utf-8"))
    assert catalogue["total"] == 500 and len(catalogue["lessons"]) == 500
    assert catalogue["mathematicalGenerationVerification"] == "NOT_RUN"
    expected = [row["lessonCode"] for row in catalogue["lessons"]]
    assert len(set(expected)) == 500, "Duplicate B001 IDs"

    broad = global_readiness_audit()
    by_code: dict[str, dict] = {}
    for row in broad["lessons"]:
        code = row["lessonCode"]
        if code in by_code:
            raise ValueError(f"Duplicate global audit lessonCode {code}")
        by_code[code] = row

    missing = sorted(set(expected) - set(by_code))
    if missing:
        raise ValueError(f"{len(missing)} B001 lessons missing from mathematics registry audit; example: {missing[:10]}")

    statuses: Counter[str] = Counter()
    skill_statuses: Counter[str] = Counter()
    semantic_statuses: Counter[str] = Counter()
    rows = []
    for item in catalogue["lessons"]:
        code = item["lessonCode"]
        row = by_code[code]
        status = str(row.get("generationReadiness") or "NO_READINESS_STATUS")
        skill_status = str(row.get("skillResolutionStatus") or "UNKNOWN")
        semantic_status = str(row.get("semanticContentStatus") or "UNKNOWN")
        statuses[status] += 1
        skill_statuses[skill_status] += 1
        semantic_statuses[semantic_status] += 1
        rows.append({
            "batchOrdinal": item["batchOrdinal"],
            "gradeOrCourse": item["gradeOrCourse"],
            "gradeOrdinal": item["gradeOrdinal"],
            "lessonCode": code,
            "title": item["title"],
            "generationReadiness": status,
            "skillResolutionStatus": skill_status,
            "semanticContentStatus": semantic_status,
            "reviewedOfficialMapping": bool(row.get("reviewedOfficialMapping")),
            "approvedMapping": bool(row.get("approvedMapping")),
            "approvedPrimarySkills": row.get("approvedPrimarySkills") or [],
            "approvedQuestionFamilies": row.get("approvedQuestionFamilies") or [],
            "hasQuestionFamily": bool(row.get("hasQuestionFamily")),
            "hasVerifiedSolverCapability": bool(row.get("hasVerifiedSolverCapability")),
            "hasContextualGenerationCapability": bool(row.get("hasContextualGenerationCapability")),
            "isV2ShadowVerified": bool(row.get("isV2ShadowVerified")),
            "reasons": row.get("reasons") or [],
            "actualPracticeQuestionTests": "NOT_RUN",
            "independentCorrectAndWrongAnswerTests": "NOT_RUN",
            "runtimeStudentIntegration": "NOT_RUN",
            "academicApproval": "NOT_GRANTED_IN_THIS_AUDIT"
        })

    assert len(rows) == 500
    return {
        "schemaVersion": 1,
        "reportType": "B001_PRELIMINARY_MATH_SKILL_CAPABILITY_TRIAGE",
        "batchId": "US-CCSS-B001",
        "cataloguePath": "docs/curriculum/batches/US-CCSS-B001-CATALOGUE.json",
        "globalAuditName": broad.get("audit"),
        "globalAuditBlockers": broad.get("blockers", []),
        "scopeCount": len(rows),
        "limitations": WARNING,
        "summary": {
            "lessonsAnalysed": len(rows),
            "readiness": dict(sorted(statuses.items())),
            "skillResolution": dict(sorted(skill_statuses.items())),
            "semanticContent": dict(sorted(semantic_statuses.items())),
            "actualPracticeQuestionsVerified": 0,
            "newLessonsAcademicallyApproved": 0,
            "newLessonsDeployed": 0,
        },
        "lessons": rows,
    }


def main() -> int:
    p = argparse.ArgumentParser()
    p.add_argument("--write-report", action="store_true")
    p.add_argument("--check", action="store_true",
                   help="Run audit and enforce coverage without writing a report")
    args = p.parse_args()
    result = build_report()
    if args.write_report:
        OUTPUT.parent.mkdir(parents=True, exist_ok=True)
        OUTPUT.write_text(json.dumps(result, ensure_ascii=False, indent=2) + "\n",
                          encoding="utf-8")
    print(json.dumps({
        "batchId": result["batchId"],
        "warning": WARNING,
        "summary": result["summary"],
        "globalAuditBlockerCount": len(result["globalAuditBlockers"]),
        "artifact": str(OUTPUT.relative_to(ROOT)) if args.write_report else None,
    }, ensure_ascii=False, indent=2))
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
