#!/usr/bin/env python3
"""Grade 8 General Arabic Stage D diagnostics; fails closed, never writes production."""
import argparse
import datetime
import json
import os
import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
G8 = ROOT / "artifacts/curriculum-delivery/UAE-MOE-MATH/G8"
OUT = ROOT / "artifacts/uae-g8-stage-d"
CORE = ROOT / "src/Edulytics.Core/Curriculum"
BLUE = CORE / "LessonBlueprints/Packs/uae-g8-general-t1-ogl-v1.lesson-blueprint.json"
PACK = CORE / "LessonContent/Packs/uae-g8-general-t1-ogl-v1.lesson-content-pack.json"
MANIFEST = ROOT / "docs/curriculum/UAE_G8_GENERAL_OFFICIAL_MANIFEST.json"
HANDOFF = ROOT / "docs/curriculum/UAE_G8_GENERAL_ABC_HANDOFF_TO_STAGE_D_20261117.md"
COUNTS = [10, 5, 8, 9, 7, 4, 7, 6, 6, 4, 6, 4]
FIELDS = ("Explanation", "KeyConceptsAndRules", "WorkedExamples",
          "StepByStepSolutions", "CommonMistakes", "QuickSummary")

def read(path):
    return json.loads(path.read_text(encoding="utf-8"))

