#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[2]
MANIFEST = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage26-28-production-rollout-manifest.v1.json"
LIMITS = ROOT / "src/Edulytics.Core/Mathematics/Runtime/MathematicsResourceLimits.cs"
GUARD = ROOT / "src/Edulytics.Services/Mathematics/Runtime/MathematicsResourceGuard.cs"
OBS = ROOT / "src/Edulytics.Services/Mathematics/Runtime/MathematicsObservability.cs"
ROLLOUT = ROOT / "src/Edulytics.Services/Mathematics/Rollout/MathematicsProductionRolloutPolicy.cs"
ENGINE = ROOT / "src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs"
WEB_POLICY = ROOT / "src/Edulytics.Web/GameRouting/MathematicsV2ProductMigrationPolicy.cs"
WEB_RESOLVER = ROOT / "src/Edulytics.Web/GameRouting/GameLessonRouteResolver.cs"
TESTS = ROOT / "tests/Edulytics.Tests/MathematicsIntelligence/Stage26To28ProductionClosureTests.cs"
CURRENT = ROOT / "docs/roadmaps/CURRENT_PLAN.md"
REPORT = ROOT / "artifacts/math-intelligence/stage26-28-production-closure-audit.json"

EXPECTED_MODES = {"LegacyOnly", "ShadowV2", "V2VerifiedOnly", "V2Preferred", "V2Only"}
EXPECTED_SELECTORS = {"domain", "skill", "curriculum", "grade"}
EXPECTED_METRICS = {
    "edulytics.math.generation.success",
    "edulytics.math.solver.success",
    "edulytics.math.verification.failure",
    "edulytics.math.alignment.rejection",
    "edulytics.math.fallback",
    "edulytics.math.unsupported",
    "edulytics.math.timeout",
    "edulytics.math.answer_equivalence.disagreement",
    "edulytics.math.content_mapping.conflict",
}


def read_json(path: Path) -> Any:
    return json.loads(path.read_text(encoding="utf-8-sig"))


