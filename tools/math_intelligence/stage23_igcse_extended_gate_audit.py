#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[2]
L10 = ROOT / "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/cambridge-igcse-l10-extended-ogl-v1.lesson-blueprint.json"
L11 = ROOT / "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/cambridge-igcse-l11-extended-ogl-v1.lesson-blueprint.json"
MANIFEST = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage23-igcse-extended-gate-manifest.v1.json"
CORPUS = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage23-igcse-extended-benchmark-corpus.v1.json"
SKILLS = ROOT / "src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json"
TESTS = ROOT / "tests/Edulytics.Tests/MathematicsIntelligence/Stage23IgcseExtendedGateTests.cs"
REPORT = ROOT / "artifacts/math-intelligence/stage23-igcse-extended-gate-audit.json"

ALLOWED_STATUS = {"VERIFIED", "CONTEXTUAL", "UNSUPPORTED"}
REQUIRED_CHECKS = {"generate", "solve", "verify", "grade", "difficulty-calibrate"}


def read_json(path: Path) -> Any:
    return json.loads(path.read_text(encoding="utf-8-sig"))


def audit() -> dict[str, Any]:
    l10 = read_json(L10)
    l11 = read_json(L11)
    manifest = read_json(MANIFEST)
    corpus = read_json(CORPUS)
    skills = read_json(SKILLS)
    blockers: list[str] = []

    source_lessons = {}
    for pack in (l10, l11):
        if pack.get("Pathway") != "Extended":
            blockers.append(f"Unexpected IGCSE pathway in {pack.get('BlueprintCode')}.")
        for lesson in pack.get("Lessons") or []:
            code = str(lesson.get("LessonCode") or "")
            if not code or code in source_lessons:
                blockers.append(f"Duplicate or missing source lesson code: {code}")
            source_lessons[code] = lesson

    rows = manifest.get("lessons") or []
    manifest_lessons = {
        str(row.get("lessonCode") or ""): row
        for row in rows
        if isinstance(row, dict)
    }
    if len(manifest_lessons) != len(rows):
        blockers.append("Stage 23 manifest contains duplicate or missing lessonCode values.")

    missing = sorted(set(source_lessons) - set(manifest_lessons))
    extras = sorted(set(manifest_lessons) - set(source_lessons))
    for code in missing:
        blockers.append(f"IGCSE Extended lesson missing from Stage 23 gate: {code}")
    for code in extras:
        blockers.append(f"Stage 23 gate contains lesson outside the Extended source inventory: {code}")

    benchmark_rows = corpus.get("benchmarks") or []
    benchmarks = {
        str(row.get("benchmarkId") or ""): row
        for row in benchmark_rows
        if isinstance(row, dict)
    }
    if len(benchmarks) != len(benchmark_rows):
        blockers.append("Stage 23 benchmark corpus contains duplicate or missing benchmarkId values.")

    skill_rows = skills.get("skills") or []
    skill_registry = {
        str(row.get("id") or ""): row
        for row in skill_rows
        if isinstance(row, dict)
    }

    verified = contextual = unsupported = formal = 0

    for code, row in sorted(manifest_lessons.items()):
        source = source_lessons.get(code)
        if source is None:
            continue

        status = str(row.get("status") or "")
        if status not in ALLOWED_STATUS:
            blockers.append(f"Invalid Stage 23 status for {code}: {status}")
            continue

        source_outcomes = list(source.get("OutcomeCodes") or [])
        source_targets = list(source.get("FormalTargets") or [])
        formal_source = bool(source_outcomes or source_targets)
        formal_row = bool(row.get("formalOutcomeMapped"))

        if formal_source != formal_row:
            blockers.append(f"Formal-mapping flag disagrees with source blueprint for {code}.")

        row_outcomes = list(row.get("formalOutcomeCodes") or [])
        if row_outcomes != source_outcomes:
            blockers.append(f"Formal OutcomeCodes drift from source blueprint for {code}.")

        if formal_row:
            formal += 1

        mapped_skills = [str(x) for x in row.get("engineSkillIds") or []]
        mapped_benchmarks = [str(x) for x in row.get("benchmarkIds") or []]

        if status == "VERIFIED":
            verified += 1
            if not formal_row:
                blockers.append(f"VERIFIED lesson lacks explicit formal curriculum mapping: {code}")
            if row.get("academicReviewStatus") != "APPROVED":
                blockers.append(f"VERIFIED lesson lacks APPROVED academic review: {code}")
            if not mapped_skills or not mapped_benchmarks:
                blockers.append(f"VERIFIED lesson lacks engine/benchmark evidence: {code}")
        elif status == "CONTEXTUAL":
            contextual += 1
            if not mapped_skills or not mapped_benchmarks:
                blockers.append(f"CONTEXTUAL lesson lacks explicit engine evidence: {code}")
            if row.get("academicReviewStatus") != "REQUIRED":
                blockers.append(f"CONTEXTUAL lesson must remain academic-review REQUIRED: {code}")
        else:
            unsupported += 1
            if mapped_skills or mapped_benchmarks:
                blockers.append(f"UNSUPPORTED lesson must not carry promoted engine mapping: {code}")

        for skill_id in mapped_skills:
            skill = skill_registry.get(skill_id)
            if skill is None:
                blockers.append(f"Stage 23 references unknown SkillId {skill_id} for {code}.")
                continue
            if skill.get("v2Status") != "ShadowVerified":
                blockers.append(f"Contextual IGCSE engine skill is not ShadowVerified: {skill_id}")

        for benchmark_id in mapped_benchmarks:
            benchmark = benchmarks.get(benchmark_id)
            if benchmark is None:
                blockers.append(f"Stage 23 references unknown benchmark {benchmark_id} for {code}.")
                continue
            if benchmark.get("skillId") not in mapped_skills:
                blockers.append(f"Benchmark {benchmark_id} SkillId does not match lesson mapping {code}.")

    gate = manifest.get("gatePolicy") or {}
    if gate.get("globalIgcseExtendedCapabilityClaimAllowed") is not False:
        blockers.append("Global IGCSE Extended capability claim is not fail-closed.")
    if gate.get("productRoutingEnabled") is not False:
        blockers.append("Stage 23 must not enable product routing.")
    if gate.get("titleSimilarityAloneNeverPromotes") is not True:
        blockers.append("Stage 23 does not explicitly prohibit title-only promotion.")

    academic = manifest.get("academicReview") or {}
    if academic.get("currentState") != "REQUIRED":
        blockers.append("Stage 23 academic review state must remain REQUIRED until approval evidence exists.")
    if int(academic.get("approvedVerifiedLessonCount") or 0) != verified:
        blockers.append("Academic approved verified count does not match VERIFIED lesson count.")

    if int(corpus.get("benchmarkCount") or 0) != len(benchmarks):
        blockers.append("Stage 23 benchmarkCount does not match corpus rows.")

    for benchmark_id, benchmark in sorted(benchmarks.items()):
        checks = set(str(x) for x in benchmark.get("requiredChecks") or [])
        if checks != REQUIRED_CHECKS:
            blockers.append(f"Benchmark {benchmark_id} does not require the full Stage 23 execution chain.")
        bands = list(benchmark.get("requiredDifficultyBands") or [])
        if bands != [1, 2, 3]:
            blockers.append(f"Benchmark {benchmark_id} does not calibrate all three UI difficulty bands.")
        if benchmark.get("curriculumClaimStatus") != "ENGINE_EVIDENCE_ONLY":
            blockers.append(f"Benchmark {benchmark_id} overstates curriculum claim status.")

    expected_summary = manifest.get("summary") or {}
    if int(expected_summary.get("totalLessons") or 0) != len(source_lessons):
        blockers.append("Stage 23 summary totalLessons does not match source inventory.")
    if int(expected_summary.get("verified") or 0) != verified:
        blockers.append("Stage 23 VERIFIED summary count is incorrect.")
    if int(expected_summary.get("contextual") or 0) != contextual:
        blockers.append("Stage 23 CONTEXTUAL summary count is incorrect.")
    if int(expected_summary.get("unsupported") or 0) != unsupported:
        blockers.append("Stage 23 UNSUPPORTED summary count is incorrect.")
    if int(expected_summary.get("formalOutcomeMapped") or 0) != formal:
        blockers.append("Stage 23 formal mapping summary count is incorrect.")

    tests = TESTS.read_text(encoding="utf-8")
    for token in [
        "GateInventoriesAllExtendedSupportingLessonsAndBlocksGlobalClaim",
        "EveryContextualMappingReferencesBenchmarkCorpusAndNoTitleOnlyPromotionExists",
        "BenchmarkCorpusGeneratesSolvesVerifiesGradesAndCalibratesAllBands",
        "AcademicReviewGatePreventsVerifiedPromotionWithoutFormalMapping",
        "MathematicsAnswerEquivalenceV2",
        "DeterministicMathematicsStrategyPlanner",
        "MathematicsDifficultyEngine",
    ]:
        if token not in tests:
            blockers.append(f"Stage 23 acceptance coverage is missing token: {token}")

    return {
        "schemaVersion": 1,
        "audit": "Stage 23 IGCSE Extended capability gate closure",
        "status": "COMPLETE" if not blockers else "BLOCKED",
        "summary": {
            "sourceLessonCount": len(source_lessons),
            "verifiedLessonCount": verified,
            "contextualLessonCount": contextual,
            "unsupportedLessonCount": unsupported,
            "formalMappedLessonCount": formal,
            "benchmarkCount": len(benchmarks),
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
