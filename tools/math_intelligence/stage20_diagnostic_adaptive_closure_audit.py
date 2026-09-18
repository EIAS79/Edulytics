#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[2]
STAGE19 = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage19-assessment-builder-migration-manifest.v1.json"
STAGE20 = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage20-diagnostic-adaptive-migration-manifest.v1.json"
CONTRACTS = ROOT / "src/Edulytics.Core/AdaptiveAssessment/AdaptiveAssessmentContracts.cs"
ENGINE = ROOT / "src/Edulytics.Services/AdaptiveAssessment/AdaptiveDiagnosticAssessmentEngine.cs"
DIFFICULTY = ROOT / "src/Edulytics.Services/Mathematics/Difficulty/MathematicsDifficultyEngine.cs"
REGISTRATION = ROOT / "src/Edulytics.Web/Extensions/AdaptiveAssessmentRegistrationExtensions.cs"
TESTS = ROOT / "tests/Edulytics.Tests/MathematicsIntelligence/Stage20DiagnosticAdaptiveMigrationTests.cs"
REPORT = ROOT / "artifacts/math-intelligence/stage20-diagnostic-adaptive-closure-audit.json"

EXPECTED_SIGNALS = {
    "skillMastery",
    "prerequisiteMastery",
    "mathematicalComplexity",
    "misconceptionHistory",
    "representationFluency",
}
EXPECTED_ENGINE_VERSION = "stage20-math-aware-v1"


def read_json(path: Path) -> Any:
    return json.loads(path.read_text(encoding="utf-8-sig"))


