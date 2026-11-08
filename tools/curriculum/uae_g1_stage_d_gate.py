#!/usr/bin/env python3
"""UAE Grade 1 Stage D fail-closed intake; structural checks are not runtime tests."""
import argparse
import collections
import datetime as dt
import hashlib
import json
import os
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
ART = ROOT / "artifacts/curriculum-delivery/UAE-MOE-MATH/G1"
PACK = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g1-common-t1-ogl-v1.lesson-content-pack.json"
BLUEPRINT = ROOT / "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g1-common-t1-ogl-v1.lesson-blueprint.json"
LEDGER = ART / "grade1-all-11-units-abc-coverage-ledger.json"
HANDOFF = ART / "grade1-final-stage-abc-handoff-20261108.json"
STAGE_D = ART / "grade1-stage-d-execution-ledger.json"


def read(path):
    return json.loads(path.read_text(encoding="utf-8-sig"))


def report(phase):
    problems = []
    blockers = {}
    closed = 0
    matched = 0
    source_numerators_known = 0
    try:
        pack, bp, ledger, handoff, stage_d = (
            read(PACK), read(BLUEPRINT), read(LEDGER), read(HANDOFF), read(STAGE_D)
        )
        lessons, blueprint, translations = (
            ledger["Lessons"], bp["Lessons"], pack["Lessons"]
        )
    except (ValueError, OSError, KeyError, TypeError) as error:
        return {
            "phase": phase, "status": "FAIL", "problems": [f"INPUT_INVALID:{error}"],
            "candidateSha": os.getenv("GITHUB_SHA"),
        }

    if len(lessons) != 102 or len(blueprint) != 102 or len(translations) != 102:
        problems.append("DENOMINATOR_MISMATCH: require 102/102/102 for selected handoff")
    if pack.get("AcademicLanguage") != "ar":
        problems.append("ARABIC_CANONICAL_LANGUAGE_MISMATCH")
    if len(ledger.get("Units", [])) != 11:
        problems.append("EXPECTED_TECHNICAL_UNITS_11")
    if handoff.get("LedgerReadyForStageD") != 102:
        problems.append("HANDOFF_RECORDED_COUNT_MISMATCH")
    if stage_d.get("sourceSnapshotId") != "sha256:" + hashlib.sha256(
        HANDOFF.read_bytes()
    ).hexdigest():
        problems.append("SOURCE_SNAPSHOT_DRIFT")
    if stage_d.get("baseABCSha") != "78bded7ad1427e08c345455d44d3110fdc27efb1":
        problems.append("BASE_ABC_SHA_MISMATCH")
    if stage_d.get("contentVersion") != pack.get("ContentVersion"):
        problems.append("CONTENT_VERSION_MISMATCH")

    codes = [lesson.get("LessonCode") for lesson in lessons]
    if len(set(codes)) != len(codes) or any(not c for c in codes):
        problems.append("DUPLICATED_OR_NULL_LESSON_CODES")
    bp_codes = [x.get("LessonCode") for x in blueprint]
    content_codes = [x.get("LessonCode") for x in translations]
    if bp_codes != codes or content_codes != codes:
        problems.append("BLUEPRINT_CONTENT_LEDGER_ORDER_MISMATCH")
    d_codes = [x.get("lessonCode") for x in stage_d.get("lessons", [])]
    if d_codes != codes:
        problems.append("STAGE_D_LEDGER_SCOPE_MISMATCH")

    for index, row in enumerate(lessons, 1):
        code = row.get("LessonCode", "")
        evidence = ART / f"lesson-{index}-source-evidence.md"
        checkpoint = ART / f"lesson-{index}-stage-a-c-checkpoint.json"
        audit = ART / f"lesson-{index}-atomic-coverage.md"
        d = stage_d["lessons"][index - 1] if index <= len(stage_d["lessons"]) else {}
        if not all(p.is_file() for p in (evidence, checkpoint, audit)):
            problems.append(f"MISSING_ABC_EVIDENCE:{index}")
            blockers.setdefault(index, []).append("MISSING_ABC_EVIDENCE")
            continue
        cp = read(checkpoint)
        if (cp.get("LessonCode") != code or cp.get("LessonOrdinal") != index or
            cp.get("Status") != "READY_FOR_STAGE_D" or
            not row.get("AcademicClosure")):
            problems.append(f"ABC_CHECKPOINT_CONFLICT:{index}")
        if cp.get("TestsRun") not in (None, "WRITTEN_NOT_RUN"):
            problems.append(f"UNSUPPORTED_STAGE_C_TEST_CLAIM:{index}")
        culture = [t for t in translations[index - 1].get("Translations", [])
                   if t.get("CultureCode") == "ar"]
        if len(culture) != 1:
            problems.append(f"ARABIC_TRANSLATION_MISSING_OR_DUPLICATE:{index}")
        elif not all(str(culture[0].get(field, "")).strip() for field in
                     ("Title", "Explanation", "WorkedExamples", "StepByStepSolutions",
                      "CommonMistakes", "QuickSummary")):
            problems.append(f"ARABIC_RICH_CONTENT_INCOMPLETE:{index}")
        if d.get("lessonOrdinal") != index or d.get("lessonCode") != code:
            problems.append(f"D_CHECKPOINT_ID_MISMATCH:{index}")
        if row.get("StageABCScope") == "READY_FOR_STAGE_D":
            closed += 1
        if cp.get("SourcePdfPages") and cp.get("SourceSha256"):
            matched += 1
        if isinstance(d.get("sourceElementCount"), int) and isinstance(
            d.get("verifiedEquivalentCount"), int
        ) and d["sourceElementCount"] > 0:
            source_numerators_known += 1
            if d["verifiedEquivalentCount"] != d["sourceElementCount"]:
                blockers.setdefault(index, []).append("UNPROVEN_SEMANTIC_PARITY")
        else:
            blockers.setdefault(index, []).append("SOURCE_ELEMENT_DENOMINATOR_UNKNOWN")
        if index >= 43:
            # Later units currently use a real but older 2019–2020 Arabic
            # student edition. A teacher's guide is not evidence of the
            # exercises in a newer student book. Future independent audit
            # may clear this only with a verified newer student PDF identity.
            newer = d.get("newerArabicStudentEditionAudit", {})
            newer_evidenced = (
                isinstance(newer, dict) and
                newer.get("status") == "STUDENT_EDITION_INDEPENDENTLY_VERIFIED" and
                newer.get("editionYear") in ("2025-2026", "2026-2027") and
                isinstance(newer.get("sourceSha256"), str) and
                len(newer["sourceSha256"]) == 64 and
                isinstance(newer.get("pdfPageRange"), list) and
                len(newer["pdfPageRange"]) == 2 and
                newer.get("allSourceExerciseFamiliesAudited") is True and
                newer.get("reviewerApproval") == "APPROVED"
            )
            if not newer_evidenced:
                blockers.setdefault(index, []).append(
                    "NEWER_ARABIC_STUDENT_EXERCISE_EDITION_NOT_VERIFIED"
                )

    if matched != 102:
        problems.append("SOURCE_PAGE_AND_SHA_RECORDS_INCOMPLETE")
    if closed != 102:
        problems.append("ABC_READY_RECORDS_INCOMPLETE")
    first_incomplete = next(
        (i for i, d in enumerate(stage_d.get("lessons", []), 1)
         if d.get("dStatus") != "D_VERIFIED"), None
    )
    if stage_d.get("firstIncompleteLessonOrdinal") != first_incomplete:
        problems.append("D_FIRST_INCOMPLETE_CURSOR_INCONSISTENT")
    if stage_d.get("DVerifiedLessonCount") != sum(
        x.get("dStatus") == "D_VERIFIED" for x in stage_d.get("lessons", [])
    ):
        problems.append("D_VERIFIED_DENOMINATOR_INCONSISTENT")

    # Never mark a lesson D_VERIFIED solely on the previous ABC checkpoint:
    # each promoted lesson must have its own semantic and runtime approvals.
    for index, d in enumerate(stage_d.get("lessons", []), 1):
        if d.get("dStatus") != "D_VERIFIED":
            continue
        mandatory = (
            d.get("sourceElementCount") is not None and
            d.get("sourceElementCount", 0) > 0 and
            d.get("verifiedEquivalentCount") == d["sourceElementCount"] and
            d.get("unitTests") == "PASS" and
            d.get("integrationTests") == "PASS" and
            d.get("authenticatedE2E") == "PASS" and
            d.get("arabicRtlVisual") == "PASS" and
            d.get("rightsStatus") == "CLEARED" and
            d.get("humanReviewStatus") == "APPROVED"
        )
        if not mandatory:
            problems.append(f"D_VERIFIED_WITHOUT_INDEPENDENT_EVIDENCE:{index}")

    critical = bool(problems)
    eligibility_blocked = bool(blockers)
    statuses = collections.Counter(d.get("dStatus", "UNKNOWN")
                                   for d in stage_d.get("lessons", []))
    missing_technical = []
    if phase == "final":
        job_evidence = Path("artifacts/uae-g1-stage-d/job-results.txt")
        jobs = {}
        if job_evidence.is_file():
            jobs = dict(
                line.strip().split("=", 1)
                for line in job_evidence.read_text(encoding="utf-8").splitlines()
                if "=" in line
            )
        current_sha = os.getenv("GITHUB_SHA")
        if not current_sha or jobs.get("github_sha") != current_sha:
            missing_technical.append("FINAL_CI_SHA_EVIDENCE_MISSING_OR_MISMATCH")
        for job in ("source_job", "dotnet_job", "e2e_job"):
            if jobs.get(job) != "success":
                missing_technical.append(f"REQUIRED_JOB_NOT_SUCCESS:{job}")
        runtime_path = Path(
            "artifacts/uae-g1-stage-d/runtime/"
            "uae-g1-stage-d-authenticated-102-lessons.json"
        )
        if not runtime_path.is_file():
            missing_technical.append("G1_102_AUTHENTICATED_RUNTIME_ARTIFACT_MISSING")
        else:
            try:
                runtime = read(runtime_path)
                required_runtime = (
                    runtime.get("candidateSha") == current_sha and
                    runtime.get("lessonPageCount") == 102 and
                    runtime.get("correctAttemptPersistenceCount") == 102 and
                    runtime.get("malformedAttemptRejectionCount") == 102 and
                    runtime.get("crossSchoolAccessDenied") is True and
                    all(runtime.get("roleLogins", {}).get(role) is True
                        for role in ("student", "teacher",
                                     "subjectSupervisor", "schoolAdmin")) and
                    len(runtime.get("lessons", [])) == 102
                )
            except (ValueError, OSError, TypeError):
                required_runtime = False
            if not required_runtime:
                missing_technical.append(
                    "G1_AUTHENTICATED_RUNTIME_COUNT_OR_SHA_MISMATCH"
                )
        # Cross-check real browser pixels, not merely HTML dir/lang attributes.
        # Browser smoke PASS is separate from human screenshot/a11y sign-off.
        browser_path = Path(
            "artifacts/uae-g1-stage-d/browser-rtl/report.json"
        )
        if not browser_path.is_file():
            missing_technical.append("G1_CHROMIUM_RTL_BROWSER_ARTIFACT_MISSING")
        else:
            try:
                browser = read(browser_path)
                pages = browser.get("pages", [])
                required_browser = (
                    browser.get("candidateSha") == current_sha and
                    browser.get("studentAuthenticated") is True and
                    browser.get("rtlDesktopLessons") == 102 and
                    browser.get("rtlMobileLessons") == 102 and
                    browser.get("screenshotsWritten") == 8 and
                    len(pages) == 204 and
                    len({(p.get("lessonCode"), p.get("viewportWidth"))
                         for p in pages}) == 204 and
                    all(
                        p.get("browserRtlComputedStyle") is True and
                        p.get("arabicActualDOM") is True and
                        p.get("allSixCanonicalSections") is True and
                        p.get("noArabicBodyOverflow") is True
                        for p in pages
                    )
                )
            except (ValueError, OSError, TypeError, AttributeError):
                required_browser = False
            if not required_browser:
                missing_technical.append(
                    "G1_CHROMIUM_102_DESKTOP_MOBILE_SHA_OR_RTL_MISMATCH"
                )
        if stage_d.get("DVerifiedLessonCount") != 102:
            missing_technical.append("STAGE_D_NOT_VERIFIED_FOR_ALL_102_LESSONS")
        if stage_d.get("rights") != "CLEARED":
            missing_technical.append("RIGHTS_APPROVAL_NOT_CLEARED")
        if stage_d.get("humanApproval") != "APPROVED":
            missing_technical.append("HUMAN_ACADEMIC_RELEASE_NOT_ATTESTED")
        if stage_d.get("arabicRtlVisual") != "PASS":
            missing_technical.append("ARABIC_RTL_BROWSER_VISUAL_NOT_ATTESTED")
        if stage_d.get("impactRegression") != "PASS":
            missing_technical.append("IMPACT_REGRESSION_NOT_ATTESTED")
        if eligibility_blocked:
            missing_technical.append("SOURCE_EDITION_OR_SEMANTIC_PARITY_NOT_PROVEN")
    failure = critical or (
        phase in ("handoff", "final") and eligibility_blocked
    ) or bool(missing_technical)
    return {
        "phase": phase,
        "status": "FAIL" if failure else "PASS",
        "candidateSha": os.getenv("GITHUB_SHA"),
        "testedRefKind": os.getenv("GITHUB_EVENT_NAME", "NOT_RUN"),
        "generatedUtc": dt.datetime.now(dt.timezone.utc).isoformat(),
        "curriculum": "UAE-MOE-MATH", "grade": 1, "language": "ar",
        "blueprintLessons": len(blueprint), "abcReady": closed,
        "sourceLocators": matched, "knownSemanticDenominators": source_numerators_known,
        "dStatuses": dict(statuses),
        "blockedLessonCount": len(blockers),
        "blockedLessons": [{"lessonOrdinal": i, "reasons": r}
                           for i, r in sorted(blockers.items())],
        "problems": problems,
        "release": (
            "RELEASE_CANDIDATE_CI_PASS" if phase == "final" and not failure
            else "NO_GO_TECHNICAL" if phase == "final"
            else "D_PENDING_A_C_EVIDENCE"
        ),
        "missingTechnicalEvidence": missing_technical,
    }


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--phase", required=True,
                        choices=("structural", "handoff", "final"))
    parser.add_argument("--out", default="")
    args = parser.parse_args()
    result = report(args.phase)
    target = (Path(args.out) if args.out else
              Path("artifacts/uae-g1-stage-d") / f"{args.phase}.json")
    target.parent.mkdir(parents=True, exist_ok=True)
    target.write_text(json.dumps(result, indent=2, ensure_ascii=False) + "\n",
                      encoding="utf-8")
    print(json.dumps({
        "phase": args.phase, "status": result["status"],
        "candidateSha": result.get("candidateSha"),
        "abcReady": result.get("abcReady"),
        "blockedLessons": result.get("blockedLessonCount"),
        "problems": result.get("problems"),
        "evidence": str(target),
    }, ensure_ascii=False))
    raise SystemExit(0 if result["status"] == "PASS" else 1)


if __name__ == "__main__":
    main()
