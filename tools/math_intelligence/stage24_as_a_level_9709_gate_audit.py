#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[2]
AS_PACK = ROOT / "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/cambridge-as-level-9709-ogl-v1.lesson-blueprint.json"
A_PACK = ROOT / "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/cambridge-a-level-9709-ogl-v1.lesson-blueprint.json"
MANIFEST = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage24-as-a-level-9709-gate-manifest.v1.json"
CORPUS = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage24-as-a-level-9709-benchmark-corpus.v1.json"
SKILLS = ROOT / "src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json"
TESTS = ROOT / "tests/Edulytics.Tests/MathematicsIntelligence/Stage24AsALevel9709GateTests.cs"
REPORT = ROOT / "artifacts/math-intelligence/stage24-as-a-level-9709-gate-audit.json"

ALLOWED_STATUS = {"VERIFIED", "CONTEXTUAL", "UNSUPPORTED"}
EXPECTED_DOMAINS = {"Pure Mathematics", "Mechanics", "Probability & Statistics"}
EXPECTED_ROUTES = {"AS-PURE", "AS-MECHANICS", "AS-PROBSTAT", "A-PURE", "A-MECHANICS", "A-PROBSTAT"}
REQUIRED_CHECKS = {"generate", "solve", "verify", "grade", "difficulty-calibrate"}


def read_json(path: Path) -> Any:
    return json.loads(path.read_text(encoding="utf-8-sig"))


