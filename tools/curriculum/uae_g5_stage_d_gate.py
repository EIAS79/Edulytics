#!/usr/bin/env python3
"""Fail-closed UAE Grade 5 Stage D CI diagnostics; no academic/release approval."""
import argparse
import json
import os
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
EVID = ROOT / "artifacts/curriculum-delivery/UAE-MOE-MATH/G5"
OUT = ROOT / "artifacts/uae-g5-stage-d"
BLUE = ROOT / "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g5-general-t1-ogl-v1.lesson-blueprint.json"
RICH = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/RichV2"
COUNTS = [9, 10, 13, 6, 14, 11, 8, 13, 12, 7, 11, 13]
TOTAL = sum(COUNTS)


def read(path):
    return json.loads(path.read_text(encoding="utf-8-sig"))


def report(phase, status, errors, **extra):
    OUT.mkdir(parents=True, exist_ok=True)
    data = {
        "phase": phase, "status": status, "errors": sorted(set(errors)),
        "candidateSha": os.getenv("GITHUB_SHA"), "testedRef": os.getenv("GITHUB_REF"),
        "actionsRunId": os.getenv("GITHUB_RUN_ID"), "actionsRunAttempt": os.getenv("GITHUB_RUN_ATTEMPT"),
        "grade": 5, "framework": "UAE-MOE-MATH", "provisionalLessons": TOTAL,
        "releaseApproved": False, "renderDeployed": False, **extra,
    }
    file = OUT / f"grade5-{phase}-gate.json"
    file.write_text(json.dumps(data, indent=2, ensure_ascii=False) + "\n", encoding="utf-8")
    print(json.dumps({"status": status, "phase": phase, "errors": errors,
                      "evidence": str(file)}, ensure_ascii=False))
    return 0 if status == "PASS" else 1


def audit_structural():
    errors = []
    expected = []
    checked = 0
    mapped = 0
    offset = 0
    for unit, count in enumerate(COUNTS, 1):
        for lesson in range(1, count + 1):
            ordinal = offset + lesson
            code = f"PED:UAE-MOE-MATH:L5:GENERAL:{unit:02d}:{lesson:02d}"
            expected.append(code)
            for suffix in ("stage-a-c-checkpoint", "atomic-source-coverage"):
                file = EVID / f"lesson-{ordinal:03d}-{suffix}.json"
                if not file.exists():
                    errors.append(f"MISSING:{file.name}")
                    continue
                item = read(file)
                if item.get("LessonOrdinal") != ordinal or item.get("LessonCode") != code:
                    errors.append(f"IDENTITY:{file.name}")
                if suffix.startswith("stage"):
                    checked += 1
                else:
                    mapped += 1
        offset += count
    blueprint = read(BLUE)
    official_codes = [x.get("LessonCode") for x in blueprint.get("Lessons", [])]
    if len(official_codes) != TOTAL or set(official_codes) != set(expected):
        errors.append("BLUEPRINT_IDENTITY_OR_COUNT_MISMATCH")
    if len(set(official_codes)) != TOTAL:
        errors.append("BLUEPRINT_DUPLICATES")
    rich_codes = []
    for file in RICH.glob("uae-g5-*.rich-lesson-v2.json"):
        for content in read(file).get("Lessons", []):
            rich_codes.append(content.get("LessonCode"))
            if not content.get("ExplanationParagraphs") or not content.get("WorkedExamples"):
                errors.append(f"EMPTY_ARABIC_CONTENT:{content.get('LessonCode')}")
    if len(rich_codes) != TOTAL or set(rich_codes) != set(expected):
        errors.append("RICH_V2_127_IDENTITY_OR_COUNT_MISMATCH")
    if len(set(rich_codes)) != TOTAL:
        errors.append("RICH_V2_DUPLICATES")
    families = read(ROOT / "src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json")
    family_ids = [x["id"] for x in families.get("families", [])]
    if len(family_ids) != len(set(family_ids)):
        errors.append("QUESTION_FAMILY_DUPLICATES")
    return report("structural", "FAIL" if errors else "PASS", errors,
                  actualCheckpoints=checked, actualAtomicRecords=mapped,
                  actualArabicRichLessons=len(rich_codes), actualBlueprintLessons=len(official_codes),
                  note="GENERAL identities are existing source-contract identifiers, not proof of an official COMMON alias.")


