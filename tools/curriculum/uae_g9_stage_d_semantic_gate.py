#!/usr/bin/env python3
"""UAE Grade 9 General: Stage D ONLY semantic-evidence acceptance gate.

Do NOT execute during Stage A–B–C. Requires independently reviewed, real
generated learner questions with scorer/verifier results, not prepared fixtures.
This gate checks evidence completeness and declared review status; it cannot
replace an expert's mathematical examination of the actual questions.
"""
from __future__ import annotations

import argparse
import json
import re
import sys
from pathlib import Path


def load(path: Path):
    with path.open("r", encoding="utf-8") as handle:
        return json.load(handle)


def check(source: dict, evidence: dict, expected_sha: str) -> list[str]:
    defects: list[str] = []
    scope = source.get("scope", {})
    if (scope.get("packCode"), scope.get("grade"), scope.get("pathway"),
        scope.get("academicLanguage")) != ("UAE-MOE-MATH", 9, "General", "ar"):
        defects.append("SOURCE_SCOPE_MISMATCH")
    if evidence.get("branchCommitSha") != expected_sha or len(expected_sha) != 40:
        defects.append("CANDIDATE_SHA_MISMATCH")
    if evidence.get("testsRunInStage") != "D":
        defects.append("TESTS_NOT_RUN_IN_STAGE_D")
    lessons = [
        (unit["unit"], lesson["code"], lesson["title"])
        for unit in source["units"] for lesson in unit["lessons"]
    ]
    expected_codes = {lesson_code for _, lesson_code, _ in lessons}
    if source.get("lessonCount") != len(lessons) or len(lessons) != len(expected_codes):
        defects.append("INDEX_DENOMINATOR_OR_IDENTITY_INVALID")
    records = evidence.get("lessons")
    if not isinstance(records, list):
        return defects + ["MISSING_REAL_LESSON_EVIDENCE"]
    actual = [entry.get("indexCode") for entry in records]
    for code in sorted(expected_codes - set(actual)):
        defects.append(f"MISSING_LESSON_EVIDENCE:{code}")
    if len(actual) != len(set(actual)):
        defects.append("DUPLICATE_LESSON_EVIDENCE")
    for record in records:
        code = record.get("indexCode", "<unknown>")
        if code not in expected_codes:
            defects.append(f"OUT_OF_SCOPE_LESSON:{code}")
            continue
        if record.get("semanticReviewer") in (None, "", record.get("author")):
            defects.append(f"NO_INDEPENDENT_REVIEWER:{code}")
        if record.get("sourceSha256") in (None, ""):
            defects.append(f"NO_SOURCE_FINGERPRINT:{code}")
        if record.get("unresolvedDefects", -1) != 0:
            defects.append(f"UNRESOLVED_SEMANTIC_DEFECTS:{code}")
        objectives = record.get("objectives")
        if not isinstance(objectives, list) or not objectives:
            defects.append(f"NO_SOURCE_OBJECTIVES:{code}")
            continue
        if len(set(x.get("objectiveId") for x in objectives)) != len(objectives):
            defects.append(f"DUPLICATE_OBJECTIVE:{code}")
        for objective in objectives:
            key = f"{code}/{objective.get('objectiveId', '<unknown>')}"
            if not objective.get("sourcePage"):
                defects.append(f"OBJECTIVE_SOURCE_PAGE_MISSING:{key}")
            if objective.get("reviewVerdict") != "PASS":
                defects.append(f"OBJECTIVE_NOT_SEMANTICALLY_APPROVED:{key}")
            samples = objective.get("realGeneratedSamples")
            if not isinstance(samples, list) or not samples:
                defects.append(f"NO_REAL_QUESTION_SAMPLES:{key}")
                continue
            for index, sample in enumerate(samples):
                skey = f"{key}/{index}"
                if not sample.get("generatorFamily") or not sample.get("actualQuestion"):
                    defects.append(f"MISSING_GENERATOR_OR_QUESTION:{skey}")
                if sample.get("displayCulture") != "ar" or sample.get("rtl") is not True:
                    defects.append(f"LEARNER_ARABIC_RTL_FAIL:{skey}")
                if re.search(r"[A-Za-z]{3,}", sample.get("actualQuestion", "")):
                    defects.append(f"ENGLISH_LEARNER_QUESTION:{skey}")
                for field in ("correctAccepted", "wrongRejected", "invalidRejected",
                              "independentVerifierPassed", "skillAlignmentReviewed"):
                    if sample.get(field) is not True:
                        defects.append(f"SAMPLE_NOT_VERIFIED:{skey}:{field}")
                if not sample.get("seed") and sample.get("seed") != 0:
                    defects.append(f"UNREPRODUCIBLE_SAMPLE:{skey}")
    return defects


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--source-index", type=Path, required=True)
    parser.add_argument("--stage-d-evidence", type=Path, required=True)
    parser.add_argument("--candidate-sha", required=True)
    args = parser.parse_args()
    defects = check(load(args.source_index), load(args.stage_d_evidence),
                    args.candidate_sha)
    result = {"gate": "GRADE9_SEMANTIC_AUDIT_GATE", "result": "FAIL" if defects else "PASS",
              "defectCount": len(defects), "defects": defects}
    print(json.dumps(result, ensure_ascii=False, indent=2))
    return 1 if defects else 0


if __name__ == "__main__":
    sys.exit(main())
