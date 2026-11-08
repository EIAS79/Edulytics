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
            blockers.setdefault(index, []).append(
                "NEWER_ARABIC_STUDENT_EXERCISE_EDITION_NOT_VERIFIED"
            )

    if matched != 102:
        problems.append("SOURCE_PAGE_AND_SHA_RECORDS_INCOMPLETE")
    if closed != 102:
        problems.append("ABC_READY_RECORDS_INCOMPLETE")
    if stage_d.get("firstIncompleteLessonOrdinal") != 1:
        problems.append("D_FIRST_INCOMPLETE_CURSOR_MUST_START_1")

    critical = bool(problems)
    eligibility_blocked = bool(blockers)
    statuses = collections.Counter(d.get("dStatus", "UNKNOWN")
                                   for d in stage_d.get("lessons", []))
    failure = critical or phase in ("handoff", "final") and eligibility_blocked
    if phase == "final":
        # Final fail-closed until exact SHA runtime, auth/DB, RTL and rights/approval.
        failure = True
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
        "release": "NO_GO_TECHNICAL" if phase == "final" else
                   "D_PENDING_A_C_EVIDENCE",
        "missingTechnicalEvidence": ([
            "FULL_BUILD_AND_TARGETED_TESTS_PINNED_TO_SHA",
            "AUTHENTICATED_MULTI_ROLE_E2E_ON_ISOLATED_DB",
            "ARABIC_RTL_VISUAL_ACCESSIBILITY",
            "IMPACT_REGRESSION", "REQUIRED_FINAL_CI",
            "HUMAN_AND_RIGHTS_APPROVAL", "PRODUCTION_DEPLOYMENT_VERIFICATION",
        ] if phase == "final" else []),
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