def audit_source():
    errors = []
    selection = read(EVID / "source-selection.json")
    blueprint = read(BLUE)
    for path, code in [
        (EVID / "grade5-a-c-handoff.md", "MISSING_A_C_HANDOFF"),
        (EVID / "source-index.json", "MISSING_SOURCE_INDEX"),
        (ROOT / "docs/curriculum/UAE_G5_COMMON_OFFICIAL_MANIFEST.json", "PLAN_NAMED_OFFICIAL_MANIFEST_MISSING"),
    ]:
        if not path.is_file():
            errors.append(code)
    if selection.get("SourceLockFor2026_2027OfficialIndex") != "LOCKED":
        errors.append("OFFICIAL_2026_2027_INDEX_NOT_LOCKED")
    if "2025" in selection.get("EditionUsed", ""):
        errors.append("2025_2026_LOCAL_BOOK_IS_NOT_PROOF_OF_2026_2027")
    if blueprint.get("VersionCode") == "MOE-2026-2027-T1" and errors:
        errors.append("BLUEPRINT_TARGET_YEAR_SOURCE_CONFLICT")
    if blueprint.get("Pathway") == "General":
        errors.append("PLAN_COMMON_VS_REPOSITORY_GENERAL_ALIAS_UNVERIFIED")
    element_rows = 0
    lessons_with_elements = 0
    lessons_with_explicit_numerator = 0
    for ordinal in range(1, TOTAL + 1):
        file = EVID / f"lesson-{ordinal:03d}-atomic-source-coverage.json"
        if not file.exists():
            errors.append(f"MISSING_ATOMIC:{ordinal}")
            continue
        item = read(file)
        rows = item.get("SourceElements")
        if isinstance(rows, list) and rows:
            lessons_with_elements += 1
            element_rows += len(rows)
        if isinstance(item.get("VerifiedEquivalentCount"), int):
            lessons_with_explicit_numerator += 1
    if lessons_with_elements != TOTAL or lessons_with_explicit_numerator != TOTAL:
        errors.append("ELEMENT_DENOMINATOR_AND_EQUIVALENCE_PROOF_INCOMPLETE")
    return report("source", "BLOCKED" if errors else "PASS", errors,
                  firstIncompleteLessonOrdinal=1 if errors else None,
                  acceptedLessons=0, sourceElementRowsRecorded=element_rows,
                  sourceElementFullDenominator=None, equivalentElementsVerified=None,
                  semanticParityPercent="UNKNOWN", officialEdition="UNVERIFIED",
                  rightsStatus="UNVERIFIED", humanReview="PENDING",
                  dStatus="D_PENDING_A_C_EVIDENCE" if errors else "D_PENDING",
                  note="Source records do not establish exhaustive page-level parity or latest-year approval.")


def audit_final():
    results = {name: os.getenv(env, "NOT_RUN") for name, env in [
        ("source", "SOURCE_RESULT"), ("structural", "STRUCTURAL_RESULT"),
        ("math", "MATH_RESULT"), ("authenticatedE2E", "E2E_RESULT"),
    ]}
    errors = [f"{name}:{status}" for name, status in results.items()
              if status != "success"]
    approval_path = EVID / "grade5-owner-academic-approval.json"
    try:
        approval = read(approval_path) if approval_path.is_file() else {}
    except (OSError, ValueError):
        approval = {}
    owner_approved = (
        approval.get("ownerAcademicApproval") == "APPROVED"
        and approval.get("ownerReleaseAuthorization") ==
        "APPROVED_SUBJECT_TO_TESTS_AND_RELEASE_SAFETY"
    )
    if not owner_approved:
        errors.append("OWNER_ACADEMIC_APPROVAL_NOT_RECORDED")
    asset_rights = approval.get("externalVisualRights", "UNVERIFIED")
    if asset_rights not in ("VERIFIED_CLEARED", "FIRST_PARTY_ONLY_VERIFIED"):
        errors.append("NON_ORIGINAL_VISUAL_ASSET_RIGHTS_NOT_CLEARED")
    # Automated browser screenshots are smoke evidence, not a human review.
    visual_human_review = approval.get("humanRtlScreenshotReview", "NOT_RECORDED")
    if visual_human_review != "APPROVED_WITH_EVIDENCE":
        errors.append("HUMAN_RTL_SCREENSHOT_REVIEW_NOT_RECORDED")
    # The source gate only checks provisional structures. Its present report
    # never certifies exhaustive 2026-2027 numerator/denominator parity.
    errors.append("SOURCE_2026_2027_ELEMENT_PARITY_UNVERIFIED")
    return report("final", "BLOCKED", errors,
                  dependencyResults=results, finalCandidateAccepted=False,
                  sourceParityPercent="UNKNOWN", digitalCoveragePercent="UNKNOWN",
                  ownerAcademicApprovalRecorded=owner_approved,
                  externalVisualRightsStatus=asset_rights,
                  authenticatedE2ETested=results["authenticatedE2E"] == "success",
                  humanRtlReviewStatus=visual_human_review,
                  releaseDecision="NO_GO_SOURCE_100", mergeAllowed=False,
                  renderAllowed=False,
                  note="Owner approval and passed HTTPS/Chromium E2E are recorded separately. Source element parity, asset rights, and human visual acceptance remain independently unverified; never substitute past or merely structural results.")


if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--phase", choices=["structural", "source", "final"], required=True)
    args = parser.parse_args()
    raise SystemExit({"structural": audit_structural, "source": audit_source,
                      "final": audit_final}[args.phase]())