def audit() -> dict[str, Any]:
    manifest = read_json(MANIFEST)
    blockers: list[str] = []

    if manifest.get("programmeStages") != [26, 27, 28]:
        blockers.append("Final closure manifest must cover exactly Programme Stages 26, 27 and 28.")
    if manifest.get("status") != "COMPLETE":
        blockers.append("Final closure manifest is not COMPLETE.")
    if manifest.get("completionBoundary") != "CONTROLLED_PRODUCTION_ROLLOUT":
        blockers.append("Final completion boundary must remain controlled production rollout.")

    security = manifest.get("securityAndResourceControls") or {}
    limits = security.get("limits") or {}
    expected_limits = {
        "maxInputLength": 4096,
        "maxTokenCount": 512,
        "maxAstDepth": 64,
        "maxAstNodes": 4096,
        "maxPolynomialDegree": 12,
        "maxMatrixDimension": 12,
        "solverTimeoutMilliseconds": 2000,
        "maxEstimatedMemoryBytes": 8388608,
        "maxConcurrentOperations": 8,
    }
    for key, value in expected_limits.items():
        if int(limits.get(key) or 0) != value:
            blockers.append(f"Stage 26 resource limit mismatch for {key}.")

    enforced = set(str(x) for x in security.get("enforcedAt") or [])
    for token in {"MathematicsResourceGuard", "MathematicsExecutionBudget", "ExactSkillContractQuestionEngine"}:
        if token not in enforced:
            blockers.append(f"Stage 26 enforcement point missing: {token}")

    observability = manifest.get("observability") or {}
    if observability.get("meter") != "Edulytics.Mathematics":
        blockers.append("Stage 27 Mathematics meter name is incorrect.")
    if set(str(x) for x in observability.get("metrics") or []) != EXPECTED_METRICS:
        blockers.append("Stage 27 metric set does not match the governing plan.")

    rollout = manifest.get("productionRollout") or {}
    if set(str(x) for x in rollout.get("modes") or []) != EXPECTED_MODES:
        blockers.append("Stage 28 rollout modes are incomplete.")
    if set(str(x) for x in rollout.get("selectors") or []) != EXPECTED_SELECTORS:
        blockers.append("Stage 28 rollout selectors are incomplete.")
    if rollout.get("explicitNonLegacyModeRequiresSelector") is not True:
        blockers.append("Stage 28 does not require scoped explicit non-legacy rollout.")
    if rollout.get("v2OnlyBlocksUnsupportedInsteadOfFallingBack") is not True:
        blockers.append("Stage 28 V2Only does not fail closed.")
    if rollout.get("legacyGrade16Compatibility") is not True:
        blockers.append("Stage 28 breaks the accepted Grade 1-6 rollout compatibility path.")
    if rollout.get("advancedShadowVerifiedAutoPromotion") is not False:
        blockers.append("Stage 28 illegally auto-promotes ShadowVerified advanced capabilities.")
    if rollout.get("globalV2OnlyEnabled") is not False:
        blockers.append("Stage 28 must not globally enable V2Only.")

    boundary = manifest.get("truthfulCapabilityBoundary") or {}
    for key in ("proof", "unfamiliarTransfer"):
        if boundary.get(key) != "UNSUPPORTED":
            blockers.append(f"Final production boundary overstates {key}.")
    if boundary.get("unsupportedCasesBlocked") is not True:
        blockers.append("Final production boundary does not explicitly block unsupported cases.")

    source_checks = {
        LIMITS: [
            "MaxInputLength",
            "MaxTokenCount",
            "MaxAstDepth",
            "MaxAstNodes",
            "MaxPolynomialDegree",
            "MaxMatrixDimension",
            "SolverTimeout",
            "MaxEstimatedMemoryBytes",
            "MaxConcurrentOperations",
        ],
        GUARD: [
            "ValidateInputText",
            "AnalyzeAst",
            "MathematicsExecutionBudget",
            "SemaphoreSlim",
            "CancelAfter",
            "MaxPolynomialDegree",
            "MaxMatrixDimension",
        ],
        OBS: [
            "Edulytics.Mathematics",
            "GenerationSuccess",
            "SolverSuccess",
            "VerificationFailure",
            "AlignmentRejection",
            "Fallback",
            "Unsupported",
            "Timeout",
            "AnswerEquivalenceDisagreement",
            "ContentMappingConflict",
        ],
        ROLLOUT: [
            "LegacyOnly",
            "ShadowV2",
            "V2VerifiedOnly",
            "V2Preferred",
            "V2Only",
            "DomainsEnvironmentVariable",
            "SkillsEnvironmentVariable",
            "CurriculaEnvironmentVariable",
            "GradesEnvironmentVariable",
            "ExplicitMode",
        ],
        ENGINE: [
            "MathematicsResourceGuard.ValidateGenerationRequest",
            "MathematicsObservability.Record(MathematicsMetricKind.GenerationSuccess",
            "MathematicsMetricKind.VerificationFailure",
        ],
        WEB_POLICY: [
            "ResolveProductionRollout",
            "MathematicsProductionRolloutPolicy.FromEnvironment",
            "READY_VERIFIED",
        ],
        WEB_RESOLVER: [
            "ResolveProductionRollout",
            "math-v2-rollout-blocked",
            "exact-lesson-skill-v2-shadow",
        ],
    }

    for path, tokens in source_checks.items():
        text = path.read_text(encoding="utf-8")
        for token in tokens:
            if token not in text:
                blockers.append(f"Production closure source check missing {token} in {path.name}.")

    tests = TESTS.read_text(encoding="utf-8")
    for token in [
        "ProductionResourceDefaultsCoverEveryRequiredSafetyBudget",
        "ResourceGuardFailsClosedForInputDepthDegreeAndMatrixOverBudget",
        "ExecutionBudgetEnforcesConcurrencyAndTimeout",
        "ObservabilityExposesEveryRequiredMetricAndExactKernelEmitsSuccess",
        "RolloutPolicySupportsFiveModesAndRequiresScopedExplicitPromotion",
        "LegacyGrade16FlagRemainsBackwardCompatibleAndFailClosed",
        "ClosureManifestKeepsAdvancedUnsupportedClaimsBlocked",
    ]:
        if token not in tests:
            blockers.append(f"Final production acceptance coverage is missing token: {token}")

    current = CURRENT.read_text(encoding="utf-8")
    for token in [
        "Programme Stage 26 — Security and resource controls: COMPLETE.",
        "Programme Stage 27 — Observability: COMPLETE.",
        "Programme Stage 28 — Production rollout: COMPLETE.",
        "Integrated Mathematics Intelligence Master Execution Plan: COMPLETE",
    ]:
        if token not in current:
            blockers.append(f"CURRENT_PLAN does not record final programme closure: {token}")

    return {
        "schemaVersion": 1,
        "audit": "Stages 26-28 final Mathematics production closure",
        "status": "COMPLETE" if not blockers else "BLOCKED",
        "summary": {
            "resourceControlCount": len(expected_limits),
            "observabilityMetricCount": len(EXPECTED_METRICS),
            "rolloutModeCount": len(EXPECTED_MODES),
            "rolloutSelectorCount": len(EXPECTED_SELECTORS),
            "blockerCount": len(blockers),
        },
        "blockers": blockers,
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