def audit() -> dict[str, Any]:
    packs = [read_json(AS_PACK), read_json(A_PACK)]
    manifest = read_json(MANIFEST)
    corpus = read_json(CORPUS)
    skills = read_json(SKILLS)
    blockers: list[str] = []

    source_lessons: dict[str, dict[str, Any]] = {}
    source_level_by_code: dict[str, str] = {}
    for pack, level in zip(packs, ("AS", "A")):
        if str(pack.get("OfficialSourceUrl") or "").find("697427-2026-2027-syllabus.pdf") < 0:
            blockers.append(f"Unexpected 9709 source URL for {level}.")
        for lesson in pack.get("Lessons") or []:
            code = str(lesson.get("LessonCode") or "")
            if not code or code in source_lessons:
                blockers.append(f"Duplicate or missing 9709 source lesson code: {code}")
            source_lessons[code] = lesson
            source_level_by_code[code] = level

    rows = manifest.get("lessons") or []
    manifest_lessons = {
        str(row.get("lessonCode") or ""): row
        for row in rows
        if isinstance(row, dict)
    }
    if len(manifest_lessons) != len(rows):
        blockers.append("Stage 24 manifest contains duplicate or missing lessonCode values.")

    missing = sorted(set(source_lessons) - set(manifest_lessons))
    extras = sorted(set(manifest_lessons) - set(source_lessons))
    for code in missing:
        blockers.append(f"9709 lesson missing from Stage 24 gate: {code}")
    for code in extras:
        blockers.append(f"Stage 24 gate contains lesson outside the source inventory: {code}")

    skill_registry = {
        str(row.get("id") or ""): row
        for row in skills.get("skills") or []
        if isinstance(row, dict)
    }
    benchmark_rows = corpus.get("benchmarks") or []
    benchmarks = {
        str(row.get("benchmarkId") or ""): row
        for row in benchmark_rows
        if isinstance(row, dict)
    }
    if len(benchmarks) != len(benchmark_rows):
        blockers.append("Stage 24 benchmark corpus contains duplicate or missing benchmarkId values.")

    verified = contextual = unsupported = formal = 0
    route_counts: dict[str, dict[str, int]] = {}

    for code, row in sorted(manifest_lessons.items()):
        source = source_lessons.get(code)
        if source is None:
            continue

        status = str(row.get("status") or "")
        route = str(row.get("paperRoute") or "")
        domain = str(row.get("domain") or "")
        level = str(row.get("level") or "")

        if status not in ALLOWED_STATUS:
            blockers.append(f"Invalid Stage 24 status for {code}: {status}")
            continue
        if route not in EXPECTED_ROUTES:
            blockers.append(f"Invalid Stage 24 paper route for {code}: {route}")
        if domain not in EXPECTED_DOMAINS:
            blockers.append(f"Invalid Stage 24 domain for {code}: {domain}")
        if level != source_level_by_code.get(code):
            blockers.append(f"Stage 24 level drift for {code}: {level}")

        source_outcomes = list(source.get("OutcomeCodes") or [])
        source_targets = list(source.get("FormalTargets") or [])
        formal_source = bool(source_outcomes or source_targets)
        formal_row = bool(row.get("formalOutcomeMapped"))
        if formal_source != formal_row:
            blockers.append(f"Formal-mapping flag disagrees with source blueprint for {code}.")
        if list(row.get("formalOutcomeCodes") or []) != source_outcomes:
            blockers.append(f"Formal OutcomeCodes drift from source blueprint for {code}.")
        if formal_row:
            formal += 1

        mapped_skills = [str(x) for x in row.get("engineSkillIds") or []]
        mapped_benchmarks = [str(x) for x in row.get("benchmarkIds") or []]

        route_counts.setdefault(route, {"total": 0, "verified": 0, "contextual": 0, "unsupported": 0})
        route_counts[route]["total"] += 1
        route_counts[route][status.lower()] += 1

        if status == "VERIFIED":
            verified += 1
            if not formal_row:
                blockers.append(f"VERIFIED 9709 lesson lacks explicit formal mapping: {code}")
            if row.get("academicReviewStatus") != "APPROVED":
                blockers.append(f"VERIFIED 9709 lesson lacks APPROVED academic review: {code}")
            if not mapped_skills or not mapped_benchmarks:
                blockers.append(f"VERIFIED 9709 lesson lacks engine/benchmark evidence: {code}")
        elif status == "CONTEXTUAL":
            contextual += 1
            if not mapped_skills or not mapped_benchmarks:
                blockers.append(f"CONTEXTUAL 9709 lesson lacks explicit engine evidence: {code}")
            if row.get("academicReviewStatus") != "REQUIRED":
                blockers.append(f"CONTEXTUAL 9709 lesson must remain academic-review REQUIRED: {code}")
        else:
            unsupported += 1
            if mapped_skills or mapped_benchmarks:
                blockers.append(f"UNSUPPORTED 9709 lesson must not carry promoted engine mapping: {code}")

        for skill_id in mapped_skills:
            skill = skill_registry.get(skill_id)
            if skill is None:
                blockers.append(f"Stage 24 references unknown SkillId {skill_id} for {code}.")
                continue
            if skill.get("v2Status") != "ShadowVerified":
                blockers.append(f"Stage 24 contextual engine skill is not ShadowVerified: {skill_id}")

        for benchmark_id in mapped_benchmarks:
            benchmark = benchmarks.get(benchmark_id)
            if benchmark is None:
                blockers.append(f"Stage 24 references unknown benchmark {benchmark_id} for {code}.")
                continue
            if benchmark.get("skillId") not in mapped_skills:
                blockers.append(f"Benchmark {benchmark_id} SkillId does not match lesson mapping {code}.")
            if benchmark.get("domain") != domain:
                blockers.append(f"Benchmark {benchmark_id} domain does not match lesson domain for {code}.")

    coverage = {
        str(row.get("paperRoute") or ""): row
        for row in manifest.get("coverageByPaper") or []
        if isinstance(row, dict)
    }
    if set(coverage) != EXPECTED_ROUTES:
        blockers.append("Stage 24 coverageByPaper does not contain exactly the six AS/A domain-paper routes.")

    for route in EXPECTED_ROUTES:
        row = coverage.get(route)
        counts = route_counts.get(route)
        if row is None or counts is None:
            continue
        for field, key in [
            ("totalLessons", "total"),
            ("verified", "verified"),
            ("contextual", "contextual"),
            ("unsupported", "unsupported"),
        ]:
            if int(row.get(field) or 0) != counts[key]:
                blockers.append(f"Stage 24 route count mismatch for {route}.{field}.")
        if row.get("productRoutingEnabled") is not False:
            blockers.append(f"Stage 24 route {route} must keep product routing disabled.")
        if row.get("capabilityClaim") != "GATED":
            blockers.append(f"Stage 24 route {route} must remain GATED.")

    gate = manifest.get("gatePolicy") or {}
    if gate.get("coverageClaimGranularity") != "domain-and-paper-route":
        blockers.append("Stage 24 does not enforce domain/paper claim granularity.")
    if gate.get("global9709CapabilityClaimAllowed") is not False:
        blockers.append("Global 9709 capability claim is not fail-closed.")
    if gate.get("productRoutingEnabled") is not False:
        blockers.append("Stage 24 must not enable 9709 product routing.")
    if gate.get("titleSimilarityAloneNeverPromotes") is not True:
        blockers.append("Stage 24 does not explicitly prohibit title-only promotion.")

    academic = manifest.get("academicReview") or {}
    if academic.get("currentState") != "REQUIRED":
        blockers.append("Stage 24 academic review state must remain REQUIRED.")
    if int(academic.get("approvedVerifiedLessonCount") or 0) != verified:
        blockers.append("Stage 24 approved verified count does not match VERIFIED lessons.")

    summary = manifest.get("summary") or {}
    expected_summary = {
        "totalLessons": len(source_lessons),
        "verified": verified,
        "contextual": contextual,
        "unsupported": unsupported,
        "formalOutcomeMapped": formal,
        "paperRouteCount": len(EXPECTED_ROUTES),
    }
    for key, value in expected_summary.items():
        if int(summary.get(key) or 0) != value:
            blockers.append(f"Stage 24 summary mismatch for {key}.")

    if set(str(x) for x in corpus.get("domains") or []) != EXPECTED_DOMAINS:
        blockers.append("Stage 24 benchmark corpus does not cover all three governing domains.")
    if corpus.get("coverageGranularity") != "domain-and-paper-route":
        blockers.append("Stage 24 benchmark corpus lacks domain/paper coverage granularity.")
    if corpus.get("globalClaimAllowed") is not False:
        blockers.append("Stage 24 benchmark corpus permits a global 9709 claim.")
    if int(corpus.get("benchmarkCount") or 0) != len(benchmarks):
        blockers.append("Stage 24 benchmarkCount does not match corpus rows.")

    for benchmark_id, benchmark in sorted(benchmarks.items()):
        if set(str(x) for x in benchmark.get("requiredChecks") or []) != REQUIRED_CHECKS:
            blockers.append(f"Benchmark {benchmark_id} does not require the full execution chain.")
        if list(benchmark.get("requiredDifficultyBands") or []) != [1, 2, 3]:
            blockers.append(f"Benchmark {benchmark_id} does not calibrate all three difficulty bands.")
        if benchmark.get("curriculumClaimStatus") != "ENGINE_EVIDENCE_ONLY":
            blockers.append(f"Benchmark {benchmark_id} overstates curriculum claim status.")

    tests = TESTS.read_text(encoding="utf-8")
    for token in [
        "GateInventoriesAll9709LessonsAndReportsCoveragePerPaperRoute",
        "EveryContextual9709LessonReferencesEngineBenchmarkAndRequiresAcademicReview",
        "BenchmarkCorpusCoversPureMechanicsAndStatisticsAndExecutesFullChain",
        "AcademicReviewAndFormalMappingAreMandatoryForAnyVerified9709Claim",
        "MathematicsAnswerEquivalenceV2",
        "MathematicsDifficultyEngine",
        "ExactMechanicsQuestionFactory",
    ]:
        if token not in tests:
            blockers.append(f"Stage 24 acceptance coverage is missing token: {token}")

    return {
        "schemaVersion": 1,
        "audit": "Stage 24 AS/A-Level 9709 capability gate closure",
        "status": "COMPLETE" if not blockers else "BLOCKED",
        "summary": {
            "sourceLessonCount": len(source_lessons),
            "paperRouteCount": len(route_counts),
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