def main(phase):
    bp, pack, manifest, progress = map(read, (BLUE, PACK, MANIFEST, G8 / "full-grade-progress.json"))
    lessons, contents, units = bp["Lessons"], pack["Lessons"], bp["Units"]
    results = []
    def check(name, truth, details=None):
        results.append({"check": name, "status": "PASS" if truth else "FAIL", "details": details})

    codes = [l["LessonCode"] for l in lessons]
    content_codes = [l["LessonCode"] for l in contents]
    ordered = sorted(lessons, key=lambda l: (l["UnitNumber"], l["LessonNumber"]))
    expected_pairs = [(u + 1, l + 1) for u, count in enumerate(COUNTS) for l in range(count)]
    tests = sorted((ROOT / "tests").glob("academic-uae-grade8-arabic-u??l??.test.cjs"))
    rich_files = sorted((CORE / "LessonContent/RichV2").glob("uae-g8-general-u??-l??-ar.rich-lesson-v2.json"))
    evidence_files = [G8 / ("lesson-%03d-source-evidence.json" % i) for i in range(1, 77)]
    checkpoints = [G8 / ("lesson-%03d-stage-a-c-checkpoint.json" % i) for i in range(1, 77)]

    if phase == "structural":
        check("12_units_76_lessons", len(units) == len(manifest["Units"]) == 12 and
              len(lessons) == len(contents) == manifest["LessonCount"] == 76 and
              [u["LessonCount"] for u in units] == COUNTS and
              [len(u["Lessons"]) for u in manifest["Units"]] == COUNTS)
        check("identical_unique_codes", len(set(codes)) == 76 and codes == content_codes and
              all(c.startswith("PED:UAE-MOE-MATH:L8:GENERAL:") for c in codes))
        check("lesson_order", [(l["UnitNumber"], l["LessonNumber"]) for l in ordered] == expected_pairs and
              [l["SortOrder"] for l in ordered] == list(range(1, 77)))
        check("ar_only_no_fallback", pack.get("AcademicLanguage") == "ar" and
              all(len(x["Translations"]) == 1 and x["Translations"][0]["CultureCode"] == "ar" and
                  all(x["Translations"][0].get(k, "").strip() for k in FIELDS) and
                  re.search(r"[\u0600-\u06ff]", x["Translations"][0]["Title"])
                  for x in contents))
        check("retired_12_05_not_in_candidate", not any(":12:05:" in c for c in codes))
        check("76_rich_v2_and_76_native_lesson_tests", len(rich_files) == len(tests) == 76,
              {"rich_v2": len(rich_files), "tests": len(tests)})
        check("76_abc_source_evidence_and_checkpoints", all(x.is_file() for x in evidence_files + checkpoints))
        if all(x.is_file() for x in checkpoints):
            cps = [read(x) for x in checkpoints]
            check("all_checkpoints_ready_in_source_order",
                  all(c.get("status") == "READY_FOR_STAGE_D" and
                      c.get("ordinal") == i and c.get("lessonCode") == codes[i - 1]
                      for i, c in enumerate(cps, start=1)))
        check("abc_status_does_not_claim_ci", progress.get("closedABCCount") == 76 and
              progress.get("testsRun") == "NOT_RUN" and
              progress.get("stageDStatus") == "PENDING" and
              progress.get("technicalReleaseReady") is False)
        check("grade_and_target_scope", bp.get("LogicalLevel") == 8 and
              bp.get("Pathway") == "General" and manifest.get("Pathway") == "General" and
              pack.get("TargetCurriculumPeriod") == "2026-2027")
        check("handoff_source_exists", HANDOFF.is_file())
    else:
        index = read(G8 / "source-index-76-arabic-reconciliation.json")
        diff_file = G8 / "edition-diff.json"
        check("source_version_difference_documented", diff_file.is_file() and
              index.get("verifiedSource", {}).get("file") == "03-9-12.pdf")
        diff = read(diff_file) if diff_file.is_file() else {}
        check("source_edition_conflict_resolved",
              diff.get("editionConflictStatus") == "FULLY_RESOLVED_WITH_LESSON_CROSSWALK",
              {"status": diff.get("editionConflictStatus", "MISSING")})
        missing_parity = []
        for i, file in enumerate(evidence_files, 1):
            if not file.is_file():
                missing_parity.append(i)
                continue
            e = read(file)
            if not (isinstance(e.get("sourceElementCount"), int) and
                    isinstance(e.get("verifiedEquivalentCount"), int) and
                    e["sourceElementCount"] > 0 and
                    e["verifiedEquivalentCount"] == e["sourceElementCount"] and
                    e.get("sourceItemExactParity") == "VERIFIED"):
                missing_parity.append(i)
        check("all_76_atomic_source_element_parities_verified", len(missing_parity) == 0,
              {"unverified_count": len(missing_parity), "first_unverified_ordinals": missing_parity[:12]})
        check("all_lesson_source_locator_and_edition_present",
              len(contents) == 76 and all(x.get("SourceLocator") and x.get("SourceEdition")
                                          for x in contents))
        check("grade8_owner_release_authorization_recorded",
              (G8 / "grade8-owner-academic-approval.json").is_file())
        check("stale_semantic_hashes_regenerated",
              (G8 / "grade8-semantic-hash-reconciliation.json").is_file())
        check("production_retirement_transaction_rehearsed",
              (G8 / "grade8-legacy-retirement-rehearsal.json").is_file())
    OUT.mkdir(parents=True, exist_ok=True)
    ok = all(x["status"] == "PASS" for x in results)
    record = {
        "schemaVersion": 1, "phase": phase,
        "candidateSha": os.environ.get("GITHUB_SHA", "LOCAL_DIAGNOSTIC_NOT_A_CI_SHA"),
        "testedRefKind": os.environ.get("GITHUB_EVENT_NAME", "LOCAL_DIAGNOSTIC"),
        "recordedAtUtc": datetime.datetime.now(datetime.timezone.utc).isoformat(),
        "status": "PASS" if ok else "D_BLOCKED",
        "checks": results, "failed": [x["check"] for x in results if x["status"] == "FAIL"],
        "stageDVerifiedLessons": 0,
        "sourceParityCertified": False,
        "productionTouched": False
    }
    (OUT / ("grade8-%s-gate.json" % phase)).write_text(
        json.dumps(record, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print(json.dumps(record, ensure_ascii=False, indent=2))
    return 0 if ok else 1

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--phase", required=True, choices=("structural", "source"))
    raise SystemExit(main(parser.parse_args().phase))
