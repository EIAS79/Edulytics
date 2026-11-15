#!/usr/bin/env python3
"""Fail-closed diagnostics for UAE Grade 7 General Arabic. Never seeds production."""
import argparse
import datetime
import json
import os
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
G7 = ROOT / "artifacts/curriculum-delivery/UAE-MOE-MATH/G7"
OUT = ROOT / "artifacts/uae-g7-stage-d"
BLUE = ROOT / "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g7-general-ar73-student-local-v1.lesson-blueprint.json"
BODY = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g7-general-ar73-student-local-v1.lesson-content-pack.json"
CONTRACTS = ROOT / "src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs"

def data(path):
    return json.loads(path.read_text(encoding="utf-8"))

def main(phase):
    checks = []
    def check(key, ok, details=None):
        checks.append({"check": key, "status": "PASS" if ok else "FAIL", "details": details})
    idx = data(G7 / "source-index.json")
    bp = data(BLUE)
    body = data(BODY)
    progress = data(G7 / "full-grade-progress.json")
    units = idx["units"]
    index_lessons = [x for u in units for x in u["lessons"]]
    lessons = bp["Lessons"]
    contents = body["Lessons"]
    codes = [x["LessonCode"] for x in lessons]
    body_codes = [x["LessonCode"] for x in contents]
    ordinal = [x["ordinal"] for x in index_lessons]
    expected_by_unit = [x["ordinal"] for x in index_lessons]
    order_by_sort = [x["SortOrder"] for x in sorted(lessons, key=lambda x: (x["UnitNumber"], x["LessonNumber"]))]
    expected_printed = list(range(1, len(lessons) + 1))
    snapshot_id = idx.get("sourceSnapshotId") or idx.get("SourceSnapshotId")
    if phase == "structural":
        pack = data(ROOT / "src/Edulytics.Core/Curriculum/Packs/uae-moe-math.curriculum-pack.json")
        official = sum(1 for x in pack["Nodes"]
                       if x["IsOfficial"] and x["Kind"] in ("Standard", "Outcome", "Reference"))
        legacy = [x["Code"] for x in pack["Nodes"] if
                  "UAE:REF:TEXTBOOK:G7:GENERAL:" in x["Code"] or
                  "UAE:CATALOG:G7:GENERAL:" in x["Code"]]
        check("retired_english_grade7_reference_nodes_absent", len(legacy) == 0, {"legacy_count":len(legacy)})
        check("uae_pack_official_counts_integrity",
              len(pack["Nodes"]) == pack["NodeCount"] == 1467 and
              official == pack["OfficialNodeCount"] == 1358 and
              len(pack["Links"]) == pack["LinkCount"] == 48,
              {"nodeCount":len(pack["Nodes"]), "declared":pack["NodeCount"],
               "officialActual":official,"officialDeclared":pack["OfficialNodeCount"],
               "links":len(pack["Links"])})
        check("curriculum_identity", idx.get("packCode") == "UAE-MOE-MATH" and idx.get("grade") == 7 and idx.get("pathway") == "General", {"code": idx.get("packCode"), "grade": idx.get("grade"), "pathway": idx.get("pathway")})
        check("academic_language_ar", body.get("AcademicLanguage") == "ar" and idx.get("academicLanguage") == "ar" and idx.get("rtl") is True)
        check("candidate_counts", len(units) == 11 and len(lessons) == len(contents) == len(index_lessons) == 78 and progress.get("closedABCCount") == 78, {"units": len(units), "blueprint": len(lessons), "content": len(contents), "index": len(index_lessons)})
        check("source_editions_separated", idx.get("originalAnnualTocLessonCount") == 73 and idx.get("supplementalEditionLessonCount") == 5 and len(units[7]["lessons"]) == 8)
        check("unique_lesson_ids", len(set(codes)) == len(codes) == len(set(body_codes)) and codes == body_codes)
        check("correct_grade_general_ped_prefix", all(c.startswith("PED:UAE-MOE-MATH:L7:GENERAL:") for c in codes))
        check("no_english_learner_titles", all(x.get("Title") and any("\u0600" <= k <= "\u06ff" for k in x["Title"]) for x in lessons))
        check("all_abc_checkpoints_present", all((G7 / f"lesson-{i:03d}-stage-a-c-checkpoint.json").is_file() for i in range(1, 79)))
        check("all_78_rich_content", len(list((ROOT / "src/Edulytics.Core/Curriculum/LessonContent/RichV2").glob("uae-g7-general-u*-l*-ar.rich-lesson-v2.json"))) == 78)
        check("all_78_node_test_files", len(list((ROOT / "tests").glob("academic-uae-grade7-arabic-u*l*.test.cjs"))) == 78)
        contracts_text = CONTRACTS.read_text(encoding="utf-8")
        check("all_codes_in_practice_contracts", all(c in contracts_text for c in codes), {"contracts_present": sum(c in contracts_text for c in codes), "expected": len(codes)})
        check("sequential_display_order_matches_textbook_units", order_by_sort == expected_printed, {"actual_order_by_unit": order_by_sort, "expected": expected_printed})
        check("course_version_spans_all_terms_without_t1_only", not str(bp.get("VersionCode", "")).endswith("-T1"), {"version": bp.get("VersionCode")})
        check("source_snapshot_id_defined", isinstance(snapshot_id, str) and bool(snapshot_id.strip()))
        check("no_runtime_english_fallback", False, "NOT_RUN: requires authenticated API/DOM fail-closed negative-case testing")
    else:
        handoff = G7 / "grade7-a-c-handoff.md"
        check("source_handoff_present", handoff.is_file())
        check("original_and_supplement_books_distinguished", idx.get("originalAnnualTocLessonCount") == 73 and idx.get("supplementalEditionLessonCount") == 5)
        check("all_78_lesson_pdf_shas", all(isinstance(x.get("sourceStudentBookSha256"), str) and len(x["sourceStudentBookSha256"]) == 64 for x in index_lessons))
        check("all_78_ready", all(x.get("stageABCStatus") == "READY_FOR_STAGE_D" for x in index_lessons))
        check("source_snapshot_id_defined", isinstance(snapshot_id, str) and bool(snapshot_id.strip()))
        check("unit_and_lesson_term_resolved", all(x.get("term") or x.get("Term") for x in index_lessons))
        ledgers = [data(G7 / f"lesson-{i:03d}-atomic-coverage.json") for i in range(1, 79)]
        check("element_level_denominators_known", all(isinstance(x.get("sourceElementCount"), int) and isinstance(x.get("verifiedEquivalentCount"), int) for x in ledgers), "Per-question anchors are not a complete educational element 1:1 map")
        check("source_parity_evidenced", progress.get("sourceParityPercent") == 100 and all(x.get("verifiedEquivalentCount") == x.get("sourceElementCount") for x in ledgers), {"reported_percent": progress.get("sourceParityPercent")})
        audit = data(G7 / "multi-edition-source-gap-audit-20261115.json")
        check("all_selected_book_editions_crosswalk_closed", audit.get("otherEditionCrosswalkStatus") == "FULLY_VERIFIED")
        check("rights_and_teacher_review_recorded", False, "NOT_VERIFIED: no lesson-level documented human signoff asserted")
    OUT.mkdir(parents=True, exist_ok=True)
    record = {
        "schemaVersion": 1,
        "phase": phase,
        "candidateSha": os.environ.get("GITHUB_SHA", "NOT_GITHUB_RUN"),
        "testedRefKind": os.environ.get("GITHUB_EVENT_NAME", "LOCAL_DIAGNOSTIC"),
        "recordedAtUtc": datetime.datetime.now(datetime.timezone.utc).isoformat(),
        "sourceSnapshotId": snapshot_id,
        "status": "PASS" if all(x["status"] == "PASS" for x in checks) else "D_BLOCKED",
        "checks": checks,
        "unpassed": [x["check"] for x in checks if x["status"] != "PASS"],
        "stageDVerifiedLessons": 0,
        "productionTouched": False,
    }
    dest = OUT / f"grade7-{phase}-gate.json"
    dest.write_text(json.dumps(record, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print(json.dumps(record, ensure_ascii=False, indent=2))
    return 0 if record["status"] == "PASS" else 1

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--phase", choices=("structural", "source"), required=True)
    args = parser.parse_args()
    raise SystemExit(main(args.phase))
