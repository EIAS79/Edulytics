#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[2]
STAGE17 = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage17-grade1-6-production-manifest.v1.json"
STAGE18 = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage18-practice-migration-manifest.v1.json"
STAGE19 = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/stage19-assessment-builder-migration-manifest.v1.json"
MAPPINGS = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json"
SERVICE = ROOT / "src/Edulytics.Services/Assessments/AssessmentBuilderService.cs"
ENGINE = ROOT / "src/Edulytics.Services/Assessments/Stage19AssessmentSkillContractEngine.cs"
SHARED_ENGINE = ROOT / "src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs"
VIEW = ROOT / "src/Edulytics.Web/Views/AssessmentBuilder/Index.cshtml"
REPORT = ROOT / "artifacts/math-intelligence/stage19-assessment-builder-closure-audit.json"

EXPECTED_METHOD = "skill-contract-assessment-solver-verified-v1"
EXPECTED_WORKFLOW = ["Select", "Generate", "Review", "Approve", "Publish"]


def read_json(path: Path) -> Any:
    return json.loads(path.read_text(encoding="utf-8-sig"))


def audit() -> dict[str, Any]:
    stage17 = read_json(STAGE17)
    stage18 = read_json(STAGE18)
    stage19 = read_json(STAGE19)
    mappings = read_json(MAPPINGS)
    blockers: list[str] = []

    s17 = {
        str(row.get("lessonCode") or ""): row
        for row in stage17.get("entries") or []
        if isinstance(row, dict)
    }
    s18 = {
        str(row.get("lessonCode") or ""): row
        for row in stage18.get("entries") or []
        if isinstance(row, dict)
    }
    mapping_by_lesson = {
        str(row.get("lessonCode") or ""): row
        for row in mappings.get("mappings") or []
        if isinstance(row, dict)
    }
    s19_entries = stage19.get("entries") or []
    s19 = {
        str(row.get("outcomeCode") or ""): row
        for row in s19_entries
        if isinstance(row, dict)
    }

    if stage19.get("status") != "COMPLETE":
        blockers.append("Stage 19 manifest is not marked COMPLETE.")
    if stage19.get("generationMethod") != EXPECTED_METHOD:
        blockers.append("Stage 19 generationMethod does not match the accepted exact Assessment method.")
    if stage19.get("workflow") != EXPECTED_WORKFLOW:
        blockers.append("Stage 19 changed the teacher Select/Generate/Review/Approve/Publish workflow.")
    if len(s19) != len(s19_entries):
        blockers.append("Stage 19 manifest contains duplicate or missing outcomeCode values.")

    evidence_lessons: set[str] = set()
    assignment_count = 0

    for outcome_code, entry in sorted(s19.items()):
        skill_id = str(entry.get("skillId") or "").strip()
        families = entry.get("allowedQuestionFamilies") or []
        lessons = entry.get("evidenceLessonCodes") or []

        if not skill_id:
            blockers.append(f"Stage 19 outcome has no SkillId: {outcome_code}")
        if not isinstance(families, list) or not families:
            blockers.append(f"Stage 19 outcome has no allowed question families: {outcome_code}")
            continue
        if len(families) != len(set(families)):
            blockers.append(f"Stage 19 outcome has duplicate question families: {outcome_code}")
        if not isinstance(lessons, list) or not lessons:
            blockers.append(f"Stage 19 outcome has no READY_VERIFIED evidence lessons: {outcome_code}")
            continue

        assignment_count += len(families)
        evidence_family_union: set[str] = set()

        for lesson_code in lessons:
            evidence_lessons.add(str(lesson_code))
            s17_row = s17.get(str(lesson_code))
            s18_row = s18.get(str(lesson_code))
            mapping = mapping_by_lesson.get(str(lesson_code))

            if s17_row is None:
                blockers.append(f"Stage 19 evidence lesson is absent from Stage 17: {lesson_code}")
                continue
            if str(s17_row.get("generationReadiness") or "") != "READY_VERIFIED":
                blockers.append(f"Stage 19 evidence lesson is not READY_VERIFIED: {lesson_code}")
            if s18_row is None:
                blockers.append(f"Stage 19 evidence lesson is absent from the exact Stage 18 family registry: {lesson_code}")
            else:
                evidence_family_union.update(str(x) for x in s18_row.get("allowedQuestionFamilies") or [])

            if mapping is None:
                blockers.append(f"Stage 19 evidence lesson has no approved mapping: {lesson_code}")
                continue
            if not bool(mapping.get("officialOutcomeMapped")):
                blockers.append(f"Stage 19 evidence lesson is not official-outcome mapped: {lesson_code}")
            if outcome_code not in [str(x) for x in mapping.get("officialOutcomeCodes") or []]:
                blockers.append(f"Stage 19 outcome {outcome_code} is not documented on evidence lesson {lesson_code}.")
            if skill_id not in [str(x) for x in mapping.get("primarySkills") or []]:
                blockers.append(f"Stage 19 SkillId {skill_id} is not the primary SkillContract for {lesson_code}.")

        unsupported_families = sorted(set(str(x) for x in families) - evidence_family_union)
        for family in unsupported_families:
            blockers.append(
                f"Stage 19 family {family} for {outcome_code} is not verified by its Stage 18 evidence lessons."
            )

    excluded_supporting = {
        str(code)
        for group in stage19.get("excludedVerifiedLessons") or []
        if isinstance(group, dict) and group.get("reason") == "SupportingLessonNoOfficialOutcomeCode"
        for code in group.get("lessonCodes") or []
    }
    verified_supporting = {
        lesson_code
        for lesson_code, row in mapping_by_lesson.items()
        if lesson_code in s17
        and str(s17[lesson_code].get("generationReadiness") or "") == "READY_VERIFIED"
        and not bool(row.get("officialOutcomeMapped"))
    }

    for lesson_code in sorted(verified_supporting - excluded_supporting):
        blockers.append(f"READY_VERIFIED supporting lesson lacks an explicit Stage 19 exclusion: {lesson_code}")
    for lesson_code in sorted(excluded_supporting - verified_supporting):
        blockers.append(f"Stage 19 supporting-lesson exclusion is not a verified supporting lesson: {lesson_code}")

    excluded_outcomes = {
        str(row.get("outcomeCode") or "")
        for row in stage19.get("excludedOfficialOutcomeCodes") or []
        if isinstance(row, dict)
    }
    if "CCSS:3.OA.B.5" not in excluded_outcomes:
        blockers.append("CCSS:3.OA.B.5 ambiguity is not explicitly fail-closed in Stage 19.")

    service = SERVICE.read_text(encoding="utf-8")
    engine = ENGINE.read_text(encoding="utf-8")
    shared = SHARED_ENGINE.read_text(encoding="utf-8")
    view = VIEW.read_text(encoding="utf-8")

    try_start = service.find("private MathematicsGenerationBatch? TryGenerate(")
    try_end = service.find("private static MathematicsGenerationBatch? TryGenerateExact(", try_start)
    try_block = service[try_start:try_end] if try_start >= 0 and try_end > try_start else ""

    required_service_tokens = [
        "Stage19AssessmentSkillContracts.TryResolve",
        "TryGenerateExact(",
        "if (exactContracts.Any(contract => contract is null))",
        "NativeMathematicsOutcomeProfileResolver.Resolve",
    ]
    for token in required_service_tokens:
        if token not in try_block:
            blockers.append(f"Assessment Builder migration routing is missing required token: {token}")

    exact_pos = try_block.find("Stage19AssessmentSkillContracts.TryResolve")
    legacy_pos = try_block.find("NativeMathematicsOutcomeProfileResolver.Resolve")
    if exact_pos < 0 or legacy_pos < 0 or exact_pos > legacy_pos:
        blockers.append("Exact Stage 19 routing does not precede legacy/native generation.")

    if "MathematicsQuestionGenerationEngine" in engine:
        blockers.append("Stage 19 exact Assessment engine must not call the legacy MathematicsQuestionGenerationEngine.")
    for token in [
        "ExactSkillContractQuestionEngine().Generate",
        "ExactSkillContractQuestionEngine.Verify",
        "broadFallbackUsed = false",
        "teacherReviewRequired = true",
        'workflow = "Select-Generate-Review-Approve-Publish"',
    ]:
        if token not in engine:
            blockers.append(f"Stage 19 exact engine is missing required contract token: {token}")

    for token in [
        "UniversalMathematicsQuestionGenerationEngine",
        "CurriculumContextCheck",
        "MathematicsAiCapabilityMatrix",
    ]:
        if token in engine or token in shared:
            blockers.append(f"Stage 19 exact path references broad fallback capability: {token}")

    for token in [
        "private static string Solve",
        "public static bool Verify",
        "ExactSkillContractQuestionEngine",
    ]:
        if token not in shared:
            blockers.append(f"Shared exact Mathematics kernel is missing required token: {token}")

    workflow_tokens = [
        'name="outcomeIds"',
        "GenerateQuestionsWithAI",
        "BuilderStatus",
        "Approve",
        "Publish",
    ]
    for token in workflow_tokens:
        if token not in view:
            blockers.append(f"Teacher Assessment Builder workflow token is missing: {token}")

    return {
        "schemaVersion": 1,
        "audit": "Stage 19 Assessment Builder migration closure",
        "status": "COMPLETE" if not blockers else "BLOCKED",
        "summary": {
            "exactOfficialOutcomeCount": len(s19),
            "verifiedEvidenceLessonCount": len(evidence_lessons),
            "questionFamilyAssignments": assignment_count,
            "explicitSupportingLessonExclusions": len(excluded_supporting),
            "blockerCount": len(blockers),
        },
        "blockers": blockers,
        "exactOutcomeCodes": sorted(s19),
        "evidenceLessonCodes": sorted(evidence_lessons),
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
            encoding="utf-8")

    print(json.dumps(report["summary"], ensure_ascii=False, indent=2))
    if args.strict and report["blockers"]:
        for blocker in report["blockers"]:
            print(f"BLOCKER: {blocker}")
        return 2
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
