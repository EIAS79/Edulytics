#!/usr/bin/env python3
"""Exact-current-run UAE G7 first-party release readiness; not a production write.

Academic parity to publisher question/illustration is explicitly UNKNOWN.
Owner approval is separate from mandatory technical and scoped migration tests.
"""
import datetime
import json
import os
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
ARTIFACTS = ROOT / "artifacts/uae-g7-stage-d"
RELEASE = ARTIFACTS / "grade7-final-release-gate.json"

NEEDED = (
    "STRUCTURE_RESULT", "SOURCE_RESULT", "LESSON_TESTS_RESULT",
    "COMPILE_RESULT", "AUTH_E2E_RESULT", "SCOPED_RESULT",
)


def main():
    failures = []
    expected = os.environ.get("GITHUB_SHA")
    if not expected or not expected.strip():
        failures.append("NO_CURRENT_GITHUB_SHA")
    for name in NEEDED:
        if os.environ.get(name) != "success":
            failures.append(name + "_NOT_SUCCESS:" + str(os.environ.get(name)))

    parsed = {}
    names = {
        "structural": "grade7-structural-gate.json",
        "source": "grade7-source-gate.json",
        "e2e": "grade7-e2e.json",
        "scoped-production": "grade7-scoped-production.json",
    }
    for phase, filename in names.items():
        path = ARTIFACTS / filename
        if not path.is_file():
            failures.append("MISSING_CURRENT_RUN_PROOF:" + phase)
            continue
        try:
            doc = json.loads(path.read_text(encoding="utf-8"))
        except (ValueError, OSError) as exc:
            failures.append("INVALID_CURRENT_RUN_PROOF:" + phase + ":" + type(exc).__name__)
            continue
        parsed[phase] = doc
        if doc.get("status") != "PASS":
            failures.append("FAILED_CURRENT_RUN_PROOF:" + phase)
        if doc.get("candidateSha") != expected:
            failures.append("CANDIDATE_SHA_MISMATCH:" + phase)

    if "structural" in parsed:
        checks = parsed["structural"].get("checks") or []
        if not checks or any(x.get("status") != "PASS" for x in checks):
            failures.append("G7_STRUCTURE_CHECKS_NOT_PASS")
    if "source" in parsed:
        source = parsed["source"]
        if source.get("sourceParityCertified") is not False or (
            source.get("sourceApprovalBasis")
            != "OWNER_APPROVED_FIRST_PARTY_WITH_DISCLOSED_SOURCE_LIMITATIONS"
        ):
            failures.append("SOURCE_PARITY_OR_OWNER_SCOPE_MISREPRESENTED")
        if any(x.get("status") != "PASS" for x in source.get("checks", [])):
            failures.append("G7_OWNER_APPROVED_FIRST_PARTY_SOURCE_GATE_NOT_PASS")
    if "e2e" in parsed:
        e2e = parsed["e2e"]
        for key, val in (
            ("authenticatedLearnerPages", 78),
            ("negativeEnglishFallbackChecks", 78),
            ("persistedPracticeAttempts", 234),
            ("chromiumPagesDesktopMobile", 156),
        ):
            if e2e.get(key) != val:
                failures.append("G7_E2E_WRONG_COUNT:" + key)
        if e2e.get("sourceLiteralParity") != "UNKNOWN" or e2e.get("productionWritten") is not False:
            failures.append("G7_E2E_SCOPE_MISREPRESENTED")
    if "scoped-production" in parsed:
        scoped = parsed["scoped-production"]
        for key, val in (
            ("approvedArabicLessonCount", 78),
            ("canonicalMetadataCount", 78),
            ("unrelatedRecordsPreserved", 5155),
            ("independentRunCount", 2),
            ("otherGradeDataChanged", False),
            ("productionDatabaseWritten", False),
        ):
            if scoped.get(key) != val:
                failures.append("G7_SCOPED_MIGRATION_INCOMPLETE:" + key)

    RELEASE.parent.mkdir(parents=True, exist_ok=True)
    output = {
        "phase": "final",
        "status": "PASS" if not failures else "NO_GO",
        "candidateSha": expected,
        "recordedAtUtc": datetime.datetime.now(datetime.timezone.utc).isoformat(),
        "ownerAcademicReleaseBasis": "FIRST_PARTY_ORIGINAL_EDUCATIONAL_EQUIVALENCE",
        "sourcePublisherItemParity": "UNKNOWN_NOT_CERTIFIED",
        "sourceCrossEditionExhaustiveness": "UNKNOWN_NOT_CERTIFIED",
        "technicalSourceAndRuntimeEvidence": sorted(parsed),
        "approvedArabicLessonCount": 78 if not failures else 0,
        "productionDatabaseWritten": False,
        "mergeDone": False,
        "renderDeploymentDone": False,
        "failedChecks": failures,
    }
    RELEASE.write_text(
        json.dumps(output, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print(json.dumps(output, ensure_ascii=False, indent=2))
    return 1 if failures else 0


if __name__ == "__main__":
    raise SystemExit(main())
