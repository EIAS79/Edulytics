#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
import re
from collections import Counter
from pathlib import Path
from typing import Any

from lesson_generation_readiness_audit import audit as readiness_audit

ROOT = Path(__file__).resolve().parents[2]
MANIFEST = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage17-grade1-6-production-manifest.v1.json"
SKILLS = ROOT / "src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json"
REPORT = ROOT / "artifacts/math-intelligence/stage17-grade1-6-closure-audit.json"

READY_VERIFIED = "READY_VERIFIED"


def read_json(path: Path) -> Any:
    return json.loads(path.read_text(encoding="utf-8-sig"))


def grade_from_code(code: str) -> int | None:
    # Stage 1-6, Grade 1-6 and Level 1-6 are the canonical primary encodings
    # currently used by Cambridge, Common Core, UAE and Polish curriculum packs.
    match = re.search(r":(?:S|G|L)([1-6]):", code, re.IGNORECASE)
    return int(match.group(1)) if match else None


def audit() -> dict[str, Any]:
    readiness = readiness_audit()
    manifest = read_json(MANIFEST)
    skill_doc = read_json(SKILLS)

    skills = {
        str(row.get("id") or "").strip(): row
        for row in skill_doc.get("skills") or []
        if isinstance(row, dict) and str(row.get("id") or "").strip()
    }

    rows = readiness.get("lessons") or []
    grade16_rows = [
        row for row in rows
        if grade_from_code(str(row.get("lessonCode") or "")) is not None
    ]
    by_code = {
        str(row.get("lessonCode") or ""): row
        for row in grade16_rows
    }

    blockers: list[str] = []
    entries = manifest.get("entries") or []
    manifest_by_code: dict[str, dict[str, Any]] = {}

    for entry in entries:
        if not isinstance(entry, dict):
            blockers.append("Stage 17 manifest contains a non-object entry.")
            continue
        code = str(entry.get("lessonCode") or "").strip()
        if not code:
            blockers.append("Stage 17 manifest contains an entry without lessonCode.")
            continue
        if code in manifest_by_code:
            blockers.append(f"Duplicate Stage 17 manifest lessonCode: {code}")
            continue
        manifest_by_code[code] = entry

        grade = grade_from_code(code)
        if grade is None:
            blockers.append(f"Manifest entry is outside Grade 1-6 code space: {code}")
            continue
        if int(entry.get("grade") or 0) != grade:
            blockers.append(f"Manifest grade does not match lesson code for {code}.")

        if bool(entry.get("usesV2ShadowSolver")):
            blockers.append(f"Stage 17 learner routing may not use a shadow-only solver: {code}")

        row = by_code.get(code)
        if row is None:
            blockers.append(f"Manifest entry is absent from the Grade 1-6 readiness audit: {code}")
            continue

        if str(row.get("generationReadiness") or "") != READY_VERIFIED:
            blockers.append(
                f"Manifest entry is not READY_VERIFIED: {code} "
                f"({row.get('generationReadiness')})"
            )
        if not bool(row.get("approvedMapping")):
            blockers.append(f"Manifest entry has no approved lesson-skill mapping: {code}")

        skill_id = str(entry.get("skillId") or "").strip()
        approved_skills = [str(x).strip() for x in row.get("approvedPrimarySkills") or []]
        if skill_id not in approved_skills:
            blockers.append(
                f"Manifest SkillId {skill_id!r} is not an approved primary skill for {code}."
            )

        skill = skills.get(skill_id)
        if skill is None:
            blockers.append(f"Manifest references unknown SkillId {skill_id!r}: {code}")
            continue
        mechanic = str(entry.get("mechanic") or "").strip()
        if str(skill.get("legacyGameMechanic") or "").strip() != mechanic:
            blockers.append(
                f"Manifest mechanic {mechanic!r} does not match the reviewed exact "
                f"legacyGameMechanic for {skill_id!r}: {code}"
            )

    ready_codes = {
        str(row.get("lessonCode") or "")
        for row in grade16_rows
        if str(row.get("generationReadiness") or "") == READY_VERIFIED
    }
    manifest_codes = set(manifest_by_code)

    for code in sorted(ready_codes - manifest_codes):
        blockers.append(f"READY_VERIFIED Grade 1-6 lesson is not production-routed: {code}")
    for code in sorted(manifest_codes - ready_codes):
        blockers.append(f"Production-routed lesson is not READY_VERIFIED: {code}")

    for row in grade16_rows:
        code = str(row.get("lessonCode") or "")
        if bool(row.get("approvedMapping")) and code not in manifest_codes:
            blockers.append(f"Approved Grade 1-6 mapping is missing from Stage 17 manifest: {code}")
        if code not in manifest_codes and not row.get("reasons"):
            blockers.append(f"Fail-closed Grade 1-6 lesson has no readiness reason: {code}")

    readiness_counts = Counter(
        str(row.get("generationReadiness") or "UNKNOWN")
        for row in grade16_rows
    )

    return {
        "schemaVersion": 1,
        "audit": "Stage 17 Grade 1-6 production closure",
        "status": "COMPLETE" if not blockers else "BLOCKED",
        "summary": {
            "grade16LessonCount": len(grade16_rows),
            "productionRoutedCount": len(manifest_codes),
            "failClosedCount": len(grade16_rows) - len(manifest_codes),
            "readyVerifiedCount": len(ready_codes),
            "readyVerifiedNotRoutedCount": len(ready_codes - manifest_codes),
            "routedNotReadyVerifiedCount": len(manifest_codes - ready_codes),
            "blockerCount": len(blockers),
            "readinessCounts": dict(sorted(readiness_counts.items())),
        },
        "blockers": blockers,
        "productionLessonCodes": sorted(manifest_codes),
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
    if args.strict and report["blockers"]:
        for blocker in report["blockers"]:
            print(f"BLOCKER: {blocker}")
        return 2
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
