#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[2]
STAGE17 = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage17-grade1-6-production-manifest.v1.json"
STAGE18 = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage18-practice-migration-manifest.v1.json"
SERVICE = ROOT / "src/Edulytics.Services/Practice/StudentPrivatePracticeService.cs"
ENGINE = ROOT / "src/Edulytics.Services/Practice/Stage18SkillContractPracticeEngine.cs"
PRACTICE = ROOT / "src/Edulytics.Services/Practice/PracticeService.cs"
REPORT = ROOT / "artifacts/math-intelligence/stage18-practice-closure-audit.json"

EXPECTED_METHOD = "skill-contract-solver-verified-v1"
EXPECTED_SOLVER = "stage18-exact-practice-solver-v1"
EXPECTED_VERIFIER = "stage18-independent-practice-verifier-v1"


def read_json(path: Path) -> Any:
    return json.loads(path.read_text(encoding="utf-8-sig"))


def audit() -> dict[str, Any]:
    stage17 = read_json(STAGE17)
    stage18 = read_json(STAGE18)
    blockers: list[str] = []

    s17_entries = {
        str(row.get("lessonCode") or ""): row
        for row in stage17.get("entries") or []
        if isinstance(row, dict)
    }
    s18_entries = {
        str(row.get("lessonCode") or ""): row
        for row in stage18.get("entries") or []
        if isinstance(row, dict)
    }

    if len(s17_entries) != len(stage17.get("entries") or []):
        blockers.append("Stage 17 manifest contains duplicate or missing lessonCode values.")
    if len(s18_entries) != len(stage18.get("entries") or []):
        blockers.append("Stage 18 manifest contains duplicate or missing lessonCode values.")

    missing = sorted(set(s17_entries) - set(s18_entries))
    extras = sorted(set(s18_entries) - set(s17_entries))
    for code in missing:
        blockers.append(f"READY_VERIFIED Stage 17 lesson is missing from Stage 18 Practice: {code}")
    for code in extras:
        blockers.append(f"Stage 18 Practice contains a lesson outside the Stage 17 verified manifest: {code}")

    family_count = 0
    for code in sorted(set(s17_entries) & set(s18_entries)):
        before = s17_entries[code]
        after = s18_entries[code]

        if str(after.get("skillId") or "") != str(before.get("skillId") or ""):
            blockers.append(f"Stage 18 SkillContract differs from Stage 17 for {code}.")
        if str(after.get("mechanic") or "") != str(before.get("mechanic") or ""):
            blockers.append(f"Stage 18 mechanic differs from Stage 17 for {code}.")

        families = after.get("allowedQuestionFamilies") or []
        if not isinstance(families, list) or not families:
            blockers.append(f"Stage 18 lesson has no allowed question families: {code}")
            continue
        if any(not isinstance(value, str) or not value.strip() for value in families):
            blockers.append(f"Stage 18 lesson has an invalid allowed question family: {code}")
        if len(families) != len(set(families)):
            blockers.append(f"Stage 18 lesson has duplicate allowed question families: {code}")
        family_count += len(families)

    if stage18.get("generationMethod") != EXPECTED_METHOD:
        blockers.append("Stage 18 manifest generationMethod is not the solver-verified Practice method.")
    if stage18.get("solver") != EXPECTED_SOLVER:
        blockers.append("Stage 18 manifest solver key is not the accepted exact Practice solver.")
    if stage18.get("verifier") != EXPECTED_VERIFIER:
        blockers.append("Stage 18 manifest verifier key is not the independent Practice verifier.")

    service = SERVICE.read_text(encoding="utf-8")
    engine = ENGINE.read_text(encoding="utf-8")
    practice = PRACTICE.read_text(encoding="utf-8")

    exact_gate = service.find("Stage18PracticeSkillContracts.TryResolve")
    contextual_fallback = service.find("BuildPedagogicalContextProfiles")
    if exact_gate < 0:
        blockers.append("StudentPrivatePracticeService does not resolve Stage 18 SkillContracts.")
    if contextual_fallback < 0:
        blockers.append("StudentPrivatePracticeService contextual path was unexpectedly removed.")
    if exact_gate >= 0 and contextual_fallback >= 0 and exact_gate > contextual_fallback:
        blockers.append("Stage 18 exact SkillContract gate appears after the broad contextual fallback.")

    required_engine_tokens = [
        "Stage18SkillContractPracticeEngine",
        "Solve(problem)",
        "Verify(problem, answer)",
        "broadFallbackUsed = false",
        "Stage18PracticeSkillContracts.GenerationMethod",
    ]
    for token in required_engine_tokens:
        if token not in engine:
            blockers.append(f"Stage 18 exact Practice engine is missing required contract token: {token}")

    forbidden_engine_tokens = [
        "UniversalMathematicsQuestionGenerationEngine",
        "CurriculumContextCheck",
        "MathematicsAiCapabilityMatrix",
    ]
    for token in forbidden_engine_tokens:
        if token in engine:
            blockers.append(f"Stage 18 exact Practice engine references broad fallback capability: {token}")

    if "MathematicsAnswerEquivalence.AreEquivalent" not in practice:
        blockers.append("Practice scoring is not using shared Mathematics answer equivalence.")

    return {
        "schemaVersion": 1,
        "audit": "Stage 18 Practice migration closure",
        "status": "COMPLETE" if not blockers else "BLOCKED",
        "summary": {
            "stage17ReadyVerifiedLessonCount": len(s17_entries),
            "stage18PracticeLessonCount": len(s18_entries),
            "allowedQuestionFamilyAssignments": family_count,
            "missingPracticeLessons": len(missing),
            "extraPracticeLessons": len(extras),
            "blockerCount": len(blockers),
        },
        "blockers": blockers,
        "practiceLessonCodes": sorted(s18_entries),
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
