#!/usr/bin/env python3
"""Fail-closed UAE G3 Stage D gates. They do not grant source parity from CI success."""
from __future__ import annotations
import argparse
import datetime as dt
import json
import os
import pathlib
import re
import subprocess
import sys

ROOT = pathlib.Path(__file__).resolve().parents[2]
ART = ROOT / "artifacts/curriculum-delivery/UAE-MOE-MATH/G3"
OUT = ROOT / "artifacts/uae-g3-stage-d"
PACK = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g3-common-t1-ogl-v1.lesson-content-pack.json"
MANIFEST = ROOT / "docs/curriculum/UAE_G3_COMMON_OFFICIAL_MANIFEST.json"
PROGRESS = ART / "full-grade-progress.json"
COUNTS = (5, 9, 7, 6, 6, 9, 8, 9, 9, 8, 7, 6, 10, 10)
assert sum(COUNTS) == 109

def read(path: pathlib.Path):
    return json.loads(path.read_text(encoding="utf-8-sig"))

def snapshot_sha() -> str | None:
    value = os.environ.get("GITHUB_SHA")
    if value:
        return value
    try:
        return subprocess.check_output(
            ["git", "-C", str(ROOT), "rev-parse", "HEAD"], text=True
        ).strip()
    except Exception:
        return None

def inspect():
    blockers, warnings = [], []
    checks = {"units": 14, "lessons": 109, "arabic": 0,
              "uniqueCodes": 0, "abcCheckpoints": 0, "evidenceLedgers": 0}
    if not (PACK.is_file() and MANIFEST.is_file() and PROGRESS.is_file()):
        return checks, ["Required Grade 3 pack/manifest/progress missing"], warnings
    pack = read(PACK)
    progress = read(PROGRESS)
    lessons = pack.get("Lessons", [])
    if len(lessons) != 109:
        blockers.append(f"Registered lessons = {len(lessons)}, expected 109")
    codes = [l.get("LessonCode") for l in lessons]
    checks["uniqueCodes"] = len(set(codes))
    if checks["uniqueCodes"] != 109:
        blockers.append("LessonCode missing or repeated")
    intended = [
        f"PED:UAE-MOE-MATH:L3:COMMON:{u:02}:{i:02}"
        for u, count in enumerate(COUNTS, 1) for i in range(1, count + 1)
    ]
    if codes != intended:
        blockers.append("Official 14-unit/109-lesson code sequence differs from manifest baseline")
    if (progress.get("A_B_C_Ready"), progress.get("lastClosedLessonOrdinal"),
        progress.get("firstIncompleteLessonOrdinal")) != (109, 109, 110):
        blockers.append("A-B-C progress ledger not fully closed at 109")
    if pack.get("AcademicLanguage") != "ar":
        blockers.append("AcademicLanguage is not ar")
    for n, lesson in enumerate(lessons, 1):
        translations = lesson.get("Translations") or []
        ar = next((t for t in translations if t.get("CultureCode") == "ar"), None)
        if ar and all(ar.get(k) and str(ar[k]).strip() for k in (
            "Title", "Explanation", "WorkedExamples", "QuickSummary")):
            checks["arabic"] += 1
        else:
            blockers.append(f"Lesson {n} missing usable Arabic authoring")
        if not lesson.get("SourceLocator") or not lesson.get("SourceSha256"):
            blockers.append(f"Lesson {n} missing source location/hash")
        checkpoint = ART / f"lesson-{n}-stage-a-c-checkpoint.json"
        ledger = ART / f"lesson-{n}-atomic-coverage.json"
        evidence = ART / f"lesson-{n}-source-evidence.md"
        if checkpoint.is_file():
            c = read(checkpoint)
            if c.get("Status") == "READY_FOR_STAGE_D":
                checks["abcCheckpoints"] += 1
            else:
                blockers.append(f"Lesson {n} has no READY_FOR_STAGE_D A-B-C checkpoint")
        else:
            blockers.append(f"Lesson {n} has no A-B-C checkpoint")
        if ledger.is_file() and evidence.is_file():
            l = read(ledger)
            checks["evidenceLedgers"] += 1
            records = l.get("SourceItems", l.get("SourceElements", l.get("sourceItems", [])))
            observed = l.get("SourceItemsObserved",
                l.get("SourceElementsCount", l.get("ObservedNumberedTasks",
                l.get("NumberedSourceItems", l.get("observedNumberedItems", len(records))))))
            mapped = l.get("SourceItemsMappedBySkill",
                l.get("SourceItemsCoveredBySkillRepresentations"))
            if not records or not isinstance(observed, int) or observed < 1:
                blockers.append(f"Lesson {n} has no auditable source elements")
            elif isinstance(mapped, int) and mapped > len(records):
                blockers.append(f"Lesson {n} mapped more items than ledger contains")
            elif not isinstance(mapped, int):
                # Older A-B-C ledgers have different schemas; source semantic
                # equivalence still requires separate authenticated verification.
                warnings.append(f"Lesson {n} legacy item schema: semantic denominator NOT_CERTIFIED")
        else:
            blockers.append(f"Lesson {n} has no atomic/source evidence")
    if checks["arabic"] != 109:
        blockers.append(f"Arabic authoring {checks['arabic']}/109")
    return checks, blockers, warnings

