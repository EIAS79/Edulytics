#!/usr/bin/env python3
"""UAE Grade 9 General: Grade-7-style scoped first-party Stage D preflight.

Separate owner-approved independent teaching-content scope from literal publisher
source parity. This never certifies the latter, grants third-party rights, changes
curriculum publish state, seeds databases, merges, or deploys production.
The independent strict parity gate remains tools/curriculum/uae_g9_stage_d_semantic_gate.py.
"""
from __future__ import annotations

import argparse
import datetime as dt
import json
import os
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
BASE = ROOT / "artifacts/curriculum-delivery/UAE-MOE-MATH/G9"
RICH = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/RichV2"
LABS = ROOT / "src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general"
CONTENT = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json"
PRACTICE = ROOT / "src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs"
OUT = ROOT / "artifacts/uae-g9-stage-d"
DISALLOWED_BINARY_MEDIA = {".png", ".jpg", ".jpeg", ".webp", ".gif", ".pdf", ".mp4", ".webm", ".mov", ".avif", ".heic"}


def load(path: Path) -> dict:
    result = json.loads(path.read_text(encoding="utf-8"))
    if not isinstance(result, dict):
        raise ValueError(f"JSON object expected: {path}")
    return result


def evaluate(phase: str) -> dict:
    checks = []

    def check(name: str, condition: bool, details=None) -> None:
        checks.append({"check": name, "status": "PASS" if condition else "FAIL", "details": details})

    index = load(BASE / "grade9-82-target-index.json")
    selection = load(BASE / "source-selection.json")
    progress = load(BASE / "full-grade-progress.json")
    approval = load(BASE / "grade9-owner-first-party-release-scope.json")
    release = load(BASE / "grade9-release-validation.json")
    pack = load(CONTENT)
    rows = index.get("records", [])
    lessons = pack.get("Lessons", [])
    owner_target = selection.get("ownerTarget", {})
    codes = [x.get("LessonCode") for x in lessons]
    source_codes = [(int(row.get("UnitOrdinal", -1)), int(row.get("LessonOrdinal", -1)))
                    for row in rows]
    entries = sorted(RICH.glob("uae-g9-general-u*-l*-ar.rich-lesson-v2.json"))
    rich_lessons = []
    media_violations = []
    for file in entries:
        block = load(file)
        for lesson in block.get("Lessons", []):
            rich_lessons.append(lesson)
            if block.get("PackCode") != "UAE-MOE-MATH" or block.get("SchemaVersion") != 1:
                media_violations.append(f"{file.name}: inconsistent RichV2 pack identity")
            if lesson.get("Videos"):
                media_violations.append(f"{file.name}: unlicensed video not cleared")
            if re.search(r"https?://|data:(?:image|video)/", json.dumps(lesson, ensure_ascii=False), re.I):
                media_violations.append(f"{file.name}: media URL or base64 video/image present")
            for visual in lesson.get("Visuals", []):
                if any(re.fullmatch(r"(?i)(src|href|url|imageurl|imagepath|externalurl|posterurl|thumbnailurl)", k)
                       for k in visual):
                    media_violations.append(f"{file.name}: visual references nonoriginal media")
    if LABS.exists():
        for file in LABS.rglob("*"):
            if file.is_file() and file.suffix.lower() in DISALLOWED_BINARY_MEDIA:
                media_violations.append(str(file.relative_to(ROOT)))

    check("correct_scope_and_language",
          index.get("packCode") == pack.get("PackCode") == "UAE-MOE-MATH"
          and index.get("grade") == approval.get("grade") == 9
          and index.get("pathway") == approval.get("pathway") == "General"
          and index.get("academicLanguage") == pack.get("AcademicLanguage") == "ar"
          and index.get("direction") == "rtl" and approval.get("rtl") is True)
    check("owner_selected_thirteen_units_eighty_two_lessons",
          index.get("expectedUnits") == progress.get("ownerTargetUnits") == 13
          and index.get("expectedLessons") == progress.get("ownerTargetLessons") == 82
          and len(rows) == len(lessons) == 82)
    check("unique_ordered_lesson_identifiers",
          len(set(codes)) == 82
          and len(set(source_codes)) == 82
          and all(code and code.startswith("PED:UAE-MOE-MATH:L9:GENERAL:")
                  for code in codes)
          and all(code.split(":")[4:6] == [f"{unit:02}", f"{lesson:02}"]
                  for code, (unit, lesson) in zip(codes, source_codes)),
          {"count": len(codes)})
    title_variants = [
        {"ordinal": i, "lessonCode": x.get("LessonCode"),
         "sourceIndexTitle": row.get("LessonArabicTitle"),
         "authoredArabicTitle": x["Translations"][0].get("Title")}
        for i, (x, row) in enumerate(zip(lessons, rows), 1)
        if x.get("Translations") and x["Translations"][0].get("Title") != row.get("LessonArabicTitle")
    ]
    check("arabic_names_and_source_pages",
          all(x.get("Translations") and
              x["Translations"][0].get("CultureCode") == "ar" and
              bool(re.search("[\\u0600-\\u06ff]", x["Translations"][0].get("Title", "")))
              and row.get("SourcePageAccessible") is True
              and row.get("SourcePageNonblank") is True
              and isinstance(x.get("SourceSha256"), str) and len(x["SourceSha256"]) == 64
              for x, row in zip(lessons, rows)),
          {"textualTitleVariantsNotAssertedEqual": len(title_variants)})

    check("all_first_party_richv2_present",
          len(entries) == len(rich_lessons) == 82
          and len({x.get("LessonCode") for x in rich_lessons}) == 82
          and set(x.get("LessonCode") for x in rich_lessons) == set(codes)
          and all(x.get("CultureCode") == "ar" for x in rich_lessons))
    check("first_party_media_no_binary_source_import_in_examined_layers",
          not media_violations, {"violations": media_violations})
    check("scope_keeps_86_raw_general_source_entries_separate",
          index.get("sourceRows") == progress.get("rawGeneralLessons") == 86
          and index.get("requiredBookCount") == 5
          and len(selection.get("approvedSourceBooks", [])) == 3
          and len(selection.get("referenceOnlyBooks", [])) == 2)
    check("A_B_C_authoring_recorded_without_asserting_D",
          progress.get("abcClosedLessonCount") == 82
          and (BASE / "first-12-stage-a-c-closure.json").exists()
          and all((BASE / f"lesson-{i}-stage-a-c-checkpoint.json").exists()
                  for i in range(13, 83)))
    check("source_snapshot_provenance_preserved",
          bool(selection.get("sourceSnapshotId"))
          and selection.get("sourceSnapshotId") == progress.get("sourceSnapshotId")
          and release.get("sourceSnapshotId") == selection.get("sourceSnapshotId"))
    if phase == "structural":
        contracts = PRACTICE.read_text(encoding="utf-8")
        # Text presence is not a registered digital contract: Grade9 code
        # literals also appear in explicit TEACHER_ONLY exclusion guards.
        # Count only real keyed byLesson assignments and require exact
        # one-to-one correspondence with the 82 authored lesson identities.
        digital_contract_codes = set(re.findall(
            r'byLesson\["(PED:UAE-MOE-MATH:L9:GENERAL:[^"]+)"\]\s*=\s*new LessonPracticeContract\(',
            contracts
        ))
        digital = [i for i, code in enumerate(codes, 1)
                   if code in digital_contract_codes]
        teacher_only = []
        for i, code in enumerate(codes, 1):
            if code in contracts:
                continue
            filename = BASE / f"lesson-{i}-stage-a-c-checkpoint.json"
            if not filename.exists():
                continue
            checkpoint = load(filename)
            stage_c = checkpoint.get("stageC", {})
            stage_b = checkpoint.get("stageB", {})
            if (checkpoint.get("lessonCode") == code
                    and checkpoint.get("closed") is True
                    and stage_c.get("practiceFamilies") == []
                    and stage_c.get("teacherReviewRequiredFor")
                    and "TEACHER_REVIEW_ONLY" in stage_c.get("status", "")
                    and stage_b.get("coolDownQuestions", 0) > 0
                    and stage_b.get("coolDownGraded") is False):
                teacher_only.append(i)
        check("all_82_lessons_have_prepared_digital_or_teacher_routes",
              len(digital) + len(teacher_only) == 82
              and teacher_only == [80, 81], {
                  "preparedDigitalContractDefinitions": len(digital),
                  "preparedIsNotRuntimeReady": True,
                  "teacherOnlyPrepared": teacher_only,
                  "teacherOnlyRuntimeVerified": False,
                  "finalAuthenticatedE2ERequired": True
              })
        check("target_school_year_and_framework_version",
              index.get("schoolYearTarget") == "2026-2027"
              and pack.get("TargetCurriculumPeriod") == "2026-2027"
              and pack.get("VersionCode") == "MOE-2026-2027-T1")
    elif phase == "source":
        check("recorded_owner_academic_permission",
              approval.get("ownerAcademicApprovalRecorded") is True
              and approval.get("ownerPermissionToProceedToReleaseGates") is True
              and approval.get("academicFirstPartyScopeApproved") is True
              and (BASE / approval.get("academicApprovalProvenance", "")).is_file())
        check("first_party_limited_not_publisher_item_parity",
              approval.get("originalPublisherElementParity") == "NOT_INDEPENDENTLY_VERIFIED"
              and approval.get("sourceLiteralParityClaim") == "UNKNOWN"
              and approval.get("sourceEditionIdentityClaim") == "NONE_OFFICIAL_2026_2027_NOT_PROVEN"
              and release.get("sourceIndexComplete") is False
              and release.get("targetLessonsOneToOneVerified") == 0)
        check("publisher_asset_rights_not_falsely_claimed",
              approval.get("publisherBookReferenceOnly") is True
              and approval.get("publisherArtworkOrVerbatimTextReuseLicensed") is False
              and approval.get("publisherArtworkOrCopiedMediaUseAuthorized") is False
              and approval.get("internalSourceProvenanceRequired") is True)
        check("technical_release_prerequisites_not_waived",
              approval.get("deploymentAuthorizedOnlyAfterSuccessfulExactShaCiAndScopedDbVerification") is True
              and approval.get("authenticatedFourRoleE2ERequired") is True
              and approval.get("isolatedDatabaseRehearsalRequired") is True)
    else:
        raise ValueError(f"Unknown phase: {phase}")

    source_parity_confirmed = (release.get("rawLessonsFullyAudited") == 86 and
        release.get("targetLessonsOneToOneVerified") == 82 and
        release.get("sourceIndexComplete") is True)
    result = {
        "schemaVersion": 1,
        "phase": phase,
        "candidateSha": os.environ.get("GITHUB_SHA", "NOT_GITHUB_RUN"),
        "testedRefKind": os.environ.get("GITHUB_EVENT_NAME", "LOCAL_DIAGNOSTIC"),
        "checkedAtUtc": dt.datetime.now(dt.timezone.utc).isoformat(),
        "scope": "UAE-MOE-MATH/G9/GENERAL/AR",
        "sourceSnapshotId": selection.get("sourceSnapshotId"),
        "status": "PASS" if all(x["status"] == "PASS" for x in checks) else "D_BLOCKED",
        "checks": checks,
        "sourceToAuthoredArabicTitleTextualVariants": title_variants,
        "allTextualTitlesExactMatch": len(title_variants) == 0,
        "unpassed": [x["check"] for x in checks if x["status"] != "PASS"],
        "ownerFirstPartyPublicationScope": "OWNER_APPROVED_LIMITED_FIRST_PARTY" if phase == "source" else "NOT_EVALUATED",
        "publisherSourceLiteralParity": "VERIFIED" if source_parity_confirmed else "UNKNOWN",
        "publisherRightsVerified": False,
        "sourceAtomicParityCertified": source_parity_confirmed,
        "productionGrade9PublishedLessons": "NOT_QUERIED_BY_THIS_LOCAL_PREFLIGHT",
        "grade9ProductionImportPerformedByThisGate": False,
        "grade9ContentPackStatus": pack.get("Status"),
        "readyForProductionRelease": False,  # no E2E/CI/DB authorization in this gate
        "requiresSeparateAuthenticatedE2E": True,
        "requiresSeparateIsolatedDatabaseRehearsal": True,
        "requiresSeparateHostedFinalCI": True,
        "strictSourceGateUnmodified": True,
        "productionWritten": False,
    }
    OUT.mkdir(parents=True, exist_ok=True)
    (OUT / f"grade9-first-party-{phase}-gate.json").write_text(
        json.dumps(result, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    return result


def main(argv=None) -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--phase", choices=("structural", "source"), required=True)
    args = parser.parse_args(argv)
    try:
        result = evaluate(args.phase)
    except Exception as exc:
        result = {"phase": args.phase, "status": "D_BLOCKED",
                  "failure": f"INVALID_EVIDENCE: {type(exc).__name__}: {exc}"}
    print(json.dumps(result, ensure_ascii=True, indent=2))
    return 0 if result["status"] == "PASS" else 1


if __name__ == "__main__":
    sys.exit(main())
