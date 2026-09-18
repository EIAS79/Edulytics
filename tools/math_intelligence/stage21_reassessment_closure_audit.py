#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[2]
STAGE20 = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage20-diagnostic-adaptive-migration-manifest.v1.json"
STAGE21 = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage21-reassessment-migration-manifest.v1.json"
CONTRACTS = ROOT / "src/Edulytics.Core/Recovery/WeaknessRecoveryContracts.cs"
RECOVERY = ROOT / "src/Edulytics.Services/Recovery/WeaknessRecoveryEngine.cs"
ORCHESTRATOR = ROOT / "src/Edulytics.Services/Recovery/EquivalentReassessmentGenerator.cs"
ENGINE = ROOT / "src/Edulytics.Services/Recovery/Stage21ExactReassessmentEngine.cs"
SHARED_EXACT = ROOT / "src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs"
REGISTRATION = ROOT / "src/Edulytics.Web/Extensions/WeaknessRecoveryRegistrationExtensions.cs"
PROGRAM = ROOT / "src/Edulytics.Web/Program.cs"
TESTS = ROOT / "tests/Edulytics.Tests/MathematicsIntelligence/Stage21ReassessmentMigrationTests.cs"
REPORT = ROOT / "artifacts/math-intelligence/stage21-reassessment-closure-audit.json"

EXPECTED_DIMENSIONS = {
    "coefficients",
    "representation",
    "strategy",
    "context",
    "misconceptionTrap",
    "cognitiveDemand",
}
EXPECTED_METHOD = "skill-contract-reassessment-math-fresh-v1"


def read_json(path: Path) -> Any:
    return json.loads(path.read_text(encoding="utf-8-sig"))


