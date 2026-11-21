#!/usr/bin/env python3
"""Fail-closed independent academic source-parity preflight for UAE Grade 9 General.

Only checks evidence actually recorded on the candidate branch. It never amends
source audit records, grants owner approvals, seeds curriculum or deploys.
"""
from __future__ import annotations

import argparse
import json
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
BASE = ROOT / "artifacts" / "curriculum-delivery" / "UAE-MOE-MATH" / "G9"
SUPPLEMENTS = ("01.pdf:7-1", "01.pdf:7-2", "01.pdf:7-3", "01.pdf:7-4")


def read(name: str) -> dict:
    with (BASE / name).open(encoding="utf-8") as handle:
        data = json.load(handle)
    if not isinstance(data, dict):
        raise ValueError(f"{name} must contain a JSON object")
    return data


def evaluate() -> dict:
    index = read("grade9-82-target-index.json")
    audit = read("semantic-audit-ledger.json")
    progress = read("full-grade-progress.json")
    raw = audit.get("rows", [])
    target = index.get("records", [])
    failures = []

    def require(condition: bool, detail: str) -> None:
        if not condition:
            failures.append(detail)

    require(index.get("expectedLessons") == 82, "canonical lesson expectation is not 82")
    require(index.get("expectedUnits") == 13, "canonical unit expectation is not 13")
    require(index.get("sourceRows") == 86, "raw source expectation is not 86")
    require(isinstance(raw, list) and len(raw) == 86, "raw audit ledger must contain 86 entries")
    require(isinstance(target, list) and len(target) == 82, "target index must contain 82 lessons")
    require(isinstance(raw, list) and len({str(x.get('rawOrdinal')) for x in raw if isinstance(x, dict)}) == len(raw), "raw source ordinals must be unique")
    require(isinstance(target, list) and len({str(x.get('ordinal')) for x in target if isinstance(x, dict)}) == len(target), "target lesson ordinals must be unique")

    raw_verified = [
        x for x in raw if isinstance(x, dict)
        and x.get("fullLessonAuditPassed") is True
        and type(x.get("elementCountDenominator")) is int
        and x["elementCountDenominator"] > 0
        and x.get("objectiveCoverageVerification") == "FULL_SOURCE_ATOMIC_LEDGER_VERIFIED"
    ]
    target_verified = [
        x for x in target if isinstance(x, dict)
        and x.get("ElementCoverage") == "FULL_SOURCE_ELEMENT_1_TO_1_VERIFIED"
    ]
    require(len(raw_verified) == 86, f"only {len(raw_verified)}/86 raw source lessons have independently verified atomic evidence")
    require(len(target_verified) == 82, f"only {len(target_verified)}/82 canonical lessons have source-element parity")

    for source in SUPPLEMENTS:
        receiving = [x for x in target if isinstance(x, dict) and source in x.get("SourceRefs", [])]
        require(len(receiving) == 1, f"supplement {source} must map to exactly one target")
        if len(receiving) == 1:
            require(receiving[0].get("ElementCoverage") == "FULL_SOURCE_ELEMENT_1_TO_1_VERIFIED",
                    f"supplement {source} receiving lesson is not parity verified")

    require(index.get("SOURCE_INDEX_COMPLETE") is True, "index SOURCE_INDEX_COMPLETE is not true")
    require(progress.get("targetIndexComplete") is True, "full grade targetIndexComplete is not true")
    require(progress.get("uniqueLessonsVerified") == 82, "uniqueLessonsVerified is not 82")

    return {
        "gate": "UAE_G9_GENERAL_SOURCE_ATOMIC_PARITY",
        "releaseDecision": "PASS" if not failures else "NO_GO",
        "expectedRawSourceLessons": 86,
        "verifiedRawSourceLessons": len(raw_verified),
        "expectedCanonicalLessons": 82,
        "verifiedCanonicalLessons": len(target_verified),
        "requiredSupplements": list(SUPPLEMENTS),
        "sourceIndexComplete": index.get("SOURCE_INDEX_COMPLETE") is True,
        "failures": failures,
        "disclaimer": "Content-file existence and authored analogous practice do not establish source parity.",
    }


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--json", action="store_true", help="emit machine-readable gate result")
    args = parser.parse_args()
    try:
        result = evaluate()
    except (ValueError, KeyError, TypeError, OSError, json.JSONDecodeError) as exc:
        result = {"gate": "UAE_G9_GENERAL_SOURCE_ATOMIC_PARITY",
                  "releaseDecision": "NO_GO", "failures": [f"invalid or missing gate evidence: {exc}"]}
    if args.json:
        print(json.dumps(result, ensure_ascii=False))
    else:
        print(f"Grade 9 source parity: {result['releaseDecision']}")
        print(f"Raw source verified: {result.get('verifiedRawSourceLessons', 0)}/86")
        print(f"Canonical parity verified: {result.get('verifiedCanonicalLessons', 0)}/82")
        for reason in result["failures"]:
            print(f"  - {reason}")
    return 0 if result["releaseDecision"] == "PASS" else 1


if __name__ == "__main__":
    sys.exit(main())
