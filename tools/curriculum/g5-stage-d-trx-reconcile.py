#!/usr/bin/env python3
"""Stage D: reconcile actual xUnit TRX unit tests against exactly 148 Grade 5 lessons.

This is a fail-closed technical diagnostic, NEVER academic approval. A successful
aggregate test run is insufficient to claim that each of the 148 lessons was tested.
Node-linked lessons are verified by a separate required CI job; this .NET-only
reconciler never claims a Node lesson passed because xUnit passed.
No runtime DB credentials, external services, or publishing permissions are needed.
"""
from __future__ import annotations
import argparse
import json
import re
import subprocess
import sys
import xml.etree.ElementTree as ET
from collections import Counter, defaultdict
from pathlib import Path

PREFIX = "PED:US-CCSS-MATH:G5:U"
MATCHER = re.compile(
    r"UsCcssG5(?:U(?P<unit>\d\d)L(?P<lesson>\d\d)|Unit(?P<unit_old>\d+)Lesson(?P<lesson_old>\d+))",
    re.IGNORECASE,
)
VALID_RESULTS = {"Passed", "Failed", "NotExecuted", "Inconclusive", "Timeout", "Aborted"}

def local_name(node: ET.Element) -> str:
    return node.tag.split("}", 1)[-1]

def match_lesson(test_class: str) -> str | None:
    match = MATCHER.search(test_class or "")
    if not match:
        return None
    unit = int(match.group("unit") or match.group("unit_old"))
    lesson = int(match.group("lesson") or match.group("lesson_old"))
    if not (1 <= unit <= 8 and 1 <= lesson <= 99):
        return None
    return f"{PREFIX}{unit:02d}:L{lesson:02d}"

def parse_test_results(trx: Path) -> list[dict]:
    root = ET.parse(trx).getroot()
    definitions = {}
    for elem in root.iter():
        if local_name(elem) != "UnitTest":
            continue
        uid = elem.attrib.get("id")
        method = next((x for x in elem if local_name(x) == "TestMethod"), None)
        if uid and method is not None:
            definitions[uid] = method.attrib.get("className", "")
    results = []
    for elem in root.iter():
        if local_name(elem) != "UnitTestResult":
            continue
        method_class = definitions.get(elem.attrib.get("testId", ""), "")
        test_name = elem.attrib.get("testName", "")
        lesson = match_lesson(method_class or test_name)
        result = elem.attrib.get("outcome", "UNKNOWN")
        if result not in VALID_RESULTS:
            result = "UNKNOWN"
        results.append({"lessonCode": lesson, "testName": test_name,
                        "className": method_class, "outcome": result,
                        "testId": elem.attrib.get("testId", "")})
    return results