def source_handoff_blockers():
    """Allow owner-authorized first-party local-source release with explicit UNKNOWN parity.
    Never equate this with certification against the unreviewed 2026-2027 books.
    """
    blockers = []
    handoff = ART / "grade3-a-c-handoff.md"
    if not handoff.is_file():
        blockers.append("Missing aggregate Grade3 source-scoped A-C handoff")
    else:
        scope = handoff.read_text(encoding="utf-8")
        if "UNKNOWN" not in scope or "109" not in scope:
            blockers.append("Handoff does not disclose limited 109-lesson source scope")
    owner_file=ART/"grade3-owner-academic-publication-authorization-20261110.json"
    if not owner_file.is_file():
        return blockers+["Missing project owner academic release decision"]
    owner=read(owner_file)
    required={
       "OwnerDecision":"ACADEMICALLY_APPROVED_BY_OWNER",
       "OwnerAcceptsKnownSourceLimitationsForOriginalContentRelease":True,
       "Independent2026_2027ElementParity":"UNKNOWN",
       "SourceParityIsNotReleaseClaim":True,
       "ThirdPartyAssetClearance":"NOT_INDEPENDENTLY_CERTIFIED",
       "NoFalseCIClaim":True
    }
    for key,value in required.items():
        if owner.get(key)!=value:
            blockers.append("Owner academic scope contract invalid: "+key)
    pdfs=owner.get("SelectedLocalSourceSha256",{})
    names={
        **{i:"01_compressed.pdf" for i in range(1,7)},
        **{i:"02_compressed.pdf" for i in range(7,11)},
        **{i:"03_compressed.pdf" for i in range(11,15)}
    }
    pack=read(PACK)
    selected=0
    for i,lesson in enumerate(pack.get("Lessons",[]),1):
        code=lesson.get("LessonCode","")
        try:unit=int(code.split(":")[-2])
        except (ValueError,IndexError):
            blockers.append(f"Lesson {i} has unparseable Unit Code");continue
        expected=pdfs.get(names.get(unit))
        if not expected or lesson.get("SourceSha256","").upper()!=expected.upper():
            blockers.append(f"Lesson {i} selected local source SHA mismatch");continue
        cp=read(ART/f"lesson-{i}-stage-a-c-checkpoint.json")
        evidence=read(ART/f"lesson-{i}-atomic-coverage.json")
        for actual in [
            cp.get("StudentSourceSha256"),cp.get("SourceSha256"),
            evidence.get("StudentSourceSha256"),
            evidence.get("SourceStudentSha256"),
            evidence.get("StudentPdfSha256"),evidence.get("SourceSha256"),
            evidence.get("sourceSha256")
        ]:
            if actual and str(actual).upper()!=expected.upper():
                blockers.append(f"Lesson {i} conflicting source snapshot SHA")
        selected+=1
        ar=next((t for t in lesson.get("Translations",[])
                 if t.get("CultureCode")=="ar"),{})
        for value in ar.values():
            if not isinstance(value,str):continue
            if any(marker in value.lower() for marker in (
                 "<img","<iframe","<svg","https://","http://",
                 "mcgraw","??????","?????? ??????")):
                blockers.append(f"Lesson {i} student body contains unreviewed third-party link or asset tag")
    if selected!=109:
        blockers.append(f"Local source-identity evidence {selected}/109")
    # Official-current-edition semantic parity and third-party licensing are
    # explicitly NOT certified; owner acceptance is distinct from those claims.
    return blockers

