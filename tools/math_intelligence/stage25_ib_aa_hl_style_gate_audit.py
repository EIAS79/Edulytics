#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
from collections import Counter
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[2]
MANIFEST = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage25-ib-aa-hl-style-gate-manifest.v1.json"
CORPUS = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage25-ib-aa-hl-style-benchmark-corpus.v1.json"
SKILLS = ROOT / "src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json"
TESTS = ROOT / "tests/Edulytics.Tests/MathematicsIntelligence/Stage25IbAaHlStyleGateTests.cs"
REPORT = ROOT / "artifacts/math-intelligence/stage25-ib-aa-hl-style-gate-audit.json"

REQUIRED_CATEGORIES = {"Routine", "Multi-step", "Modelling", "Reasoning", "Proof", "Unfamiliar transfer"}
UNSUPPORTED_REQUIRED = {"Proof", "Unfamiliar transfer"}
REQUIRED_CHECKS = {"generate", "solve", "verify", "grade", "strategy-plan", "difficulty-calibrate"}


def read_json(path: Path) -> Any:
    return json.loads(path.read_text(encoding="utf-8-sig"))


def audit() -> dict[str, Any]:
    manifest = read_json(MANIFEST)
    corpus = read_json(CORPUS)
    skills = read_json(SKILLS)
    blockers: list[str] = []

    rows = manifest.get("categoryCoverage") or []
    coverage = {
        str(row.get("category") or ""): row
        for row in rows
        if isinstance(row, dict)
    }
    if len(coverage) != len(rows):
        blockers.append("Stage 25 manifest contains duplicate or missing category values.")
    if set(coverage) != REQUIRED_CATEGORIES:
        blockers.append("Stage 25 manifest does not contain exactly the six required IB AA HL-style demand classes.")

    benchmark_rows = corpus.get("benchmarks") or []
    benchmarks = {
        str(row.get("benchmarkId") or ""): row
        for row in benchmark_rows
        if isinstance(row, dict)
    }
    if len(benchmarks) != len(benchmark_rows):
        blockers.append("Stage 25 benchmark corpus contains duplicate or missing benchmarkId values.")

    if int(corpus.get("benchmarkCount") or 0) != len(benchmarks):
        blockers.append("Stage 25 benchmarkCount does not match corpus rows.")
    if set(str(x) for x in corpus.get("requiredCategories") or []) != REQUIRED_CATEGORIES:
        blockers.append("Stage 25 corpus does not declare exactly the six required demand classes.")
    if set(str(x) for x in corpus.get("unsupportedCategories") or []) != UNSUPPORTED_REQUIRED:
        blockers.append("Stage 25 corpus must keep Proof and Unfamiliar transfer explicitly unsupported.")
    if corpus.get("claimBoundary") != "ENGINE_STYLE_CAPABILITY_ONLY":
        blockers.append("Stage 25 corpus claim boundary is not engine-style capability only.")
    if corpus.get("globalClaimAllowed") is not False:
        blockers.append("Stage 25 corpus permits a global IB AA HL claim.")

    counts = Counter(str(row.get("category") or "") for row in benchmark_rows)
    evidenced = unsupported = 0
    for category in sorted(REQUIRED_CATEGORIES):
        row = coverage.get(category)
        if row is None:
            continue
        status = str(row.get("status") or "")
        expected_count = counts.get(category, 0)
        if int(row.get("benchmarkCount") or 0) != expected_count:
            blockers.append(f"Stage 25 category benchmark count mismatch for {category}.")

        if category in UNSUPPORTED_REQUIRED:
            unsupported += 1
            if status != "UNSUPPORTED":
                blockers.append(f"Stage 25 must keep {category} UNSUPPORTED.")
            if expected_count != 0:
                blockers.append(f"Unsupported Stage 25 category {category} must not carry executable benchmarks.")
            if row.get("capabilityClaim") != "BLOCKED":
                blockers.append(f"Unsupported Stage 25 category {category} must keep capability claim BLOCKED.")
        else:
            evidenced += 1
            if status != "EVIDENCED":
                blockers.append(f"Stage 25 evidenced category {category} is not marked EVIDENCED.")
            if expected_count <= 0:
                blockers.append(f"Stage 25 evidenced category {category} has no executable benchmarks.")
            if row.get("capabilityClaim") != "NARROW_ENGINE_SLICE_ONLY":
                blockers.append(f"Stage 25 category {category} overstates its capability claim.")

    gate = manifest.get("gatePolicy") or {}
    if set(str(x) for x in gate.get("requiredBenchmarkCategories") or []) != REQUIRED_CATEGORIES:
        blockers.append("Stage 25 gate policy does not require exactly the six demand classes.")
    if gate.get("globalIbAaHlCapabilityClaimAllowed") is not False:
        blockers.append("Global IB AA HL capability claim is not fail-closed.")
    if gate.get("productRoutingEnabled") is not False:
        blockers.append("Stage 25 must not enable product routing.")
    if gate.get("officialIbCurriculumMappingPresent") is not False:
        blockers.append("Stage 25 incorrectly claims an official IB curriculum mapping.")
    if gate.get("categoryEvidenceDoesNotEqualCurriculumApproval") is not True:
        blockers.append("Stage 25 does not separate engine evidence from curriculum approval.")
    if gate.get("unsupportedCategoriesMustFailClosed") is not True:
        blockers.append("Stage 25 does not fail closed for unsupported demand classes.")

    academic = manifest.get("academicReview") or {}
    if academic.get("currentState") != "REQUIRED":
        blockers.append("Stage 25 academic review state must remain REQUIRED.")
    if int(academic.get("approvedVerifiedCategoryCount") or 0) != 0:
        blockers.append("Stage 25 must not record approved verified IB categories before academic review.")

    summary = manifest.get("summary") or {}
    expected_summary = {
        "categoryCount": 6,
        "evidencedCategories": evidenced,
        "unsupportedCategories": unsupported,
        "benchmarkCount": len(benchmarks),
        "formalIbMappings": 0,
    }
    for key, value in expected_summary.items():
        if int(summary.get(key) or 0) != value:
            blockers.append(f"Stage 25 summary mismatch for {key}.")

    skill_registry = {
        str(row.get("id") or ""): row
        for row in skills.get("skills") or []
        if isinstance(row, dict)
    }

    for benchmark_id, benchmark in sorted(benchmarks.items()):
        category = str(benchmark.get("category") or "")
        if category not in REQUIRED_CATEGORIES:
            blockers.append(f"Stage 25 benchmark {benchmark_id} uses an unknown category.")
        if category in UNSUPPORTED_REQUIRED:
            blockers.append(f"Stage 25 benchmark {benchmark_id} illegally evidences unsupported category {category}.")
        if set(str(x) for x in benchmark.get("requiredChecks") or []) != REQUIRED_CHECKS:
            blockers.append(f"Stage 25 benchmark {benchmark_id} does not require the full execution chain.")
        if list(benchmark.get("requiredDifficultyBands") or []) != [1, 2, 3]:
            blockers.append(f"Stage 25 benchmark {benchmark_id} does not calibrate all three difficulty bands.")
        if benchmark.get("evidenceLevel") != "ENGINE_EVIDENCE_ONLY":
            blockers.append(f"Stage 25 benchmark {benchmark_id} overstates evidence level.")
        if benchmark.get("officialIbMapping") is not False:
            blockers.append(f"Stage 25 benchmark {benchmark_id} incorrectly claims official IB mapping.")

        skill_id = str(benchmark.get("skillId") or "")
        skill = skill_registry.get(skill_id)
        if skill is None:
            blockers.append(f"Stage 25 references unknown SkillId {skill_id}.")
        elif skill.get("v2Status") != "ShadowVerified":
            blockers.append(f"Stage 25 engine evidence skill is not ShadowVerified: {skill_id}")

    tests = TESTS.read_text(encoding="utf-8")
    for token in [
        "GateDefinesAllSixDemandClassesAndFailsClosedForUnsupportedAdvancedClasses",
        "ExecutableEngineEvidenceRunsFullChainAcrossAllDifficultyBands",
        "ReasoningEvidenceRemainsNarrowAndDoesNotPromoteProof",
        "OfficialIbClaimsRemainBlockedWithoutFormalMappingAndAcademicApproval",
        "MathematicsAnswerEquivalenceV2",
        "DeterministicMathematicsStrategyPlanner",
        "MathematicsDifficultyEngine",
        "ExactMechanicsQuestionFactory",
    ]:
        if token not in tests:
            blockers.append(f"Stage 25 acceptance coverage is missing token: {token}")

    return {
        "schemaVersion": 1,
        "audit": "Stage 25 IB AA HL-style capability gate closure",
        "status": "COMPLETE" if not blockers else "BLOCKED",
        "summary": {
            "categoryCount": len(coverage),
            "evidencedCategoryCount": evidenced,
            "unsupportedCategoryCount": unsupported,
            "benchmarkCount": len(benchmarks),
            "formalIbMappingCount": 0,
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
