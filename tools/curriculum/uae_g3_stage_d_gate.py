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
    blockers = []
    handoff = ART / "grade3-a-c-handoff.md"
    if not handoff.is_file():
        blockers.append("Missing required aggregate grade3-a-c-handoff.md")
    else:
        text = handoff.read_text(encoding="utf-8")
        if "UNKNOWN" in text or "NOT_CERTIFIED" in text:
            blockers.append("The source handoff explicitly leaves 2026-2027 equivalence unverified")
    # A-B-C evidence is skill-scoped to local books; no 1:1 independently
    # verified official-2026-27 denominator is present in this candidate.
    blockers.append("Official 2026-2027 source edition and complete per-item semantic parity UNKNOWN")
    blockers.append("Teacher-guide/rights and human-review gates not independently certified")
    return blockers

def final_blockers():
    blockers = []
    for key in ("STRUCTURAL_JOB_RESULT", "SOURCE_JOB_RESULT",
                "DOTNET_JOB_RESULT", "E2E_JOB_RESULT"):
        if os.environ.get(key) != "success":
            blockers.append(f"{key} != success (actual: {os.environ.get(key, 'NOT_RUN')})")
    blockers.append("No authenticated 109-lesson HTTPS, persisted attempts and RTL visual evidence")
    blockers.append("No same-SHA rights, academic parity and merge/Render release approval")
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