def audit() -> dict[str, Any]:
    stage20 = read_json(STAGE20)
    stage21 = read_json(STAGE21)
    blockers: list[str] = []

    if stage21.get("status") != "COMPLETE":
        blockers.append("Stage 21 manifest is not marked COMPLETE.")

    if stage21.get("generationMethod") != EXPECTED_METHOD:
        blockers.append("Stage 21 generation method does not match the accepted exact reassessment method.")

    dimensions = set(str(x) for x in stage21.get("requiredFreshnessDimensions") or [])
    if dimensions != EXPECTED_DIMENSIONS:
        blockers.append(
            f"Stage 21 freshness dimensions differ from the governing six-dimension contract: {sorted(dimensions)}"
        )

    stage20_entries = {
        str(row.get("outcomeCode") or ""): row
        for row in stage20.get("exactOutcomes") or []
        if isinstance(row, dict)
    }
    stage21_entries = {
        str(row.get("outcomeCode") or ""): row
        for row in stage21.get("exactOutcomes") or []
        if isinstance(row, dict)
    }

    if len(stage21_entries) != len(stage21.get("exactOutcomes") or []):
        blockers.append("Stage 21 exactOutcomes contains duplicate or missing outcomeCode values.")

    missing = sorted(set(stage20_entries) - set(stage21_entries))
    extras = sorted(set(stage21_entries) - set(stage20_entries))
    for code in missing:
        blockers.append(f"Stage 20 exact outcome missing from Stage 21 reassessment migration: {code}")
    for code in extras:
        blockers.append(f"Stage 21 contains an exact outcome not approved by Stage 20: {code}")

    for code in sorted(set(stage20_entries) & set(stage21_entries)):
        before = str(stage20_entries[code].get("skillId") or "")
        after = str(stage21_entries[code].get("skillId") or "")
        if before != after:
            blockers.append(f"Stage 21 SkillContract drift for {code}: {before} -> {after}")

    history = stage21.get("historyPolicy") or {}
    for key in [
        "exactPriorExposureRequiresMathematicalSignature",
        "signatureMustBelongToDeclaredExposureHistory",
    ]:
        if history.get(key) is not True:
            blockers.append(f"Stage 21 history policy must enable {key}.")

    for key in [
        "promptOnlyFreshnessAccepted",
        "fingerprintOnlyFreshnessAccepted",
    ]:
        if history.get(key) is not False:
            blockers.append(f"Stage 21 history policy must reject {key}.")

    item_policy = stage21.get("exactItemPolicy") or {}
    if item_policy.get("solverVerified") is not True:
        blockers.append("Stage 21 exact items are not marked solver verified.")
    if item_policy.get("independentVerifier") is not True:
        blockers.append("Stage 21 exact items are not marked independently verified.")
    if item_policy.get("allowedFamiliesOnly") is not True:
        blockers.append("Stage 21 exact item policy does not restrict generation to allowed families.")
    if item_policy.get("coefficientReuseAllowed") is not False:
        blockers.append("Stage 21 exact item policy allows coefficient reuse.")
    if int(item_policy.get("minimumChangedNonCoefficientDimensionsAgainstPrior") or 0) < 2:
        blockers.append("Stage 21 mathematical distance threshold is below two non-coefficient dimensions.")

    fail_closed = set(str(x) for x in stage21.get("failClosed") or [])
    required_fail_closed = {
        "exactPriorExposureMissingMathematicalSignature",
        "mathematicalSignatureOutsideDeclaredExposureHistory",
        "coefficientReuse",
        "insufficientMathematicalDimensionChange",
        "disallowedQuestionFamily",
        "solverVerificationFailure",
    }
    if not required_fail_closed.issubset(fail_closed):
        blockers.append("Stage 21 manifest is missing one or more mandatory fail-closed conditions.")

    contracts = CONTRACTS.read_text(encoding="utf-8")
    recovery = RECOVERY.read_text(encoding="utf-8")
    orchestrator = ORCHESTRATOR.read_text(encoding="utf-8")
    engine = ENGINE.read_text(encoding="utf-8")
    shared = SHARED_EXACT.read_text(encoding="utf-8")
    registration = REGISTRATION.read_text(encoding="utf-8")
    program = PROGRAM.read_text(encoding="utf-8")
    tests = TESTS.read_text(encoding="utf-8")

    for token in [
        "ReassessmentMathematicalSignature",
        "CoefficientSignature",
        "Representation",
        "Strategy",
        "Context",
        "MisconceptionTrap",
        "CognitiveDemand",
        "PreviousMathematicalSignatures",
    ]:
        if token not in contracts:
            blockers.append(f"Stage 21 recovery contracts are missing token: {token}")

    for token in [
        "ValidateMathematicalSignatures",
        "PreviousMathematicalSignatures",
        "outside the declared exposure history",
    ]:
        if token not in recovery:
            blockers.append(f"Stage 21 recovery plan validation is missing token: {token}")

    exact_pos = orchestrator.find("Stage19AssessmentSkillContracts.TryResolve")
    legacy_pos = orchestrator.find("var previousFingerprints")
    if exact_pos < 0:
        blockers.append("Equivalent reassessment orchestrator does not route exact Stage 19 outcomes.")
    if legacy_pos < 0:
        blockers.append("Equivalent reassessment legacy path could not be located.")
    if exact_pos >= 0 and legacy_pos >= 0 and exact_pos > legacy_pos:
        blockers.append("Stage 21 exact routing does not precede the legacy wording-freshness generator.")

    required_engine_tokens = [
        'GenerationMethod = "skill-contract-reassessment-math-fresh-v1"',
        "ExactSkillContractQuestionEngine().Generate",
        "ExactSkillContractQuestionEngine.Verify",
        "CoefficientSignature",
        "Representation",
        "Strategy",
        "Context",
        "MisconceptionTrap",
        "CognitiveDemand",
        "IsMathematicallyFresh",
        "changedDimensions < 2",
        "RequireVariation",
        "wordingOnlyFreshness = false",
        "mathematicalFreshnessVerified = true",
        "contract.AllowedQuestionFamilies.Contains",
        "recovery.ValidateEquivalentReassessment",
    ]
    for token in required_engine_tokens:
        if token not in engine:
            blockers.append(f"Stage 21 exact reassessment engine is missing required token: {token}")

    forbidden_engine_tokens = [
        "ReviewedPromptPrefixes",
        "EnsureFreshPromptShape",
        "MathematicsQuestionGenerationEngine.GeneratorVersion",
    ]
    for token in forbidden_engine_tokens:
        if token in engine:
            blockers.append(f"Stage 21 exact reassessment engine still depends on wording-only legacy behavior: {token}")

    for token in [
        "private static string Solve",
        "public static bool Verify",
    ]:
        if token not in shared:
            blockers.append(f"Shared exact Mathematics kernel is missing required solver/verifier token: {token}")

    for token in [
        "AddSingleton<WeaknessRecoveryEngine>()",
        "AddSingleton<Stage21ExactReassessmentEngine>()",
        "AddSingleton<EquivalentReassessmentGenerator>()",
    ]:
        if token not in registration:
            blockers.append(f"Stage 21 runtime registration is missing token: {token}")

    if ".AddWeaknessRecoveryPhase36();" not in program:
        blockers.append("Stage 21 weakness-recovery services are not enabled in the web runtime.")

    required_tests = [
        "EveryExactOutcomeGeneratesMathematicallyFreshVerifiedReassessment",
        "PriorExactExposureRequiresMathematicalSignatureNotOnlyPromptOrFingerprint",
        "ExactReassessmentDoesNotReusePriorCoefficientsAndChangesMultipleMathDimensions",
        "RecoveryPlanCarriesOnlyDeclaredReconstructableMathematicalHistory",
        "MathematicalHistoryOutsideDeclaredExposureFailsClosed",
        "NonMigratedOutcomeRetainsLegacyPhase36Generator",
    ]
    for token in required_tests:
        if token not in tests:
            blockers.append(f"Stage 21 acceptance coverage is missing test: {token}")

    return {
        "schemaVersion": 1,
        "audit": "Stage 21 reassessment migration closure",
        "status": "COMPLETE" if not blockers else "BLOCKED",
        "summary": {
            "exactOutcomeCount": len(stage21_entries),
            "freshnessDimensionCount": len(dimensions),
            "missingExactOutcomes": len(missing),
            "extraExactOutcomes": len(extras),
            "failClosedPolicyCount": len(fail_closed),
            "blockerCount": len(blockers),
        },
        "blockers": blockers,
        "exactOutcomeCodes": sorted(stage21_entries),
        "freshnessDimensions": sorted(dimensions),
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
