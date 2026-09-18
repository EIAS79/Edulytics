#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
import re
from collections import Counter
from pathlib import Path
from typing import Any

from lesson_semantic_content_audit import audit as semantic_audit

ROOT = Path(__file__).resolve().parents[2]
PRACTICE_CONTRACTS = ROOT / "src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs"
REPORT = ROOT / "artifacts/math-intelligence/supporting-content-quality-gate.json"
REPAIRED = {
    "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:BUILD",
}


def practice_codes() -> set[str]:
    text = PRACTICE_CONTRACTS.read_text(encoding="utf-8")
    return set(re.findall(r'"(PED:[A-Z0-9][^"]+)"', text))


def audit() -> dict[str, Any]:
    semantic = semantic_audit()
    practice = practice_codes()
    rows: list[dict[str, Any]] = []
    blockers: list[str] = []
    summary = Counter()

    by_code = {
        str(row.get("lessonCode") or ""): row
        for row in semantic.get("lessons") or []
        if isinstance(row, dict)
    }

    for code in sorted(REPAIRED):
        row = by_code.get(code)
        if row is None:
            blockers.append(f"R6 repaired lesson is absent from semantic audit: {code}")
            continue
        status = str(row.get("status") or "")
        if status == "CONTENT_WEAK":
            blockers.append(f"R6 repaired lesson is still CONTENT_WEAK: {code}")
        if code not in practice:
            blockers.append(f"R6 repaired lesson has no exact Practice contract: {code}")

        rows.append({
            "lessonCode": code,
            "state": "REPAIRED" if status != "CONTENT_WEAK" else "REPAIR_FAILED",
            "semanticContentStatus": status,
            "terminalPracticeState": "READY_VERIFIED" if code in practice and status != "CONTENT_WEAK" else "EXPLICITLY_BLOCKED",
            "reason": (
                "Target-specific source-backed worked examples and verification steps were added."
                if status != "CONTENT_WEAK"
                else "Content remains too weak after attempted repair."
            ),
        })
        summary["repairedCount"] += 1

    for row in semantic.get("lessons") or []:
        if not isinstance(row, dict):
            continue
        if str(row.get("sourceType") or "") != "PedagogicalUnmapped":
            continue
        if str(row.get("status") or "") != "CONTENT_WEAK":
            continue

        code = str(row.get("lessonCode") or "")
        if code in REPAIRED:
            continue

        if code in practice:
            blockers.append(
                f"CONTENT_WEAK Supporting lesson is incorrectly Practice-enabled: {code}"
            )

        rows.append({
            "lessonCode": code,
            "state": "CONTENT_WEAK",
            "semanticContentStatus": "CONTENT_WEAK",
            "terminalPracticeState": "EXPLICITLY_BLOCKED",
            "reason": (
                "Current target-bearing worked examples are not strong enough to authorize "
                "lesson-scoped exact Practice. Strengthen from the trusted source and exact "
                "target before enabling a Practice contract."
            ),
        })
        summary["explicitlyBlockedContentWeakCount"] += 1

    summary["decisionCount"] = len(rows)
    summary["blockerCount"] = len(blockers)

    return {
        "schemaVersion": 1,
        "audit": "Supporting lesson content-quality terminal gate",
        "authority": (
            "CONTENT_WEAK Supporting lessons fail closed. A weak lesson may not have an exact "
            "Practice contract. Repair requires target-specific source-backed evidence; generic "
            "template expansion is not accepted."
        ),
        "baselineSupportingContentWeakCount": 335,
        "summary": dict(summary),
        "blockers": blockers,
        "decisions": rows,
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
    if args.strict and report["blockers"]:
        for blocker in report["blockers"]:
            print(f"BLOCKER: {blocker}")
        return 2
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