def reconcile(lesson_ledger: dict, results: list[dict], candidate: str) -> dict:
    source_lessons = lesson_ledger.get("lessons")
    if not isinstance(source_lessons, list) or len(source_lessons) != 148:
        raise ValueError("Not exactly 148 source lessons")
    codes = [x.get("LessonCode") for x in source_lessons]
    if len(set(codes)) != 148:
        raise ValueError("Duplicate lesson codes in G5 A-C ledger")
    for i, code in enumerate(codes):
        if not isinstance(code, str) or not code.startswith(PREFIX):
            raise ValueError(f"Wrong Grade 5 LessonCode at ordinal {i+1}")
    by_lesson = defaultdict(list)
    for result in results:
        code = result["lessonCode"]
        if code and code in codes:
            by_lesson[code].append(result)
    rows = []
    for idx, code in enumerate(codes, 1):
        direct = by_lesson[code]
        outcomes = Counter(x["outcome"] for x in direct)
        linked_test = source_lessons[idx - 1].get("testPath") or source_lessons[idx - 1].get("testFile") or ""
        expected_runner = ("DOTNET" if linked_test.endswith(".cs") else
                           "NODE" if linked_test.endswith(".cjs") else "UNKNOWN")
        status = ("DIRECT_TEST_FAILURE" if any(x != "Passed" for x in outcomes) else
                  "DIRECT_TESTS_PASSED_OTHER_D_GATES_OPEN" if direct else
                  "NODE_SHARD_EVIDENCE_REQUIRED" if expected_runner == "NODE" else
                  "REQUIRED_XUNIT_TEST_MISSING" if expected_runner == "DOTNET" else
                  "UNDECLARED_TEST_RUNNER")
        rows.append({
            "ordinal": idx, "lessonCode": code, "directResultCount": len(direct),
            "expectedTestRunner": expected_runner, "linkedTestPath": linked_test,
            "outcomes": dict(outcomes), "technicalStatus": status,
            "testNames": sorted({x["testName"] for x in direct}),
            "dVerified": False, "academicApproved": False,
        })
    missing = [x["lessonCode"] for x in rows if not x["directResultCount"]]
    failing = [x["lessonCode"] for x in rows if x["technicalStatus"] == "DIRECT_TEST_FAILURE"]
    missing_required_xunit = [x["lessonCode"] for x in rows if x["technicalStatus"] == "REQUIRED_XUNIT_TEST_MISSING"]
    outstanding_node = [x["lessonCode"] for x in rows if x["technicalStatus"] == "NODE_SHARD_EVIDENCE_REQUIRED"]
    unknown_runner = [x["lessonCode"] for x in rows if x["technicalStatus"] == "UNDECLARED_TEST_RUNNER"]
    shared = [x for x in results if x["lessonCode"] is None]
    return {
        "schemaVersion": 1,
        "kind": "GRADE5_STAGE_D_REAL_TRX_LESSON_ATTRIBUTION_NOT_RELEASE_APPROVAL",
        "candidateSha": candidate, "scope": {"lessons": 148, "units": 8},
        "totalXunitResultCount": len(results),
        "xunitOutcomes": dict(Counter(x["outcome"] for x in results)),
        "lessonDirectTestCount": len(rows) - len(missing),
        "lessonWithoutDirectTestCount": len(missing),
        "lessonWithDirectFailureCount": len(failing),
        "requiredXunitMissingCount": len(missing_required_xunit),
        "requiredXunitMissingLessons": missing_required_xunit,
        "nodeShardEvidenceRequiredCount": len(outstanding_node),
        "nodeShardEvidenceRequiredLessons": outstanding_node,
        "undeclaredRunnerCount": len(unknown_runner),
        "undeclaredRunnerLessons": unknown_runner,
        "missingDirectLessonTests": missing,
        "directFailures": failing,
        "crossLessonOrUnattributedTestCount": len(shared),
        "dVerified": 0, "academicApproved": 0,
        "note": "A passing shared xUnit result is not attributed to a lesson unless its test class names the lesson. Node-linked lessons require independent passing Node shard evidence; no Node PASS is inferred from xUnit. Source parity and authenticated E2E remain separate.",
        "lessons": rows
    }

def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--trx", required=True, type=Path)
    parser.add_argument("--ledger", required=True, type=Path)
    parser.add_argument("--output", required=True, type=Path)
    args = parser.parse_args()
    ledger = json.loads(args.ledger.read_text(encoding="utf-8"))
    candidate = subprocess.check_output(["git", "rev-parse", "HEAD"], text=True).strip()
    report = reconcile(ledger, parse_test_results(args.trx), candidate)
    args.output.parent.mkdir(parents=True, exist_ok=True)
    args.output.write_text(json.dumps(report, indent=2, ensure_ascii=False) + "\n", encoding="utf-8")
    print("G5_STAGE_D_PER_LESSON_TRX " + json.dumps({
        "candidateSha": candidate,
        "totalXunit": report["totalXunitResultCount"],
        "outcomes": report["xunitOutcomes"],
        "directLessons": report["lessonDirectTestCount"],
        "missingDirect": report["lessonWithoutDirectTestCount"],
        "requiredXunitMissing": report["requiredXunitMissingCount"],
        "nodeShardOutstanding": report["nodeShardEvidenceRequiredCount"],
        "directFailures": report["lessonWithDirectFailureCount"],
        "artifact": str(args.output)
    }))
    if report["requiredXunitMissingCount"] or report["undeclaredRunnerCount"] or report["lessonWithDirectFailureCount"]:
        print("G5_STAGE_D_XUNIT_INCOMPLETE: required C#-linked direct evidence absent or failed", file=sys.stderr)
        return 2
    if report["nodeShardEvidenceRequiredCount"]:
        print("G5_STAGE_D_XUNIT_SCOPE_COMPLETE: Node-only lesson evidence remains mandatory in separate D1–D3 shard jobs")
    return 0

if __name__ == "__main__":
    raise SystemExit(main())