def audit() -> dict[str, Any]:
    stage19 = read_json(STAGE19)
    stage20 = read_json(STAGE20)
    blockers: list[str] = []

    if stage20.get("status") != "COMPLETE":
        blockers.append("Stage 20 manifest is not marked COMPLETE.")

    if stage20.get("engineVersion") != EXPECTED_ENGINE_VERSION:
        blockers.append("Stage 20 engineVersion does not match the accepted math-aware version.")

    signals = set(str(x) for x in stage20.get("requiredSignals") or [])
    if signals != EXPECTED_SIGNALS:
        blockers.append(
            f"Stage 20 required signals differ from the governing five-signal contract: {sorted(signals)}"
        )

    stage19_entries = {
        str(row.get("outcomeCode") or ""): row
        for row in stage19.get("entries") or []
        if isinstance(row, dict)
    }
    stage20_entries = {
        str(row.get("outcomeCode") or ""): row
        for row in stage20.get("exactOutcomes") or []
        if isinstance(row, dict)
    }

    if len(stage20_entries) != len(stage20.get("exactOutcomes") or []):
        blockers.append("Stage 20 exactOutcomes contains duplicate or missing outcomeCode values.")

    missing = sorted(set(stage19_entries) - set(stage20_entries))
    extras = sorted(set(stage20_entries) - set(stage19_entries))
    for code in missing:
        blockers.append(f"Stage 19 exact outcome missing from Stage 20 adaptive migration: {code}")
    for code in extras:
        blockers.append(f"Stage 20 contains an exact outcome not approved in Stage 19: {code}")

    for code in sorted(set(stage19_entries) & set(stage20_entries)):
        before = str(stage19_entries[code].get("skillId") or "")
        after = str(stage20_entries[code].get("skillId") or "")
        if before != after:
            blockers.append(f"Stage 20 SkillContract drift for {code}: {before} -> {after}")

    prerequisite_policy = str(stage20.get("prerequisitePolicy") or "")
    if "does not invent prerequisite SkillIds" not in prerequisite_policy:
        blockers.append("Stage 20 prerequisite policy does not explicitly prohibit invented prerequisite SkillIds.")

    fail_closed = set(str(x) for x in stage20.get("failClosed") or [])
    required_fail_closed = {
        "exactOutcomeMissingMathematicsState",
        "mixedExactAndNonExactOutcomeScope",
        "outcomeSkillContractMismatch",
        "masteryProfileMismatch",
        "invalidPrerequisiteMastery",
        "disallowedRepresentationQuestionFamily",
        "disallowedMisconceptionQuestionFamily",
        "invalidComplexityScore",
    }
    if not required_fail_closed.issubset(fail_closed):
        blockers.append("Stage 20 manifest is missing one or more required fail-closed conditions.")

    contracts = CONTRACTS.read_text(encoding="utf-8")
    engine = ENGINE.read_text(encoding="utf-8")
    difficulty = DIFFICULTY.read_text(encoding="utf-8")
    registration = REGISTRATION.read_text(encoding="utf-8")
    tests = TESTS.read_text(encoding="utf-8")

    required_contract_tokens = [
        "AdaptiveMathematicsSkillState",
        "SkillMastery",
        "PrerequisiteMastery",
        "CurrentComplexityScore",
        "MisconceptionHistory",
        "RepresentationFluency",
        "TargetComplexityScore",
        "TargetQuestionFamily",
        "TargetRepresentation",
        "MisconceptionFocusId",
        "MathematicsAware",
    ]
    for token in required_contract_tokens:
        if token not in contracts:
            blockers.append(f"Stage 20 adaptive contracts are missing token: {token}")

    required_engine_tokens = [
        'MathematicsEngineVersion = "stage20-math-aware-v1"',
        "Stage19AssessmentSkillContracts.TryResolve",
        "MathematicsDifficultyEngine",
        "state.SkillMastery",
        "state.PrerequisiteMastery",
        "state.CurrentComplexityScore",
        "state.MisconceptionHistory",
        "state.RepresentationFluency",
        "mixed",
        "complete mathematics-aware adaptive state",
        "TargetComplexityScore: targetComplexity",
        "TargetQuestionFamily: questionFamily",
        "TargetRepresentation: weakestRepresentation?.Representation",
        "MisconceptionFocusId: misconception?.MisconceptionId",
    ]
    for token in required_engine_tokens:
        if token not in engine:
            blockers.append(f"Stage 20 adaptive engine is missing required token: {token}")

    if engine.find("Stage19AssessmentSkillContracts.TryResolve") > engine.find("return DecideLegacy"):
        blockers.append("Stage 20 exact Outcome routing does not precede the legacy adaptive path.")

    if "Recommend(new MathematicsAdaptiveLearnerState" not in engine:
        blockers.append("Stage 20 engine does not consume the shared MathematicsDifficultyEngine adaptive recommendation.")

    difficulty_tokens = [
        "SkillMastery",
        "PrerequisiteMastery",
        "RepresentationFluency",
        "RecentMisconceptionCount",
        "TargetComplexityScore",
    ]
    for token in difficulty_tokens:
        if token not in difficulty:
            blockers.append(f"Shared Mathematics difficulty engine is missing adaptive signal/output token: {token}")

    if "AddSingleton<MathematicsDifficultyEngine>()" not in registration:
        blockers.append("Stage 20 MathematicsDifficultyEngine is not registered for runtime DI.")
    if "AddSingleton<AdaptiveDiagnosticAssessmentEngine>()" not in registration:
        blockers.append("AdaptiveDiagnosticAssessmentEngine runtime registration is missing.")

    required_test_tokens = [
        "ExactOutcomeRequiresCompleteMathematicsAwareState",
        "MathematicsAwareDecisionConsumesAllFiveSignalsAndTargetsActiveMisconception",
        "WeakestRepresentationDrivesFamilyWhenThereIsNoActiveMisconception",
        "MixedExactAndNonExactOutcomeScopeFailsClosed",
        "DisallowedRepresentationFamilyFailsClosed",
        "SkillMasteryMustAgreeWithAuthoritativeMasteryProfile",
        "LegacyUnmigratedOutcomeKeepsPhase35Behavior",
    ]
    for token in required_test_tokens:
        if token not in tests:
            blockers.append(f"Stage 20 acceptance coverage is missing test: {token}")

    return {
        "schemaVersion": 1,
        "audit": "Stage 20 Diagnostic/adaptive migration closure",
        "status": "COMPLETE" if not blockers else "BLOCKED",
        "summary": {
            "exactOutcomeCount": len(stage20_entries),
            "requiredSignalCount": len(signals),
            "missingExactOutcomes": len(missing),
            "extraExactOutcomes": len(extras),
            "failClosedPolicyCount": len(fail_closed),
            "blockerCount": len(blockers),
        },
        "blockers": blockers,
        "exactOutcomeCodes": sorted(stage20_entries),
        "requiredSignals": sorted(signals),
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
