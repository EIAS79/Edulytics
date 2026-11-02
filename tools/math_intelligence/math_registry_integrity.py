#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
from collections import Counter
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[2]
SKILLS = ROOT / "src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json"
CAPABILITIES = ROOT / "src/Edulytics.Core/Mathematics/Skills/capability-registry.v1.json"
FAMILIES = ROOT / "src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json"
REPORT = ROOT / "artifacts/math-intelligence/math-registry-integrity.json"


def read_json(path: Path) -> Any:
    return json.loads(path.read_text(encoding="utf-8-sig"))


def ids(rows: Any, key: str, errors: list[str], label: str) -> set[str]:
    result: set[str] = set()
    if not isinstance(rows, list):
        errors.append(f"{label} must be a list.")
        return result
    for row in rows:
        if not isinstance(row, dict):
            errors.append(f"{label} contains a non-object row.")
            continue
        value = str(row.get(key) or "").strip()
        if not value:
            errors.append(f"{label} row is missing {key}.")
            continue
        if value in result:
            errors.append(f"Duplicate {label} id: {value}")
        result.add(value)
    return result


def audit() -> dict[str, Any]:
    skill_doc = read_json(SKILLS)
    capability_doc = read_json(CAPABILITIES)
    family_doc = read_json(FAMILIES)
    errors: list[str] = []

    skill_rows = skill_doc.get("skills") or []
    capability_rows = capability_doc.get("capabilities") or []
    family_rows = family_doc.get("families") or []

    skill_ids = ids(skill_rows, "id", errors, "skill")
    capability_ids = ids(capability_rows, "id", errors, "capability")
    family_ids = ids(family_rows, "id", errors, "question family")

    for row in family_rows:
        if not isinstance(row, dict):
            continue
        family_id = str(row.get("id") or "").strip()
        skill_id = str(row.get("skillId") or "").strip()
        if skill_id not in skill_ids:
            errors.append(f"Question family {family_id} references unknown SkillId {skill_id!r}.")
        for capability_id in row.get("requiredCapabilities") or []:
            capability_id = str(capability_id).strip()
            if capability_id not in capability_ids:
                errors.append(
                    f"Question family {family_id} references unknown CapabilityId {capability_id!r}."
                )
        if row.get("status") == "ShadowVerified" and bool(row.get("productionRouting")):
            errors.append(f"Shadow-verified family {family_id} must not enable productionRouting.")

    for row in skill_rows:
        if not isinstance(row, dict):
            continue
        skill_id = str(row.get("id") or "").strip()
        for capability_id in row.get("v2Capabilities") or []:
            capability_id = str(capability_id).strip()
            if capability_id not in capability_ids:
                errors.append(
                    f"Skill {skill_id} references unknown V2 CapabilityId {capability_id!r}."
                )
        for family_id in row.get("v2QuestionFamilies") or []:
            family_id = str(family_id).strip()
            if family_id not in family_ids:
                errors.append(
                    f"Skill {skill_id} references unknown V2 question family {family_id!r}."
                )

    status_counts = Counter(
        str(row.get("status") or "Unspecified")
        for row in family_rows
        if isinstance(row, dict)
    )
    return {
        "schemaVersion": 1,
        "audit": "Edulytics Mathematics V2 registry integrity",
        "summary": {
            "skillCount": len(skill_ids),
            "capabilityCount": len(capability_ids),
            "questionFamilyCount": len(family_ids),
            "questionFamilyStatusCounts": dict(status_counts),
            "blockerCount": len(errors),
        },
        "blockers": errors,
    }


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--write-report", action="store_true")
    parser.add_argument("--strict", action="store_true")
    args = parser.parse_args()

    report = audit()
    if args.write_report:
        REPORT.parent.mkdir(parents=True, exist_ok=True)
        REPORT.write_text(json.dumps(report, indent=2) + "\n", encoding="utf-8")

    print(json.dumps(report["summary"], indent=2))
    if args.strict and report["summary"]["blockerCount"]:
        return 2
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