def final_blockers():
    blockers = []
    for key in ("STRUCTURAL_JOB_RESULT", "SOURCE_JOB_RESULT",
                "DOTNET_JOB_RESULT", "E2E_JOB_RESULT"):
        if os.environ.get(key) != "success":
            blockers.append(f"{key} != success (actual: {os.environ.get(key, 'NOT_RUN')})")
    expected_sha = snapshot_sha()
    runtime_path = OUT / "runtime/uae-g3-stage-d-authenticated-109-lessons.json"
    browser_path = OUT / "browser/report.json"
    if not runtime_path.is_file():
        blockers.append("Missing authenticated 109-lesson HTTPS and persisted attempt artifact for current run")
    else:
        runtime = read(runtime_path)
        for key, count in {
            "lessonPageCount":109,
            "correctAttemptPersistenceCount":109,
            "wrongAttemptPersistenceCount":109,
            "malformedAttemptRejectionCount":109,
            "htmlRtlMarkedCount":109
        }.items():
            if runtime.get(key) != count:
                blockers.append(f"Runtime {key} {runtime.get(key)} != {count}")
        if runtime.get("candidateSha") != expected_sha:
            blockers.append("Runtime artifact SHA does not match CI checkout SHA")
        if runtime.get("crossSchoolAccessDenied") is not True:
            blockers.append("No independently verified cross-school denial")
        if set(runtime.get("roleLogins", {}).values()) != {True}:
            blockers.append("Four-role authenticated login proof incomplete")
        if len(runtime.get("lessons",[])) != 109:
            blockers.append("Authenticated practice evidence missing registered lessons")
    for step,filename in (
        ("structural","grade3-structural-gate.json"),
        ("source","grade3-handoff-gate.json")):
        fp=OUT/step/filename
        if not fp.is_file():
            blockers.append(f"{step} current-run evidence JSON missing")
            continue
        report=read(fp)
        if report.get("status")!="PASS" or report.get("candidateSha")!=expected_sha:
            blockers.append(f"{step} evidence not green for checked SHA")
    import xml.etree.ElementTree as ET
    for filename in ("uae-g3-math.trx","g1-g2-us-regressions.trx"):
        fp=OUT/"trx"/filename
        if not fp.is_file():
            blockers.append("Current-run TRX unavailable: "+filename)
            continue
        try:
            xml=ET.parse(fp).getroot()
            counters=next((x for x in xml.iter() if x.tag.endswith("Counters")),None)
            if counters is None or int(counters.get("total",0))<1 or int(counters.get("failed",1))!=0:
                blockers.append("TRX does not contain actual all-passing tests: "+filename)
        except Exception:
            blockers.append("TRX cannot be parsed: "+filename)
    if not browser_path.is_file():
        blockers.append("Missing Chromium desktop/mobile authenticated RTL evidence")
    else:
        browser = read(browser_path)
        if (browser.get("rtlDesktopLessons"), browser.get("rtlMobileLessons")) != (109,109):
            blockers.append("Chromium did not render 109/109 desktop and mobile lessons")
        if browser.get("candidateSha") != expected_sha:
            blockers.append("Chromium artifact SHA does not match CI checkout SHA")
        if browser.get("screenshotsWritten", 0) < 8:
            blockers.append("Chromium screenshot sample evidence missing")
    return blockers

def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--phase", choices=("structural", "handoff", "final"), required=True)
    args = parser.parse_args()
    checks, structurals, warnings = inspect()
    blockers = list(structurals)
    if args.phase in ("handoff", "final"):
        blockers.extend(source_handoff_blockers())
    if args.phase == "final":
        blockers.extend(final_blockers())
    report = dict(
        phase=args.phase, status="PASS" if not blockers else "D_BLOCKED",
        candidateSha=snapshot_sha(),
        testedRefKind=os.environ.get("GITHUB_REF_TYPE", "local_branch"),
        sourceSnapshotId="UNKNOWN_MULTI_VOLUME_PARTIAL_HANDOFF",
        targetEditionParity="UNKNOWN",
        academicReleaseScope="OWNER_APPROVED_LIMITED_FIRST_PARTY_SCOPE",
        sourceParityIsNotReleaseClaim=True,
        releaseVerification="PRE_MERGE_CI_CANDIDATE_ONLY",
        unitCount=checks["units"], lessonCount=checks["lessons"],
        checks=checks, blockerCount=len(blockers),
        blockers=blockers, warnings=warnings,
        capturedAtUtc=dt.datetime.now(dt.timezone.utc).isoformat())
    OUT.mkdir(parents=True, exist_ok=True)
    out = OUT / f"grade3-{args.phase}-gate.json"
    out.write_text(json.dumps(report, ensure_ascii=False, indent=2)+"\n", encoding="utf-8")
    print(json.dumps(report, ensure_ascii=True))
    print("EVIDENCE_PATH", out)
    return 0 if not blockers else 1

if __name__ == "__main__":
    sys.exit(main())
