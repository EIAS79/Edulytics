#!/usr/bin/env python3
"""Exact-current-CI UAE G8 technical release gate; never writes production."""
import datetime
import json
import os
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
ART = ROOT / "artifacts/uae-g8-stage-d"
OWNER = ROOT / "artifacts/curriculum-delivery/UAE-MOE-MATH/G8/grade8-owner-academic-approval.json"
JOBS = ("STRUCTURE_RESULT", "SOURCE_RESULT", "LESSON_RESULT", "DOTNET_RESULT",
        "ISOLATED_RESULT", "AUTH_RESULT")
FILES = {
    "structural": "grade8-structural-gate.json",
    "source": "grade8-source-gate.json",
    "isolated": "grade8-isolated-db.json",
    "rehearsal": "grade8-scoped-77-to-76-rehearsal.json",
    "e2e": "grade8-e2e.json",
}
def main():
    errors = []
    sha = os.environ.get("GITHUB_SHA")
    if not sha or len(sha) < 12:
        errors.append("MISSING_CURRENT_CI_SHA")
    for job in JOBS:
        if os.environ.get(job) != "success":
            errors.append(job + "_NOT_SUCCESS:" + str(os.environ.get(job)))
    parsed={}
    for label,filename in FILES.items():
        f=ART / filename
        try:
            data=json.loads(f.read_text(encoding="utf-8"))
            parsed[label]=data
            if data.get("candidateSha") != sha:
                errors.append("CROSS_SHA:" + label)
            if data.get("status") != "PASS":
                errors.append("NOT_PASS:" + label)
        except (OSError,ValueError):
            errors.append("MISSING_OR_INVALID:" + label)
    try:
        owner=json.loads(OWNER.read_text(encoding="utf-8"))
    except (OSError,ValueError):
        owner={}
        errors.append("MISSING_OWNER_APPROVAL")
    for key,value in {
        "grade":8,
        "pathway":"General",
        "ownerAcademicApproval":True,
        "ownerFirstPartyAuthorshipRepresentation":True,
        "publisherArtworkOrVerbatimTextReuseLicensed":False,
        "deploymentAuthorizedOnlyAfterCiAndScopedDbVerification":True,
    }.items():
        if owner.get(key)!=value:
            errors.append("WRONG_OWNER_APPROVAL:" + key)
    structure=parsed.get("structural",{})
    if not structure.get("checks") or any(c.get("status")!="PASS" for c in structure.get("checks",[])):
        errors.append("STRUCTURAL_INCOMPLETE")
    source=parsed.get("source",{})
    if source.get("sourceParityCertified") is not False or any(
        c.get("status")!="PASS" for c in source.get("checks",[])
    ):
        errors.append("SOURCE_LIMITS_OR_APPROVAL_MISREPRESENTED")
    for label,expected in {
        "isolated":{
            "generalLessons":76,"advancedLessons":77,"publishedGeneralMetadata":76,
            "legacyExtra":0,"persistedTranslations":0,"productionWritten":False,
        },
        "rehearsal":{
            "originalEnglishFixture":77,"ownerApprovedArabicLessons":76,
            "independentCutoverRuns":2,"grade8AdvancedRetained":77,
            "productionDatabaseWritten":False,
        },
        "e2e":{
            "authenticatedLearnerPages":76,"negativeEnglishFallbackChecks":76,
            "persistedPracticeAttempts":228,"chromiumPagesDesktopMobile":152,
            "sourceLiteralParity":"UNKNOWN","productionWritten":False,
        },
    }.items():
        for key,value in expected.items():
            if parsed.get(label,{}).get(key)!=value:
                errors.append("WRONG_"+label.upper()+":"+key)
    if parsed.get("e2e") and (parsed["e2e"].get("screenshots") or 0)<8:
        errors.append("CHROMIUM_SCREENSHOTS_MISSING")
    output={
        "phase":"final","status":"READY_FOR_MANUAL_PRODUCTION_CUTOVER" if not errors else "NO_GO",
        "candidateSha":sha,
        "recordedAtUtc":datetime.datetime.now(datetime.timezone.utc).isoformat(),
        "ownerAcademicApprovalRecorded":owner.get("ownerAcademicApproval") is True,
        "lessonCount":76,
        "sourcePublisherElementParity":"UNKNOWN_NOT_CERTIFIED",
        "publisherImageOrTextCopyLicense":"NOT_CLAIMED",
        "productionDatabaseWritten":False,"merged":False,"renderDeployed":False,
        "evidenceReceived":sorted(parsed),"failures":errors,
    }
    ART.mkdir(parents=True,exist_ok=True)
    (ART/"grade8-final-release-gate.json").write_text(
        json.dumps(output,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
    print(json.dumps(output,ensure_ascii=False,indent=2))
    return 1 if errors else 0

if __name__ == "__main__":
    raise SystemExit(main())
